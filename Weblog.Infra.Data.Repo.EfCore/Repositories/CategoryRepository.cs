using Microsoft.EntityFrameworkCore;
using Weblog.Domain.Core.CategoryAgg.Contracts.Repository;
using Weblog.Domain.Core.CategoryAgg.Entities;
using Weblog.Infra.Db.SqlServer.EfCore;

namespace Weblog.Infra.Data.Repo.EfCore.Repositories
{
    public class CategoryRepository(AppDbContext dbContext):ICategoryRepository
    {
        public int Add(Category category)
        {
            dbContext.Categories
                .Add(category);
            dbContext
                .SaveChanges();
            return category.Id;
        }

        public bool Delete(int id)
        {
            var category = dbContext.Categories
                .Find(id);
            if (category == null)
            {
                return false;
            }
            dbContext.Categories
                .Remove(category);
            return dbContext
                .SaveChanges() > 0;
        }

        public List<Category> GetAllCategories()
        {
            return dbContext.Categories
                .AsNoTracking()
                .ToList();
        }

        public Category GetById(int id)
        {
            return dbContext.Categories
                .Find(id);
        }

        public List<Category> GetByUserId(string userId)
        {
            return dbContext.Categories
                .AsNoTracking()
                .Where(c => c.OwnerId == userId)
                .ToList();
        }

        public int Update(Category category)
        {
            return dbContext.Categories
                .Where(c => c.Id == category.Id)
                .ExecuteUpdate(setters => setters
                    .SetProperty(c => c.Name, category.Name));
        }

        public bool IsNameExist(string userId, string name)
        {
            return dbContext.Categories
                .Any(c => c.OwnerId == userId && c.Name == name);
        }

    }
}
