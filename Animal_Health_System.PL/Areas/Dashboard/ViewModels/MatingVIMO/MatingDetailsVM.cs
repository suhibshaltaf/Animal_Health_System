using Animal_Health_System.DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Animal_Health_System.PL.Areas.Dashboard.ViewModels.MatingVIMO
{
    public class MatingDetailsVM
    {
        public int Id { get; set; }

        public string Name { get; set; }


        public DateTime MatingDate { get; set; }


        public string Notes { get; set; }

        public int FarmId { get; set; }
        public Farm Farm { get; set; }

        public int MaleAnimalId { get; set; }
        public Animal MaleAnimal { get; set; }

        public int FemaleAnimalId { get; set; }
        public Animal FemaleAnimal { get; set; }

        public bool Ispregnancyevent { get; set; }

    }
}
