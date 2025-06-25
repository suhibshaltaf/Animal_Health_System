using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Animal_Health_System.PL.Areas.Dashboard.ViewModels.FarmStaffVIMO
{
    public class FarmStaffFormVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(100, ErrorMessage = "Full Name must be between 3 and 100 characters.", MinimumLength = 3)]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Full Name must contain only letters and spaces.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Job Title is required.")]
        [StringLength(50, ErrorMessage = "Job Title must be between 2 and 50 characters.", MinimumLength = 2)]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Job Title must contain only letters.")]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        [Required(ErrorMessage = "Phone Number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone Number must be exactly 10 digits.")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Salary is required.")]
        [Range(300, double.MaxValue, ErrorMessage = "Salary must be at least 300.")]
        [Display(Name = "Salary")]
        public decimal Salary { get; set; }

        [Required(ErrorMessage = "Farm selection is required.")]
        [Display(Name = "Farm")]
        public int FarmId { get; set; }
        public List<SelectListItem> Farms { get; set; } = new List<SelectListItem>();

        [Required(ErrorMessage = "Date Hired is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date Hired")]
        [CustomValidation(typeof(FarmStaffFormVM), nameof(ValidateDateHired))]
        public DateTime? DateHired { get; set; }

        public static ValidationResult ValidateDateHired(DateTime? date, ValidationContext context)
        {
            if (!date.HasValue)
                return new ValidationResult("Date Hired is required.");

            DateTime today = DateTime.Today;
            DateTime minDate = today.AddYears(-50); // قبل 80 سنة
            DateTime maxDate = today; // لا يسمح بالتواريخ المستقبلية

            if (date.Value < minDate)
                return new ValidationResult($"Date Hired cannot be earlier than {minDate:yyyy-MM-dd} (80 years ago).");

            if (date.Value > maxDate)
                return new ValidationResult("Date Hired cannot be in the future.");

            return ValidationResult.Success;
        }
    }
}
