using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Weblog.Domain.Core.PostAgg.Dtos
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int CategoryId { get; set; }
        public string AuthorId { get; set; }

        public List<IFormFile> ImageFiles { get; set; }
    }
}
