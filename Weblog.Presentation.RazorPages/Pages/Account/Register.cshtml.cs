using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Weblog.Infra.Db.SqlServer.EfCore;
using Weblog.Presentation.RazorPages.ViewModels;

namespace Weblog.Presentation.RazorPages.Pages.Account
{
    public class RegisterModel(SignInManager<ApplicationUser> _signInManager,UserManager<ApplicationUser> _userManager) : PageModel
    {
        [BindProperty]
        public RegisterViewModel Input { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                // Reusing your logic
                var user = new ApplicationUser
                {
                    FullName = Input.Name, // Using the new property we added
                    Email = Input.Email,
                    UserName = Input.Email,
                };

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    // Redirect to Login as per your original controller logic
                    return RedirectToPage("./Login");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return Page();
                }
            }
            return Page();
        }
    }
}
