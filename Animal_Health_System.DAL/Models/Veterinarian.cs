using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animal_Health_System.DAL.Models
{
    public class Veterinarian : EntityBase
    {
        public int Id { get; set; }


        public string FullName { get; set; }

        public string Specialty { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public int YearsOfExperience { get; set; }


        public decimal salary { get; set; }



        public ICollection<MedicalExamination> MedicalExaminations { get; set; } = new List<MedicalExamination>();

        public ICollection<VaccineHistory> VaccineHistories { get; set; } = new List<VaccineHistory>();


        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}