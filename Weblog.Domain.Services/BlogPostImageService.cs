using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Weblog.Domain.Core.FileAgg.Contracts;
using Weblog.Domain.Core.PostAgg.Contracts.Repository;
using Weblog.Domain.Core.PostAgg.Contracts.Service;
using Weblog.Domain.Core.PostAgg.Entities;

namespace Weblog.Domain.Services
{
    public class BlogPostImageService(IBlogPostImageRepository _repo, IFileService fileService):IBlogPostImageService
    {
        public List<string> Create(int blogPostId, List<IFormFile> files)
        {
            var images = new List<BlogPostImage>();
            var storedPaths = new List<string>();

            foreach (var file in files)
            {
                var path = fileService.Upload(file, "BlogImages");
                storedPaths.Add(path);

                var image = new BlogPostImage
                {
                    ImagePath = path,
                    BlogPostId = blogPostId
                };
                images.Add(image);
            }

            if (images.Count > 0)
            {
                _repo.Create(images);
            }

            return storedPaths;
        }

        public List<string> GetAll(int blogPostId)
        {
            return _repo.GetAllPaths(blogPostId);
        }
    }
}
