using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Models
{
    public class Compensation
    {
        [Key]
        public string EmployeeId { get; set; }
        public decimal Salary { get; set; }
        public DateTime EffectiveDate{ get; set; }
    }
}
