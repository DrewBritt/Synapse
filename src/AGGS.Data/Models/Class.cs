using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AGGS.Data.Models
{
    public partial class Class
    {
        public int ClassId { get; set; }
        public int TeacherId { get; set; }
        public string ClassName { get; set; }
        public string Period { get; set; }
        public string Location { get; set; }
    }
}
