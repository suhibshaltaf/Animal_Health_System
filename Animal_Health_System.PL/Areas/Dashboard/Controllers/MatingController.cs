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
        public class MatingController : Controller
        {
            private readonly IUnitOfWork unitOfWork;
            private readonly IMapper mapper;
            private readonly ILogger<MatingController> logger;

            public MatingController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<MatingController> logger)
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
                    // تأكد من تحميل الـ Farm بشكل صحيح
                    var matings = await unitOfWork.matingRepository.GetAllAsync();

                    // تحقق من أن المزرعة تم تحميلها بشكل صحيح
                    foreach (var mating in matings)
                    {
                        if (mating.Farm == null)
                        {
                            mating.Farm = new Farm { Name = "No Farm" }; // يمكنك تحديد قيمة افتراضية هنا
                        }
                    }

                    // تحويل الكائنات إلى ViewModel باستخدام AutoMapper
                    var matingVm = mapper.Map<IEnumerable<MatingVM>>(matings);
                    return View(matingVm);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while fetching matings.");
                    TempData["ErrorMessage"] = "An error occurred while loading the mating data.";
                    return RedirectToAction("Index");
                }
            }



            [HttpGet]
            public async Task<IActionResult> Create()
            {
                try
                {
                    var farms = await unitOfWork.farmRepository.GetAllAsync();
                    var vm = new MatingFormVM
                    {
                        Farms = farms.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Name }).ToList()
                    };

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
            public async Task<IActionResult> Create(MatingFormVM vm)
            {
                if (!ModelState.IsValid)
                {
                    var farms = await unitOfWork.farmRepository.GetAllAsync();
                    vm.Farms = farms.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Name }).ToList();
                    TempData["ErrorMessage"] = "Please correct the errors and try again.";
                    return View(vm);
                }

                try
                {
                    var mating = mapper.Map<Mating>(vm);
                    await unitOfWork.matingRepository.AddAsync(mating);
                    await unitOfWork.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Mating added successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error occurred while adding the mating.");
                    TempData["ErrorMessage"] = "An error occurred while adding the mating.";
                    return View(vm);
                }
            }
            
            [HttpGet]
            public async Task<IActionResult> Edit(int id)
            {
                try
                {
                    var mating = await unitOfWork.matingRepository.GetAsync(id);
                    if (mating == null)
                    {
                        TempData["ErrorMessage"] = "Mating not found.";
                        return RedirectToAction(nameof(Index));
                    }

                    var farms = await unitOfWork.farmRepository.GetAllAsync();
                    var animals = await unitOfWork.animalRepository.GetAnimalsByFarmIdAsync(mating.FarmId);

                    var vm = mapper.Map<MatingFormVM>(mating);
                    vm.FarmId = mating.FarmId;
                    vm.Farms = farms.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Name }).ToList();

                    // تصنيف الحيوانات حسب الجنس
                    vm.MaleAnimals = animals
                        .Where(a => a.Gender == Gender.Male)
                        .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name })
                        .ToList();

                    vm.FemaleAnimals = animals
                        .Where(a => a.Gender == Gender.Female)
                        .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name })
                        .ToList();

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
            public async Task<IActionResult> Edit(MatingFormVM vm)
            {
                // التأكد من تعبئة حقل المزرعة
                if (vm.FarmId == 0)
                {
                    ModelState.AddModelError("FarmId", "Farm is required.");
                }

                // إعادة تحميل بيانات المزارع
                var farms = await unitOfWork.farmRepository.GetAllAsync();
                vm.Farms = farms.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Name }).ToList();

                // إعادة تحميل الحيوانات بناءً على المزرعة المحددة
                if (vm.FarmId.HasValue && vm.FarmId.Value > 0)
                {
                    var animals = await unitOfWork.animalRepository.GetAnimalsByFarmIdAsync(vm.FarmId.Value);
                    vm.MaleAnimals = animals
                        .Where(a => a.Gender == Gender.Male)
                        .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name })
                        .ToList();

                    vm.FemaleAnimals = animals
                        .Where(a => a.Gender == Gender.Female)
                        .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name })
                        .ToList();
                }
                else
                {
                    vm.MaleAnimals = new List<SelectListItem>();
                    vm.FemaleAnimals = new List<SelectListItem>();
                }

                // إذا كان هناك أخطاء في الإدخال، لا تكمل العملية وأعد النموذج مع الرسالة
                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessage"] = "Please correct the errors and try again.";
                    return View(vm);
                }

                try
                {
                    var mating = await unitOfWork.matingRepository.GetAsync(vm.Id);
                    if (mating == null)
                    {
                        TempData["ErrorMessage"] = "Mating not found.";
                        return RedirectToAction(nameof(Index));
                    }

                    // تحديث بيانات التزاوج بناءً على الفورم
                    mapper.Map(vm, mating);
                    await unitOfWork.matingRepository.UpdateAsync(mating);

                    // التحقق من إضافة الحمل بناءً على خيار "IsPregnancyEvent"
                    if (vm.Ispregnancyevent)
                    {
                        // البحث عن حمل سابق لهذا التزاوج
                        var existingPregnancy = await unitOfWork.pregnancyRepository.FindAsync(p => p.MatingId == mating.Id);

                        if (existingPregnancy == null)
                        {
                            // إضافة حمل جديد
                            var pregnancy = new Pregnancy
                            {
                                Name = $"Pregnancy for {mating.Name}",
                                MatingDate = mating.MatingDate,
                                ExpectedBirthDate = mating.MatingDate.AddMonths(9),
                                ActualBirthDate = new DateTime(2026, 1, 1),
                                HasComplications = false,
                                Status = PregnancyStatus.Pregnant,
                                Notes = "sasasas",
                                AnimalId = mating.FemaleAnimalId,  // ربط الحمل بالحيوان الأنثى
                                MatingId = mating.Id,
                            };

                            // إضافة الحمل إلى قاعدة البيانات
                            await unitOfWork.pregnancyRepository.AddAsync(pregnancy);
                        }
                        await unitOfWork.SaveChangesAsync();

                    }

                    TempData["SuccessMessage"] = "Mating updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error occurred while updating the mating.");
                    TempData["ErrorMessage"] = "An error occurred while updating the mating: " + ex.Message;
                    return View(vm);
                }
            }





            [HttpGet]
            public async Task<IActionResult> Details(int id)
            {
                try
                {
                    // استرجاع الكائن مع تضمين العلاقات المرتبطة
                    var mating = await unitOfWork.matingRepository.GetWithRelationsAsync(id);
                    if (mating == null)
                    {
                        TempData["ErrorMessage"] = "Mating not found.";
                        return RedirectToAction(nameof(Index));
                    }

                    var matingVm = mapper.Map<MatingDetailsVM>(mating);
                    return View(matingVm);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error occurred while retrieving mating details.");
                    TempData["ErrorMessage"] = "An error occurred while fetching mating details.";
                    return RedirectToAction(nameof(Index));
                }
            }

            // Soft delete mating
            [HttpPost]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                try
                {
                    var mating = await unitOfWork.matingRepository.GetAsync(id);
                    if (mating == null)
                    {
                        return Json(new { success = false, message = "Animal not found." });
                    }

                    await unitOfWork.matingRepository.DeleteAsync(id);
                    return Json(new { success = true, message = "Animal deleted successfully." });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while deleting the mating.");
                    return Json(new { success = false, message = "An error occurred while deleting the mating." });
                }
            }

            // Filter animals based on selected farm
            [HttpGet]
            public async Task<IActionResult> GetAnimalsByFarm(int farmId)
            {
                try
                {
                    var animals = await unitOfWork.animalRepository.GetAnimalsByFarmIdAsync(farmId);

                    var maleAnimals = animals
                        .Where(a => a.Gender == Gender.Male)
                        .Select(a => new { value = a.Id, text = a.Name })
                        .ToList();

                    var femaleAnimals = animals
                        .Where(a => a.Gender == Gender.Female)
                        .Select(a => new { value = a.Id, text = a.Name })
                        .ToList();

                    return Json(new { males = maleAnimals, females = femaleAnimals });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Error fetching animals for farm ID: {farmId}");
                    return Json(new { males = new List<object>(), females = new List<object>() });
                }
            }
            




        }
    }