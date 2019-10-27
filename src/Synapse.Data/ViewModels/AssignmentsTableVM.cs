using Synapse.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Synapse.Data.ViewModels
{
    public class AssignmentsTableVM
    {
        public int ClassId { get; set; }
        public List<Assignment> classAssignments { get; set; }
        public List<AssignmentCategory> classAssignmentCategories { get; set; }
        public List<Grade> studentGrades { get; set; }
    }
}
