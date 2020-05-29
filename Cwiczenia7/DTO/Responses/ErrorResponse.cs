using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cwiczenia7.DTO.Responses
{
    public class ErrorResponse
    {
        [Required]
        public string Message { get; set; }
    }
}