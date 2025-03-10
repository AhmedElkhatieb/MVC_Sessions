using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IKEA.DAL.Models.Employees;
using IKEA.DAL.Persistance.Data;
using IKEA.DAL.Persistance.Repsitories._Generic;
using Microsoft.EntityFrameworkCore;

namespace IKEA.DAL.Persistance.Repsitories.Employees
{
    internal class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            //Ask CLR For Object From ApplicationDbContext Immplicitly

        }
    }
}
