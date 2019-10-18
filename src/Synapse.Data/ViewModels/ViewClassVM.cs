using Synapse.Data.Models;
using System.Collections.Generic;

namespace Synapse.Data.ViewModels
{
    public class ViewClassVM
    {
        public int ClassId { get; set; }
        public int TeacherId { get; set; }
        public string TeacherFirstName { get; set; }
        public string TeacherLastName { get; set; }
        public string Email { get; set; }
        public string ClassName { get; set; }
        public string Period { get; set; }
        public string Location { get; set; }
        public List<Student> EnrolledStudents { get; set; }
        public List<int> StudentAverages { get; set; }
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
