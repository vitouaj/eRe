using eRe.Infrastructure;
using eRe.User;
using Microsoft.EntityFrameworkCore;

namespace eRe.Repository;

public interface IUserRepostory
{
    Task<object> GetAllAsync(); // development only
    Task<UserDto?> GetAsync(string userId);
    Task<bool> CreateAsync(UserDto userDto);
    Task<bool> UpdateAsync(string userId);
    Task<bool> DeleteAsync(string userId);

}
public class UserRepository(AppDbContext context) : IUserRepostory
{
    private readonly AppDbContext db = context;

    public async Task<bool> CreateAsync(UserDto userDto)
    {
        bool result;
        // check if email already exist
        var user = new User.User
        {
            UserId = Utilities.GenerateUserId(),
            Firstname = userDto.Firstname,
            Lastname = userDto.Lastname,
            Email = userDto.Email,
            Phone = userDto.Phone,
            OtherContact = userDto.OtherContact,
            ProfileId = userDto.ProfileId,
            RoleId = (UserRole) userDto.RoleId
        };


        try
        {
            result = true;
            await db.Users.AddAsync(user);
            if ((UserRole)userDto.RoleId == UserRole.STUDENT)
            {
                await db.Students.AddAsync(new Classroom.Student { StudentId = user.UserId });
            }
            await db.SaveChangesAsync();
        }
        catch
        {
            throw;
        }

        return result;
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

}

public record UserDto
(
    string? UserId,
    string Firstname,
    string Lastname,
    string Email,
    string Phone,
    string OtherContact,
    string? ProfileId,
    int? RoleId
);
