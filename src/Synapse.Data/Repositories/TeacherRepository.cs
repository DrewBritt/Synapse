using Synapse.Data.Models;
using Synapse.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synapse.Data.Repositories
{
    public class TeacherRepository
    {
        private readonly SynapseContext _context;

        public TeacherRepository(SynapseContext context)
        {
            _context = context;
        }

        #region Class Functions
        public List<Class> GetTeacherClasses(string email)
        {
            List<Class> teacherClasses = new List<Class>();

            var teacherClassesQuery = (from teachers in _context.Teachers
                                       join classes in _context.Classes on teachers.TeacherId equals classes.TeacherId
                                       select new
                                       {
                                           classes.ClassId,
                                           classes.TeacherId,
                                           classes.ClassName,
                                           classes.Period,
                                           classes.Location,
                                           teachers.Email
                                       }).Where(c => c.Email == email).ToList();

            foreach(var teacherClass in teacherClassesQuery)
            {
                Class newClass = new Class
                {
                    ClassId = teacherClass.ClassId,
                    TeacherId = teacherClass.TeacherId,
                    ClassName = teacherClass.ClassName,
                    Period = teacherClass.Period,
                    Location = teacherClass.Location
                };

                teacherClasses.Add(newClass);
            }

            return teacherClasses;
        }

        public List<Student> GetEnrolledStudents(int classid)
        {
            var listOfStudentsInClass = (from students in _context.Students
                                         join studentclasses in _context.StudentsClasses on students.StudentId equals studentclasses.StudentId
                                         join classes in _context.Classes on studentclasses.ClassId equals classes.ClassId
                                         select new
                                         {
                                             students.StudentId,
                                             students.StudentFirstName,
                                             students.StudentLastName,
                                             students.Email,
                                             students.GradeLevel,
                                             studentclasses.ClassId
                                         }).OrderBy(s => s.StudentLastName);

            List<Student> ListOfStudentsEnrolled = new List<Student>();
            foreach (var item in listOfStudentsInClass)
            {
                if (item.ClassId == classid)
                {
                    Student newStudent = new Student
                    {
                        StudentId = item.StudentId,
                        StudentFirstName = item.StudentFirstName,
                        StudentLastName = item.StudentLastName,
                        Email = item.Email,
                        GradeLevel = item.GradeLevel
                    };

                    ListOfStudentsEnrolled.Add(newStudent);
                }

            }

            return ListOfStudentsEnrolled;
        }
        #endregion

        #region Assignment Functions
        public List<AssignmentCategory> GetAssignmentCategories(int classid)
        {
            List<AssignmentCategory> assignmentCategories = new List<AssignmentCategory>();

            var assignmentCategoriesQuery = (from assignmentcategories in _context.AssignmentCategories
                                             select new
                                             {
                                                 assignmentcategories.CategoryId,
                                                 assignmentcategories.ClassId,
                                                 assignmentcategories.CategoryName,
                                                 assignmentcategories.CategoryWeight
                                             }).Where(a => a.ClassId == classid).ToList();

            foreach(var category in assignmentCategoriesQuery)
            {
                AssignmentCategory newCategory = new AssignmentCategory
                {
                    CategoryId = category.CategoryId,
                    ClassId = category.ClassId,
                    CategoryName = category.CategoryName,
                    CategoryWeight = category.CategoryWeight
                };

                assignmentCategories.Add(newCategory);
            }

            return assignmentCategories;
        }

        public List<Assignment> GetClassAssignments(int classid)
        {
            List<Assignment> assignmentsList = new List<Assignment>();

            var assignmentsQuery = (from assignments in _context.Assignments
                                    select new
                                    {
                                        assignments.AssignmentId,
                                        assignments.AssignmentName,
                                        assignments.ClassId,
                                        assignments.CategoryId,
                                        assignments.DueDate
                                    }).Where(a => a.ClassId == classid).OrderBy(a => a.DueDate).ToList();

            foreach(var assignment in assignmentsQuery)
            {
                Assignment newAssignment = new Assignment
                {
                    AssignmentId = assignment.AssignmentId,
                    AssignmentName = assignment.AssignmentName,
                    ClassId = assignment.ClassId,
                    CategoryId = assignment.CategoryId,
                    DueDate = assignment.DueDate
                };

                assignmentsList.Add(newAssignment);
            }

            return assignmentsList;
        }

        public async Task AddAssignment(int classid, string assignmentname, int categoryid, string duedate)
        {
            //Create Assignment object with data
            var assignment = new Assignment()
            {
                ClassId = classid,
                AssignmentName = assignmentname,
                CategoryId = categoryid,
                DueDate = Convert.ToDateTime(duedate)
            };

            //Add to Assignments table
            _context.Assignments.Add(assignment);
            await _context.SaveChangesAsync();

            //Create filler grades in database for new assignment
            await AddFillerGrades(classid, assignmentname);
        }

        public async Task DeleteAssignment(int assignmentid)
        {
            var assignmentToDelete = _context.Assignments.FirstOrDefault(a => a.AssignmentId == assignmentid);

            _context.Assignments.Remove(assignmentToDelete);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Grade Functions
        public GradesVM GetGradesForClass(int classid)
        {
            GradesVM gradeVM = new GradesVM();

            var gradesQuery = (from classes in _context.Classes
                               select new
                               {
                                   classes.ClassId,
                                   classes.TeacherId,
                                   classes.ClassName,
                                   classes.Period
                               }).Where(c => c.ClassId == classid).FirstOrDefault();

            gradeVM.ClassId = gradesQuery.ClassId;
            gradeVM.TeacherId = gradesQuery.TeacherId;
            gradeVM.ClassName = gradesQuery.ClassName;
            gradeVM.Period = gradesQuery.Period;

            return gradeVM;
        }

        public List<Grade> GetEnrolledStudentsGrades(int classid)
        {
            List<Grade> enrolledStudentsGrades = new List<Grade>();

            var gradesQuery = (from grades in _context.Grades
                               join students in _context.Students on grades.StudentId equals students.StudentId
                               select new
                               {
                                   grades.GradeId,
                                   grades.AssignmentId,
                                   grades.ClassId,
                                   grades.StudentId,
                                   grades.GradeValue,
                                   students.StudentLastName
                               }).Where(g => g.ClassId == classid).OrderBy(s => s.StudentLastName).ThenBy(s => s.GradeId).ToList();

            foreach(var grade in gradesQuery)
            {
                Grade newGrade = new Grade
                {
                    GradeId = grade.GradeId,
                    AssignmentId = grade.AssignmentId,
                    ClassId = grade.ClassId,
                    StudentId = grade.StudentId,
                    GradeValue = grade.GradeValue
                };

                enrolledStudentsGrades.Add(newGrade);
            }

            return enrolledStudentsGrades;
        }

        public List<int> GetStudentAverages(int classid)
        {
            List<int> averages = new List<int>();

            List<AssignmentCategory> AssignmentCategories = GetAssignmentCategories(classid);
            List<Assignment> ClassAssignments = GetClassAssignments(classid);
            List<Student> EnrolledStudents = GetEnrolledStudents(classid);
            List<Grade> StudentGrades = GetEnrolledStudentsGrades(classid);

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

            return averages;
        }

        public async Task SubmitGrade(int gradeid, string gradevalue)
        {
            //Grab grade object from database
            var gradeToUpdate = _context.Grades.FirstOrDefault(g => g.GradeId == gradeid);

            //Update gradevalue for this grade
            gradeToUpdate.GradeValue = gradevalue;

            //Explicitly setting gradeid to avoid duplicates
            gradeToUpdate.GradeId = gradeid;

            //Save and flush changes to database
            _context.Update(gradeToUpdate);
            await _context.SaveChangesAsync();
        }

        public async Task AddFillerGrades(int classid, string assignmentname)
        {
            //Get new assignment from database
            var newAssignment = (from assignments in _context.Assignments
                                 select new
                                 {
                                     assignments.AssignmentId,
                                     assignments.AssignmentName
                                 }).Where(a => a.AssignmentName == assignmentname).FirstOrDefault();

            var enrolledStudentsIds = (from studentsclasses in _context.StudentsClasses
                                       select new
                                       {
                                           studentsclasses.StudentId,
                                           studentsclasses.ClassId
                                       }).Where(s => s.ClassId == classid).ToList();

            List<Grade> fillerGrades = new List<Grade>();

            foreach (var id in enrolledStudentsIds)
            {
                Grade fillerGrade = new Grade
                {
                    AssignmentId = newAssignment.AssignmentId,
                    ClassId = classid,
                    StudentId = id.StudentId,
                    GradeValue = ""
                };

                fillerGrades.Add(fillerGrade);
            }

            _context.Grades.AddRange(fillerGrades);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGrades(int assignmentid)
        {
            List<Grade> gradesOfAssignment = new List<Grade>();

            var gradesQuery = (from grades in _context.Grades
                               select new
                               {
                                   grades.GradeId,
                                   grades.AssignmentId
                               }).Where(g => g.AssignmentId == assignmentid).ToList();

            foreach (var grade in gradesQuery)
            {
                Grade gradeToDelete = new Grade
                {
                    GradeId = grade.GradeId,
                    AssignmentId = grade.AssignmentId
                };

                gradesOfAssignment.Add(gradeToDelete);
            }

            _context.Grades.RemoveRange(gradesOfAssignment);
            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
