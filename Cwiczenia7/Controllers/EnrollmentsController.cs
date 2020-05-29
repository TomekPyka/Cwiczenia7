using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Cwiczenia7.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IStudentDbService _dbService;

        public EnrollmentsController(IStudentDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet("{idEnrollment}")]
        public IActionResult GetEnrollment(int idEnrollment)
        {
            var enrollment = _dbService.GetEnrollment(idEnrollment);
            if (enrollment != null)
                return Ok(new GetEntrollmentResponse
                {
                    IdEnrollment = enrollment.IdEnrollment,
                    IdStudy = enrollment.IdStudy,
                    Semester = enrollment.Semester,
                    StartDate = enrollment.StartDate
                });
            else
                return BadRequest("Zapisy nie znalezione");
        }

        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            var enrollment = _dbService.CreateStudentEnrollment(
                request.IndexNumber, request.FirstName, request.LastName,
                DateTime.ParseExact(request.BirthDate, "dd.MM.yyyy", null), request.Studies);
            if (enrollment != null)
            {
                return CreatedAtAction(nameof(GetEnrollment),
                    new { idEnrollment = enrollment.IdEnrollment },
                    new GetEntrollmentResponse
                    {
                        IdEnrollment = enrollment.IdEnrollment,
                        IdStudy = enrollment.IdStudy,
                        Semester = enrollment.Semester,
                        StartDate = enrollment.StartDate
                    });
            }
            else
                return BadRequest("Nie udało się przetworzyć żądania");
        }
    }

    [HttpPost("promocje")]
    public IActionResult PromoteStudents(PromoteStudentsRequest request)
    {
        var studies = _dbService.GetStudies(request.Studies);

        var enrollment = _dbService.GetEnrollment(studies.IdStudy, request.Semester);

        var newEnrollment = _dbService.SemesterPromote(studies.IdStudy, request.Semester);
        return CreatedAtAction(nameof(GetEnrollment),
            new { idEnrollment = enrollment.IdEnrollment },
            new GetEntrollmentResponse
            {
                IdEnrollment = newEnrollment.IdEnrollment,
                IdStudy = newEnrollment.IdStudy,
                Semester = newEnrollment.Semester,
                StartDate = newEnrollment.StartDate
            });
    }
}