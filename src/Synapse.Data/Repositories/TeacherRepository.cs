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

        /// <summary>
        /// Gets TeacherID of teacher attached to email in database
        /// </summary>
        /// <param name="email">Email of teacher to look for.</param>
        /// <returns>int: teacherid</returns>
        public int GetTeacherIdFromEmail(string email)
        {
            var teacherQuery = (from teachers in _context.Teachers
                                select new
                                {
                                    teachers.TeacherId,
                                    teachers.Email
                                }).Where(t => t.Email == email).FirstOrDefault();

            return teacherQuery.TeacherId;
        }

        #region Class Functions
        /// <summary>
        /// Gets list of classes attached to teacher mapped to email
        /// </summary>
        /// <param name="email">Email of teacher to get classes for</param>
        /// <returns>List(Class) of classes attached to teacher</returns>
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

        /// <summary>
        /// Gets list of students enrolled in class mapped to classid
        /// </summary>
        /// <param name="classid">ID of class to get enrolled students for.</param>
        /// <returns>List(Student) of students enrolled</returns>
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

        /// <summary>
        /// Verifies if the teacher of a class is the same as the teacher trying to access said class.
        /// </summary>
        /// <param name="teacherid">ID of teacher to verify</param>
        /// <param name="classid">ID of class to verify teacher of</param>
        /// <returns>Class.TeacherID == teacherid</returns>
        public bool IsTeacherForClass(int teacherid, int classid)
        {
            var classQuery = (from classes in _context.Classes
                              select new
                              {
                                  classes.ClassId,
                                  classes.TeacherId
                              }).Where(c => c.ClassId == classid).FirstOrDefault();

            return classQuery.TeacherId == teacherid;
        }
        #endregion

        #region Assignment Functions
        /// <summary>
        /// Gets list of AssignmentCategories attached to classid
        /// </summary>
        /// <param name="classid">ID of class to get AssignmentCategories for</param>
        /// <returns>List(AssignmentCategory) of categories attached to classid</returns>
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

        /// <summary>
        /// Gets list of Assignments attached to classid.
        /// </summary>
        /// <param name="classid">ID of class to get assignments for</param>
        /// <returns>List(Assignment) of assignmetns attached to classid</returns>
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

        /// <summary>
        /// Adds new assignment to database.
        /// </summary>
        /// <param name="classid">ID of class to add assignment to</param>
        /// <param name="assignmentname">Name of new assignment</param>
        /// <param name="categoryid">ID of AssignmentCategory of new assignment</param>
        /// <param name="duedate">Date that new assignment is due</param>
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

        /// <summary>
        /// Deletes assignment attached to assignmentid from database.
        /// </summary>
        /// <param name="assignmentid">ID of assignment to delete</param>
        public async Task DeleteAssignment(int assignmentid)
        {
            var assignmentToDelete = _context.Assignments.FirstOrDefault(a => a.AssignmentId == assignmentid);

            _context.Assignments.Remove(assignmentToDelete);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Grade Functions
        /// <summary>
        /// Gets list of Grades for class mapped to classid
        /// </summary>
        /// <param name="classid">ID of class to get grades for</param>
        /// <returns>GradeVM with list of grades</returns>
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

        /// <summary>
        /// Gets list of Grades of students for class mapped to classid.
        /// </summary>
        /// <param name="classid">ID of class to get grades for</param>
        /// <returns>List(Grade) of student's grades</returns>
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

        /// <summary>
        /// Calculates averages of students in class mapped to classid
        /// </summary>
        /// <param name="classid">ID of class to calculate student averages for</param>
        /// <returns>List(int) of student averages</returns>
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

        /// <summary>
        /// Updates Grade record mapped to gradeid
        /// </summary>
        /// <param name="gradeid">ID of Grade record to update</param>
        /// <param name="gradevalue">New value of Grade record</param>
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

        /// <summary>
        /// Adds empty filler grades for new assignment (called by AddAssignment())
        /// </summary>
        /// <param name="classid">ID of class to add filler grades for</param>
        /// <param name="assignmentname">Name of assignment to add filler grades for</param>
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

        /// <summary>
        /// Deletes all grades attached to assignment mapped to assignmentid.
        /// </summary>
        /// <param name="assignmentid">ID of assignment to delete grades for</param>
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

        #region Referral Functions
        /// <summary>
        /// Returns list of referrals attached to teacherid.
        /// </summary>
        /// <param name="teacherid">ID of teacher to get submitted referrals for</param>
        public List<ReferralVM> GetTeacherReferrals(int teacherid)
        {
            //LINQ Query to pull Referrals + associated Student/Teacher data
            var referralsQuery = (from referrals in _context.Referrals
                                  join students in _context.Students on referrals.StudentId equals students.StudentId
                                  join teachers in _context.Teachers on referrals.TeacherId equals teachers.TeacherId
                                  select new
                                  {
                                      referrals.ReferralId,
                                      students.StudentId,
                                      students.StudentFirstName,
                                      students.StudentLastName,
                                      teachers.TeacherId,
                                      teachers.TeacherFirstName,
                                      teachers.TeacherLastName,
                                      referrals.DateIssued,
                                      referrals.Description,
                                      referrals.Handled
                                  }).Where(r => r.TeacherId == teacherid).OrderBy(r => r.Handled);

            List<ReferralVM> referralsList = new List<ReferralVM>();
            foreach(var r in referralsQuery)
            {
                ReferralVM newReferral = new ReferralVM()
                {
                    ReferralId = r.ReferralId,
                    StudentId = r.StudentId,
                    StudentFirstName = r.StudentFirstName,
                    StudentLastName = r.StudentLastName,
                    TeacherId = r.TeacherId,
                    TeacherFirstName = r.TeacherFirstName,
                    TeacherLastName = r.TeacherLastName,
                    DateIssued = r.DateIssued,
                    Description = r.Description,
                    Handled = r.Handled
                };

                referralsList.Add(newReferral);
            }

            return referralsList;
        }
        
        /// <summary>
        /// Adds a referral to the database attached to studentid.
        /// </summary>
        /// <param name="studentid">ID of student that the referral is being submitted for</param>
        /// <param name="teacherid">ID of teacher submitting the referral</param>
        /// <param name="description">Description (100 character max) of referral</param>
        public async Task AddReferral(int studentid, int teacherid, string description)
        {
            Referral referralToSubmit = new Referral()
            {
                StudentId = studentid,
                TeacherId = teacherid,
                DateIssued = DateTime.Today,
                Description = description,
                Handled = false
            };

            _context.Referrals.Add(referralToSubmit);
            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
