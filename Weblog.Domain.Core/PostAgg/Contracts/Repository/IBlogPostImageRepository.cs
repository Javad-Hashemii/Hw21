using System;
using System.Collections.Generic;
using System.Text;
using Weblog.Domain.Core.PostAgg.Entities;

namespace Weblog.Domain.Core.PostAgg.Contracts.Repository
{
    public interface IBlogPostImageRepository
    {
        void Create(List<BlogPostImage> images);

        List<string> GetAllPaths(int blogPostId);

        List<BlogPostImage> GetByPostId(int blogPostId);
    }
}
