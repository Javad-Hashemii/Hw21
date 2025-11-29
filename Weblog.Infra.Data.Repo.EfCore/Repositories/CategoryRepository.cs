using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Weblog.Domain.Core.CategoryAgg.Contracts.Repository;
using Weblog.Domain.Core.CategoryAgg.Entities;
using Weblog.Infra.Db.SqlServer.EfCore;

namespace Weblog.Infra.Data.Repo.EfCore.Repositories
{
    public class CategoryRepository(AppDbContext dbContext):ICategoryRepository
    {
        public int Add(Category category)
        {
            dbContext.Categories.Add(category);
            dbContext.SaveChanges();
            return category.Id;
        }

        public bool Delete(int id)
        {
            var category = dbContext.Categories.Find(id);
            if (category == null) return false;

            // Note: If you try to delete a category that has posts, 
            // EF Core will throw an exception due to the Foreign Key "Restrict" rule we set earlier.
            // You might want to wrap this in a try-catch in a real app.
            dbContext.Categories.Remove(category);
            return dbContext.SaveChanges() > 0;
        }

        public List<Category> GetAll()
        {
            return dbContext.Categories
                .AsNoTracking()
                .ToList();
        }

        public Category GetById(int id)
        {
            return dbContext.Categories.Find(id);
        }

        public List<Category> GetByUserId(string userId)
        {
            return dbContext.Categories
                .AsNoTracking()
                .Where(c => c.OwnerId == userId)
                .ToList();
        }

        public void Update(Category category)
        {
            dbContext.Categories
                .Where(c => c.Id == category.Id)
                .ExecuteUpdate(setters => setters
                    .SetProperty(c => c.Name, category.Name));
        }
        public bool IsNameExist(string userId, string name)
        {
            return dbContext.Categories.Any(c => c.OwnerId == userId && c.Name == name);
        }

    }
}
