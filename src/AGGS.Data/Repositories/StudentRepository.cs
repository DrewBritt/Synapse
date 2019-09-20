using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AGGS.Data.Repositories
{
    public class StudentRepository
    {
        private readonly AGGSContext _context;

        public StudentRepository(AGGSContext context)
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
    }
}
