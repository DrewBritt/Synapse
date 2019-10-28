using System;
using System.Collections.Generic;
using System.Text;
using Synapse.Data.Models;

namespace Synapse.Data.ViewModels
{
    public class StudentGradesVM
    {
        public int StudentId { get; set; }

        public List<ClassWithTeacherInfo> StudentSchedule { get; set; }
        public List<int?> ClassAverages { get; set; }
    }
}
