using Animal_Health_System.BLL.Interface;
using Animal_Health_System.DAL.Models;
using Animal_Health_System.PL.Areas.Dashboard.ViewModels.FarmVIMO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Animal_Health_System.PL.Areas.Dashboard.Controllers
{
    [Authorize]

    [Area("Dashboard")]
    public class FarmController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<FarmController> logger;

        public FarmController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<FarmController> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var farms = await unitOfWork.farmRepository.GetAllAsync();
            var farmVm = mapper.Map<IEnumerable<FarmVM>>(farms);
            return View(farmVm);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var owners = await unitOfWork.ownerRepository.GetAllAsync();
            var vm = new FarmFormVM
            {
                Owners = owners.Select(o => new SelectListItem
                {
                    Value = o.Id.ToString(),
                    Text = o.FullName
                }).ToList()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FarmFormVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Owners = (await unitOfWork.ownerRepository.GetAllAsync())
                    .Select(o => new SelectListItem
                    {
                        Value = o.Id.ToString(),
                        Text = o.FullName
                    }).ToList();
                return View(vm);
            }

            bool exists = await unitOfWork.farmRepository.GetAllAsync()
                .ContinueWith(t => t.Result.Any(f => f.Name == vm.Name));

            if (exists)
            {
                ModelState.AddModelError("Name", "A farm with this name already exists.");
                vm.Owners = (await unitOfWork.ownerRepository.GetAllAsync())
                    .Select(o => new SelectListItem
                    {
                        Value = o.Id.ToString(),
                        Text = o.FullName
                    }).ToList();
                return View(vm);
            }

            var farm = mapper.Map<Farm>(vm);
            await unitOfWork.farmRepository.AddAsync(farm);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var farm = await unitOfWork.farmRepository.GetAsync(id);
            if (farm == null) return NotFound();

            var owners = await unitOfWork.ownerRepository.GetAllAsync();
            var vm = mapper.Map<FarmFormVM>(farm);
            vm.Owners = owners.Select(o => new SelectListItem
            {
                Value = o.Id.ToString(),
                Text = o.FullName,
                Selected = (o.Id == farm.Id)
            }).ToList();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(FarmFormVM vm)
        {
            var farm = await unitOfWork.farmRepository.GetAsync(vm.Id);
            if (farm == null) return NotFound();

            if (!ModelState.IsValid)
            {
                vm.Owners = (await unitOfWork.ownerRepository.GetAllAsync())
                    .Select(o => new SelectListItem
                    {
                        Value = o.Id.ToString(),
                        Text = o.FullName,
                        Selected = (o.Id == vm.Id)
                    }).ToList();
                return View(vm);
            }

            bool exists = await unitOfWork.farmRepository.GetAllAsync()
                .ContinueWith(t => t.Result.Any(f => f.Name == vm.Name && f.Id != vm.Id));

            if (exists)
            {
                ModelState.AddModelError("Name", "A farm with this name already exists.");
                vm.Owners = (await unitOfWork.ownerRepository.GetAllAsync())
                    .Select(o => new SelectListItem
                    {
                        Value = o.Id.ToString(),
                        Text = o.FullName,
                        Selected = (o.Id == vm.OwnerId)
                    }).ToList();
                return View(vm);
            }

            mapper.Map(vm, farm);
            await unitOfWork.farmRepository.UpdateAsync(farm);
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var farm = await unitOfWork.farmRepository.GetAsync(id);
            if (farm == null) return NotFound();

            var model = mapper.Map<FarmDetailsVM>(farm);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var farm = await unitOfWork.farmRepository.GetAsync(id);
            if (farm == null) return NotFound();

            await unitOfWork.farmRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
