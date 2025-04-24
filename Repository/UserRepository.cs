using ERE.DTO;
using Microsoft.EntityFrameworkCore;
using ERE.Infrastructure;
using ERE.Models;
using ERE.CustomExceptions;
using Microsoft.IdentityModel.Tokens;
using ERE.Utilities;
using ERE.DTO;

namespace ERE.Repository;
public interface IUserRepostory
{
    Task<Response> CreateUser(RegisterRequestDto request);
    Task<Response> Login(LoginRequestDto request);
    Task<Response> GetUser(string identifier);
}
public class UserRepository(AppDbContext context, UtilityService utils, IStudentRepository studentService) : IUserRepostory
{
    private readonly UtilityService utility = utils;
    private readonly IStudentRepository studentService = studentService;
    private readonly AppDbContext db = context;
    public async Task<Response> CreateUser(RegisterRequestDto request)
    {
        var existingUser = await db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (existingUser != null) {
            throw new UserAlreadyExistException();
        }

        Response response = new Response();
        RoleId userRole = (RoleId)request.Role;
        SubjectId subjectId = (SubjectId)request?.Subject;
       
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = new User {
            Firstname = request.FirstName,
            Lastname = request.LastName,
            Email = request.Email,
            Password = hashedPassword,
            Phone = request.Phone,
            RoleId = userRole,
        };

        // create Teacher | Student | Parent record for this user
        using var transaction = await db.Database.BeginTransactionAsync();
        try {
            switch (userRole) {
                case RoleId.STUDENT:
                    var student = new Student(user);
                    student.LevelId = (LevelId)request.LevelId;
                    var parents = new List<Parent>();
                    var users = new List<User>(){user};
                    var contactRecords = new List<Contact>();
                    
                    var contactPhones = request.Contacts.Select(c => c.Phone).ToList(); // request
                    var existingParents = await db.Parents
                        .Where(u => contactPhones.Contains(u.Phone))
                        .ToListAsync();
                    var existingParentPhones = existingParents.Select(c => c.Phone).ToList();
                    

                    if (!contactPhones.IsNullOrEmpty()) {
                        foreach (var contact in request.Contacts) {
                            if (!existingParentPhones.Contains(contact.Phone)) {
                                var parentUser = new User {
                                    Firstname = contact.FirstName,
                                    Lastname = contact.LastName,
                                    Email = contact.Email,
                                    Phone = contact.Phone,
                                    RoleId = RoleId.PARENT,
                                };
                                users.Add(parentUser);

                                var parent = new Parent(parentUser);
                                parents.Add(parent);

                                var cont = new Contact {
                                    ParentId = parent.Id,
                                    StudentId = student.Id,
                                    FirstName = contact.FirstName,
                                    LastName = contact.LastName,
                                    Email = contact.Email,
                                    Phone = contact.Phone,
                                    HomeNumber = contact.HomeNumber,
                                    Street = contact.Street,
                                    Village = contact.Village,
                                    Commune = contact.Commune,
                                    District = contact.District,
                                    Province = contact.Province,
                                };
                                contactRecords.Add(cont);
                            } else {
                                var existingParent = existingParents.FirstOrDefault(u => u.Phone == contact.Phone);

                                var cont = new Contact {
                                    ParentId = existingParent.Id,
                                    StudentId = student.Id,
                                    FirstName = contact.FirstName,
                                    LastName = contact.LastName,
                                    Email = contact.Email,
                                    Phone = contact.Phone,
                                    HomeNumber = contact.HomeNumber,
                                    Street = contact.Street,
                                    Village = contact.Village,
                                    Commune = contact.Commune,
                                    District = contact.District,
                                    Province = contact.Province,
                                };
                                contactRecords.Add(cont);
                            }
                        }
                    }
                    db.AddRange(users);
                    db.AddRange(parents);
                    db.AddRange(contactRecords);
                    db.Add(student);
                    response.Payload = new { 
                        user = student, 
                        contacts = contactRecords, 
                     };
                    break;
                case RoleId.TEACHER:
                    var teacher = new Teacher(user, subjectId);
                    db.Teachers.Add(teacher);
                    response.Payload = new { user = teacher };
                    break;
                default:
                    throw new InvalidRoleException();
            }
            await db.SaveChangesAsync();
            await transaction.CommitAsync();
            response.Success = true;
            response.Message = "User created successfully";
            return response;
        } catch {
            await transaction.RollbackAsync();
            throw;
        } 
    }

