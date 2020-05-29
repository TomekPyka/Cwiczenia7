using Cwiczenia7.Model;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Cwiczenia7.Service
{
    public class SQLServerStudentDbService : IStudentDbService
    {
        private readonly string connectionString = "Data Source=PC-KOMPUTER;Initial Catalog=s12343;Integrated Security=True";
        private SqlConnection SqlConnection => new SqlConnection(connectionString);

        public Student GetRefreshTokenOwner(string refreshToken)
        {
            using var connection = SqlConnection;
            using var command = new SqlCommand
            {
                Connection = connection,
                CommandText = "SELECT * FROM RefreshToken WHERE Id = @refreshToken"
            };
            command.Parameters.AddWithValue("refreshToken", refreshToken);
            connection.Open();
            using var dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                var refreshTokenModel = new RefreshToken
                {
                    Id = dataReader["Id"].ToString(),
                    IndexNumber = dataReader["IndexNumber"].ToString()
                };
                return GetStudent(refreshTokenModel.IndexNumber);
            }
            return null;
        }

        public int CreateRefreshToken(RefreshToken refreshToken)
        {
            using var connection = SqlConnection;
            using var command = new SqlCommand
            {
                Connection = connection,
                CommandText = "INSERT INTO RefreshToken " +
                "VALUES(@id, @indexNumber)"
            };

            command.Parameters.AddWithValue("id", refreshToken.Id);
            command.Parameters.AddWithValue("indexNumber", refreshToken.IndexNumber);
            connection.Open();
            return command.ExecuteNonQuery();
        }
        public int CreateStudent(Student student)
        {
            using var connection = SqlConnection;
            using var command = new SqlCommand
            {
                Connection = connection,
                CommandText = "INSERT INTO Student " +
                "VALUES(@indexNumber, @firstName, @lastName, @birthDate, @idEnrollment)"
            };
            command.Parameters.AddWithValue("indexNumber", student.IndexNumber);
            command.Parameters.AddWithValue("firstName", student.FirstName);
            command.Parameters.AddWithValue("lastName", student.LastName);
            command.Parameters.AddWithValue("birthDate", student.BirthDate);
            command.Parameters.AddWithValue("idEnrollment", student.IdEnrollment);
            connection.Open();
            return command.ExecuteNonQuery();
        }

        public int UpdateStudent(string indexNumber, Student student)
        {
            using var connection = SqlConnection;
            using var command = new SqlCommand
            {
                Connection = connection,
                CommandText = "UPDATE Student " +
                "SET IndexNumber = @newIndexNumber, FirstName = @firstName, " +
                "LastName = @lastName, BirthDate = @birthDate, " +
                "IdEnrollment = @idEnrollment " +
                "WHERE IndexNumber = @oldIndexNumber"
            };
            command.Parameters.AddWithValue("newIndexNumber", student.IndexNumber);
            command.Parameters.AddWithValue("firstName", student.FirstName);
            command.Parameters.AddWithValue("lastName", student.LastName);
            command.Parameters.AddWithValue("birthDate", student.BirthDate);
            command.Parameters.AddWithValue("idEnrollment", student.IdEnrollment);
            command.Parameters.AddWithValue("oldIndexNumber", indexNumber);
            connection.Open();
            return command.ExecuteNonQuery();
        }

        public int DeleteStudent(string indexNumber)
        {
            using var connection = SqlConnection;
            using var command = new SqlCommand
            {
                Connection = connection,
                CommandText = "DELETE FROM Student WHERE IndexNumber = @indexNumber"
            };
            command.Parameters.AddWithValue("indexNumber", indexNumber);
            connection.Open();
            return command.ExecuteNonQuery();
        }

        public Enrollment GetEnrollment(int idEnrollment)
        {
            using var connection = SqlConnection;
            using var command = new SqlCommand
            {
                Connection = connection,
                CommandText = "SELECT * FROM Enrollment WHERE IdEnrollment = @idEnrollment"
            };
            command.Parameters.AddWithValue("idEnrollment", idEnrollment);
            connection.Open();
            using var dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                var enrollment = new Enrollment
                {
                    IdEnrollment = IntegerType.FromObject(dataReader["IdEnrollment"]),
                    Semester = IntegerType.FromObject(dataReader["Semester"]),
                    StartDate = dataReader["StartDate"].ToString(),
                    IdStudy = IntegerType.FromObject(dataReader["IdStudy"])
                };
                return enrollment;
            }
            return null;
        }

        public Student GetStudent(string indexNumber)
        {
            using var connection = SqlConnection;
            using var command = new SqlCommand
            {
                Connection = connection,
                CommandText = "SELECT * FROM Student WHERE IndexNumber = @indexNumber"
            };
            command.Parameters.AddWithValue("indexNumber", indexNumber);
            connection.Open();
            using var dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                var student = new Student
                {
                    IndexNumber = dataReader["IndexNumber"].ToString(),
                    FirstName = dataReader["FirstName"].ToString(),
                    LastName = dataReader["LastName"].ToString(),
                    BirthDate = dataReader["BirthDate"].ToString(),
                    IdEnrollment = IntegerType.FromObject(dataReader["IdEnrollment"])
                };
                return student;
            }
            return null;
        }

        public Enrollment GetStudentEnrollment(string indexNumber)
        {
            using var connection = SqlConnection;
            using var command = new SqlCommand
            {
                Connection = connection,
                CommandText = "SELECT Enrollment.IdEnrollment, Semester, StartDate, Name " +
                "FROM Student " +
                "JOIN Enrollment ON Student.IdEnrollment = Enrollment.IdEnrollment " +
                "JOIN Studies ON Enrollment.IdStudy = Studies.IdStudy " +
                "WHERE IndexNumber = @indexNumber"
            };
            command.Parameters.AddWithValue("indexNumber", indexNumber);
            connection.Open();
            using var dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                var enrollment = new Enrollment
                {
                    IdEnrollment = IntegerType.FromObject(dataReader["IdEnrollment"]),
                    Semester = IntegerType.FromObject(dataReader["Semester"]),
                    StartDate = dataReader["StartDate"].ToString(),
                    Name = dataReader["Name"].ToString(),
                };
                return enrollment;
            }
            return new Enrollment();
        }

        public IEnumerable<Student> GetStudents(string orderBy)
        {
            if (orderBy == null)
                orderBy = "IndexNumber";
            List<Student> students = new List<Student>();
            using var connection = SqlConnection;
            using var command = new SqlCommand()
            {
                Connection = connection,
                CommandText = $"SELECT * FROM Student ORDER BY {orderBy}"
            };
            connection.Open();
            using var dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                var student = new Student
                {
                    IndexNumber = dataReader["IndexNumber"].ToString(),
                    FirstName = dataReader["FirstName"].ToString(),
                    LastName = dataReader["LastName"].ToString(),
                    BirthDate = dataReader["BirthDate"].ToString(),
                    IdEnrollment = IntegerType.FromObject(dataReader["IdEnrollment"])
                };
                students.Add(student);
            }
            return students;
        }

        public Studies GetStudies(string studiesName)
        {
            using var connection = SqlConnection;
            using var command = new SqlCommand
            {
                Connection = connection,
                CommandText = "SELECT * FROM Studies WHERE Name = @studiesName"
            };
            command.Parameters.AddWithValue("studiesName", studiesName);
            connection.Open();
            using var dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                Studies studies = new Studies
                {
                    IdStudy = IntegerType.FromObject(dataReader["IdStudy"]),
                    Name = dataReader["Name"].ToString()
                };
                return studies;
            }
            return null;
        }

        public Enrollment SemesterPromotion(int idStudy, int semester)
        {
            using var connection = SqlConnection;
            using var command = new SqlCommand
            {
                Connection = connection,
                CommandText = "sp_SemesterPromotion",
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("id_study", idStudy);
            command.Parameters.AddWithValue("semester", semester);
            connection.Open();
            using var dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                var enrollment = new Enrollment
                {
                    IdEnrollment = IntegerType.FromObject(dataReader["IdEnrollment"]),
                    Semester = IntegerType.FromObject(dataReader["Semester"]),
                    StartDate = dataReader["StartDate"].ToString(),
                    IdStudy = IntegerType.FromObject(dataReader["IdStudy"])
                };
                return enrollment;
            }
            return null;
        }

        public Enrollment CreateStudentEnrollment(string indexNumber, string firstName, string lastName, DateTime birthDate, string studiesName)
        {
            //sprawdzenie warunków dot. indeksu, a następnie dodanie do bazy
            throw new NotImplementedException();
        }


    }
}