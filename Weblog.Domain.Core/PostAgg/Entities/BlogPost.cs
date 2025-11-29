using System;
using System.Collections.Generic;
using System.Text;
using Weblog.Domain.Core.CategoryAgg.Entities;

namespace Weblog.Domain.Core.PostAgg.Entities
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime PublishedDate { get; set; }
        public string? ImageUrl { get; set; }
        public string AuthorId { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<Comment> Comments { get; set; }
        public List<BlogPostImage> Images { get; set; }

    }
}
