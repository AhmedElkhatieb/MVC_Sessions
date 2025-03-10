using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IKEA.DAL.Models.Departments;
using IKEA.DAL.Persistance.Repsitories._Generic;

namespace IKEA.DAL.Persistance.Repsitories.Departments
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        IEnumerable<Department> GetSpecificDepartments();
    }
}
