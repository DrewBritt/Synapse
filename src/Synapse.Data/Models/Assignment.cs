using System;
using System.Collections.Generic;
using System.Text;

namespace Synapse.Data.Models
{
    public partial class Assignment
    {
        public int AssignmentId { get; set; }
        public int ClassId { get; set; }
        public string AssignmentName { get; set; }
        public int CategoryId { get; set; }
    }
}
