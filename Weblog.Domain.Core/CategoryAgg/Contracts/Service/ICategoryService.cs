using Weblog.Domain.Core.CategoryAgg.Dtos;

namespace Weblog.Domain.Core.CategoryAgg.Contracts.Service
{
    public interface ICategoryService
    {
        public bool IsCategoryNameUnique(string userId, string categoryName);
        public List<CategoryDto> GetCategoryByUserId(string userId);
        public List<CategoryDto> GetAllCategories();
        public CategoryDto GetCategoryById(int id);
        public bool Delete(int id, string userId);
        public bool Update(CreateCategoryDto dto);
        public int CreateCategory(CreateCategoryDto dto);

    }
}
