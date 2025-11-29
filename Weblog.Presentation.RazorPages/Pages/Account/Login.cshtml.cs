using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Weblog.Infra.Db.SqlServer.EfCore;
using Weblog.Presentation.RazorPages.ViewModels;

namespace Weblog.Presentation.RazorPages.Pages.Account
{
    public class LoginModel(SignInManager<ApplicationUser> _signInManager,UserManager<ApplicationUser> _userManager) : PageModel
    {


        [BindProperty]
        public LoginViewModel Input { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                // Reusing your logic: PasswordSignInAsync
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToPage("/Author/Index");
                }
                else
                {
                    ModelState.AddModelError("", "Email or password is incorrect.");
                    return Page();
                }
            }
            return Page();
        }
    }
}
