namespace Animal_Health_System.PL.Areas.Dashboard.ViewModels.PregnancyVIMO
{
    public class PregnancyVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ExpectedBirthDate { get; set; }
        public string Status { get; set; }
        public string PregnancyDurationText { get; set; }
        public string TimeToBirthText { get; set; }
    }
}
