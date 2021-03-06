﻿using System;

namespace Synapse.Data.Models
{
    public partial class Assignment
    {
        public int AssignmentId { get; set; }
        public int ClassId { get; set; }
        public string AssignmentName { get; set; }
        public int CategoryId { get; set; }
        public DateTime DueDate { get; set; }

        public string DueDateString
        {
            get
            {
                return DueDate.ToString("yyyy-MM-dd");
            }
        }
    }
}
