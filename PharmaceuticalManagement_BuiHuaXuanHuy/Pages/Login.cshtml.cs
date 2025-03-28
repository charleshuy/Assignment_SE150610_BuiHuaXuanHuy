using BO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Repo;

namespace FUNewsManagement_BuiHuaXuanHuy.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public LoginModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string txtEmail = Request.Form["txtEmail"];
            string txtPassword = Request.Form["txtPassword"];

            var user = _unitOfWork.GetRepository<SystemAccount>()
                                  .Entities
                                  .FirstOrDefault(u => u.AccountEmail == txtEmail && u.AccountPassword == txtPassword);

            if (user == null)
            {
                return RedirectToPage("./Error");
            }

            // Create Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.AccountEmail),
                new Claim(ClaimTypes.NameIdentifier, user.AccountId.ToString()),
                new Claim(ClaimTypes.Role, user.AccountRole.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Sign In User
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            // Set Session Variables
            HttpContext.Session.SetString("Email", txtEmail);
            HttpContext.Session.SetString("Role", user.AccountRole.ToString());

            return RedirectToPage("./Index");
        }
    }
}
