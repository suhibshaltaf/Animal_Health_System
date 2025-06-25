using Animal_Health_System.DAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace Animal_Health_System.PL.Areas.Dashboard.ViewModels.MedicalRecordVIMO
{
    public class MedicalRecordDetailsVM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; } // أضف set هنا

        public int AnimalId { get; set; }
        public Animal Animal { get; set; }

        public int FarmId { get; set; }
        public Farm Farm { get; set; }

        public ICollection<MedicalExamination> Examinations { get; set; } = new List<MedicalExamination>();

        public ICollection<VaccineHistory>  vaccineHistories { get; set; } = new List<VaccineHistory>();
    }
}
