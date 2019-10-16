using Synapse.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Synapse.Data.ViewModels
{
    public class GradesVM
    {
        public int ClassId { get; set; }
        public int TeacherId { get; set; }
        public string ClassName { get; set; }
        public string Period { get; set; }

        public List<AssignmentCategory> AssignmentCategories { get; set; }
        public List<Assignment> ClassAssignments { get; set; }

        public List<Student> EnrolledStudents { get; set; }
        public List<Grade> StudentGrades { get; set; }

        //List of Student Averages sorted by student last name, populated by PopulateStudentAverages function
        public List<int> StudentAverages { get; set; }

        public void PopulateStudentAverages()
        {
            List<int> averages = new List<int>();

            //Iterate through all enrolled students, keep index for grades math
            for (int studentsIndex = 0; studentsIndex < EnrolledStudents.Count; studentsIndex++)
            {
                double studentAverage = 0;

                int weightTotal = 0;
                int gradesWithWeightTotal = 0;

                for (int assignmentsIndex = 0; assignmentsIndex < ClassAssignments.Count; assignmentsIndex++)
                {
                    Grade gradeToAccess = StudentGrades[assignmentsIndex + (studentsIndex * ClassAssignments.Count)];

                    if (gradeToAccess.GradeValue == "")
                    {
                        continue;
                    }

                    int gradeWeight = AssignmentCategories.Find(c => c.CategoryId == ClassAssignments[assignmentsIndex].CategoryId).CategoryWeight;
                    weightTotal += gradeWeight;

                    if (gradeToAccess.GradeValue != "M" && gradeToAccess.GradeValue != "m")
                    {
                        gradesWithWeightTotal += Int32.Parse(gradeToAccess.GradeValue) * gradeWeight;
                    }
                }

                studentAverage += (double)gradesWithWeightTotal / weightTotal;

                averages.Add((int)Math.Round(studentAverage, MidpointRounding.AwayFromZero));
            }

            StudentAverages = averages;
        }
    }
}
