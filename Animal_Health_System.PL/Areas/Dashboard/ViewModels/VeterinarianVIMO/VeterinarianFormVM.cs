using Animal_Health_System.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace Animal_Health_System.PL.Areas.Dashboard.ViewModels.VeterinarianVIMO
{
    public class VeterinarianFormVM
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(100, ErrorMessage = "Full Name cannot exceed 100 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Full Name must only contain letters and spaces.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Specialty is required.")]
        [StringLength(30, ErrorMessage = "Specialty cannot exceed 30 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Specialty must only contain letters and spaces.")]
        public string Specialty { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Years of experience is required.")]
        [Range(0, 50, ErrorMessage = "Years of experience must be between 0 and 50.")]
        public int YearsOfExperience { get; set; }

        [Required(ErrorMessage = "Salary is required.")]
        [Range(0, 100000, ErrorMessage = "Salary must be a positive value.")]
        public decimal Salary { get; set; }

        public bool IsDeleted { get; set; }

    }
}
