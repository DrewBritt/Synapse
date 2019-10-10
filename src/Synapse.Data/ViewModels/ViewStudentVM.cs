using Synapse.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synapse.Data.ViewModels
{
    public class ViewStudentVM
    {
        public int StudentId { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
        public string Email { get; set; }
        public byte GradeLevel { get; set; }
        public List<ClassWithTeacherInfo> Classes { get; set; }
    }
}
