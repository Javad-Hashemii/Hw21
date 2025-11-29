using Weblog.Domain.Core.PostAgg.Dtos;

namespace Weblog.Domain.Core.PostAgg.Contracts.Service
{
    public interface IBlogPostService
    {
        int Create(CreatePostDto dto);
        int Update(UpdatePostDto dto);
        int Delete(int id, string userId);

        ShowPostDto GetById(int id);
        List<ShowPostDto> GetRecents(int count);
        List<ShowPostDto> GetByCategory(int categoryId);
        List<ShowPostDto> GetUserPosts(string userId);
    }
}
