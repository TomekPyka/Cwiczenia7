using Cwiczenia7.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cwiczenia7.Service
{
    public interface IStudentDbService
    {
        public IEnumerable<Student> GetStudents(string orderBy);
        public Student GetStudent(string indexNumber);
        public int CreateStudent(Student student);
        public Student GetRefreshTokenOwner(string refreshToken);
        public int CreateRefreshToken(RefreshToken refreshToken);

        public int UpdateStudent(string indexNumber, Student student);
        public int DeleteStudent(string indexNumber);
        public Enrollment GetStudentEnrollment(string indexNumber);
        public Studies GetStudies(string studiesName);
        public Enrollment CreateStudentEnrollment(
            string indexNumber, string firstName, string lastName, DateTime birthDate, string studiesName);
        public Enrollment GetEnrollment(int idEnrollment);
        public Enrollment SemesterPromotion(int idStudy, int semester);

        public Enrollment GetEnrollment(int idStudy, int semester);
    }
}