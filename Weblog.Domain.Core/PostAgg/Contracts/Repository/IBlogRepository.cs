using System.Globalization;
using Weblog.Domain.Core.PostAgg.Entities;

namespace Weblog.Domain.Core.PostAgg.Contracts.Repository
{
    public interface IBlogRepository
    {
        BlogPost GetById(int postId);
        List<BlogPost> GetAll();
        List<BlogPost> GetRecents(int count);
        List<BlogPost> GetByCategoryId(int categoryId);
        List<BlogPost> GetByUserId(string userId);
        int Add(BlogPost post);
        int Update(BlogPost post);
        int Delete(int id);
    }
}
