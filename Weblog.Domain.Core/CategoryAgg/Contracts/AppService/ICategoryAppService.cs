using Weblog.Domain.Core.CategoryAgg.Dtos;

namespace Weblog.Domain.Core.CategoryAgg.Contracts.AppService
{
    public interface ICategoryAppService
    {
        bool IsCategoryNameUnique(string userId, string categoryName);
        List<CategoryDto> GetCategoryByUserId(string userId);
        List<CategoryDto> GetAllCategories();
        CategoryDto GetCategoryById(int id);
        bool Delete(int id, string userId);
        bool Update(CreateCategoryDto dto);
        int CreateCategory(CreateCategoryDto dto);
    }
}
