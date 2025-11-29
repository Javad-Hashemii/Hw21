using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Weblog.Domain.Core.CategoryAgg.Contracts.Service;
using Weblog.Domain.Core.CategoryAgg.Dtos;
using Weblog.Infra.Db.SqlServer.EfCore;

namespace Weblog.Presentation.RazorPages.Pages.Author.Categories
{
    [Authorize]
    public class CreateModel(ICategoryService _categoryService,
                             UserManager<ApplicationUser> _userManager) : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; } = new();

        public void OnGet() { }

        public IActionResult OnPost()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId)) return Challenge();

            if (!ModelState.IsValid) return Page();

            try
            {
                var dto = new CreateCategoryDto { Name = Input.Name, UserId = userId };
                _categoryService.Create(dto);
                TempData["StatusMessage"] = "دسته‌بندی با موفقیت ایجاد شد.";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

        public class InputModel
        {
            [Required(ErrorMessage = "نام دسته‌بندی الزامی است")]
            [StringLength(50)]
            [Display(Name = "نام دسته‌بندی")]
            public string Name { get; set; }
        }
    }
}