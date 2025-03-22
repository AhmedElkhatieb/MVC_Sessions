using IKEA.BLL.Models.Departments;
using IKEA.BLL.Models.Employees;
using IKEA.BLL.Services.Departments;
using IKEA.BLL.Services.Employees;
using IKEA.DAL.Models.Departments;
using IKEA.PL.Models.Departments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IKEA.PL.Controllers
{
    public class EmployeeController : Controller
    {
        #region Services
        private readonly IEmployeeService _employeeService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<EmployeeController> _logger;
        private readonly IDepartmentService _departmentService;

        public EmployeeController(IEmployeeService employeeService, IWebHostEnvironment webHostEnvironment, ILogger<EmployeeController> logger, IDepartmentService departmentService)
        {
            _logger = logger;
            _departmentService = departmentService;
            _webHostEnvironment = webHostEnvironment;
            _employeeService = employeeService;

        }
        #endregion
        #region Index
        [HttpGet]
        //BaseURL/Employee/Index
        public IActionResult Index(string search)
        {
            var employees = _employeeService.GetEmployees(search);
            //if (Request.IsAjaxRequest())
            //{
            //    return PartialView("EmployeeListPartial", employees);
            //}
            return View(employees);
        }
        #endregion
        #region Create
        #region Get
        [HttpGet]
        public IActionResult Create()
        {
            var departments = _departmentService.GetAllDepartments();
            ViewData["Departments"] = new SelectList(departments, "Id", "Name");
            return View();
        }
        #endregion
        #region Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreatedEmployeeDto employee)
        {
            if (!ModelState.IsValid)
            {
                return View(employee);
            }
            var message = string.Empty;
            try
            {
                var Resault = _employeeService.CreateEmployee(employee);
                if (Resault > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    message = "Sorry, The Employee has not been created";
                    ModelState.AddModelError(string.Empty, message);
                    return View(employee);
                }
            }
            catch (Exception ex)
            {
                // Log Exception
                _logger.LogError(ex, ex.Message);
                // Set Friendly Message
                if (_webHostEnvironment.IsDevelopment())
                {
                    message = ex.Message;
                    return View(employee);
                }
                else
                {
                    message = "Sorry, The Employee has not been created";
                    return View("Error", message);
                }

            }
        }
        #endregion
        #endregion
        #region Details
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            var employee = _employeeService.GetEmployeeById(id.Value);
            if (employee is null)
            {
                return NotFound();
            }
            return View(employee);
        }
        #endregion
        #region Edit
        #region Get
        [HttpGet]
        public IActionResult Edit(int? id)
        {

            if (id is null)
            {
                return BadRequest();
            }
            var employee = _employeeService.GetEmployeeById(id.Value);
            var departments = _departmentService.GetAllDepartments();
            ViewData["Departments"] = new SelectList(departments, "Id", "Name");
            if (employee is null)
            {
                return NotFound();
            }
            return View(new UpdatedEmployeeDto()
            {
                Name = employee.Name,
                Address = employee.Address,
                Email = employee.Email,
                Age = employee.Age,
                Salary = employee.Salary,
                PhoneNumber = employee.PhoneNumber,
                IsActive = employee.IsActive,
                EmployeeType = employee.EmployeeType,
                Gender = employee.Gender,
                HiringDate = employee.HiringDate
            });
        }
        #endregion
        #region Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, UpdatedEmployeeDto employee)
        {
            if (!ModelState.IsValid)
            {
                return View(employee);
            }
            var message = string.Empty;
            try
            {
                var updated = _employeeService.UpdateEmployee(employee) > 0;
                if (updated)
                {
                    return RedirectToAction(nameof(Index));
                }
                message = "Sorry, An error occured while updating the employee";

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                message = _webHostEnvironment.IsDevelopment() ? ex.Message : "Sorry, An error occured while updating the department";
            }
            ModelState.AddModelError(string.Empty, message);
            return View(employee);
        }
        #endregion
        #endregion
        #region Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var message = string.Empty;
            try
            {
                var deleted = _employeeService.DeleteEmployee(id);
                if (deleted)
                {
                    return RedirectToAction(nameof(Index));
                }
                message = "An Error Occured During Deleting The Employee";
            }
            catch (Exception ex)
            {
                // Logger
                _logger.LogError(ex, ex.Message);
                // Friendlt Message
                message = _webHostEnvironment.IsDevelopment() ? ex.Message : "An Error Occured During Deleting The Employee";

            }
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
