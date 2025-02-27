using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IKEA.BLL.Models.Departments;
using IKEA.DAL.Models.Departments;

namespace IKEA.BLL.Services.Departments
{
    public interface IDepartmentService
    {
        IEnumerable<DepartmentToReturnDto> GetAllDepartments();
        DepartmentDetailsToReturnDto GetDepartmentById(int id);
        int CreateDepartment(CreatedDepartmentDto departmentDto);
        int UpdateDepartment(UpdateDepartmentDto departmentDto);
        bool DeleteDepartment(int id);

    }
}
