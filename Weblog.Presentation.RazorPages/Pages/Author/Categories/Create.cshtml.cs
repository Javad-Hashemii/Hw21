using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Weblog.Domain.Core.CategoryAgg.Contracts.AppService;
using Weblog.Domain.Core.CategoryAgg.Dtos;
using Weblog.Infra.Db.SqlServer.EfCore;
using Weblog.Presentation.RazorPages.ViewModels;

namespace Weblog.Presentation.RazorPages.Pages.Author.Categories
{
    [Authorize]
    public class CreateModel(ICategoryAppService _categoryAppService,UserManager<ApplicationUser> _userManager) : PageModel
    {
        [BindProperty]
        public CreateCategoryViewModel Input { get; set; } = new();

        public IActionResult OnPost()
        {
            var userId = _userManager.GetUserId(User);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var dto = new CreateCategoryDto 
                {
                    Name = Input.Name,
                    UserId = userId 
                };
                _categoryAppService.CreateCategory(dto);
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}