using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;

namespace CodeChallenge.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(ILogger<EmployeeService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public Employee Create(Employee employee)
        {
            if(employee != null)
            {
                _employeeRepository.Add(employee);
                _employeeRepository.SaveAsync().Wait();
            }

            return employee;
        }

        public Employee GetById(string id)
        {
            if(!String.IsNullOrEmpty(id))
            {
                return _employeeRepository.GetById(id);
            }

            return null;
        }

        public Employee Replace(Employee originalEmployee, Employee newEmployee)
        {
            if(originalEmployee != null)
            {
                _employeeRepository.Remove(originalEmployee);
                if (newEmployee != null)
                {
                    // ensure the original has been removed, otherwise EF will complain another entity w/ same id already exists
                    _employeeRepository.SaveAsync().Wait();

                    _employeeRepository.Add(newEmployee);
                    // overwrite the new id with previous employee id
                    newEmployee.EmployeeId = originalEmployee.EmployeeId;
                }
                _employeeRepository.SaveAsync().Wait();
            }

            return newEmployee;
        }

        // This function makes calls to the recursive helper function CountNumberOfReports, to count the number of reporting employees depth-first.
        // Due to the limitation in EmployeeRepository.GetByID(), the IDs are passed into the recursive function so at each layer of recursion,
        // the function can call GetByID() again and get the proper nested employees.
        public ReportingStructure GetReportingByID(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                ReportingStructure report = new()
                {
                    Employee = _employeeRepository.GetById(id)
                };

                report.NumberOfReports = CountNumberOfReports(report.Employee.EmployeeId);
                return report;
            }
            return null;
        }

        // Recursive helper function called by GetReportingByID(). Exit condition is based on if the DirectReports list within Employee is null.
        private int CountNumberOfReports(string id)
        {
            var employee = _employeeRepository.GetById(id);

            if (employee.DirectReports == null)
            {
                return 0;
            }

            int sumSubReports = 0;
            foreach(Employee emp in employee.DirectReports)
            {
                sumSubReports += 1 + CountNumberOfReports(emp.EmployeeId); 
            }
            return sumSubReports;
        }
    }
}
