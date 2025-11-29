using Weblog.Domain.Core.PostAgg.Entities;

namespace Weblog.Domain.Core.PostAgg.Dtos
{
    public class ShowCommentDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string AuthorName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Rating { get; set; }
        public CommentStatus Status { get; set; }
    }
}
