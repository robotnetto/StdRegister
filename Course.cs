using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFcore
{
    internal class Course
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; } = null!;
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public virtual List<Student>? Students { get; set; } = new List<Student>();

    }
}
