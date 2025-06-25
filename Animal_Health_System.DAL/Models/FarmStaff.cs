using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animal_Health_System.DAL.Models
{
    public class FarmStaff : EntityBase
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string JobTitle { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public decimal salary { get; set; }
        [ForeignKey(nameof(Farm))]

        public int FarmId { get; set; }
        public Farm Farm { get; set; }

        public DateTime? DateHired { get; set; } 
        public string TimeWorked
        {
            get
            {
                if (!DateHired.HasValue)
                    return "N/A"; // في حالة عدم توفر تاريخ التوظيف

                var hireDate = DateHired.Value;
                var today = DateTime.Today;

                int years = today.Year - hireDate.Year;
                int months = today.Month - hireDate.Month;
                int days = today.Day - hireDate.Day;

                if (days < 0)
                {
                    months--;
                    days += DateTime.DaysInMonth(today.Year, today.Month - 1);
                }
                if (months < 0)
                {
                    years--;
                    months += 12;
                }

                return $"{years} years, {months} months, {days} days";
            }
        }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }


    }


}