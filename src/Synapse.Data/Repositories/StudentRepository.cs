using System;
using System.Collections.Generic;
using System.Text;
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

        public List<Grade> GetAllStudentGrades(int studentid)
        {
            var getgradesquery = (from grades in _context.Grades
                                  select new
                                  {
                                      grades.GradeId,
                                      grades.AssignmentId,
                                      grades.ClassId,
                                      grades.StudentId,
                                      grades.GradeValue
                                  }).Where(g => g.StudentId == studentid)
                                  .OrderBy(g => g.ClassId);

            List<Grade> studentGrades = new List<Grade>();

            foreach(var grade in getgradesquery)
            {
                Grade newGrade = new Grade();

                newGrade.GradeId = grade.GradeId;
                newGrade.AssignmentId = grade.AssignmentId;
                newGrade.ClassId = grade.ClassId;
                newGrade.StudentId = grade.StudentId;
                newGrade.GradeValue = grade.GradeValue;
            }

            return studentGrades;
        }
    }
}
