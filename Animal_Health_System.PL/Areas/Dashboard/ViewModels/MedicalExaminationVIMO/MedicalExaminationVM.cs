using Animal_Health_System.DAL.Models;

namespace Animal_Health_System.PL.Areas.Dashboard.ViewModels.MedicalExaminationVIMO
{
    public class MedicalExaminationVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ExaminationDate { get; set; }
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
        public string ExaminationType { get; set; }
        public Animal Animal { get; set; }

        public MedicalRecord MedicalRecord { get; set; }

        public Veterinarian Veterinarian { get; set; }
    }
}
