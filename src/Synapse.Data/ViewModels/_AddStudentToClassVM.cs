using Synapse.Data.Models;
using System.Collections.Generic;

namespace Synapse.Data.ViewModels
{
    public class _AddStudentToClassVM
    {
        public int StudentId { get; set; }

        public List<ClassWithTeacherInfo> AllClasses { get; set; }

        public List<ClassWithTeacherInfo> CurrentClasses { get; set; }
    }
}
