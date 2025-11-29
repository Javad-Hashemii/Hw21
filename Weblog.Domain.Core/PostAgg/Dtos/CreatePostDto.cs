using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Weblog.Domain.Core.PostAgg.Dtos
{
    public class CreatePostDto
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public int CategoryId { get; set; }
        public string AuthorId { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
