using Animal_Health_System.DAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace Animal_Health_System.PL.Areas.Dashboard.ViewModels.MedicalRecordVIMO
{
    public class MedicalRecordVM
    {
        public int Id { get; set; }

        public string Name { get; set; }


        public int AnimalId { get; set; }

        public int FarmId { get; set; }

        public Animal Animal { get; set; }

        public Farm Farm { get; set; }


    }
}
