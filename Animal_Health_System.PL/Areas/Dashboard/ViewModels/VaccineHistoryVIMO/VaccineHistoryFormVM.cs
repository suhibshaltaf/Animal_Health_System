using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Animal_Health_System.DAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Animal_Health_System.PL.Areas.Dashboard.ViewModels.VaccineHistoryVIMO
{
    public class VaccineHistoryFormVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        [StringLength(100, ErrorMessage = "The Name can't be longer than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Veterinarian field is required.")]
        public int VeterinarianId { get; set; }


        [Required(ErrorMessage = "The Vaccine field is required.")]
        public int VaccineId { get; set; }


        [Required(ErrorMessage = "The Farm field is required.")]
        public int? FarmId { get; set; }


        public int? AnimalId { get; set; }


        public int? MedicalRecordId { get; set; }


        [Required(ErrorMessage = "The Date field is required.")]
        [DataType(DataType.Date)]
        public DateTime AdministrationDate { get; set; }


        public IEnumerable<SelectListItem> Veterinarians { get; set; }
        public IEnumerable<SelectListItem> Vaccines { get; set; }
        public IEnumerable<SelectListItem> Farms { get; set; }
        public IEnumerable<SelectListItem> Animals { get; set; }
        public IEnumerable<SelectListItem> MedicalRecords { get; set; }
    }
}
