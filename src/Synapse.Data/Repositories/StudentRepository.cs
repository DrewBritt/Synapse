using System;
using System.Collections.Generic;
using System.Linq;
using Synapse.Data.Models;

namespace Synapse.Data.Repositories
{
    public class StudentRepository
    {
        private readonly SynapseContext _context;

        public StudentRepository(SynapseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets ID of student attached to email
        /// </summary>
        /// <param name="email">Email of student to get ID of</param>
        /// <returns>int: ID of student</returns>
        public int GetStudentIdWithEmail(string email)
        {
            var studentidquery = (from students in _context.Students
                             select new
                             {
                                 students.StudentId,
                                 students.Email
                             }).Where(s => s.Email == email).FirstOrDefault();

            return studentidquery.StudentId;
        }

        /// <summary>
        /// Calculates student's averages in attached classes
        /// </summary>
        /// <param name="studentid">ID of student to check averages of</param>
        /// <returns>List(int) of student's averages</returns>
        public List<int?> CalculateStudentAverages(int studentid)
        {
            List<int?> averages = new List<int?>();

            List<ClassWithTeacherInfo> studentSchedule = GetStudentSchedule(studentid);
            List<Assignment> studentClassesAssignments = GetAssignmentsInStudentSchedule(studentid);
            List<AssignmentCategory> studentClassesAssignmentCategories = GetAssignmentCategoriesInStudentSchedule(studentid);
            List<Grade> studentGrades = GetStudentsGrades(studentid);

            int assignmentsIndex = 0;
            for(int classIndex = 0; classIndex < studentSchedule.Count; classIndex++)
            {
                double average = 0;

                int weightTotal = 0;
                int gradesWithWeightTotal = 0;

                int currentClassId = studentSchedule[classIndex].ClassId;
                while(assignmentsIndex < studentClassesAssignments.Count)
                {
                    Grade gradeToAccess = studentGrades[assignmentsIndex];

                    if(currentClassId != gradeToAccess.ClassId)
                    {
                        currentClassId = gradeToAccess.ClassId;
                        break;
                    }

                    if(gradeToAccess.GradeValue != "")
                    {
                        int gradeWeight = studentClassesAssignmentCategories.Find(c => c.CategoryId == studentClassesAssignments[assignmentsIndex].CategoryId).CategoryWeight;
                        weightTotal += gradeWeight;

                        if(gradeToAccess.GradeValue != "M" && gradeToAccess.GradeValue != "m")
                        {
                            gradesWithWeightTotal += Int32.Parse(gradeToAccess.GradeValue) * gradeWeight;
                        }
                    }

                    assignmentsIndex++;
                }

                average += (double)gradesWithWeightTotal / weightTotal;

                average = Math.Round(average, MidpointRounding.AwayFromZero);
                int averageInt = (int)average;

                if (averageInt < 0)
                    averages.Add(null);
                else
                    averages.Add(averageInt);
            }

            return averages;
        }

        /// <summary>
        /// Gets schedule of student mapped to studentid
        /// </summary>
        /// <param name="studentid">ID of student to get schedule of</param>
        /// <returns>List(ClassWithTeacherInfo) of classes in student's schedule</returns>
        public List<ClassWithTeacherInfo> GetStudentSchedule(int studentid)
        {
            var schedule = (from studentsclasses in _context.StudentsClasses
                            join classes in _context.Classes on studentsclasses.ClassId equals classes.ClassId
                            join teachers in _context.Teachers on classes.TeacherId equals teachers.TeacherId
                            select new
                            {
                                classes.ClassId,
                                classes.TeacherId,
                                teachers.TeacherFirstName,
                                teachers.TeacherLastName,
                                teachers.Email,
                                classes.ClassName,
                                classes.Period,
                                classes.Location,
                                studentsclasses.StudentId
                            }).Where(s => s.StudentId == studentid).OrderBy(c => c.Period)
                            .ToList();

            List<ClassWithTeacherInfo> EnrolledClasses = new List<ClassWithTeacherInfo>();

            foreach (var item in schedule)
            {
                ClassWithTeacherInfo studentClass = new ClassWithTeacherInfo
                {
                    ClassId = item.ClassId,
                    TeacherId = item.TeacherId,
                    TeacherFirstName = item.TeacherFirstName,
                    TeacherLastName = item.TeacherLastName,
                    Email = item.Email,
                    ClassName = item.ClassName,
                    Period = item.Period,
                    Location = item.Location
                };

                EnrolledClasses.Add(studentClass);
            }

            return EnrolledClasses;
        }

        /// <summary>
        /// Gets list of all assignments of classes attached to studentid
        /// </summary>
        /// <param name="studentid">ID of student to get assignments for</param>
        /// <returns>List(Assignment) of all assignments attached to studentid</returns>
        public List<Assignment> GetAssignmentsInStudentSchedule(int studentid)
        {
            var assignmentsQuery = (from assignments in _context.Assignments
                                    join studentsclasses in _context.StudentsClasses on assignments.ClassId equals studentsclasses.ClassId
                                    join classes in _context.Classes on assignments.ClassId equals classes.ClassId
                                    select new
                                    {
                                        assignments.AssignmentId,
                                        assignments.ClassId,
                                        assignments.AssignmentName,
                                        assignments.CategoryId,
                                        assignments.DueDate,
                                        classes.Period,
                                        studentsclasses.StudentId
                                    }).Where(a => a.StudentId == studentid).OrderBy(a => a.Period);

            List<Assignment> assignmentsList = new List<Assignment>();
            foreach(var a in assignmentsQuery)
            {
                Assignment newAssignment = new Assignment()
                {
                    AssignmentId = a.AssignmentId,
                    ClassId = a.ClassId,
                    AssignmentName = a.AssignmentName,
                    CategoryId = a.CategoryId,
                    DueDate = a.DueDate
                };

                assignmentsList.Add(newAssignment);
            }

            return assignmentsList;
        }

        /// <summary>
        /// Gets list of all assignment categories of classes attached to studentid
        /// </summary>
        /// <param name="studentid">ID of student to get assignment categories for</param>
        /// <returns>List(AssignmentCategory) of all assignment categories attached to studentid</returns>
        public List<AssignmentCategory> GetAssignmentCategoriesInStudentSchedule(int studentid)
        {
            var categoriesQuery = (from assignmentcategories in _context.AssignmentCategories
                                   join studentclasses in _context.StudentsClasses on assignmentcategories.ClassId equals studentclasses.ClassId
                                   join classes in _context.Classes on assignmentcategories.ClassId equals classes.ClassId
                                   select new
                                   {
                                       assignmentcategories.CategoryId,
                                       assignmentcategories.ClassId,
                                       assignmentcategories.CategoryName,
                                       assignmentcategories.CategoryWeight,
                                       classes.Period,
                                       studentclasses.StudentId
                                   }).Where(c => c.StudentId == studentid).OrderBy(c => c.Period);

            List<AssignmentCategory> categories = new List<AssignmentCategory>();
            foreach(var c in categoriesQuery)
            {
                AssignmentCategory newCategory = new AssignmentCategory()
                {
                    CategoryId = c.CategoryId,
                    ClassId = c.ClassId,
                    CategoryName = c.CategoryName,
                    CategoryWeight = c.CategoryWeight
                };

                categories.Add(newCategory);
            }

            return categories;
        }

        /// <summary>
        /// Gets list of all grades of all classes attached to studentid
        /// </summary>
        /// <param name="studentid">ID of student to get grades of</param>
        /// <returns>List(Grade) of all grades attached to studentid</returns>
        public List<Grade> GetStudentsGrades(int studentid)
        {
            var gradesQuery = (from grades in _context.Grades
                               join classes in _context.Classes on grades.ClassId equals classes.ClassId
                               select new
                               {
                                   grades.GradeId,
                                   grades.AssignmentId,
                                   grades.ClassId,
                                   grades.StudentId,
                                   grades.GradeValue,
                                   classes.Period
                               }).Where(g => g.StudentId == studentid).OrderBy(g => g.Period);

            List<Grade> studentGrades = new List<Grade>();
            foreach(var g in gradesQuery)
            {
                Grade grade = new Grade()
                {
                    GradeId = g.GradeId,
                    AssignmentId = g.AssignmentId,
                    ClassId = g.ClassId,
                    StudentId = g.StudentId,
                    GradeValue = g.GradeValue
                };

                studentGrades.Add(grade);
            }

            return studentGrades;
        }
    }
}
