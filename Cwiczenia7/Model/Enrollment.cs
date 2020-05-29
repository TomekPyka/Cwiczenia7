using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cwiczenia7.Model
{
        public class Enrollment
        {
            public int IdEnrollment { get; set; }
            public int Semester { get; set; }
            public string StartDate { get; set; }
            public string Name { get; set; }
            public int IdStudy { get; set; }
        }
    }