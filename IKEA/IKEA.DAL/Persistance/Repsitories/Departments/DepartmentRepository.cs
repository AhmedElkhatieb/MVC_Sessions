using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IKEA.DAL.Models.Departments;
using IKEA.DAL.Persistance.Data;
using IKEA.DAL.Persistance.Repsitories._Generic;
using Microsoft.EntityFrameworkCore;

namespace IKEA.DAL.Persistance.Repsitories.Departments
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            //Ask CLR For Object From ApplicationDbContext Immplicitly
        }

        public IEnumerable<Department> GetSpecificDepartments()
        {
            //_DbContext
            throw new NotImplementedException();
        }
    }
}
