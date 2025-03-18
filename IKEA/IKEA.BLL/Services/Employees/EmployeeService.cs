using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IKEA.BLL.Models.Employees;
using IKEA.DAL.Models.Employees;
using IKEA.DAL.Persistance.Data;
using IKEA.DAL.Persistance.Repsitories.Employees;
using Microsoft.EntityFrameworkCore;

namespace IKEA.BLL.Services.Employees
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public IEnumerable<EmployeeDto> GetAllEmployees()
        {
            return _employeeRepository.GetAllAsQueryable()
                .Where(E =>!E.IsDeleted)
                .Include(E => E.Department)
                .Select(employeeDto => new EmployeeDto
            {
                Id = employeeDto.Id,
                Name = employeeDto.Name,
                Age = employeeDto.Age,
                IsActive = employeeDto.IsActive,
                Salary = employeeDto.Salary,
                Email = employeeDto.Email,
                Gender = employeeDto.Gender.ToString(),
                EmployeeType = employeeDto.EmployeeType.ToString(),
                Department = employeeDto.Department.Name
            });
        }

        public EmployeeDetailsDto? GetEmployeeById(int id)
        {
            var employeeDto = _employeeRepository.GetById(id);
            if (employeeDto is { })
            {
                return new EmployeeDetailsDto()
                {
                    Id = employeeDto.Id,
                    Name = employeeDto.Name,
                    Age = employeeDto.Age,
                    Address = employeeDto.Address,
                    IsActive = employeeDto.IsActive,
                    Salary = employeeDto.Salary,
                    Email = employeeDto.Email,
                    PhoneNumber = employeeDto.PhoneNumber,
                    HiringDate = employeeDto.HiringDate,
                    Gender = employeeDto.Gender,
                    EmployeeType = employeeDto.EmployeeType,
                    Department = employeeDto.Department?.Name ?? "No Department"
                };
            }
            return null;
        }

        public int CreateEmployee(CreatedEmployeeDto employeeDto)
        {
            var employee = new Employee()
            {
                Name = employeeDto.Name,
                Age = employeeDto.Age,
                Address = employeeDto.Address,
                IsActive = employeeDto.IsActive,
                Salary = employeeDto.Salary,
                Email = employeeDto.Email,
                PhoneNumber = employeeDto.PhoneNumber,
                HiringDate = employeeDto.HiringDate,
                Gender = employeeDto.Gender,
                EmployeeType = employeeDto.EmployeeType,
                DepartmentId = employeeDto.DepartmentId,
                CreatedBy = 1,
                LastModificationBy = 1,
                LastModificationOn = DateTime.UtcNow
            };
            return _employeeRepository.Add(employee);
        }
        public int UpdateEmployee(UpdatedEmployeeDto employeeDto)
        {
            var employee = new Employee()
            {
                Id = employeeDto.Id,
                Name = employeeDto.Name,
                Age = employeeDto.Age,
                Address = employeeDto.Address,
                IsActive = employeeDto.IsActive,
                Salary = employeeDto.Salary,
                Email = employeeDto.Email,
                PhoneNumber = employeeDto.PhoneNumber,
                HiringDate = employeeDto.HiringDate,
                Gender = employeeDto.Gender,
                EmployeeType = employeeDto.EmployeeType,
                DepartmentId = employeeDto.DepartmentId,
                CreatedBy = 1,
                LastModificationBy = 1,
                LastModificationOn = DateTime.UtcNow
            };
            return _employeeRepository.Update(employee);
        }

        public bool DeleteEmployee(int id)
        {
            var employee = _employeeRepository.GetById(id);
            if (employee is { })
            {
                return _employeeRepository.Delete(employee) > 0;
            }
            return false;
        }

    }
}
