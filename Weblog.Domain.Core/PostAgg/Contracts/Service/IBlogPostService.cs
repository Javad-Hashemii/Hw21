using System;
using System.Collections.Generic;
using System.Text;
using Weblog.Domain.Core.PostAgg.Dtos;
using Weblog.Domain.Core.PostAgg.Entities;

namespace Weblog.Domain.Core.PostAgg.Contracts.Service
{
    public interface IBlogPostService
    {
        int Create(CreatePostDto dto);
        void Update(UpdatePostDto dto);
        void Delete(int id, string userId);

        ShowPostDto GetById(int id);
        List<ShowPostDto> GetRecents(int count);
        List<ShowPostDto> GetByCategory(int categoryId);
        List<ShowPostDto> GetUserPosts(string userId);
    }
}
