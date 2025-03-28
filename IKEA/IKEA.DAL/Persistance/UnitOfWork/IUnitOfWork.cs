﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IKEA.DAL.Persistance.Repsitories.Departments;
using IKEA.DAL.Persistance.Repsitories.Employees;

namespace IKEA.DAL.Persistance.UnitOfWork
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        public IEmployeeRepository EmployeeRepository { get; }
        public IDepartmentRepository DepartmentRepository { get; }
        Task<int> CompleteAsync();
    }
}
