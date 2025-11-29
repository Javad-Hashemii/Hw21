using System.ComponentModel.DataAnnotations;

namespace Weblog.Presentation.RazorPages.ViewModels
{
    public class EditCategoryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "نام دسته بندی الزامی است")]
        [StringLength(50)]
        [Display(Name = "نام دسته بندی")]
        public string Name { get; set; }
    }
}
