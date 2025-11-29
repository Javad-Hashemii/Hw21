using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Weblog.Domain.Core.PostAgg.Contracts.Repository;
using Weblog.Domain.Core.PostAgg.Entities;
using Weblog.Infra.Db.SqlServer.EfCore;

namespace Weblog.Infra.Data.Repo.EfCore.Repositories
{
    public class BlogPostImageRepository(AppDbContext _dbContext):IBlogPostImageRepository
    {

        public void Create(List<BlogPostImage> images)
        {
            if (images == null || !images.Any()) return;

            _dbContext.PostImages.AddRange(images);
            _dbContext.SaveChanges();
        }

        public List<string> GetAllPaths(int blogPostId)
        {
            return _dbContext.PostImages
                .Where(p => p.BlogPostId == blogPostId)
                .Select(p => p.ImagePath)
                .ToList();
        }

        public List<BlogPostImage> GetByPostId(int blogPostId)
        {
            return _dbContext.PostImages
                .Where(x => x.BlogPostId == blogPostId)
                .ToList();
        }
    }
}
