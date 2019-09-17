using AGGS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGGS.Data.Repositories
{
    public class AdminRepository
    {
        private readonly AGGSContext _context;

        public AdminRepository(AGGSContext context)
        {
            _context = context;
        }

        public List<Student> GetAllStudents()
        {
            var students = from s in _context.Students
                           select s;

            students = students.OrderBy(s => s.StudentLastName);

            return students.AsNoTracking().ToList();
        }

        public Student GetStudent(int studentid)
        {
            var student = (from students in _context.Students
                           select new
                           {
                               students.StudentId,
                               students.StudentFirstName,
                               students.StudentLastName,
                               students.Email,
                               students.GradeLevel
                           }).Where(s => s.StudentId == studentid).FirstOrDefault();

            Student studentToView = new Student();

            studentToView.StudentId = student.StudentId;
            studentToView.StudentFirstName = student.StudentFirstName;
            studentToView.StudentLastName = student.StudentLastName;
            studentToView.Email = student.Email;
            studentToView.GradeLevel = student.GradeLevel;

            return studentToView;
        }
    }
}
