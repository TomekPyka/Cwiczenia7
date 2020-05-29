using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cwiczenia7.DTO.Requests.Responses
{
    public class GetEntrollmentResponse
    {
        [Required]
        public int IdEnrollment { get; set; }
        [Required]
        public int Semester { get; set; }
        [Required]
        public string StartDate { get; set; }
        [Required]
        public int IdStudy { get; set; }
    }
}