using Animal_Health_System.DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Animal_Health_System.PL.Areas.Dashboard.ViewModels.BirthVIMO
{
    public class BirthVM
    {
        public int Id { get; set; }



        public int PregnancyId { get; set; }
        public Pregnancy Pregnancy { get; set; }

        public DateTime BirthDate { get; set; }

        public int NumberOfOffspring { get; set; }


    }
}
