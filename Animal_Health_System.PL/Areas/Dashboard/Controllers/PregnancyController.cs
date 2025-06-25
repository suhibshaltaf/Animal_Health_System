using Animal_Health_System.BLL.Interface;
using Animal_Health_System.DAL.Models;
using Animal_Health_System.PL.Areas.Dashboard.ViewModels.AnimalVIMO;
using Animal_Health_System.PL.Areas.Dashboard.ViewModels.MatingVIMO;
using Animal_Health_System.PL.Areas.Dashboard.ViewModels.PregnancyVIMO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Animal_Health_System.PL.Areas.Dashboard.Controllers
{
    [Authorize]

    [Area("Dashboard")]
    public class PregnancyController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<PregnancyController> logger;

        public PregnancyController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PregnancyController> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var pregnancies = await unitOfWork.pregnancyRepository.GetAllAsync();

                // **إضافة فحص تلقائي لإنشاء سجل الولادة عند وصول تاريخ الولادة المتوقع**
                foreach (var pregnancy in pregnancies)
                {
                    if (pregnancy.ExpectedBirthDate.Date <= DateTime.UtcNow.Date)
                    {
                        var existingBirth = await unitOfWork.birthRepository.GetAsyncByPregnancyId(pregnancy.Id);
                        if (existingBirth == null)
                        {
                            var birth = new Birth
                            {
                                Name = "Birth for " + pregnancy.Animal.Name,
                                PregnancyId = pregnancy.Id,
                                BirthDate = DateTime.UtcNow,
                                NumberOfOffspring = 1,
                                BirthCondition = "Normal",
                                AnimalId = pregnancy.AnimalId ?? 0
                            };

                            await unitOfWork.birthRepository.AddAsync(birth);
                            logger.LogInformation($"Auto-created Birth Record for Pregnancy ID: {pregnancy.Id}");
                        }
                    }
                }

                if (pregnancies == null || !pregnancies.Any())
                {
                    TempData["ErrorMessage"] = "No pregnancy records found.";
                    return RedirectToAction("Index");
                }

                var pregnancyVm = mapper.Map<IEnumerable<PregnancyVM>>(pregnancies);
                return View(pregnancyVm);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching pregnancies.");
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return RedirectToAction("Index");
            }
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var vm = await PreparePregnancyFormVM();
                return View(vm);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while preparing the create view.");
                TempData["ErrorMessage"] = "An error occurred while preparing the form.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PregnancyFormVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm = await PreparePregnancyFormVM(vm);
                TempData["ErrorMessage"] = "Please correct the errors and try again.";
                return View(vm);
            }

            try
            {
                // إضافة السجل في Pregnancy
                var pregnancy = mapper.Map<Pregnancy>(vm);
                await unitOfWork.pregnancyRepository.AddAsync(pregnancy);

                TempData["SuccessMessage"] = "Pregnancy record added successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while adding the Pregnancy.");
                TempData["ErrorMessage"] = "An error occurred while adding the Pregnancy.";
                return View(vm);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var Pregnancy = await unitOfWork.pregnancyRepository.GetAsync(id);
                if (Pregnancy == null)
                {
                    TempData["ErrorMessage"] = "Pregnancy not found.";
                    return RedirectToAction(nameof(Index));
                }

                var vm = mapper.Map<PregnancyFormVM>(Pregnancy);
                vm = await PreparePregnancyFormVM(vm);  // Re-populate the select lists
                return View(vm);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while preparing the edit view.");
                TempData["ErrorMessage"] = "An error occurred while preparing the form.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PregnancyFormVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm = await PreparePregnancyFormVM(vm);  // Re-populate the select lists
                TempData["ErrorMessage"] = "Please correct the errors and try again.";
                return View(vm);
            }

            try
            {
                var pregnancy = await unitOfWork.pregnancyRepository.GetAsync(vm.Id);
                if (pregnancy == null)
                {
                    TempData["ErrorMessage"] = "Pregnancy not found.";
                    return RedirectToAction(nameof(Index));
                }

                // تحديث الحمل
                mapper.Map(vm, pregnancy);
                await unitOfWork.pregnancyRepository.UpdateAsync(pregnancy);

                TempData["SuccessMessage"] = "Pregnancy record updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating the Pregnancy.");
                TempData["ErrorMessage"] = "An error occurred while updating the Pregnancy.";
                return View(vm);
            }
        }



        private async Task<PregnancyFormVM> PreparePregnancyFormVM(PregnancyFormVM vm = null)
        {
            // Fetch animals and matings once for both create and edit
            var Animals = await unitOfWork.animalRepository.GetAllAsync();
            var matings = await unitOfWork.matingRepository.GetAllAsync();

            // Initialize or set the Animals and Matings select lists
            vm ??= new PregnancyFormVM();  // If vm is null, initialize a new one
            vm.Animals = Animals.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Name }).ToList();
            vm.Matings = matings.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Name }).ToList();

            return vm;
        }



        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var pregnancy = await unitOfWork.pregnancyRepository.GetAsync(id);
                if (pregnancy == null)
                {
                    TempData["ErrorMessage"] = "Pregnancy not found.";
                    return RedirectToAction(nameof(Index));
                }

                var pregnancyVm = mapper.Map<PregnancyDetailsVM>(pregnancy);
                return View(pregnancyVm);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving Pregnancy details.");
                TempData["ErrorMessage"] = "An error occurred while fetching Pregnancy details.";
                return RedirectToAction(nameof(Index));
            }
        }


        // Soft delete Pregnancy
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var Pregnancy = await unitOfWork.pregnancyRepository.GetAsync(id);
                if (Pregnancy == null)
                {
                    return Json(new { success = false, message = "Pregnancy not found." });
                }

                await unitOfWork.pregnancyRepository.DeleteAsync(id);
                return Json(new { success = true, message = "Animal deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while deleting the Pregnancy.");
                return Json(new { success = false, message = "An error occurred while deleting the Pregnancy." });
            }
        }
    }
}
