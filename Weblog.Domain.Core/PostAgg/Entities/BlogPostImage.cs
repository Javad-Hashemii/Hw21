using System;
using System.Collections.Generic;
using System.Text;

namespace Weblog.Domain.Core.PostAgg.Entities
{
    public class BlogPostImage
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public int BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }
    }
}
