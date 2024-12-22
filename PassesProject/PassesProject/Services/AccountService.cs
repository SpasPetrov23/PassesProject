using PassesProject.Controllers.Dtos;
using PassesProject.Data;
using PassesProject.Data.Models;
using PassesProject.Utils;

namespace PassesProject.Services;

public class AccountService
{
    private readonly IConfiguration _config;
    private readonly TokenProvider _tokenProvider;
    private readonly EncryptionHelper _encryptionHelper;
    private readonly PassesDbContext _dbContext;

    public AccountService(IConfiguration config, EncryptionHelper encryptionHelper, PassesDbContext dbContext)
    {
        _config = config;
        _tokenProvider = new(_config);
        _encryptionHelper = encryptionHelper;
        _dbContext = dbContext;
    }
    
    public string Login(UserLoginRequest userLoginInfo)
    {
        User user = Authenticate(userLoginInfo.Email, userLoginInfo.Password);
        
        string jwtToken = _tokenProvider.GenerateJwtToken(user);

        return jwtToken;
    }
    
    private User Authenticate(string email, string password)
    {
        User? user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
        
        if (user == null)
        {
            throw new AppException(ErrorMessages.INVALID_EMAIL);
        }

        string hashedPasswordAndSalt = _encryptionHelper.GetHashedPasswordAndSalt(password, user.Salt);

        if (user.Password != hashedPasswordAndSalt)
        {
            throw new AppException(ErrorMessages.INVALID_PASSWORD);
        }

        return user;
    }
}