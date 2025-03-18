using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IKEA.DAL.Models.Employees;
using IKEA.DAL.Persistance.Repsitories._Generic;

namespace IKEA.DAL.Persistance.Repsitories.Employees
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        
    }
}
