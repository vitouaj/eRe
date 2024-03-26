using System.Reflection.Metadata.Ecma335;
using eRe.Repository;

namespace eRe;

public static class ClassroomEndpoints
{
    public static void MapClassroomEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (IClassroomRepository service, ClassroomDto classDto) =>
        {
            bool result;
            try {
                result = await service.CreateAsync(classDto);

            } catch 
            {
                throw;
            }
            return result;
        });

        app.MapPost("/{classroomId}/students", async (IClassroomRepository service, string classroomId, List<string> studentIds) =>
        {
            EnrollResult result;
            try 
            {
                result = await service.EnrollStudents(classroomId, studentIds);

            } catch 
            {
                throw;
            }
            return result;
        });

        app.MapPost("/{classroomId}/subjectItem", async (IClassroomRepository service, string classroomId, SubjectItemDto dto) =>
        {
            bool result;
            try
            {
                result = await service.AddSubjectItemAsync(classroomId, dto);
            } catch 
            {
                throw;
            }
            return result;
        });

        app.MapPost("/{teacherId}/report", async (IReportRepository service, string teacherId, ReportDto reportDto) =>
        {
            object result;
            try
            {
                result = await service.CreateAsync(teacherId, reportDto);
            } catch 
            {
                throw;
            }
            return result;
        });

        app.MapGet("/{classroomId}", async (IReportRepository service, string classroomId) =>
        {
            object result;
            try
            {
                result = await service.GetAllAsync(classroomId);
            } catch
            {
                throw;
            }
            return result;
        });
    }
    
}
