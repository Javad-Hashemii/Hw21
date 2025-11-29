using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Weblog.Domain.Core.PostAgg.Contracts.Service;
using Weblog.Domain.Core.PostAgg.Dtos;

namespace Weblog.Presentation.RazorPages.Pages.Blog
{
    public class DetailsModel(IBlogPostService _blogPostService, ICommentService _commentService) : PageModel
    {
        public ShowPostDto Post { get; set; }
        public List<ShowCommentDto> ApprovedComments { get; set; } = new();

        [BindProperty]
        public CommentInput Input { get; set; } = new();

        [TempData]
        public string StatusMessage { get; set; }

        public IActionResult OnGet(int id)
        {
            if (!LoadData(id))
                return NotFound();

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            if (!LoadPost(id))
                return NotFound();

            if (!User.Identity.IsAuthenticated)
            {
                // guests must provide name + email
                if (string.IsNullOrWhiteSpace(Input.Name))
                    ModelState.AddModelError(nameof(Input.Name), "نام را وارد کنید.");
                if (string.IsNullOrWhiteSpace(Input.Email))
                    ModelState.AddModelError(nameof(Input.Email), "ایمیل را وارد کنید.");
            }

            if (!ModelState.IsValid)
            {
                LoadComments(id);
                return Page();
            }

            var dto = new AddCommentDto
            {
                BlogPostId = id,
                Text = Input.Message,
                Rating = Input.Rating,
                GuestName = Input.Name,
                GuestEmail = Input.Email
            };

            var userId = User.Identity.IsAuthenticated ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;
            var userName = User.Identity.IsAuthenticated ? User.Identity?.Name : null;

            try
            {
                _commentService.AddComment(dto, userId, userName);
                StatusMessage = "کامنت شما پس از تایید نمایش داده خواهد شد.";
                return RedirectToPage(new { id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                LoadComments(id);
                return Page();
            }
        }

        private bool LoadData(int id)
        {
            if (!LoadPost(id))
                return false;

            LoadComments(id);
            return true;
        }

        private bool LoadPost(int id)
        {
            Post = _blogPostService.GetById(id);
            return Post != null;
        }

        private void LoadComments(int id)
        {
            ApprovedComments = _commentService.GetApprovedByPostId(id);
        }

        public class CommentInput
        {
            [Required(ErrorMessage = "امتیاز را وارد کنید.")]
            [Range(1, 5)]
            public int Rating { get; set; } = 5;

            [Required(ErrorMessage = "متن کامنت الزامی است.")]
            [MaxLength(1000, ErrorMessage = "حداکثر 1000 کاراکتر.")]
            public string Message { get; set; }

            public string? Name { get; set; }

            [EmailAddress(ErrorMessage = "ایمیل معتبر نیست.")]
            public string? Email { get; set; }
        }
    }
}

