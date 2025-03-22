using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IKEA.BLL.Models.Departments;
using IKEA.DAL.Models.Departments;
using IKEA.DAL.Persistance.Repsitories.Departments;
using IKEA.DAL.Persistance.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace IKEA.BLL.Services.Departments
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
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
            var departments = _unitOfWork.DepartmentRepository.GetAllAsQueryable()
                .Select(department => new DepartmentToReturnDto
                {
                    Id = department.Id,
                    Code = department.Code,
                    Name = department.Name,
                    CreationDate = department.CreationDate
                }).AsNoTracking().ToList();
            return departments;
        }

        public DepartmentDetailsToReturnDto GetDepartmentById(int id)
        {
            var department = _unitOfWork.DepartmentRepository.GetById(id);
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
            _unitOfWork.DepartmentRepository.Add(CreatedDepartment);
            return _unitOfWork.Complete();
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
            _unitOfWork.DepartmentRepository.Update(updatedDepartment);
            return _unitOfWork.Complete();
        }

        public bool DeleteDepartment(int id)
        {
            var deletedDepartment = _unitOfWork.DepartmentRepository.GetById(id);
            if (deletedDepartment is { })
            {
                _unitOfWork.DepartmentRepository.Delete(deletedDepartment);
            }
            return _unitOfWork.Complete() > 0;
        }
    }
}
