using Synapse.Data.Models;
using System.Collections.Generic;

namespace Synapse.Data.ViewModels
{
    public class _AddStudentToClassVM
    {
        public int StudentId { get; set; }

        public List<ClassVM> AllClasses { get; set; }

        public List<Class> CurrentClasses { get; set; }
    }
}
