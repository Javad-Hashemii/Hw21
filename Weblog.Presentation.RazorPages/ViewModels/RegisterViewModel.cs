using System.ComponentModel.DataAnnotations;

namespace Weblog.Presentation.RazorPages.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "نام الزامی است")]
        public string Name { get; set; }


        [Required(ErrorMessage = "ایمیل الزامی است")]
        [EmailAddress]
        public string Email { get; set; }


        [Required(ErrorMessage = "رمز عبور الزامی است")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "The {0} must be at {2} and at max {1} charachters long")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword", ErrorMessage = "پسورد ها یکسان نیستند")]
        public string Password { get; set; }


        [Required(ErrorMessage = "تایید رمز عبور الزامی است.")]
        [DataType(DataType.Password)]
        [Display(Name = "تایید رمز عبور")]
        public string ConfirmPassword { get; set; }
    }
}
