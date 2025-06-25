using Animal_Health_System.DAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Animal_Health_System.PL.Areas.Dashboard.ViewModels.MedicalRecordVIMO
{
    public class MedicalRecordFormVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Farm selection is required")]
        public int? FarmId { get; set; } // Make nullable

        public SelectList Farms { get; set; } = new SelectList(Enumerable.Empty<Farm>(), "Id", "Name");

        [Required(ErrorMessage = "Animal selection is required")]
        public int? AnimalId { get; set; } // Make nullable

        public SelectList Animals { get; set; } = new SelectList(Enumerable.Empty<Animal>(), "Id", "Name");
    }
}