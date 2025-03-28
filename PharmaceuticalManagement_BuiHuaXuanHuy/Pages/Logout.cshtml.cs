using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagement_BuiHuaXuanHuy.Pages
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnPostAsync()
        {
            // Clear user claims and authentication
            await HttpContext.SignOutAsync();

            // Clear session data
            HttpContext.Session.Clear();

            // Redirect to Index page after logout
            return RedirectToPage("/Index");
        }
    }
}
