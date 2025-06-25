using Animal_Health_System.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace Animal_Health_System.PL.Areas.Dashboard.ViewModels.MedicationVIMO
{
    public class MedicationFormVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Medication name is required.")]
        [RegularExpression(@"^[^\d]+$", ErrorMessage = "Name cannot contain numbers.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Dosage is required.")]
        public string Dosage { get; set; }

        [Required(ErrorMessage = "Description is required.")] // 🔹 التحقق من الإدخال
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")] // 🔹 تحديد الطول الأقصى
        public string Description { get; set; }

        [Required(ErrorMessage = "Expiry date is required.")]
        public DateTime ExpiryDate { get; set; }

        [Required(ErrorMessage = "Production date is required.")]
        public DateTime ProductionDate { get; set; }

   
    }

}
