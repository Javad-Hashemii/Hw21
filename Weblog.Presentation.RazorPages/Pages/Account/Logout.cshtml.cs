using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Weblog.Infra.Db.SqlServer.EfCore;

namespace Weblog.Presentation.RazorPages.Pages.Account
{
    public class LogoutModel(SignInManager<ApplicationUser> _signInManager) : PageModel
    {
        public async Task<IActionResult> OnPostAsync()
        {
            await _signInManager.SignOutAsync();

            // Redirect to Home page after logout
            return RedirectToPage("/Index");
        }
    }
}
