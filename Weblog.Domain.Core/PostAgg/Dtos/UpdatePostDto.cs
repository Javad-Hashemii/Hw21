using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Weblog.Domain.Core.PostAgg.Dtos
{
    public class UpdatePostDto
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "عنوان پست الزامی است.")]
        [MaxLength(150, ErrorMessage = "عنوان نمی‌تواند بیش از 150 کاراکتر باشد.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "متن اصلی پست الزامی است.")]
        public string Text { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public string AuthorId { get; set; }

        public DateTime? PublishedDate { get; set; }

        public IFormFile ImageFile { get; set; }
    }
}

