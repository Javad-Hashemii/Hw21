using Weblog.Domain.Core.PostAgg.Entities;

namespace Weblog.Domain.Core.PostAgg.Contracts.Repository
{
    public interface ICommentRepository
    {
        Comment GetById(int id);
        List<Comment> GetByPostId(int postId);
        List<Comment> GetByAuthor(string authorId);

        int Add(Comment comment);
        int Update(Comment comment);
        bool Delete(int id);
    }
}
