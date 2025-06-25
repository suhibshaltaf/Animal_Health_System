using System;
using System.ComponentModel.DataAnnotations;

namespace Animal_Health_System.PL.Areas.Dashboard.ViewModels.VaccineVIMO
{
    public class VaccineFormVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Dose is required")]
        [StringLength(50, ErrorMessage = "Dose cannot be longer than 50 characters")]
        public string Dose { get; set; }

        [Required(ErrorMessage = "Description is required.")] // 🔹 التحقق من الإدخال
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")] // 🔹 تحديد الطول الأقصى
        public string Description { get; set; }
        [Required(ErrorMessage = "Expiry date is required.")] 
        [DataType(DataType.Date)]

        public DateTime ExpiryDate { get; set; }

        [Required(ErrorMessage = "Production date is required.")]
        [DataType(DataType.Date)]

        public DateTime ProductionDate { get; set; }

       

        public DateTime UpdatedAt { get;  set; }
    }
}