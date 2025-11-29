namespace Weblog.Domain.Core.PostAgg.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Rating { get; set; }
        public CommentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UserId { get; set; }
        public int BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }

    }
}
