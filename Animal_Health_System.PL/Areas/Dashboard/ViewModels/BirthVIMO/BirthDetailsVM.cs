using Animal_Health_System.DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Animal_Health_System.PL.Areas.Dashboard.ViewModels.BirthVIMO
{
    public class BirthDetailsVM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [ForeignKey(nameof(Pregnancy))]

        public int PregnancyId { get; set; }
        public Pregnancy Pregnancy { get; set; }
        public int AnimalId { get; set; }
        public Animal Animal { get; set; }
        public DateTime BirthDate { get; set; }

        public int NumberOfOffspring { get; set; }

        public string BirthCondition { get; set; }

    }
}
