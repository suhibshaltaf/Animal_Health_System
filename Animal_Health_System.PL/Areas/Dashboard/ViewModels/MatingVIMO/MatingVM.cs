using Animal_Health_System.DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Animal_Health_System.PL.Areas.Dashboard.ViewModels.MatingVIMO
{
    public class MatingVM
    {
        public int Id { get; set; }
        public DateTime MatingDate { get; set; }
        public string Notes { get; set; }
        public int FarmId { get; set; }
        public Farm Farm { get; set; }
    }
}
