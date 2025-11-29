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
    public class EditModel(ICategoryAppService _categoryAppService, UserManager<ApplicationUser> _userManager) : PageModel
    {
        [BindProperty]
        public EditCategoryViewModel Input { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            var userId = _userManager.GetUserId(User);

            var category = _categoryAppService.GetCategoryById(id);

            if (category == null || category.OwnerId != userId)
            {
                return NotFound();
            }

            Input = new EditCategoryViewModel
            {
                Id = category.Id,
                Name = category.Name
            };
            return Page();
        }

        public IActionResult OnPost()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var dto = new CreateCategoryDto
                {
                    Id = Input.Id,
                    Name = Input.Name,
                    UserId = userId
                };

                _categoryAppService.Update(dto);
                TempData["StatusMessage"] = "دسته بندی ویرایش شد";
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