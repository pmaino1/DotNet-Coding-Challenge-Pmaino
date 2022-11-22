using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using CodeChallenge.Models;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/compensation")]
    public class CompensationController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;

        public CompensationController(ILogger<EmployeeController> logger, ICompensationService compensationService)
        {
            _logger = logger;
            _compensationService = compensationService;
        }

        // Post endpoint for creating a new compensation for an employee - compensations will persist.
        [HttpPost]
        public IActionResult CreateCompensation([FromBody] Compensation compensation)
        {
            var compensatedEmployeeId = compensation.EmployeeId;

            _logger.LogDebug($"Received compensation create request for employee '{compensatedEmployeeId}'");

            _compensationService.Create(compensation);

            return CreatedAtRoute("GetCompensationByEmployeeId", new { id = compensatedEmployeeId }, compensation);
        }

        //Get endpoint for accessing existing compensations.
        [HttpGet("{id}", Name = "GetCompensationByEmployeeId")]
        public IActionResult GetCompensationByEmployeeId(String id)
        {
            _logger.LogDebug($"Received compensation get request for employee '{id}'");

            var compensation = _compensationService.GetByEmployeeId(id);

            if (compensation == null)
                return NotFound();

            return Ok(compensation);
        }
    }
}
