using ERE.DTO;
using ERE.Repository;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using ERE.Utilities;
using ERE.Validators;
using FluentValidation;

namespace ERE.APIS;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/register", async (IUserRepostory service, UserRegisterValidator validator, RegisterRequestDto request) => {
            // Validate the request
            var result = new Response();
            try {
                validator.ValidateAndThrow(request);
                result = await service.CreateUser(request);
            } catch (Exception ex) {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result.Success == true ? Results.Ok(result) : Results.BadRequest(result);
        });
        app.MapPost("/login", async (IUserRepostory service, UtilityService util, LoginRequestValidator validator, LoginRequestDto request) => {
            var result = new Response();
            try {
                validator.ValidateAndThrow(request);
                result = await service.Login(request);
            } catch (Exception ex) {
                result.Success = false;
                result.Message = ex.Message;
            }

            if (result.Success == true) {
                string token = util.GenerateJwtToken(request.Email);
                result.Payload = new { token };
            }
            return result.Success == true ? Results.Ok(result) : Results.BadRequest(result);
        });

        app.MapGet("/me", [Authorize] async (IUserRepostory service, ClaimsPrincipal user) => {
            return Results.Ok("Message");
        });
    }
}