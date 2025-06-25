using Animal_Health_System.DAL.Models;

namespace Animal_Health_System.PL.Areas.Dashboard.ViewModels.VaccineVIMO
{
    public class VaccineVM
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Dose { get; set; }



        public DateTime ExpiryDate { get; set; }


        public string FormattedDaysUntilExpiry
        {
            get
            {
                var today = DateTime.UtcNow.Date;
                if (ExpiryDate <= today)
                    return "Expired";

                var totalDays = (ExpiryDate - today).Days;
                var years = totalDays / 365;
                var months = (totalDays % 365) / 30;
                var days = (totalDays % 365) % 30;

                if (years > 0)
                    return $"{years} year(s), {months} month(s), {days} day(s)";
                else if (months > 0)
                    return $"{months} month(s), {days} day(s)";
                else
                    return $"{days} day(s)";
            }
        }
    }
}
