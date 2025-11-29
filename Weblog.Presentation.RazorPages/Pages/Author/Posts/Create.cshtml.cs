using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Weblog.Domain.Core.CategoryAgg.Contracts.AppService;
using Weblog.Domain.Core.CategoryAgg.Dtos;
using Weblog.Domain.Core.PostAgg.Contracts.AppService;
using Weblog.Domain.Core.PostAgg.Dtos;
using Weblog.Infra.Db.SqlServer.EfCore;
using Weblog.Presentation.RazorPages.ViewModels;

namespace Weblog.Presentation.RazorPages.Pages.Author.Posts
{
    [Authorize]
    public class CreateModel(ICategoryAppService _categoryAppService, IBlogAppService _blogAppService, UserManager<ApplicationUser> _userManager) : PageModel
    {
        public List<CategoryDto> Categories { get; set; } = new();

        [BindProperty]
        public CreatePostViewModel Input { get; set; } = new();

        [TempData]
        public string StatusMessage { get; set; }

        public IActionResult OnGet()
        {
            var userId = _userManager.GetUserId(User);

            Categories = _categoryAppService.GetCategoryByUserId(userId);
            if (!Categories.Any())
            {
                StatusMessage = "ابتدا حداقل یک دسته‌بندی ایجاد کنید.";
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            Categories = _categoryAppService.GetCategoryByUserId(userId);
            if (!Categories.Any())
            {
                ModelState.AddModelError(string.Empty, "ابتدا دسته‌بندی بسازید.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var dto = new CreatePostDto
            {
                Title = Input.Title,
                Text = Input.Text,
                CategoryId = Input.CategoryId,
                ImageFile = Input.ImageFile,
                AuthorId = userId
            };

            try
            {
                _blogAppService.Create(dto);
                StatusMessage = "پست با موفقیت ایجاد شد.";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }


    }
}

