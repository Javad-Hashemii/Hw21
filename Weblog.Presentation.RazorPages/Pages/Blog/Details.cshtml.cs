using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Weblog.Domain.Core.PostAgg.Contracts.AppService;
using Weblog.Domain.Core.PostAgg.Dtos;
using Weblog.Presentation.RazorPages.ViewModels;

namespace Weblog.Presentation.RazorPages.Pages.Blog
{
    public class DetailsModel(IBlogAppService _blogAppService, ICommentAppService _commentAppService) : PageModel
    {
        public ShowPostDto Post { get; set; }
        public List<ShowCommentDto> ApprovedComments { get; set; } = new();

        [BindProperty]
        public CommentViewModel Input { get; set; } = new();

        [TempData]
        public string StatusMessage { get; set; }

        public IActionResult OnGet(int id)
        {
            if (!LoadData(id))
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            if (!LoadPost(id))
                return NotFound();

            if (!User.Identity.IsAuthenticated)
            {
                if (string.IsNullOrWhiteSpace(Input.Name))
                {
                    ModelState.AddModelError(nameof(Input.Name), "نام را وارد کنید.");
                }
                if (string.IsNullOrWhiteSpace(Input.Email))
                {
                    ModelState.AddModelError(nameof(Input.Email), "ایمیل را وارد کنید.");
                }
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
                _commentAppService.AddComment(dto, userId, userName);
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
            Post = _blogAppService.GetById(id);
            return Post != null;
        }

        private void LoadComments(int id)
        {
            ApprovedComments = _commentAppService.GetApprovedByPostId(id);
        }

    }
}

