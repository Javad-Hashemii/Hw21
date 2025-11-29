using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Weblog.Domain.Core.CategoryAgg.Contracts.AppService;
using Weblog.Domain.Core.CategoryAgg.Contracts.Service;
using Weblog.Domain.Core.PostAgg.Contracts.AppService;
using Weblog.Domain.Core.PostAgg.Contracts.Service;
using Weblog.Domain.Core.PostAgg.Entities;
using Weblog.Infra.Db.SqlServer.EfCore;

namespace Weblog.Presentation.RazorPages.Pages.Author
{
    [Authorize]
    public class IndexModel(ICategoryAppService _categoryAppService, IBlogAppService _blogAppService, ICommentAppService _commentAppService, UserManager<ApplicationUser> _userManager) : PageModel
    {
        public int CategoryCount { get; set; }
        public int PostCount { get; set; }
        public int PendingComments { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);

            var categories = _categoryAppService.GetCategoryByUserId(userId);
            var posts = _blogAppService.GetUserPosts(userId);
            var pendingComments = _commentAppService.GetAuthorComments(userId, CommentStatus.Pending);

            CategoryCount = categories.Count;
            PostCount = posts.Count;
            PendingComments = pendingComments.Count;

            return Page();
        }
    }
}

