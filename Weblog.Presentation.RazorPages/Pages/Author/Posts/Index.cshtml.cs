using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Weblog.Domain.Core.PostAgg.Contracts.AppService;
using Weblog.Domain.Core.PostAgg.Dtos;
using Weblog.Infra.Db.SqlServer.EfCore;

namespace Weblog.Presentation.RazorPages.Pages.Author.Posts
{
    [Authorize]
    public class IndexModel(IBlogAppService _blogAppService, UserManager<ApplicationUser> _userManager) : PageModel
    {
        public List<ShowPostDto> Posts { get; set; } = new();

        [TempData]
        public string StatusMessage { get; set; }

        public IActionResult OnGet()
        {
            var userId = _userManager.GetUserId(User);

            Posts = _blogAppService.GetUserPosts(userId);
            return Page();
        }

        public IActionResult OnPostDelete(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            try
            {
                _blogAppService.Delete(id, userId);
                StatusMessage = "پست حذف شد.";
            }
            catch (Exception ex)
            {
                StatusMessage = ex.Message;
            }

            return RedirectToPage();
        }
    }
}

