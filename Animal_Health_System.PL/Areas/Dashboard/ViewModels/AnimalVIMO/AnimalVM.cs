using Animal_Health_System.DAL.Models;


namespace Animal_Health_System.PL.Areas.Dashboard.ViewModels.AnimalVIMO
{
    public class AnimalVM
        {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Species { get; set; }

        public string Breed { get; set; }

        public decimal Weight { get; set; }

        public string Age { get; set; }


        public Gender Gender { get; set; }

        public int FarmId { get; set; }

        public Farm Farm { get; set; }
    }
    }
