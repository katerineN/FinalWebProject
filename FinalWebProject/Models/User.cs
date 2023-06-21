using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace FinalWebProject.Models;

public class User
{
    [Required] public string Login { get; set; }
    [Required] public string Password { get; set; }
    [Required] public string Role { get; set; }
}

public class AuthOptions
{
    public const string ISSUER = "MyAuthServer"; // издатель токена
    public const string AUDIENCE = "MyAuthClient"; // потребитель токена
    const string KEY = "mysupersecret_secretkey!123"; // ключ для шифрации
    public const int LIFETIME = 1; // время жизни токена - 1 минута

    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }
}