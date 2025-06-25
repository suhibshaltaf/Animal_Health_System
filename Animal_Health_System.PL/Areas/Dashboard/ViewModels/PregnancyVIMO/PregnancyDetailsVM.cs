using Animal_Health_System.DAL.Models;
using Animal_Health_System.PL.Areas.Dashboard.ViewModels.BirthVIMO;

namespace Animal_Health_System.PL.Areas.Dashboard.ViewModels.PregnancyVIMO
{
    public class PregnancyDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime MatingDate { get; set; }
        public DateTime ExpectedBirthDate { get; set; }
        public DateTime? ActualBirthDate { get; set; }
        public string Status { get; set; }
        public bool HasComplications { get; set; }
        public string Notes { get; set; }

        // علاقات البيانات
        public int? AnimalId { get; set; }
        public Animal Animal { get; set; }
        public int MatingId { get; set; }
        public Mating  mating { get; set; }

        // معلومات محسوبة
        public string PregnancyDurationText { get; set; }
        public string TimeToBirthText { get; set; }

        // قائمة المواليد إذا كانت موجودة
        public List<BirthVM> Births { get; set; } = new List<BirthVM>();
    }
}
