using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cwiczenia7.DTO.Requests;
using Cwiczenia7.DTO.Responses;
using Cwiczenia7.Model;
using Cwiczenia7.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Cwiczenia7.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentDbService _dbService;
        private readonly IConfiguration _configuration;

        public StudentsController(IStudentDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login(LoginRequest request)
        {
            var student = _dbService.GetStudent(request.Username);
            if (student == null)
                return NotFound(new ErrorResponse
                {
                    Message = "Hasło i/lub użytkownik nieprawidłowe"
                });

            static string CreateHash(string password, string salt)
            {
                return Convert.ToBase64String(
                    KeyDerivation.Pbkdf2(
                        password: password,
                        salt: Encoding.UTF8.GetBytes(salt),
                        prf: KeyDerivationPrf.HMACSHA512,
                        iterationCount: 25555,
                        numBytesRequested: 512 / 8
                    )
                );
            }
        }
         


        [HttpGet]
        public IActionResult GetStudents(string orderBy)
        {
            return Ok(_dbService.GetStudents(orderBy));
        }

        [HttpGet("{indexNumber}")]
        public IActionResult GetStudent(string indexNumber)
        {
            var student = _dbService.GetStudent(indexNumber);
            return Ok(student);
        }

        [HttpGet("{indexNumber}/enrollment")]
        public IActionResult GetStudentEnrollment(string indexNumber)
        {
            var student = _dbService.GetStudentEnrollment(indexNumber);
            if (student != null)
                return Ok(student);
            else
                return NotFound("Brak takiego studenta w bazie");
        }

        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            return Ok(student);
        }

        [HttpDelete("{indexNumber}")]
        public IActionResult DeleteStudent(string indexNumber)
        {
            return Ok("Usuwanie ukończone");
        }
    }
}