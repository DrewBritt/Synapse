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
                Class newClass = new Class();

                newClass.ClassId = teacherClass.ClassId;
                newClass.TeacherId = teacherClass.TeacherId;
                newClass.ClassName = teacherClass.ClassName;
                newClass.Period = teacherClass.Period;
                newClass.Location = teacherClass.Location;

                teacherClasses.Add(newClass);
            }

            return teacherClasses;
        }

        public GradesVM ViewGradesForClass(int classid)
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
                AssignmentCategory newCategory = new AssignmentCategory();

                newCategory.CategoryId = category.CategoryId;
                newCategory.ClassId = category.ClassId;
                newCategory.CategoryName = category.CategoryName;
                newCategory.CategoryWeight = category.CategoryWeight;

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
                                    }).Where(a => a.ClassId == classid).ToList();

            foreach(var assignment in assignmentsQuery)
            {
                Assignment newAssignment = new Assignment();

                newAssignment.AssignmentId = assignment.AssignmentId;
                newAssignment.AssignmentName = assignment.AssignmentName;
                newAssignment.ClassId = assignment.ClassId;
                newAssignment.CategoryId = assignment.CategoryId;
                newAssignment.DueDate = assignment.DueDate;

                assignmentsList.Add(newAssignment);
            }

            return assignmentsList;
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
                Grade newGrade = new Grade();

                newGrade.GradeId = grade.GradeId;
                newGrade.AssignmentId = grade.AssignmentId;
                newGrade.ClassId = grade.ClassId;
                newGrade.StudentId = grade.StudentId;
                newGrade.GradeValue = grade.GradeValue;

                enrolledStudentsGrades.Add(newGrade);
            }

            return enrolledStudentsGrades;
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
                Grade fillerGrade = new Grade();
                fillerGrade.AssignmentId = newAssignment.AssignmentId;
                fillerGrade.ClassId = classid;
                fillerGrade.StudentId = id.StudentId;
                fillerGrade.GradeValue = "";

                fillerGrades.Add(fillerGrade);
            }

            _context.Grades.AddRange(fillerGrades);
            await _context.SaveChangesAsync();
        }
    }
}
