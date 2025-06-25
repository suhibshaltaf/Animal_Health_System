using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animal_Health_System.DAL.Models
{

  public  class ApplicationUser:IdentityUser
    {
        [MaxLength(100)]
        public string? FullName { get; set; }
        [MaxLength(50)]
        public string? Address { get; set; }

        public string Role { get; set; }

        public Owner? Owner { get; set; }
        public FarmStaff? FarmStaff { get; set; }
        public Veterinarian? Veterinarian { get; set; }
    }
}
