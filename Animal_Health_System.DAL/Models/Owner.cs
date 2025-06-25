using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animal_Health_System.DAL.Models
{
    public class Owner : EntityBase
    {
        public int Id { get; set; }


        public string FullName { get; set; }


        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<Farm> Farms { get; set; } = new List<Farm>();



    }
}