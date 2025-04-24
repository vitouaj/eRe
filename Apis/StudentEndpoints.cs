using System.Security.Claims;
using ERE.DTO;
using ERE.Repository;
using ERE.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;

namespace ERE.APIS;

public static class StudentEndpoints
{
    public static void MapStudentEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/enroll", [Authorize] async (IStudentRepository service, ClaimsPrincipal user, EnrollmentValidator validator, EnrollmentDto request) => {
            var identifier = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var studentId = service.GetStudentId(identifier);
            if (string.IsNullOrEmpty(identifier)) {
                return Results.Unauthorized();
            }    
            var result = new Response();
            var temps = new List<DTO.EnrollmentDto>();
            try {
                foreach (var courseId in request.CourseIds) {
                    var enrollment = new DTO.EnrollmentDto() {
                        StudentId = studentId,
                        CourseId = courseId
                    };
                    temps.Add(enrollment);
                    // validator.ValidateAndThrow(request);
                }
                result = await service.EnrollCourse(temps);
            } catch (Exception ex) {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result.Success == true ? Results.Ok(result) : Results.BadRequest(result);
        });

        app.MapPatch("/comment", [Authorize] async (IStudentRepository service, ClaimsPrincipal user, ProvideFeedbackDto request) => {
            var identifier = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(identifier)) {
                return Results.Unauthorized();
            }    
            var result = new Response();
            try {
                result = await service.Feedback(request);
            } catch (Exception ex) {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result.Success == true ? Results.Ok(result) : Results.BadRequest(result);
        });
        
    }
    public class EnrollmentDto {
        public List<string> CourseIds { get; set; } = new List<string>();
    }
}
