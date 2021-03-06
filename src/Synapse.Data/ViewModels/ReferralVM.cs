﻿using System;

namespace Synapse.Data.ViewModels
{
    public class ReferralVM
    {
        public int ReferralId { get; set; }
        public int StudentId { get; set; }

        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
        public int TeacherId { get; set; }
        public string TeacherFirstName { get; set; }
        public string TeacherLastName { get; set; }
        public DateTime DateIssued { get; set; }
        public string Description { get; set; }
        public bool Handled { get; set; }

        public string StudentFullName
        {
            get
            {
                return StudentFirstName + " " + StudentLastName;
            }
        }
        
        public string TeacherFullName
        {
            get
            {
                return TeacherFirstName + " " + TeacherLastName;
            }
        }

        public string DescriptionTruncated
        {
            get
            {
                if (Description.Length <= 17)
                    return Description;

                return Description.Substring(0, 17) + "...";
            }
        }

        public string DateIssuedString
        {
            get
            {
                return DateIssued.ToString("MM/dd/yyyy");
            }
        }
    }
}
