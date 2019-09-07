using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AGGS.ViewModels
{
    public class ClassVM
    {
        public int ClassId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ClassName { get; set; }
        public string Period { get; set; }

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }
}
