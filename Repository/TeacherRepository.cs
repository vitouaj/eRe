using ERE.DTO;
using Microsoft.EntityFrameworkCore;
using ERE.Infrastructure;
using ERE.Models;
using ERE.CustomExceptions;

namespace ERE.Repository;
public interface ITeacherRepository
{
    Task<Response> CreateCourse(CreateCourseDto request);
}
public class TeacherRepository(AppDbContext context) : ITeacherRepository
{
    private readonly AppDbContext db = context;

    public async Task<Response> CreateCourse(CreateCourseDto request)
    {
        // check if teacher exists
        var response = new Response();
        var teacher = await db.Teachers.FirstOrDefaultAsync(t => t.Id == request.TeacherId);
        if (teacher == null)
        {
            throw new TeacherNotFoundException();
        }
        // check if course already exists
        var course = await db.Courses.FirstOrDefaultAsync(c => c.TeacherId == request.TeacherId && c.LevelId == request.Level);
        if (course != null)
        {
            throw new CourseAlreadyExistsException();
        }
        // create course
        var newCourse = new Course(teacher, request.Level);
        newCourse.MaxScore = request.MaxScore;
        newCourse.PassingRate = request.PassingRate;
        db.Courses.Add(newCourse);
        await db.SaveChangesAsync();

        response.Payload = newCourse;
        response.Message = "Course created successfully";
        response.Success = true;
        return response;

    }
}