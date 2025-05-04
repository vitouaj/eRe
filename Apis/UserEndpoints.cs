using ERE.DTO;
using ERE.Repository;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using ERE.Utilities;
using ERE.Validators;
using FluentValidation;
using System.IdentityModel.Tokens.Jwt;

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
            return result.Success == true ? Results.Ok(result) : Results.BadRequest(result);
        });

        app.MapGet("/me", [Authorize] async (IUserRepostory service, ClaimsPrincipal user) => {
            var identifier = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(identifier)) {
                return Results.Unauthorized();
            }
            
            Response response = await service.GetUser(identifier);
            if (response.Success == false) {
                return Results.NotFound(response);
            }
            return Results.Ok(response);
        });

        app.MapGet("/static-options", async (IUserRepostory service) => {
            Response response = service.GetStaticOptions();
            if (response.Success == false) {
                return Results.NotFound(response);
            }
            return Results.Ok(response);
        });

        app.MapPatch("/me", [Authorize] async (IUserRepostory service, ClaimsPrincipal user, UpdateUserDto request) => {
            var identifier = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(identifier)) {
                return Results.Unauthorized();
            }
            Response response = await service.UpdateUser(identifier, request);
            if (response.Success == false) {
                return Results.NotFound(response);
            }
            return Results.Ok(response);
        });
    }
}