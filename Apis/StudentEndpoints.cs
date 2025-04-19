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
        app.MapPost("/enroll", [Authorize] async (IStudentRepository service, EnrollmentValidator validator, List<EnrollmentDto> requests) => {
            var result = new Response();
            try {
                result = await service.EnrollCourse(requests);
            } catch (Exception ex) {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result.Success == true ? Results.Ok(result) : Results.BadRequest(result);
        });
        
    }
}   