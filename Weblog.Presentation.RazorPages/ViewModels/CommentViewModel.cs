using System.ComponentModel.DataAnnotations;

namespace Weblog.Presentation.RazorPages.ViewModels
{
    public class CommentViewModel
    {
        [Required(ErrorMessage = "امتیاز را وارد کنید.")]
        [Range(1, 5)]
        public int Rating { get; set; } = 5;

        [Required(ErrorMessage = "متن کامنت الزامی است.")]
        [MaxLength(1000, ErrorMessage = "حداکثر 1000 کاراکتر.")]
        public string Message { get; set; }

        public string? Name { get; set; }

        [EmailAddress(ErrorMessage = "ایمیل معتبر نیست.")]
        public string? Email { get; set; }
    }
}
