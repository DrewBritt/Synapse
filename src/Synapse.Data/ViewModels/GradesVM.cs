using Synapse.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Synapse.Data.ViewModels
{
    public class GradesVM
    {
        public int ClassId { get; set; }
        public int TeacherId { get; set; }
        public string ClassName { get; set; }
        public string Period { get; set; }

        public List<AssignmentCategory> AssignmentCategories { get; set; }
        public List<Assignment> ClassAssignments { get; set; }

        public List<Student> EnrolledStudents { get; set; }

        public List<Grade> StudentGrades { get; set; }
    }
}
