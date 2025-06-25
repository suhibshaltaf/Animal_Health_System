using Animal_Health_System.DAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Animal_Health_System.PL.Areas.Dashboard.ViewModels.PregnancyVIMO
{
    public class PregnancyFormVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name must not exceed 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Mating date is required")]
        [DataType(DataType.Date)]
        public DateTime MatingDate { get; set; }

        [Required(ErrorMessage = "Expected birth date is required")]
        [DataType(DataType.Date)]
        [DateGreaterThan(nameof(MatingDate), ErrorMessage = "Expected birth date must be after mating date")]
        public DateTime ExpectedBirthDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ActualBirthDate { get; set; }

        [Required(ErrorMessage = "Pregnancy status is required")]
        public PregnancyStatus Status { get; set; }

        public bool HasComplications { get; set; }

        [StringLength(500, ErrorMessage = "Notes must not exceed 500 characters")]
        public string Notes { get; set; }

      
        [Required(ErrorMessage = "Animal selection is required")]
        public int? AnimalId { get; set; }
        public List<SelectListItem> Animals { get; set; } = new List<SelectListItem>();

        [Required(ErrorMessage = "Mating selection is required")]
        public int MatingId { get; set; }
        public List<SelectListItem> Matings { get; set; } = new List<SelectListItem>();

    }

    // Custom validation attribute to ensure ExpectedBirthDate is greater than MatingDate
    public class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateGreaterThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var comparisonValue = validationContext.ObjectType.GetProperty(_comparisonProperty)
                ?.GetValue(validationContext.ObjectInstance) as DateTime?;

            var currentValue = value as DateTime?;

            if (currentValue.HasValue && comparisonValue.HasValue && currentValue <= comparisonValue)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
