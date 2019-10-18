using System;

namespace Synapse.Data.Models
{
    public class ClassWithTeacherInfo : IEquatable<ClassWithTeacherInfo>
    {
        public int ClassId { get; set; }
        public int TeacherId { get; set; }
        public string TeacherFirstName { get; set; }
        public string TeacherLastName { get; set; }
        public string Email { get; set; }
        public string ClassName { get; set; }
        public string Period { get; set; }
        public string Location { get; set; }   

        public string TeacherFullName
        {
            get
            {
                return TeacherFirstName + " " + TeacherLastName;
            }
        }

        public bool Equals(ClassWithTeacherInfo obj)
        {
            return this.ClassId == obj.ClassId;
        }
    }
}
