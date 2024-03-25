using System.Reflection.Metadata.Ecma335;
using eRe.Repository;

namespace eRe;

public static class ClassroomEndpoints
{
    public static void MapClassroomEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (IClassroomRepository service, ClassroomDto classDto) =>
        {
            var result = await service.CreateAsync(classDto);
            return result;
        });

        app.MapPost("/{classroomId}/students", async (IClassroomRepository service, string classroomId, List<string> studentIds) =>
        {
            var result = await service.EnrollStudents(classroomId, studentIds);
            return result;
        });

        app.MapPost("/{classroomId}/subjectItem", async (IClassroomRepository service, string classroomId, SubjectItemDto dto) =>
        {
            var result = await service.AddSubjectItemAsync(classroomId, dto);
            return result;
        });

        app.MapPost("/{teacherId}/report", async (IReportRepository service, string teacherId, ReportDto reportDto) =>
        {
            var result = await service.CreateAsync(teacherId, reportDto);
            return result;
        });

        app.MapGet("/{classroomId}", async (IReportRepository service, string classroomId) =>
        {
            return await service.GetAllAsync(classroomId);
        });
    }

}
