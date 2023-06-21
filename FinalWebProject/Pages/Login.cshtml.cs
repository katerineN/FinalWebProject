using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FinalWebProject.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public LoginInputModel Input { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var httpClient = _httpClientFactory.CreateClient();

            var formValues = new Dictionary<string, string>
            {
                { "username", Input.Username },
                { "password", Input.Password },
                { "token", Input.Token } // Добавляем токен в значения формы
            };

            var loginResponse = await httpClient.PostAsync("/token", new FormUrlEncodedContent(formValues));
            
            if (loginResponse.IsSuccessStatusCode)
            {
                // Аутентификация успешна
                return RedirectToPage("/Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid username, password, or token.");
                return Page();
            }
        }

        public class LoginInputModel
        {
            [Required]
            public string Username { get; set; }

            [Required]
            public string Password { get; set; }

            [Required]
            public string Token { get; set; }
        }
    }
}