﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Synapse.Data.Models
{
    public partial class Student
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int StudentId { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
        public string Email { get; set; }
        public byte GradeLevel { get; set; }

        public string StudentFullName
        {
            get
            {
                return StudentFirstName + " " + StudentLastName;
            }
        }
    }
}
