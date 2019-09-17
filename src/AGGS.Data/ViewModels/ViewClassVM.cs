using AGGS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AGGS.ViewModels
{
    public class ViewClassVM
    {
        public int ClassId { get; set; }
        public string TeacherFirstName { get; set; }
        public string TeacherLastName { get; set; }
        public string Email { get; set; }
        public string ClassName { get; set; }
        public string Period { get; set; }
        public string Location { get; set; }
        public List<Student> EnrolledStudents { get; set; }
        public List<Teacher> AllTeachers { get; set; }

        public string TeacherFullName
        {
            get
            {
                return TeacherFirstName + " " + TeacherLastName;
            }
        }
    }
}
