using AutoMapper;
using IKEA.BLL.Models.Departments;
using IKEA.BLL.Services.Departments;
using IKEA.PL.Models.Departments;
using Microsoft.AspNetCore.Mvc;

namespace IKEA.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly IMapper _mapper;

        #region Services
        public DepartmentController(IDepartmentService departmentService, ILogger<DepartmentController> logger, IWebHostEnvironment environment, IMapper mapper)
        {
            _departmentService = departmentService;
            _logger = logger;
            _environment = environment;
            _mapper = mapper;
        }
        #endregion
        #region Index
        [HttpGet]
        //BaseURL/Department/Index
        public async Task<IActionResult> Index()
        {
            // Notes
            // View Storage Dictionary: Pass Data From Controller [Action] To View
            // From This View i can send message to partial view or layout
            // 1- ViewData: Is a dictionary type property introduced in ASP.Net FrameWork 3.5
                // It helps us to transfer data from controller [Action] to view
           // 2- ViewBag: is a dynamic type property introduced in 4.0 based on dynamic property
                // it helps us to transfer the data from Action to View
            ViewData["Message"] = "Hello View Data";
            ViewBag.Message = "Hello View Bag";
            var departments = await _departmentService.GetAllDepartmentsAsync();
            return View(departments);
        }
        #endregion
        #region Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var departmentVM = new DepartmentEditViewModel();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentEditViewModel departmentVM)
        {
            if (!ModelState.IsValid)
            {
                return View(departmentVM);
            }
            var message = string.Empty;
            try
            {
                var createdDepartment = _mapper.Map<CreatedDepartmentDto>(departmentVM);
                var Resault = await _departmentService.CreateDepartmentAsync(createdDepartment);
                // Temp Data: is a property of type dictionary object introduced in 3.5
                // is used for transfering the data between 2 requests
                if (Resault > 0)
                {
                    TempData["Message"] = "Department Is Created";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Message"] = "Department Isn't Created";
                    message = "Sorry, The Department has not been created";
                    ModelState.AddModelError(string.Empty, message);
                    return View(departmentVM);
                }

            }
            catch (Exception ex)
            {
                // Log Exception
                _logger.LogError(ex, ex.Message);
                // Set Friendly Message
                if (_environment.IsDevelopment())
                {
                    message = ex.Message;
                    return View(departmentVM);
                }
                else
                {
                    message = "Sorry, The Department has not been created";
                    return View("Error", message);
                }

            }
        }
        #endregion
        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            var department = await _departmentService.GetDepartmentByIdAsync(id.Value);
            if (department is null)
            {
                return NotFound();
            }
            return View(department);
        }
        #endregion
        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            var department = await _departmentService.GetDepartmentByIdAsync(id.Value);
            if (department is null)
            {
                return NotFound();
            }
            var departmentVM = _mapper.Map<DepartmentDetailsToReturnDto, DepartmentEditViewModel>(department);
            return View(departmentVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DepartmentEditViewModel departmentVM)
        {
            if (!ModelState.IsValid)
            {
                return View(departmentVM);
            }
            var message = string.Empty;
            try
            {
                //// Manual Mapping
                //var updatedDepartment = new UpdateDepartmentDto()
                //{
                //    Id = id,
                //    Code = departmentVM.Code,
                //    Name = departmentVM.Name,
                //    CreationDate = departmentVM.CreationDate,
                //    Description = departmentVM.Description
                //};
                var updatedDepartment = _mapper.Map<UpdateDepartmentDto>(departmentVM);
                var result = await _departmentService.UpdateDepartmentAsync(updatedDepartment);
                if (result > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                message = "Sorry, An error occured while updating the department";

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                message = _environment.IsDevelopment() ? ex.Message : "Sorry, An error occured while updating the department";
            }
            ModelState.AddModelError(string.Empty, message);
            return View(departmentVM);
        }
        #endregion
        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            var department = await _departmentService.GetDepartmentByIdAsync(id.Value);
            if (department is null)
            {
                return NotFound();
            }
            return View(department);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var message = string.Empty;
            try
            {
                var deleted = await _departmentService.DeleteDepartmentAsync(id);
                if (deleted)
                {
                    return RedirectToAction(nameof(Index));
                }
                message = "An Error Occured During Deleting The Department";
            }
            catch (Exception ex)
            {
                // Logger
                _logger.LogError(ex, ex.Message);
                // Friendlt Message
                message =  _environment.IsDevelopment() ? ex.Message : "An Error Occured During Deleting The Department";

            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

    }
}
