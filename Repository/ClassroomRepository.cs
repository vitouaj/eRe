using eRe.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Npgsql.Internal;

namespace eRe.Repository;

public interface IClassroomRepository
{
    Task<object> GetAllAsync();
    Task<bool> CreateAsync(ClassroomDto classDto);
    Task<bool> UpdateAsync(ClassroomDto classDto);
    Task<bool> DeleteAsync(string classId);
    Task<ClassroomDto> GetAsync(string classId);


    /*
        Add Student to Classroom
    */
    Task<bool> AddSubjectItemAsync(string classroomId, SubjectItemDto subjectItemDto);
    Task<object> GetSubjectItemsAsync(string classId);
    Task<EnrollResult> EnrollStudents(string classId, List<string> StudentIds);

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
                throw new Exception($" Invalid student Id {Id}");
            }
            else
            {
                await db.Enrollments.AddAsync(new Classroom.Enrollment { ClassroomId = classId, StudentId = Id });
            }
        }

        await db.SaveChangesAsync();
        return new EnrollResult(StudentIds.Count - invalidIds.Count, invalidIds.Count, invalidIds);

    }

    public async Task<bool> CreateAsync(ClassroomDto classDto)
    {

        var classroom = new Classroom.Classroom
        {
            Id = Utilities.GenerateClassId(),
            Name = classDto.Name,
            CreatedAt = DateTime.Now,
            CreatedBy = classDto.TeacherId,
            TeacherId = classDto.TeacherId
        };

        bool result = false;
        try
        {

            await db.Classrooms.AddAsync(classroom);
            await db.SaveChangesAsync();
            result = true;
        }
        catch
        {
            throw;
        }
        return result;
    }

    public Task<bool> DeleteAsync(string classId)
    {
        throw new NotImplementedException();
    }

    public async Task<object> GetAllAsync()
    {
        return await db.Classrooms.ToListAsync();
    }

    public Task<ClassroomDto> GetAsync(string classId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(ClassroomDto classDto)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> AddSubjectItemAsync(string classroomId, SubjectItemDto subjectItemDto)
    {
        bool result;
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
            result = true;
        }
        catch
        {
            throw;
        }
        return result;
    }

    public async Task<object> GetSubjectItemsAsync(string classId)
    {
        return await db.SubjectItems.FirstOrDefaultAsync(si => si.ClassroomId.Equals(classId));
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