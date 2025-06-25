using Animal_Health_System.DAL.Models;
using Animal_Health_System.PL.Areas.Dashboard.ViewModels.MedicalExaminationVIMO;
using Animal_Health_System.PL.Areas.Dashboard.ViewModels.VaccineHistoryVIMO;

namespace Animal_Health_System.PL.Areas.Dashboard.ViewModels.VeterinarianVIMO
{
    public class VeterinarianDetailsVM
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Specialty { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int YearsOfExperience { get; set; }
        public decimal Salary { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<MedicalExaminationVM> MedicalExaminations { get; set; }
        public ICollection<VaccineHistoryVM> VaccineHistories { get; set; }


    }
}
