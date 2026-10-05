using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace DarkContinent.Helpers;


public class HashPassword
{
    const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_-+=<>?";

    public static byte[] GetBytes(int length)
    {
        var bytes = new byte[length];
        using (var random = RandomNumberGenerator.Create())
        {
            random.GetBytes(bytes);
        }
        return bytes;
    }

    public static string GetPassRandom(int length)
    {
        StringBuilder pass = new StringBuilder();
        for (int i = 0; i < length; i++)
        {
            Random random = new Random();
            int index = random.Next(chars.Length);
            pass.Append(chars[index]);
        }
        return pass.ToString();
    }


    public static string HashPass(string password)
    {

        byte[] salt = GetBytes(9);

        string passHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 30000,
            numBytesRequested: 32));
        return Convert.ToBase64String(salt) + "|" + passHash;
    }

    public static bool VerifyPass(string password, string hashedPassword)
    {
        var parts = hashedPassword.Split('|');
        if (parts.Length != 2)
        {
            return false;
        }

        var salt = Convert.FromBase64String(parts[0]);
        var storedHash = parts[1];
        byte[] hash = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 30000,
            numBytesRequested: 32
            );

        string passHash = Convert.ToBase64String(hash);

        return passHash == storedHash;
    }

}