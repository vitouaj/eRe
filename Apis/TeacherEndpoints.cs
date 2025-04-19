using ERE.DTO;
using ERE.Repository;
using ERE.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;

namespace ERE.APIS;

public static class TeacherEndpoints
{
    public static void MapTeacherEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/course", [Authorize] async (ITeacherRepository service, CreateCourseValidator validator, CreateCourseDto request) => {
            // Validate the request
            var result = new Response();
            try {
                validator.ValidateAndThrow(request);
                result = await service.CreateCourse(request);
            } catch (Exception ex) {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result.Success == true ? Results.Ok(result) : Results.BadRequest(result);
        });
        
    }
}