using System.ComponentModel.DataAnnotations;
using eRe.Dto;
using eRe.Repository;
using Microsoft.AspNetCore.Mvc;

namespace eRe;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/register", async (IUserRepostory service, UserDto userDto) =>
        {
            var result = new Response();
            result = await service.CreateAsync(userDto);
            return result.Success == true ? Results.Ok(result) : Results.BadRequest(result);
        });
        app.MapPost("/login", async (IUserRepostory service, UserLoginData data) =>
        {
            var result = new Response();

            try
            {
                result = await service.Login(data);
            }
            catch
            {
                throw;
            }

            return result.Success == true ? Results.Ok(result) : Results.BadRequest(result);
        });

        app.MapGet("/me", async (IUserRepostory service, string userId) =>
        {
            var result = new GetUserResponse();
            try
            {
                result = await service.GetByUserIdAsync(userId);
            }
            catch
            {
                throw;
            }
            return Results.Ok(result);
        });
    }
}

public record UserLoginData(string email, string password);
