using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Weblog.Domain.Core.CategoryAgg.Contracts.AppService;
using Weblog.Domain.Core.CategoryAgg.Dtos;
using Weblog.Domain.Core.PostAgg.Contracts.AppService;
using Weblog.Domain.Core.PostAgg.Dtos;

namespace Weblog.Presentation.RazorPages.Pages
{
    public class IndexModel(IBlogAppService _blogAppService, ICategoryAppService _categoryAppService) : PageModel
    {
        public List<ShowPostDto> RecentPosts { get; set; }
        public List<CategoryDto> Categories { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? CategoryId { get; set; }

        public string SelectedCategoryName { get; set; }

        public void OnGet()
        {
            Categories = _categoryAppService.GetAllCategories();

            if (CategoryId.HasValue && CategoryId.Value > 0)
            {
                RecentPosts = _blogAppService.GetByCategory(CategoryId.Value);
                SelectedCategoryName = Categories.FirstOrDefault(c => c.Id == CategoryId)?.Name;
            }
            else
            {
                RecentPosts = _blogAppService.GetRecents(10);
            }
        }
    }
}
