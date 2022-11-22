using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;

namespace CodeChallenge.Services
{
    public class CompensationService : ICompensationService
    {
        private readonly IEmployeeRepository _employeeRepository; // This service accesses the employee repository as well as the compensation repository to check if employees exist.
        private readonly ICompensationRepository _compensationRepository; 
        private readonly ILogger<CompensationService> _logger;

        public CompensationService(ILogger<CompensationService> logger, IEmployeeRepository employeeRepository, ICompensationRepository compensationRepository)
        {
            _employeeRepository = employeeRepository;
            _compensationRepository = compensationRepository;
            _logger = logger;
        }

        public Compensation Create(Compensation compensation)
        {
            // Additional check done here to ensure an employee actually exists before creating a compensation for them.
            // Also checks if a compensation for that employee exists already.
            if(compensation != null && _employeeRepository.GetById(compensation.EmployeeId) != null && _compensationRepository.GetByEmployeeId(compensation.EmployeeId) == null )
            {
                _compensationRepository.Add(compensation);
                _compensationRepository.SaveAsync().Wait();
            }

            return compensation;
        }

        public Compensation GetByEmployeeId(string id)
        {
            if(!String.IsNullOrEmpty(id))
            {
                return _compensationRepository.GetByEmployeeId(id);
            }

            return null;
        }
    }
}
