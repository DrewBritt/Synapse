using System.Collections.Generic;
using Synapse.Data.Models;

namespace Synapse.Data.ViewModels
{
    public class ViewTeacherVM
    {
        public int TeacherId { get; set; }
        public string TeacherFirstName { get; set; }
        public string TeacherLastName { get; set; }
        public string TeacherFullName 
        {
            get
            {
                return TeacherFirstName + " " + TeacherLastName;
            }
        }
        public string Email { get; set; }

        public List<Class> Classes { get; set; }
    }
}
