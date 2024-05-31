using eRe.Dto;
using eRe.Infrastructure;
using eRe.User;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Npgsql.Internal;

namespace eRe.Repository;

public interface IClassroomRepository
{
    Task<List<Classroom.Classroom>> GetAllAsync(string Id);
    Task<Response> CreateAsync(ClassroomDto classDto);
    Task<bool> UpdateAsync(ClassroomDto classDto);
    Task<bool> DeleteAsync(string classId);
    Task<ClassroomDto> GetAsync(string classId);

    Task<Response> GetSubjectsAsync();

    /*
        Add Student to Classroom
    */
    Task<Response> AddSubjectItemAsync(string classroomId, SubjectItemDto subjectItemDto);
    Task<Response> GetSubjectItemsAsync(string classId);
    Task<EnrollResult> EnrollStudents(string classId, List<string> StudentIds);

    Task<Response> GetMonthsAsync();
    Task<Response> GetEnrollStudentAsync(string classId);

}

public class ClassroomRepository(AppDbContext context) : IClassroomRepository
{
    private readonly AppDbContext db = context;

    public async Task<EnrollResult> EnrollStudents(string classId, List<string> StudentIds)
    {
        List<string> invalidIds = [];

        foreach (var Id in StudentIds)
        {
            var valid = await db.Users.Where(u => u.UserId.Equals(Id)).AnyAsync();

            if (valid == false)
            {
                invalidIds.Add(Id);
            }
            else
            {
                await db.Enrollments.AddAsync(new Classroom.Enrollment { ClassroomId = classId, StudentId = Id });
            }
        }

        await db.SaveChangesAsync();
        return new EnrollResult(StudentIds.Count - invalidIds.Count, invalidIds.Count, invalidIds);

    }

    public async Task<Response> CreateAsync(ClassroomDto classDto)
    {

        var response = new Response();

        if (classDto.Name == string.Empty)
        {
            response.Success = false;
            response.Message = "Classname can't be empty. Please provide classname";
            return response;
        }

        var classroom = new Classroom.Classroom
        {
            Id = Utilities.GenerateClassId(),
            Name = classDto.Name,
            CreatedAt = DateTime.Now,
            CreatedBy = classDto.TeacherId,
            TeacherId = classDto.TeacherId
        };

        try
        {

            await db.Classrooms.AddAsync(classroom);
            await db.SaveChangesAsync();
            response.Success = true;
            response.Message = "Classroom created successfully!";
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public Task<bool> DeleteAsync(string classId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Classroom.Classroom>> GetAllAsync(string Id)
    {
        var result = await db.Classrooms.Where(c => c.TeacherId == Id).ToListAsync();
        return result;
    }

    public Task<ClassroomDto> GetAsync(string classId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(ClassroomDto classDto)
    {
        throw new NotImplementedException();
    }

    public async Task<Response> AddSubjectItemAsync(string classroomId, SubjectItemDto subjectItemDto)
    {
        var result = new Response();
        try
        {

            var subjectItem = new SubjectItem
            {
                ClassroomId = classroomId,
                SubjectId = subjectItemDto.SubjectId,
                MaxScore = subjectItemDto.MaxScore,
                PassingScore = subjectItemDto.PassingScore
            };

            await db.SubjectItems.AddAsync(subjectItem);
            await db.SaveChangesAsync();
            result.Success = true;
            result.Message = "Subject Item Created Successfully!";
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = ex.Message;
        }
        return result;
    }

    public async Task<Response> GetSubjectItemsAsync(string classId)
    {
        var response = new Response();
        var result = await db.SubjectItems
            .Where(si => si.ClassroomId == classId)
            .Select(si => new
            {
                SubjectItemId = si.Id,
                SubjectId = ((SubjectType)si.SubjectId).ToString(),
                si.MaxScore,
                si.PassingScore
            })
            .ToListAsync();

        response.Success = true;
        response.Payload = result;
        return response;
    }

    public async Task<Response> GetSubjectsAsync()
    {
        var response = new Response();
        var result = await db.Subjects.Select(s => new
        {
            SubjectId = s.Id,
            SubjectName = s.Name.ToString()
        }).ToListAsync();

        response.Success = true;
        response.Payload = result;
        return response;
    }

    public async Task<Response> GetEnrollStudentAsync(string classId)
    {
        var response = new Response();
        var students = await db.Enrollments.Where(e => e.ClassroomId == classId).ToListAsync();

        var enrollStudents = new List<object>();
        foreach (var student in students)
        {
            var user = await db.Users
                .Where(s => s.UserId == student.StudentId)
                .Select(s => new
                {
                    StudentId = s.UserId,
                    Name = s.Firstname + " " + s.Lastname,
                })
                .FirstOrDefaultAsync();
            if (user != null)
            {
                enrollStudents.Add(user);
            }
        }

        response.Success = true;
        response.Payload = enrollStudents;
        return response;
    }

    async public Task<Response> GetMonthsAsync()
    {
        var response = new Response();

        var months = await db.Months
            .Select(m => new
            {
                Id = m.Id,
                Value = ((MonthOfYear)m.Value).ToString()
            })
            .ToListAsync();
        response.Success = true;
        response.Payload = months;
        return response;
    }

}

public record ClassroomDto
(
    string? Id,
    string Name,
    string TeacherId,
    string? TeacherName
);

public record EnrollResult
(
    int Success,
    int Failed,
    List<string> InvalidIds
);


public record SubjectItemDto
(
    int SubjectId,
    float MaxScore,
    float PassingScore
);