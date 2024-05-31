using eRe.Dto;
using eRe.Infrastructure;
using eRe.User;
using Microsoft.EntityFrameworkCore;
using Npgsql.Replication;

namespace eRe.Repository;

public interface IUserRepostory
{
    Task<Response> Login(UserLoginData data);
    Task<object> GetAllAsync(); // development only
    Task<UserDto?> GetAsync(string userId);
    Task<Response> CreateAsync(UserDto userDto);
    Task<bool> UpdateAsync(string userId);
    Task<bool> DeleteAsync(string userId);
    Task<GetUserResponse> GetByUserIdAsync(string userId);

}
public class UserRepository(AppDbContext context) : IUserRepostory
{
    private readonly AppDbContext db = context;
    public async Task<Response> CreateAsync(UserDto userDto)
    {
        var response = new Response();
        var email = userDto.Email;
        var result = await db.Users.FirstOrDefaultAsync(x => x.Email == email);

        if (result != null)
        {
            response.Success = false;
            response.Message = "User already exists";
            return response;
        }


        // check if email already exist
        var user = new User.User
        {
            UserId = Utilities.GenerateUserId(),
            Firstname = userDto.Firstname,
            Lastname = userDto.Lastname,
            Email = userDto.Email,
            Password = userDto.Password,
            Phone = userDto.Phone,
            OtherContact = userDto.OtherContact,
            ProfileId = userDto.ProfileId,
            RoleId = (UserRole)userDto.RoleId
        };

        try
        {
            await db.Users.AddAsync(user);
            if ((UserRole)userDto.RoleId == UserRole.STUDENT)
            {
                await db.Students.AddAsync(new Classroom.Student { StudentId = user.UserId });
            }
            await db.SaveChangesAsync();

            response.Success = true;
            response.Message = "User created successfully!";
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public Task<bool> DeleteAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<object> GetAllAsync()
    {
        return await db.Users.ToListAsync();
    }

    public Task<bool> UpdateAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto?> GetAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<Response> Login(UserLoginData data)
    {
        var user = await db.Users.Where(u => u.Email == data.email && u.Password == data.password).FirstOrDefaultAsync();

        return user != null ? new Response(true, user.UserId, "Login Successfull") : new Response(false, null, "Incorrect Login");
    }

    public async Task<GetUserResponse> GetByUserIdAsync(string userId)
    {
        var result = await db.Users.Where(u => u.UserId == userId).FirstOrDefaultAsync();


        var response = new GetUserResponse
        {
            UserID = result.UserId,
            Username = result.Firstname + " " + result.Lastname,
            Email = result.Email,
            Phonenumber = result.Phone,
            ParentEmail = result.OtherContact,
            Role = result.RoleId.ToString(),
        };

        return response;
    }
}

public record UserDto
(
    string? UserId,
    string Firstname,
    string Lastname,
    string Email,
    string Password,
    string Phone,
    string OtherContact,
    string? ProfileId,
    int? RoleId
);