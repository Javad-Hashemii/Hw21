using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Weblog.Domain.Core.CategoryAgg.Contracts.Service;
using Weblog.Domain.Core.CategoryAgg.Dtos;
using Weblog.Domain.Core.PostAgg.Contracts.Service;
using Weblog.Domain.Core.PostAgg.Dtos;
using Weblog.Infra.Db.SqlServer.EfCore;

namespace Weblog.Presentation.RazorPages.Pages.Author.Posts
{
    [Authorize]
    public class EditModel(ICategoryService _categoryService,
                           IBlogPostService _blogPostService,
                           UserManager<ApplicationUser> _userManager) : PageModel
    {
        public List<CategoryDto> Categories { get; set; } = new();

        [BindProperty]
        public EditPostInput Input { get; set; } = new();

        public string ExistingCover { get; set; }

        public IActionResult OnGet(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId))
                return Challenge();

            var post = _blogPostService.GetById(id);
            if (post == null || post.AuthorId != userId)
                return NotFound();

            Categories = _categoryService.GetByUserId(userId);

            Input = new EditPostInput
            {
                Id = post.Id,
                Title = post.Title,
                Text = post.Text,
                CategoryId = post.CategoryId,
                PublishedDate = post.PublishedDate
            };

            ExistingCover = post.CoverImageUrl;

            return Page();
        }

        public IActionResult OnPost()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId))
                return Challenge();

            Categories = _categoryService.GetByUserId(userId);

            if (Input.ImageFile != null && !IsValidImage(Input.ImageFile))
            {
                ModelState.AddModelError(nameof(Input.ImageFile), "فقط تصاویر PNG یا JPG با حداکثر حجم 2 مگابایت مجاز است.");
            }

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
                _blogPostService.Update(dto);
                TempData["StatusMessage"] = "پست به‌روزرسانی شد.";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

        private static bool IsValidImage(IFormFile file)
        {
            var allowedExtensions = new[] { ".png", ".jpg", ".jpeg" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return file.Length <= 2 * 1024 * 1024 && allowedExtensions.Contains(extension);
        }

        public class EditPostInput
        {
            [Required]
            public int Id { get; set; }

            [Required(ErrorMessage = "عنوان پست را وارد کنید.")]
            [StringLength(150, ErrorMessage = "حداکثر ۱۵۰ کاراکتر مجاز است.")]
            public string Title { get; set; }

            [Required(ErrorMessage = "متن پست الزامی است.")]
            public string Text { get; set; }

            [Required]
            public int CategoryId { get; set; }

            public DateTime? PublishedDate { get; set; }

            public IFormFile ImageFile { get; set; }
        }
    }
}

