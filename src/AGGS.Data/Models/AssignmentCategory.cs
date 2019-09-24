using System;
using System.Collections.Generic;
using System.Text;

namespace AGGS.Data.Models
{
    public partial class AssignmentCategory
    {
        public int CategoryId { get; set; }
        public int TeacherId { get; set; }
        public string CategoryName { get; set; }
        public int CategoryWeight { get; set; }
    }
}
