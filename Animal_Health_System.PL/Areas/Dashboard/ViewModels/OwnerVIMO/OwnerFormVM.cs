using Animal_Health_System.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace Animal_Health_System.PL.Areas.Dashboard.ViewModels.OwnerVIMO
{
    public class OwnerFormVM
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(100, ErrorMessage = "Full Name cannot exceed 100 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Full Name must only contain letters and spaces.")]
        public string FullName { get; set; }

     

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
    }
}
