using Animal_Health_System.DAL.Models;

namespace Animal_Health_System.PL.Areas.Dashboard.ViewModels.FarmStaffVIMO
{
    public class FarmStaffDetailsVM
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string JobTitle { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public decimal Salary { get; set; }
        public string DateHired { get; set; }
        public string TimeWorked { get; set; }  
        
        public int FarmId { get; set; }
        public Farm Farm { get; set; }
    }
}
