using Weblog.Domain.Core.CategoryAgg.Contracts.AppService;
using Weblog.Domain.Core.CategoryAgg.Contracts.Service;
using Weblog.Domain.Core.CategoryAgg.Dtos;

namespace Weblog.Domain.Appservices
{
    public class CategoryAppService(ICategoryService categoryService) : ICategoryAppService
    {
        public bool IsCategoryNameUnique(string userId, string categoryName)
        {
            return categoryService.IsCategoryNameUnique(userId, categoryName);
        }
        public List<CategoryDto> GetCategoryByUserId(string userId)
        {
            return categoryService.GetCategoryByUserId(userId);
        }
        public List<CategoryDto> GetAllCategories()
        {
            return categoryService.GetAllCategories();
        }
        public CategoryDto GetCategoryById(int id)
        {
            return categoryService.GetCategoryById(id);
        }
        public bool Delete(int id, string userId)
        {
            return categoryService.Delete(id, userId);
        }
        public bool Update(CreateCategoryDto dto)
        {
            return categoryService.Update(dto);
        }
        public int CreateCategory(CreateCategoryDto dto)
        {
            return categoryService.CreateCategory(dto);
        }

    }
}
