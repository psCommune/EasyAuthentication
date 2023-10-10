using EasyAuthentication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace EasyAuthentication.Controllers
{
    public class HomeController :Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController (ILogger<HomeController> logger)
        {
            _logger = logger;
            
        }

        public IActionResult Index ()
        {
            return View();
        }

        [Authorize]
        public IActionResult Privacy ()
        {
            return View();
        }

        public async Task<IActionResult> Login (string username, string password)
        {
            // список утверждений о пользователе
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, "User"),
                new Claim("TestValue", "Test")
            };

            // создаем "паспорт"
            ClaimsIdentity identity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme, // название схемы
                ClaimTypes.Name, // название Claim, в котором записано имя
                ClaimTypes.Role // название Claim, в котором записана роль
             );
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            if (username == "admin" && password == "admin"){
                // вход в систему: генерация и отправка cookie 
                await HttpContext.SignInAsync(principal);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public async Task<IActionResult> Logout ()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error ()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}