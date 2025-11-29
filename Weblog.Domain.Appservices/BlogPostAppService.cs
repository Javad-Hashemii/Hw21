using System;
using System.Collections.Generic;
using System.Text;
using Weblog.Domain.Core.PostAgg.Contracts.AppService;
using Weblog.Domain.Core.PostAgg.Contracts.Service;
using Weblog.Domain.Core.PostAgg.Dtos;

namespace Weblog.Domain.Appservices
{
    public class BlogPostAppService(IBlogPostService blogPostService):IBlogAppService
    {
        public int Create(CreatePostDto dto)
        {
           return blogPostService.Create(dto);
        }
        public int Update(UpdatePostDto dto)
        {
           return blogPostService.Update(dto);
        }
        public int Delete(int id, string userId)
        {
           return blogPostService.Delete(id, userId);
        }

        public ShowPostDto GetById(int id)
        {
            return blogPostService.GetById(id);
        }
        public List<ShowPostDto> GetRecents(int count)
        {
           return blogPostService.GetRecents(count);
        }

        public List<ShowPostDto> GetByCategory(int categoryId)
        {
            return blogPostService.GetByCategory(categoryId);
        }

        public List<ShowPostDto> GetUserPosts(string userId)
        {
            return blogPostService.GetUserPosts(userId);
        }

    }
}
