using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using Weblog.Domain.Core.PostAgg.Contracts.Service;
using Weblog.Domain.Core.PostAgg.Dtos;
using Weblog.Infra.Db.SqlServer.EfCore;

namespace Weblog.Presentation.RazorPages.Pages.Author.Posts
{
    [Authorize]
    public class IndexModel(IBlogPostService _blogPostService,
                            UserManager<ApplicationUser> _userManager) : PageModel
    {
        public List<ShowPostDto> Posts { get; set; } = new();

        [TempData]
        public string StatusMessage { get; set; }

        public IActionResult OnGet()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId))
                return Challenge();

            Posts = _blogPostService.GetUserPosts(userId);
            return Page();
        }

        public IActionResult OnPostDelete(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId))
                return Challenge();

            try
            {
                _blogPostService.Delete(id, userId);
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

