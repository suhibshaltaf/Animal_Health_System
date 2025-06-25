using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animal_Health_System.DAL.Models
{
    

    public class VaccineHistory : EntityBase
    {
        public int Id { get; set; }

        public string Name { get; set; }


        public DateTime AdministrationDate { get; set; }



        [ForeignKey(nameof(veterinarian))]
        public int VeterinarianId { get; set; }

      
        [ForeignKey(nameof(medicalRecord))]
        public int medicalRecordId { get; set; }


       

        [ForeignKey(nameof(vaccine))]
        public int  VaccineId { get; set; }
        public Veterinarian veterinarian { get; set; }

        public Vaccine vaccine { get; set; }

        public MedicalRecord  medicalRecord { get; set; }



    }


}
