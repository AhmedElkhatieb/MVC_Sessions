using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IKEA.DAL.Models;
using IKEA.DAL.Models.Departments;
using IKEA.DAL.Persistance.Data;
using Microsoft.EntityFrameworkCore;

namespace IKEA.DAL.Persistance.Repsitories._Generic
{
    public class GenericRepository<T> : IGenericRepository<T> where T : ModelBase
    {
        private protected readonly ApplicationDbContext _DbContext;
        public GenericRepository(ApplicationDbContext dbContext)
        {
            _DbContext = dbContext;
        }
        public IEnumerable<T> GetAll(bool WithNoTracking = true)
        {
            if (WithNoTracking)
            {
                return _DbContext.Set<T>().Where(X => !X.IsDeleted).AsNoTracking().ToList();
            }
            return _DbContext.Set<T>().Where(X => !X.IsDeleted);
        }

        public IQueryable<T> GetAllAsQueryable()
        {
            return _DbContext.Set<T>();
        }

        public T? GetById(int id)
        {
            //var department = _DbContext.Departments.Local.FirstOrDefault(D => D.Id == id);
            //return department;
            ////.Local to search it locally if we already got it or use find
            return _DbContext.Set<T>().Find(id);
        }

        public void Add(T entity)
        {
            _DbContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            _DbContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            entity.IsDeleted = true;
            _DbContext.Set<T>().Remove(entity);
        }
    }
}
