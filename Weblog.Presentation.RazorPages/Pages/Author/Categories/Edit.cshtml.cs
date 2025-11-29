using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Weblog.Domain.Core.CategoryAgg.Contracts.Service;
using Weblog.Domain.Core.CategoryAgg.Dtos;
using Weblog.Infra.Db.SqlServer.EfCore;

namespace Weblog.Presentation.RazorPages.Pages.Author.Categories
{
    [Authorize]
    public class EditModel(ICategoryService _categoryService,
                           UserManager<ApplicationUser> _userManager) : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId)) return Challenge();

            var category = _categoryService.GetById(id);

            // Security Check: Does category exist and belong to user?
            if (category == null || category.OwnerId != userId)
                return NotFound();

            Input = new InputModel { Id = category.Id, Name = category.Name };
            return Page();
        }

        public IActionResult OnPost()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId)) return Challenge();

            if (!ModelState.IsValid) return Page();

            try
            {
                var dto = new CreateCategoryDto
                {
                    Id = Input.Id,
                    Name = Input.Name,
                    UserId = userId
                };

                _categoryService.Update(dto);
                TempData["StatusMessage"] = "????????? ?????? ??.";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

        public class InputModel
        {
            public int Id { get; set; }

            [Required(ErrorMessage = "??? ????????? ?????? ???")]
            [StringLength(50)]
            [Display(Name = "??? ?????????")]
            public string Name { get; set; }
        }
    }
}