using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Synapse.Data.Models
{
    public partial class AssignmentCategory
    {
        [Key]
        public int CategoryId { get; set; }
        public int ClassId { get; set; }
        public string CategoryName { get; set; }
        public int CategoryWeight { get; set; }
    }
}
