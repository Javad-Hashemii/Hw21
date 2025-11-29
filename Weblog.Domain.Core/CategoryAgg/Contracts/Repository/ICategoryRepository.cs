using Weblog.Domain.Core.CategoryAgg.Entities;

namespace Weblog.Domain.Core.CategoryAgg.Contracts.Repository
{
    public interface ICategoryRepository
    {
        Category GetById(int id);
        List<Category> GetAllCategories();
        List<Category> GetByUserId(string userId);
        bool IsNameExist(string userId, string name);
        int Add(Category category);
        int Update(Category category);
        bool Delete(int id);
    }
}
