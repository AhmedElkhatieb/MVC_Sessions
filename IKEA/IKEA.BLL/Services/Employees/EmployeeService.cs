using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IKEA.BLL.Models.Employees;
using IKEA.DAL.Models.Employees;
using IKEA.DAL.Persistance.Data;
using IKEA.DAL.Persistance.Repsitories.Employees;
using IKEA.DAL.Persistance.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace IKEA.BLL.Services.Employees
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeService(IUnitOfWork unitOfWork)
        {
            //Ask CLR for creating object from class implementing IUnitOfWork
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<EmployeeDto> GetEmployees(string search)
        {
            return _unitOfWork.EmployeeRepository.GetAllAsQueryable()
                .Where(E => !E.IsDeleted && (string.IsNullOrEmpty(search) || E.Name.ToLower().Contains(search.ToLower())))
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
                }).ToList();
        }
        public EmployeeDetailsDto? GetEmployeeById(int id)
        {
            var employeeDto = _unitOfWork.EmployeeRepository.GetById(id);
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
            _unitOfWork.EmployeeRepository.Add(employee);
            return _unitOfWork.Complete();
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
            _unitOfWork.EmployeeRepository.Update(employee);
            return _unitOfWork.Complete();
        }
        public bool DeleteEmployee(int id)
        {
            var employee = _unitOfWork.EmployeeRepository.GetById(id);
            if (employee is { })
            {
                _unitOfWork.EmployeeRepository.Delete(employee);
            }
            return _unitOfWork.Complete() > 0 ;
        }

    }
}
