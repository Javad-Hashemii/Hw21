using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Weblog.Domain.Core.PostAgg.Contracts.AppService;
using Weblog.Domain.Core.PostAgg.Dtos;
using Weblog.Domain.Core.PostAgg.Entities;
using Weblog.Infra.Db.SqlServer.EfCore;

namespace Weblog.Presentation.RazorPages.Pages.Author.Comments
{
    [Authorize]
    public class IndexModel(ICommentAppService _commentAppService, UserManager<ApplicationUser> _userManager) : PageModel
    {
        public List<ManageCommentDto> Comments { get; set; } = new();

        [BindProperty(SupportsGet = true, Name = "status")]
        public CommentStatus? StatusFilter { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = await GetUserIdAsync();
            LoadComments(userId);
            return Page();
        }

        public async Task<IActionResult> OnPostApproveAsync(int id)
        {
            var userId = await GetUserIdAsync();
            if (userId == null)
            {
                return Challenge();
            }

            try
            {
                _commentAppService.Approve(id, userId);
                StatusMessage = "کامنت تایید شد.";
            }
            catch (Exception ex)
            {
                StatusMessage = ex.Message;
            }

            return RedirectToPage(new { status = StatusFilter });
        }

        public async Task<IActionResult> OnPostRejectAsync(int id)
        {
            var userId = await GetUserIdAsync();
            if (userId == null)
            {
                return Challenge();
            }

            try
            {
                _commentAppService.Reject(id, userId);
                StatusMessage = "کامنت رد شد.";
            }
            catch (Exception ex)
            {
                StatusMessage = ex.Message;
            }

            return RedirectToPage(new { status = StatusFilter });
        }

        private void LoadComments(string userId)
        {
            Comments = _commentAppService.GetAuthorComments(userId, StatusFilter);
        }

        private async Task<string?> GetUserIdAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            return user?.Id;
        }
    }
}

