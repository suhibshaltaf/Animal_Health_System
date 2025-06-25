using Microsoft.AspNetCore.Mvc.Rendering;
using Animal_Health_System.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace Animal_Health_System.PL.Areas.Dashboard.ViewModels.AnimalVIMO
{
    public class AnimalFormVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Animal name is required.")]
        [StringLength(100, ErrorMessage = "Animal name cannot exceed 100 characters.")]
        [RegularExpression(@"^[A-Za-z][A-Za-z0-9 ]*$", ErrorMessage = "Animal name cannot start with a number and must contain only letters and numbers.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Species is required.")]
        [StringLength(50, ErrorMessage = "Species cannot exceed 50 characters.")]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Species must contain only letters.")]
        public string Species { get; set; }

        [Required(ErrorMessage = "Breed is required.")]
        [StringLength(50, ErrorMessage = "Breed cannot exceed 50 characters.")]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Breed must contain only letters.")]
        public string Breed { get; set; }

        [Required(ErrorMessage = "Weight is required.")]
        [Range(0.1, 10000, ErrorMessage = "Weight must be between 0.1 and 10000 kg.")]
        public decimal Weight { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        [CustomValidation(typeof(AnimalFormVM), nameof(ValidateDateOfBirth))]
        public DateTime DateOfBirth { get; set; }

        public static ValidationResult ValidateDateOfBirth(DateTime date, ValidationContext context)
        {
            DateTime minDate = new DateTime(1950, 1, 1);
            DateTime maxDate = DateTime.UtcNow.Date; // يمنع تاريخ المستقبل

            if (date < minDate || date > maxDate)
            {
                return new ValidationResult($"Date of birth must be between {minDate:yyyy-MM-dd} and {maxDate:yyyy-MM-dd}.");
            }

            return ValidationResult.Success;
        }




        [Required(ErrorMessage = "Farm selection is required.")]
        public int FarmId { get; set; }

        public List<SelectListItem> Farms { get; set; } = new List<SelectListItem>();

       
        public bool IsDeleted { get; set; } // لدعم الحذف الناعم
    }
}
