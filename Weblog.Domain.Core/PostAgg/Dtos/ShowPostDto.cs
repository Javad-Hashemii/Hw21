using System;
using System.Collections.Generic;
using System.Text;

namespace Weblog.Domain.Core.PostAgg.Dtos
{
    public class ShowPostDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string AuthorId { get; set; }
        public DateTime PublishedDate { get; set; }
        public string CoverImageUrl { get; set; }
        public List<string> ImageUrls { get; set; } = new List<string>();
    }
}
