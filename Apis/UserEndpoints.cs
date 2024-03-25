using eRe.Repository;

namespace eRe;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/register", async (IUserRepostory service, UserDto userDto) =>
        {
            bool result;
            try
            {
                result = await service.CreateAsync(userDto);
            }
            catch
            {
                throw;
            }
            return result;
        });
    }
}
