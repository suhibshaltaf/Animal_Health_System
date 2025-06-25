using Animal_Health_System.DAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Animal_Health_System.PL.Areas.Dashboard.ViewModels.MatingVIMO
{

    public class MatingFormVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Mating Date is required")]
        public DateTime MatingDate { get; set; }

        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string Notes { get; set; }

        [Required(ErrorMessage = "Farm is required.")]
        public int? FarmId { get; set; }

        public List<SelectListItem> Farms { get; set; } = new List<SelectListItem>();// استخدام List<SelectListItem> لعرض المزرعة في الـ View

        [Required(ErrorMessage = "Male animal is required")]
        public int MaleAnimalId { get; set; }

        [Required(ErrorMessage = "Female animal is required")]
        public int FemaleAnimalId { get; set; }

        public List<SelectListItem> FemaleAnimals { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> MaleAnimals { get; set; } = new List<SelectListItem>();


        public bool Ispregnancyevent { get; set; }

    }
}
