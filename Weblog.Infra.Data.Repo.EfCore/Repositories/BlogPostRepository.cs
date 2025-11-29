using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using System;
using System.Collections.Generic;
using System.Text;
using Weblog.Domain.Core.PostAgg.Contracts.Repository;
using Weblog.Domain.Core.PostAgg.Entities;
using Weblog.Infra.Db.SqlServer.EfCore;

namespace Weblog.Infra.Data.Repo.EfCore.Repositories
{
    public class BlogPostRepository(AppDbContext dbContext) : IBlogRepository
    {
        public int Add(BlogPost post)
        {
            dbContext.BlogPosts.Add(post);
            dbContext.SaveChanges();
            return post.Id;
        }

        public bool Delete(int id)
        {
            var post = dbContext.BlogPosts.Find(id);
            if (post == null) return false;

            dbContext.BlogPosts.Remove(post);
            return dbContext.SaveChanges() > 0;
        }

        public List<BlogPost> GetAll()
        {
            return dbContext.BlogPosts
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Images)
                .OrderByDescending(p => p.PublishedDate)
                .ToList();
        }

        public List<BlogPost> GetByCategoryId(int categoryId)
        {
            return dbContext.BlogPosts
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Where(p => p.CategoryId == categoryId)
                .OrderByDescending(p => p.PublishedDate)
                .ToList();
        }

        public BlogPost GetById(int postId)
        {
            return dbContext.BlogPosts
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Images)
                .FirstOrDefault(p => p.Id == postId);
        }

        public List<BlogPost> GetByUserId(string userId)
        {
            return dbContext.BlogPosts
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Where(p => p.AuthorId == userId)
                .OrderByDescending(p => p.PublishedDate)
                .ToList();
        }

        public List<BlogPost> GetRecents(int count)
        {
            return dbContext.BlogPosts
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Images)
                .OrderByDescending(p => p.PublishedDate)
                .Take(count)
                .ToList();
        }

        public void Update(BlogPost post)
        {

                dbContext.BlogPosts
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
