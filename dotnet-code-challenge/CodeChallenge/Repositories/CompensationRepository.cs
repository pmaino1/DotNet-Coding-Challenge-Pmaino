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
    public class CompensationRepository : ICompensationRepository
    {
        private readonly EmployeeContext _employeeContext; 
        private readonly ILogger<IEmployeeRepository> _logger;

        public CompensationRepository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Compensation Add(Compensation compensation)
        {
            //Compensations exist in the same DB Conext as Employees, just in a different DB Set (Compensations).  
            _employeeContext.Compensations.Add(compensation);
            return compensation;
        }

        // Original

        //public Compensation GetByEmployeeId(string id)
        //{
        //    return _compensationContext.Compensations.SingleOrDefault(e => e.Employee.EmployeeID == id);
        //}

        // Due to restraints on the key for the Compensation time, Compensations don't run into the same issue with employees nested within them.
        // Compensations simply store the EmployeeID of their associated employee. The same fix used in EmployeeRepository.GetByID() was not needed here.
        public Compensation GetByEmployeeId(string id)
        {
            return _employeeContext.Compensations.SingleOrDefault(c => c.EmployeeId == id);
        }


        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Compensation Remove(Compensation compensation)
        {
            return _employeeContext.Remove(compensation).Entity;
        }
    }
}
