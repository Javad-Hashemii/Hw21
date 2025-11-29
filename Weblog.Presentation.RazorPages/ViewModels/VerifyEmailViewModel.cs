using System.ComponentModel.DataAnnotations;

namespace Weblog.Presentation.RazorPages.ViewModels
{
    public class VerifyEmailViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
