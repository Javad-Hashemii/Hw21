using System.ComponentModel.DataAnnotations;

namespace Weblog.Presentation.RazorPages.ViewModels
{
    public class EditPostViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "عنوان پست را وارد کنید.")]
        [StringLength(150, ErrorMessage = "حداکثر ۱۵۰ کاراکتر مجاز است.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "متن پست الزامی است.")]
        public string Text { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public DateTime? PublishedDate { get; set; }

        public IFormFile ImageFile { get; set; }
    }
}
