using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Weblog.Infra.Db.SqlServer.EfCore;
using Weblog.Presentation.RazorPages.ViewModels;

namespace Weblog.Presentation.RazorPages.Pages.Account
{
    public class LoginModel(SignInManager<ApplicationUser> _signInManager) : PageModel
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
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToPage("/Author/Index");
                }
                else
                {
                    ModelState.AddModelError("", "ایمیل یا پسورد اشتباه است");
                    return Page();
                }
            }
            return Page();
        }
    }
}
