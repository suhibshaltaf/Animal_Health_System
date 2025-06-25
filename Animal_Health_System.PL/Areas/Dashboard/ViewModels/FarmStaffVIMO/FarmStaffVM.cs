using Animal_Health_System.DAL.Models;

namespace Animal_Health_System.PL.Areas.Dashboard.ViewModels.FarmStaffVIMO
{
    public class FarmStaffVM
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string JobTitle { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int FarmId { get; set; }
        public Farm Farm { get; set; }
      
    }
}
