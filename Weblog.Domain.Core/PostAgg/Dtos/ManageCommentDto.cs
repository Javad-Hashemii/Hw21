using System;
using Weblog.Domain.Core.PostAgg.Entities;

namespace Weblog.Domain.Core.PostAgg.Dtos
{
    public class ManageCommentDto
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string PostTitle { get; set; }
        public string CommenterName { get; set; }
        public string CommenterEmail { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public CommentStatus Status { get; set; }
    }
}

