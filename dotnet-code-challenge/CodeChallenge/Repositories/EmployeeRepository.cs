using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CodeChallenge.Data;

namespace CodeChallenge.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public EmployeeRepository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Employee Add(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            _employeeContext.Employees.Add(employee);
            return employee;
        }

        // Original version of this function did not get an employee's directReports nested employees correctly. 

        //public Employee GetById(string id)
        //{
        //    return _employeeContext.Employees.SingleOrDefault(e => e.EmployeeId == id);
        //}

        // Added Include() to GetByID() to include the list of employees that directly report to the employee in question.
        // Note that this doesn't fetch any of the other employees lower on the tree, so getById()'s must be called with this in mind.
        public Employee GetById(string id)
        {
            return _employeeContext.Employees.Include(x => x.DirectReports).SingleOrDefault(e => e.EmployeeId == id);
        }


        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Employee Remove(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }
    }
}
