using Animal_Health_System.DAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Animal_Health_System.PL.Areas.Dashboard.ViewModels.MedicalExaminationVIMO
{
    public class MedicalExaminationDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ExaminationDate { get; set; }
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
        public string ExaminationType { get; set; }
        public List<Medication> Medications { get; set; } = new List<Medication>();
        public Animal Animal { get; set; }

        public MedicalRecord MedicalRecord { get; set; }


        public Veterinarian Veterinarian { get; set; }
    }

}
