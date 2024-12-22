using System.Security.Cryptography;
using System.Text;
using PassesProject.Data.Models;

namespace PassesProject.Utils;

public class EncryptionHelper
{
    public string GetHashedPasswordAndSalt(string password, string salt)
    {
        using (SHA512 algorithm = SHA512.Create())
        {
            byte[] hashedBytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }
    
    public User EncryptPassword(User user)
    {
        user.Salt = GenerateSalt();
        string hashedPasswordAndSalt = GetHashedPasswordAndSalt(user.Password, user.Salt);
        user.Password = hashedPasswordAndSalt;
        return user;
    }

    private string GenerateSalt()
    {
        byte[] salt = new byte[8];
        using (RandomNumberGenerator random = RandomNumberGenerator.Create())
        {
            random.GetBytes(salt);
        }

        return BitConverter.ToString(salt).Replace("-", "").ToLower();
    }
}