    public Task<Response> Login(LoginRequestDto request)
    {
        Response response = new Response();
        var user = db.Users.FirstOrDefault(u => u.Email == request.EmailOrPhoneNumber || u.Phone == request.EmailOrPhoneNumber);
        if (user == null) {
            throw new InvalidLoginException();
        }
        // null check if user.password
        Boolean isValidPassword = false;
        if (!string.IsNullOrEmpty(user.Password)) {
            isValidPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
        }
        if (!isValidPassword && user.DefaultPassword != request.Password) {
            throw new InvalidLoginException();
        }

        response.Success = true;
        response.Payload = new {
            token = utility.GenerateJwtToken(user),
        };
        response.Message = "Login successful";
        return Task.FromResult(response);
    }

    public async Task<Response> GetUser(string identifier) 
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == identifier || u.Phone == identifier || u.Id == identifier);
        if (user == null) {
            throw new StudentNotFoundException();
        }
        var response = new Response();
        if (user.RoleId == RoleId.STUDENT) {
            var getMainReportDto = new GetMainReportDto();

            var student = await db.Students
                .Include(s => s.User__r)
                .FirstOrDefaultAsync(s => s.UserId == user.Id);

            getMainReportDto.StudentIds = new List<string>() { student.Id };
            var result = await studentService.GetMainReports(getMainReportDto); 
            response.Payload = new {
                Student = student,
                MainReport = result[student.Id],
            };
        } 
        else if (user.RoleId == RoleId.TEACHER) {
            var teacher = await db.Teachers
                .Include(t => t.User__r)
                .FirstOrDefaultAsync(t => t.UserId == user.Id);
            response.Payload = teacher;} 

        else if (user.RoleId == RoleId.PARENT) {
            var parent = await db.Parents
                .FirstOrDefaultAsync(t => t.UserId == user.Id);

            var studentIds = await db.Contacts
                .Where(c => c.ParentId == parent.Id)
                .Select(c => c.StudentId)
                .ToListAsync();

            var getMainReportDto = new GetMainReportDto();
            getMainReportDto.StudentIds = studentIds;

            var result = await studentService.GetMainReports(getMainReportDto);
            response.Payload = new {
                Parent = parent,
                Students = result.Select(r => new {
                    StudentId = r.Key,
                    StudentName = r.Value.FirstOrDefault().StudentName,
                    StudentEmail = r.Value.FirstOrDefault().StudentEmail,
                    MainReport = r.Value,
                }).ToList(),
            };


        }
        
        response.Success = true;
        response.Message = "User found successfully";
        return response;
    }

    class CourseReportDto {
        public string Id { get; set; }
        public string MainReportId { get; set; }
        public string StatusId { get; set; }
        public string TeacherName {get; set;}
        public float Score { get; set; }
        public float Absences {get; set;}

        public string TeacherCmt { get; set; }
    }

    class MainReportDto {
        public string Id { get; set; }
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }
        public MonthId MonthId { get; set; }
        public LevelId LevelId { get; set; }
        public string Status { get; set; }
        public HashSet<CourseReportDto> CourseReports { get; set; }
    }
}

[Serializable]
internal class ContactAlreadyExistException : Exception
{
    public ContactAlreadyExistException()
    {
    }

    public ContactAlreadyExistException(string? message) : base(message)
    {
    }

    public ContactAlreadyExistException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}