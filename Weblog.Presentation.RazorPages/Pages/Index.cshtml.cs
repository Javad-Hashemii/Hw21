using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using Weblog.Domain.Core.CategoryAgg.Contracts.Service;
using Weblog.Domain.Core.CategoryAgg.Dtos;
using Weblog.Domain.Core.PostAgg.Contracts.Service;
using Weblog.Domain.Core.PostAgg.Dtos;

namespace Weblog.Presentation.RazorPages.Pages
{
    public class IndexModel(IBlogPostService _blogPostService, ICategoryService _categoryService) : PageModel
    {
        public List<ShowPostDto> RecentPosts { get; set; }
        public List<CategoryDto> Categories { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? CategoryId { get; set; }

        public string SelectedCategoryName { get; set; }

        public void OnGet()
        {
            Categories = _categoryService.GetAll();

            if (CategoryId.HasValue && CategoryId.Value > 0)
            {
                RecentPosts = _blogPostService.GetByCategory(CategoryId.Value);
                SelectedCategoryName = Categories.FirstOrDefault(c => c.Id == CategoryId)?.Name;
            }
            else
            {
                RecentPosts = _blogPostService.GetRecents(10);
            }
        }
    }
}
