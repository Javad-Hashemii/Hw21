using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Weblog.Domain.Core.CategoryAgg.Contracts.Service;
using Weblog.Domain.Core.PostAgg.Contracts.Service;
using Weblog.Domain.Core.PostAgg.Entities;
using Weblog.Infra.Db.SqlServer.EfCore;

namespace Weblog.Presentation.RazorPages.Pages.Author
{
    [Authorize]
    public class IndexModel(ICategoryService _categoryService,
                            IBlogPostService _blogPostService,
                            ICommentService _commentService,
                            UserManager<ApplicationUser> _userManager) : PageModel
    {
        public int CategoryCount { get; set; }
        public int PostCount { get; set; }
        public int PendingComments { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId))
                return Challenge();

            var categories = _categoryService.GetByUserId(userId);
            var posts = _blogPostService.GetUserPosts(userId);
            var pendingComments = _commentService.GetAuthorComments(userId, CommentStatus.Pending);

            CategoryCount = categories.Count;
            PostCount = posts.Count;
            PendingComments = pendingComments.Count;

            return Page();
        }
    }
}

