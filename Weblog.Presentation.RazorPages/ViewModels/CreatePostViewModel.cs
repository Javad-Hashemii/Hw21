using System.ComponentModel.DataAnnotations;

namespace Weblog.Presentation.RazorPages.ViewModels
{
    public class CreatePostViewModel
    {
        [Required(ErrorMessage = "عنوان پست را وارد کنید.")]
        [StringLength(150, ErrorMessage = "حداکثر ۱۵۰ کاراکتر مجاز است.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "متن پست الزامی است.")]
        public string Text { get; set; }

        [Required(ErrorMessage = "دسته‌بندی را انتخاب کنید.")]
        public int CategoryId { get; set; }

        public IFormFile ImageFile { get; set; }
    }
}
