using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Animal_Health_System.PL.Areas.Dashboard.ViewModels.FarmVIMO
{
    public class FarmFormVM
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Farm name is required")]
        [RegularExpression(@"^[^\d][A-Za-z0-9\s]*$", ErrorMessage = "Name cannot start with a number")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [RegularExpression(@"^[^\d][A-Za-z0-9\s]*$", ErrorMessage = "Location cannot start with a number")]
        public string Location { get; set; }


        [Required(ErrorMessage = "Owner is required")]
        public int OwnerId { get; set; }

        public List<SelectListItem> Owners { get; set; } = new List<SelectListItem>();
    }
}
