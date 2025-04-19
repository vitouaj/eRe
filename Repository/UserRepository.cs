using ERE.DTO;
using Microsoft.EntityFrameworkCore;
using ERE.Infrastructure;
using ERE.Models;
using ERE.CustomExceptions;

namespace ERE.Repository;
public interface IUserRepostory
{
    Task<Response> CreateUser(RegisterRequestDto request);
    Task<Response> Login(LoginRequestDto request);
}
public class UserRepository(AppDbContext context) : IUserRepostory
{
    private readonly AppDbContext db = context;
    public async Task<Response> CreateUser(RegisterRequestDto request)
    {
        Response response = new Response();
        RoleId userRole = (RoleId)request.Role;
        SubjectId subjectId = (SubjectId)request?.Subject;

        var user = new User
        {
            Firstname = request.FirstName,
            Lastname = request.LastName,
            Email = request.Email,
            Password = request.Password,
            Phone = request.Phone,
            RoleId = userRole,
        };

        var existingUser = await db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (existingUser != null) {
            throw new UserAlreadyExistException();
        }

        // create Teacher | Student | Parent record for this user
        switch (userRole) {
            case RoleId.STUDENT:
                var student = new Student(user);
                db.Students.Add(student);
                response.Payload = new { user = student };
                break;
            case RoleId.TEACHER:
                var teacher = new Teacher(user, subjectId);
                db.Teachers.Add(teacher);
                response.Payload = new { user = teacher };
                break;
            case RoleId.PARENT:
                var parent = new Parent(user);
                db.Parents.Add(parent);
                response.Payload = new { user = parent };
                break;
            default:
                throw new InvalidRoleException();
        }

        user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
        db.Users.Add(user);
        await db.SaveChangesAsync();
        response.Success = true;
        response.Message = "User created successfully";
        return response;
    }

    public Task<Response> Login(LoginRequestDto request)
    {
        Response response = new Response();
        var user = db.Users.FirstOrDefault(u => u.Email == request.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        {
            throw new InvalidLoginException();
        }
        response.Success = true;
        response.Payload = new {
            token = Guid.NewGuid().ToString(),
        };
        response.Message = "Login successful";
        return Task.FromResult(response);
    }
}