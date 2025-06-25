using Animal_Health_System.DAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Animal_Health_System.PL.Areas.Dashboard.ViewModels.FarmVIMO
{
    public class FarmVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int OwnerId { get; set; }
        public Owner  owner { get; set; }  // إظهار اسم المالك بدلاً من الكائن بالكامل

        public int AnimalCount => Animals?.Count ?? 0;
        public ICollection<Animal> Animals { get; set; } = new List<Animal>();

    }
}
