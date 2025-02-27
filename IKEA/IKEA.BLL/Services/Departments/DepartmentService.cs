using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IKEA.BLL.Models.Departments;
using IKEA.DAL.Models.Departments;
using IKEA.DAL.Persistance.Repsitories.Departments;
using Microsoft.EntityFrameworkCore;

namespace IKEA.BLL.Services.Departments
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository) 
        {
            _departmentRepository = departmentRepository;
        }
        public IEnumerable<DepartmentToReturnDto> GetAllDepartments()
        {
            //var departments = _departmentRepository.GetAll();
            //foreach (var department in departments)
            //{
            //    // Manual Mapping
            //    yield return new DepartmentToReturnDto()
            //    {
            //        Id = department.Id,
            //        Code = department.Code,
            //        Name = department.Name,
            //        Description = department.Description,
            //        CreationDate = department.CreationDate
            //    };
            //}
            var departments = _departmentRepository.GetAllAsQueryable()
                .Select(department => new DepartmentToReturnDto
                {
                    Id = department.Id,
                    Code = department.Code,
                    Name = department.Name,
                    Description = department.Description,
                    CreationDate = department.CreationDate
                }).AsNoTracking().ToList();
            return departments;
        }

        public DepartmentDetailsToReturnDto GetDepartmentById(int id)
        {
            var department = _departmentRepository.GetById(id);
            if (department is { })
            {
                return new DepartmentDetailsToReturnDto()
                {
                    Id = department.Id,
                    Code = department.Code,
                    Name = department.Name,
                    Description = department.Description,
                    CreationDate = department.CreationDate,
                    CreatedBy = department.CreatedBy,
                    CreatedOn = department.CreatedOn,
                    LastModificationBy = department.LastModificationBy,
                    LastModificationOn = department.LastModificationOn
                };
            }
            return null;
            
        }

        public int CreateDepartment(CreatedDepartmentDto departmentDto)
        {
            var CreatedDepartment = new Department
            {
                Code = departmentDto.Code,
                Name = departmentDto.Name,
                Description = departmentDto.Description,
                CreationDate = departmentDto.CreationDate,
                CreatedBy = 1,
                LastModificationBy = 1,
                LastModificationOn = DateTime.UtcNow
            };
            return _departmentRepository.Add(CreatedDepartment);
        }

        public int UpdateDepartment(UpdateDepartmentDto departmentDto)
        {
            var updatedDepartment = new Department()
            {
                Id = departmentDto.Id,
                Code = departmentDto.Code,
                Name = departmentDto.Name,
                Description = departmentDto.Description,
                CreationDate = departmentDto.CreationDate,
                CreatedBy = 1,
                LastModificationBy = 1,
                LastModificationOn = DateTime.UtcNow
            };
            return _departmentRepository.Update(updatedDepartment);
        }

        public bool DeleteDepartment(int id)
        {
            var deletedDepartment = _departmentRepository.GetById(id);
            if (deletedDepartment is { })
            {
                return _departmentRepository.Delete(deletedDepartment) > 0;
            }
            return false;
        }
    }
}
