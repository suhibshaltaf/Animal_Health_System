using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animal_Health_System.DAL.Models
{
    public enum PregnancyStatus
    {
        NotPregnant,
        Pregnant,
        Complications
    }

    public class Pregnancy : EntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime MatingDate { get; set; }
        public DateTime ExpectedBirthDate { get; set; }
        public DateTime? ActualBirthDate { get; set; }
        public PregnancyStatus Status { get; set; }
        public bool HasComplications { get; set; }
        public string Notes { get; set; }
        public ICollection<Birth> Births { get; set; } = new List<Birth>();

        // العلاقة مع الحيوان (الأنثى)
        [ForeignKey(nameof(Animal))]
        public int? AnimalId { get; set; }
        public Animal Animal { get; set; }

        // ✅ العلاقة مع التزاوج
        [ForeignKey(nameof(Mating))]
        public int MatingId { get; set; } 
        public Mating Mating { get; set; }

        public static string CalculatePregnancyDuration(Pregnancy pregnancy)
        {
            if (pregnancy.MatingDate != default && pregnancy.ExpectedBirthDate != default)
            {
                var duration = pregnancy.ExpectedBirthDate - pregnancy.MatingDate;
                return GetFormattedDuration(duration);
            }
            return "Not Available";
        }

        public static string CalculateTimeToBirth(DateTime expectedBirthDate)
        {
            if (expectedBirthDate != default)
            {
                var duration = expectedBirthDate - DateTime.UtcNow;
                return GetFormattedDuration(duration);
            }
            return "Not Available";
        }

        private static string GetFormattedDuration(TimeSpan duration)
        {
            int years = (int)(duration.Days / 365);
            int months = (int)((duration.Days % 365) / 30);
            int days = (int)((duration.Days % 365) % 30);

            return $"{years} years, {months} months, {days} days";
        }



    }


}
