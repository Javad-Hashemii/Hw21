using System;
using System.Collections.Generic;
using System.Text;
using Weblog.Domain.Core.PostAgg.Entities;

namespace Weblog.Domain.Core.CategoryAgg.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string OwnerId { get; set; }
        public List<BlogPost> BlogPosts { get; set; }
    }
}
