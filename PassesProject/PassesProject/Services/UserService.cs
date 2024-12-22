using Microsoft.EntityFrameworkCore;
using PassesProject.Controllers.Dtos;
using PassesProject.Data;
using PassesProject.Data.Models;
using PassesProject.Utils;

namespace PassesProject.Services;

public class UserService
{
    private readonly PassesDbContext _dbContext;
    private readonly EncryptionHelper _encryptionHelper;

    public UserService(PassesDbContext dbContext, EncryptionHelper encryptionHelper)
    {
        _dbContext = dbContext;
        _encryptionHelper = encryptionHelper;
    }

    public async Task<UserUpdateDto> GetUser(int userId)
    {
        User? dbUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (dbUser == null)
        {
            throw new AppException(ErrorMessages.USER_NOT_FOUND);
        }

        UserUpdateDto result = new()
        {
            Name = dbUser.Name,
            Avatar = dbUser.Avatar,
            WebsiteUrl = dbUser.WebsiteUrl,
            Email = dbUser.Email
        };

        return result;
    }

    public async Task<bool> CreateUser(UserCreateDto inputUser)
    {
        if (inputUser == null)
        {
            throw new AppException(ErrorMessages.REQUEST_CANNOT_BE_NULL);
        }

        if (_dbContext.Users.Any(u => u.Email == inputUser.Email))
        {
            throw new AppException(ErrorMessages.USERNAME_IS_ALREADY_USED);
        }

        User newUser = new()
        {
            Name = inputUser.Name,
            Avatar = inputUser.AvatarUrl,
            WebsiteUrl = inputUser.WebsiteUrl,
            Email = inputUser.Email,
            Password = inputUser.Password
        };
        
        newUser = _encryptionHelper.EncryptPassword(newUser);

        _dbContext.Users.Add(newUser);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<UserUpdateDto> UpdateUser(int userId, UserUpdateDto inputUser)
    {
        User? dbUser = _dbContext.Users.FirstOrDefault(u => u.Id == userId);

        if (dbUser == null)
        {
            throw new AppException(ErrorMessages.USER_NOT_FOUND);
        }

        dbUser.Name = inputUser.Name;
        dbUser.Email = inputUser.Email;
        dbUser.Avatar = inputUser.Avatar;
        dbUser.WebsiteUrl = inputUser.WebsiteUrl;

        await _dbContext.SaveChangesAsync();

        return inputUser;
    }
}