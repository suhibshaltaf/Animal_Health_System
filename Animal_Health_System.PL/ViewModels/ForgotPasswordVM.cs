using System.ComponentModel.DataAnnotations;

namespace Animal_Health_System.PL.ViewModels
{
    public class ForgotPasswordVM
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email format.")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
