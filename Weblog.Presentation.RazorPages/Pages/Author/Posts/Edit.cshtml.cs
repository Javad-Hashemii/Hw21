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
    public class EditModel(ICategoryAppService _categoryAppService, IBlogAppService _blogPostAppService, UserManager<ApplicationUser> _userManager) : PageModel
    {
        public List<CategoryDto> Categories { get; set; } = new();

        [BindProperty]
        public EditPostViewModel Input { get; set; } = new();

        public string ExistingCover { get; set; }

        public IActionResult OnGet(int id)
        {
            var userId = _userManager.GetUserId(User);

            var post = _blogPostAppService.GetById(id);
            if (post == null || post.AuthorId != userId)
            {
                return NotFound();
            }

            Categories = _categoryAppService.GetCategoryByUserId(userId);

            Input = new EditPostViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Text = post.Text,
                CategoryId = post.CategoryId,
                PublishedDate = post.PublishedDate
            };

            ExistingCover = post.ImageUrl;

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

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var dto = new UpdatePostDto
            {
                Id = Input.Id,
                Title = Input.Title,
                Text = Input.Text,
                CategoryId = Input.CategoryId,
                ImageFile = Input.ImageFile,
                AuthorId = userId,
                PublishedDate = Input.PublishedDate
            };

            try
            {
                _blogPostAppService.Update(dto);
                TempData["StatusMessage"] = "پست به‌روزرسانی شد.";
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

