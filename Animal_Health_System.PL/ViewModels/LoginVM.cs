using System.ComponentModel.DataAnnotations;

namespace Animal_Health_System.PL.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Email is Required ..")]
        [MinLength(5)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is Required ..")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
