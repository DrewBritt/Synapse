using System;
using System.Collections.Generic;
using System.Text;
using Synapse.Data.Models;

namespace Synapse.Data.ViewModels
{
    public class StudentGradesVM
    {
        public int StudentId { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
        public string StudentFullName
        {
            get
            {
                return StudentFirstName + " " + StudentLastName;
            }
        }
        public string Email { get; set; }
        public int GradeLevel { get; set; }

        public List<ClassWithTeacherInfo> StudentSchedule { get; set; }
        public List<int> ClassAverages { get; set; }
    }
}
