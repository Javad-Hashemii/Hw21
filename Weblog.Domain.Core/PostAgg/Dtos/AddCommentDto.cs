using System.ComponentModel.DataAnnotations;
namespace Weblog.Domain.Core.PostAgg.Dtos
{
    public class AddCommentDto
    {
        [Required]
        public int BlogPostId { get; set; }

        [Required(ErrorMessage = "متن کامنت الزامی است.")]
        [MaxLength(1000)]
        public string Text { get; set; }

        [Range(1, 5, ErrorMessage = "امتیاز باید بین 1 تا 5 باشد.")]
        public int Rating { get; set; }

        // Guest info (required when not logged in)
        [MaxLength(100)]
        public string? GuestName { get; set; }

        [EmailAddress(ErrorMessage = "ایمیل معتبر نیست.")]
        public string? GuestEmail { get; set; }
    }
}
