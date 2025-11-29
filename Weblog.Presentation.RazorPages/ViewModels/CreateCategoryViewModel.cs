using System.ComponentModel.DataAnnotations;

namespace Weblog.Presentation.RazorPages.ViewModels
{
    public class CreateCategoryViewModel
    {

        [Required(ErrorMessage = "نام دسته‌بندی الزامی است")]
        [StringLength(50)]
        [Display(Name = "نام دسته‌بندی")]
        public string Name { get; set; }

    }
}
