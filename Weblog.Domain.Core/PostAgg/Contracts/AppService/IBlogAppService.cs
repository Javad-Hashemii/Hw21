using System;
using System.Collections.Generic;
using System.Text;
using Weblog.Domain.Core.PostAgg.Dtos;

namespace Weblog.Domain.Core.PostAgg.Contracts.AppService
{
    public interface IBlogAppService
    {
        List<ShowPostDto> GetUserPosts(string userId);
        List<ShowPostDto> GetByCategory(int categoryId);
        List<ShowPostDto> GetRecents(int count);
        ShowPostDto GetById(int id);
        int Delete(int id, string userId);
        int Update(UpdatePostDto dto);
        int Create(CreatePostDto dto);

    }
}
