using System.ComponentModel.DataAnnotations;

namespace Weblog.Presentation.RazorPages.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "ایمیل الزامی است."),]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "پسورد الزامی است"),]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "به خاطر بسپار")]
        public bool RememberMe { get; set; }
    }
}
