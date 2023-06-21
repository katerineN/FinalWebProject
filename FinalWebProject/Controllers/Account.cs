using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FinalWebProject.Data;
using FinalWebProject.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace FinalWebProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IDataProtector _protector;
        private readonly AuthContext _context;

        public AccountController(ILogger<AccountController> logger, IDataProtectionProvider provider,
            AuthContext context)
        {
            _logger = logger;
            _protector = provider.CreateProtector("FinalWebProject.Auth.v1");
            _context = context;
        }

        [HttpPost("/token")]
        public IActionResult Token(string username, string password, string token)
        {
            string unprotectedToken = _protector.Unprotect(token);

            if (!IsValidToken(unprotectedToken))
            {
                return BadRequest(new { errorText = "Invalid token." });
            }

            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            return Content(JsonConvert.SerializeObject(response), "application/json");
        }

        [Authorize]
        [HttpGet("/logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Ok();
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            var person =
                _context.users.FirstOrDefault(x => Decrypt(x.Login) == username && Decrypt(x.Password) == password);
            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, Decrypt(person.Login)),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role)
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                return claimsIdentity;
            }

            // Если пользователя не найдено
            return null;
        }

        private bool IsValidToken(string token)
        {
            // Дополнительная проверка токена, например, в базе данных или внешнем сервисе
            // Здесь реализуйте проверку валидности токена по вашим требованиям
            // В данном примере всегда считаем токен валидным
            return true;
        }

        public string Encrypt(string plaintext)
        {
            return _protector.Protect(plaintext);
        }

        public string Decrypt(string ciphertext)
        {
            return _protector.Unprotect(ciphertext);
        }
    }
}