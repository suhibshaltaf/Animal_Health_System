using Animal_Health_System.DAL.Models;

namespace Animal_Health_System.PL.Areas.Dashboard.ViewModels.VaccineVIMO
{
    public class VaccineDetailsVM
    {


        public int Id { get; set; }
        public string Name { get; set; }
        public string Dose { get; set; }
        public string Description { get; set; }
        public DateTime ProductionDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
