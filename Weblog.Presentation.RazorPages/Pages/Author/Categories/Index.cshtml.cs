using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Weblog.Domain.Core.CategoryAgg.Contracts.Service;
using Weblog.Domain.Core.CategoryAgg.Dtos;
using Weblog.Infra.Db.SqlServer.EfCore;

namespace Weblog.Presentation.RazorPages.Pages.Author.Categories
{
    [Authorize]
    public class IndexModel(ICategoryService _categoryService,
                            UserManager<ApplicationUser> _userManager) : PageModel
    {
        public List<CategoryDto> Categories { get; set; } = new();

        [TempData]
        public string StatusMessage { get; set; }

        public IActionResult OnGet()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId)) return Challenge();

            Categories = _categoryService.GetByUserId(userId);
            return Page();
        }

        public IActionResult OnPostDelete(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId)) return Challenge();

            try
            {
                _categoryService.Delete(id, userId);
                StatusMessage = "دسته‌بندی حذف شد.";
            }
            catch (Exception ex)
            {
                StatusMessage = ex.Message;
            }

            return RedirectToPage();
        }
    }
}