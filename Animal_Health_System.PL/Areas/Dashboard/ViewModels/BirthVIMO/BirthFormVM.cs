using Animal_Health_System.DAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Animal_Health_System.PL.Areas.Dashboard.ViewModels.BirthVIMO
{
    public class BirthFormVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Birth name is required.")]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Birth date is required.")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Number of offspring is required.")]
        [Range(1, 20, ErrorMessage = "Number of offspring must be between 1 and 20.")]
        public int NumberOfOffspring { get; set; }

        [Required(ErrorMessage = "Birth condition is required.")]
        [StringLength(500, ErrorMessage = "Birth condition can't be longer than 500 characters.")]
        public string BirthCondition { get; set; }

        [Required(ErrorMessage = "Pregnancy selection is required.")]
        public int PregnancyId { get; set; }

        public List<SelectListItem> Pregnancy { get; set; } = new List<SelectListItem>();

        [Required(ErrorMessage = "Animal selection is required.")]
        public int AnimalId { get; set; }

        public List<SelectListItem> Animal { get; set; } = new List<SelectListItem>();
    }
}
