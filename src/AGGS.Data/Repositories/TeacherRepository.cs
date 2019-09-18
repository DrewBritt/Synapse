using AGGS.Models;
using System.Collections.Generic;
using System.Linq;

namespace AGGS.Data.Repositories
{
    public class TeacherRepository
    {
        private readonly AGGSContext _context;

        public TeacherRepository(AGGSContext context)
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
    }
}
