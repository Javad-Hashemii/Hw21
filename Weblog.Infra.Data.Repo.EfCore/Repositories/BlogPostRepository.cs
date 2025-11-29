using Microsoft.EntityFrameworkCore;
using Weblog.Domain.Core.PostAgg.Contracts.Repository;
using Weblog.Domain.Core.PostAgg.Entities;
using Weblog.Infra.Db.SqlServer.EfCore;

namespace Weblog.Infra.Data.Repo.EfCore.Repositories
{
    public class BlogPostRepository(AppDbContext dbContext) : IBlogRepository
    {
        public int Add(BlogPost post)
        {
            dbContext.BlogPosts
                .Add(post);
            dbContext
                .SaveChanges();
            return post.Id;
        }

        public int Delete(int id)
        {
            var post = dbContext.BlogPosts
                .Find(id);

            if (post == null)
            {
                return 0;
            }

            dbContext.BlogPosts
                .Remove(post);

            return dbContext
                .SaveChanges();
        }

        public List<BlogPost> GetAll()
        {
            return dbContext.BlogPosts
                .AsNoTracking()
                .Include(p => p.Category)
                .OrderByDescending(p => p.PublishedDate)
                .ToList();
        }

        public List<BlogPost> GetByCategoryId(int categoryId)
        {
            return dbContext.BlogPosts
                .AsNoTracking()
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId)
                .OrderByDescending(p => p.PublishedDate)
                .ToList();
        }

        public BlogPost GetById(int postId)
        {
            return dbContext.BlogPosts
                .AsNoTracking()
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == postId);
        }

        public List<BlogPost> GetByUserId(string userId)
        {
            return dbContext.BlogPosts
                .AsNoTracking()
                .Include(p => p.Category)
                .Where(p => p.AuthorId == userId)
                .OrderByDescending(p => p.PublishedDate)
                .ToList();
        }

        public List<BlogPost> GetRecents(int count)
        {
            return dbContext.BlogPosts
                .AsNoTracking()
                .Include(p => p.Category)
                .OrderByDescending(p => p.PublishedDate)
                .Take(count)
                .ToList();
        }

        public int Update(BlogPost post)
        {
            return dbContext.BlogPosts
                .Where(p => p.Id == post.Id)
                .ExecuteUpdate(setters => setters
                    .SetProperty(p => p.Title, post.Title)
                    .SetProperty(p => p.Text, post.Text)
                    .SetProperty(p => p.ImageUrl, post.ImageUrl)
                    .SetProperty(p => p.PublishedDate, post.PublishedDate)
                    .SetProperty(p => p.CategoryId, post.CategoryId));
        }
    }
}
