using Animal_Health_System.DAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Animal_Health_System.PL.Areas.Dashboard.ViewModels.MedicalExaminationVIMO
{
    public class MedicalExaminationFormVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The name of the examination is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The examination date is required.")]
        public DateTime ExaminationDate { get; set; }

        [Required(ErrorMessage = "Diagnosis is required.")]
        public string Diagnosis { get; set; }

        [Required(ErrorMessage = "Examination type is required.")]
        public string ExaminationType { get; set; }

        [Required(ErrorMessage = "Treatment is required.")]
        public string Treatment { get; set; }

        [Required(ErrorMessage = "At least one medication must be selected.")]
        public List<int> SelectedMedications { get; set; } = new List<int>();

        // ✅ إضافة الخاصية لحل الخطأ
        public SelectList? MedicationsList { get; set; }

        [Required(ErrorMessage = "Animal is required.")]
        public int? AnimalId { get; set; }
        public SelectList? Animal { get; set; }

        [Required(ErrorMessage = "Medical record is required.")]
        public int? MedicalRecordId { get; set; }
        public SelectList? MedicalRecord { get; set; }

        [Required(ErrorMessage = "Veterinarian is required.")]
        public int? VeterinarianId { get; set; }
        public SelectList? Veterinarian { get; set; }

        public int? FarmId { get; set; }
        public SelectList? Farm { get; set; }
    }
}
