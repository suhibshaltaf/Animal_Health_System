using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animal_Health_System.DAL.Models
{
    public class Birth : EntityBase
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [ForeignKey(nameof(Pregnancy))]

        public int PregnancyId { get; set; }
        public   Pregnancy Pregnancy { get; set; }

        public DateTime BirthDate { get; set; }

        public int NumberOfOffspring { get; set; }

        public string BirthCondition { get; set; }
        [ForeignKey(nameof(Animal))]
        public int AnimalId { get; set; }
        public Animal Animal { get; set; }



    }

}
