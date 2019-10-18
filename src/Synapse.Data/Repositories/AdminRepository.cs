using Synapse.Data.ViewModels;
using Synapse.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synapse.Data.Repositories
{
    public class AdminRepository
    {
        private readonly SynapseContext _context;

        public AdminRepository(SynapseContext context)
        {
            _context = context;
        }

        #region Student Functions
        public List<Student> GetAllStudents()
        {
            var students = from s in _context.Students
                           select s;

            students = students.OrderBy(s => s.StudentLastName);

            return students.AsNoTracking().ToList();
        }

        public ViewStudentVM GetStudent(int studentid)
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

            ViewStudentVM studentToView = new ViewStudentVM
            {
                StudentId = student.StudentId,
                StudentFirstName = student.StudentFirstName,
                StudentLastName = student.StudentLastName,
                Email = student.Email,
                GradeLevel = student.GradeLevel
            };

            return studentToView;
        }

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
                    ClassName = item.ClassName,
                    Period = item.Period,
                    Location = item.Location
                };

                EnrolledClasses.Add(studentClass);
            }

            return EnrolledClasses;
        }

        public async Task EditStudentInfo(int? studentid, string name, string email, byte gradelevel)
        {
            //Grab student from database based on studentid
            var studentToUpdate = _context.Students.FirstOrDefault(s => s.StudentId == studentid);

            //Save student's current email as string for changing their user data
            string studentCurrentEmail = studentToUpdate.Email;

            //Split name into firstname and lastname
            string studentfirstname = name.Substring(0, name.IndexOf(' '));
            string studentlastname = name.Substring(name.IndexOf(' ') + 1, name.Length - name.IndexOf(' ') - 1);

            //Update info from page
            studentToUpdate.StudentFirstName = studentfirstname;
            studentToUpdate.StudentLastName = studentlastname;
            studentToUpdate.Email = email;
            studentToUpdate.GradeLevel = Convert.ToByte(gradelevel);

            //Change student data in local database instance
            _context.Update(studentToUpdate);

            /*
            //Grab user with same email as student
            var userStudentToUpdate = _context.Users.FirstOrDefault(u => u.Email == studentCurrentEmail);

            //Update user email to inputted email
            userStudentToUpdate.Email = email;

            //Save user data in local database instance
            _context.Update(userStudentToUpdate);
            */

            //Flush changes to database
            await _context.SaveChangesAsync();


        }

        public async Task AddStudent(string firstname, string lastname, string email, byte gradelevel)
        {
            //Create student object to add to database
            Student newStudent = new Student()
            {
                StudentFirstName = firstname,
                StudentLastName = lastname,
                Email = email,
                GradeLevel = gradelevel
            };

            //Add student to table
            _context.Students.Add(newStudent);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Teacher Functions
        public List<Teacher> GetAllTeachers()
        {
            var allTeachers = (from teachers in _context.Teachers
                               select new
                               {
                                   teachers.TeacherId,
                                   teachers.TeacherFirstName,
                                   teachers.TeacherLastName,
                                   teachers.Email
                               }).OrderBy(teachers => teachers.TeacherLastName).ToList();

            List<Teacher> TeachersList = new List<Teacher>();
            foreach (var teacher in allTeachers)
            {
                Teacher newTeacher = new Teacher
                {
                    TeacherId = teacher.TeacherId,
                    TeacherFirstName = teacher.TeacherFirstName,
                    TeacherLastName = teacher.TeacherLastName,
                    Email = teacher.Email
                };

                TeachersList.Add(newTeacher);
            }

            return TeachersList;
        }

        public ViewTeacherVM GetTeacher(int teacherid)
        {
            var teacher = (from teachers in _context.Teachers
                           select new
                           {
                               teachers.TeacherId,
                               teachers.TeacherFirstName,
                               teachers.TeacherLastName,
                               teachers.Email
                           }).Where(t => t.TeacherId == teacherid).FirstOrDefault();

            ViewTeacherVM teacherToView = new ViewTeacherVM
            {
                TeacherId = teacher.TeacherId,
                TeacherFirstName = teacher.TeacherFirstName,
                TeacherLastName = teacher.TeacherLastName,
                Email = teacher.Email
            };

            return teacherToView;
        }
        
        public List<Class> GetTeacherSchedule(int teacherid)
        {
            List<Class> teacherSchedule = new List<Class>();

            var classesQuery = (from classes in _context.Classes
                                select new
                                {
                                    classes.ClassId,
                                    classes.TeacherId,
                                    classes.ClassName,
                                    classes.Period,
                                    classes.Location
                                }).Where(c => c.TeacherId == teacherid).OrderBy(c => c.Period).ToList();

            foreach(var c in classesQuery)
            {
                Class teacherClass = new Class()
                {
                    ClassId = c.ClassId,
                    TeacherId = teacherid,
                    ClassName = c.ClassName,
                    Period = c.Period,
                    Location = c.Location
                };

                teacherSchedule.Add(teacherClass);
            }

            return teacherSchedule;
        }

        public async Task EditTeacherInfo(int? teacherid, string name, string email)
        {
            //Grab teacher from database based on teacherid
            var teacherToUpdate = _context.Teachers.FirstOrDefault(s => s.TeacherId == teacherid);

            //Save student's current email as string for changing their user data
            string teacherCurrentEmail = teacherToUpdate.Email;

            //Split name into firstname and lastname
            string teacherfirstname = name.Substring(0, name.IndexOf(' '));
            string teacherlastname = name.Substring(name.IndexOf(' ') + 1, name.Length - name.IndexOf(' ') - 1);

            //Update info from page
            teacherToUpdate.TeacherFirstName = teacherfirstname;
            teacherToUpdate.TeacherLastName = teacherlastname;
            teacherToUpdate.Email = email;

            //Change student data in local database instance
            _context.Update(teacherToUpdate);
            await _context.SaveChangesAsync();
        }

        public async Task AddTeacher(string firstname, string lastname, string email)
        {
            //Create Teacher object to add to database
            Teacher newTeacher = new Teacher()
            {
                TeacherFirstName = firstname,
                TeacherLastName = lastname,
                Email = email
            };

            //Add Teacher to teachers table
            _context.Teachers.Add(newTeacher);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Class Functions
        public List<ClassVM> GetAllClasses()
        {
            List<ClassVM> AllClasses = new List<ClassVM>();


            //LINQ Query to pull Classes + associated Teacher data
            var classList = (from classes in _context.Classes
                             join teachers in _context.Teachers on classes.TeacherId equals teachers.TeacherId
                             select new
                             {
                                 classes.ClassId,
                                 teachers.TeacherFirstName,
                                 teachers.TeacherLastName,
                                 teachers.Email,
                                 classes.ClassName,
                                 classes.Period,
                                 classes.Location
                             }).OrderBy(c => c.TeacherLastName).ThenBy(c => c.Period).ToList();

            //Add query results to List<ClassVM>
            foreach (var item in classList)
            {
                ClassVM newClass = new ClassVM
                {
                    ClassId = item.ClassId,
                    TeacherFirstName = item.TeacherFirstName,
                    TeacherLastName = item.TeacherLastName,
                    Email = item.Email,
                    ClassName = item.ClassName,
                    Period = item.Period,
                    Location = item.Location
                };

                AllClasses.Add(newClass);
            }

            return AllClasses;
        }

        public ViewClassVM GetClass(int classid)
        {
            ViewClassVM ClassToView = new ViewClassVM();

            //LINQ Query to pull Class data
            var viewClass = (from classes in _context.Classes
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
                                 classes.Location
                             })
                             .Where(s => s.ClassId == classid).FirstOrDefault();

            //Insert LINQ values into ViewClassVM
            ClassToView.ClassId = viewClass.ClassId;
            ClassToView.TeacherId = viewClass.TeacherId;
            ClassToView.TeacherFirstName = viewClass.TeacherFirstName;
            ClassToView.TeacherLastName = viewClass.TeacherLastName;
            ClassToView.Email = viewClass.Email;
            ClassToView.ClassName = viewClass.ClassName;
            ClassToView.Period = viewClass.Period;
            ClassToView.Location = viewClass.Location;

            return ClassToView;
        }

        public async Task EditClassInfo(int? classid, int teacherid, string location, string period)
        {
            var classToUpdate = _context.Classes.FirstOrDefault(s => s.ClassId == classid);

            classToUpdate.TeacherId = teacherid;
            classToUpdate.Location = location;
            classToUpdate.Period = period;

            _context.Update(classToUpdate);
            await _context.SaveChangesAsync();
        }

        public async Task AddClass(string classname, int teacherid, string period, string location)
        {
            //Create Class object to add to database
            Class newClass = new Class()
            {
                ClassName = classname,
                TeacherId = teacherid,
                Period = period,
                Location = location
            };

            //Add Class to classes table
            _context.Classes.Add(newClass);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Referral Functions
        public List<ReferralVM> GetAllReferrals()
        {
            List<ReferralVM> ReferralVMList = new List<ReferralVM>();

            //LINQ Query to pull Referrals + associated Student/Teacher data
            var referralList = (from referrals in _context.Referrals
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
                                })
                                .ToList();

            //Order referrals by date
            referralList = referralList.OrderBy(referrals => referrals.DateIssued).ToList();

            //Add query results to List<ReferralVM>
            foreach (var item in referralList)
            {
                ReferralVM newReferral = new ReferralVM
                {
                    ReferralId = item.ReferralId,
                    StudentId = item.StudentId,
                    StudentFirstName = item.StudentFirstName,
                    StudentLastName = item.StudentLastName,
                    TeacherId = item.TeacherId,
                    TeacherFirstName = item.TeacherFirstName,
                    TeacherLastName = item.TeacherLastName,
                    DateIssued = item.DateIssued,
                    Description = item.Description,
                    Handled = item.Handled
                };

                ReferralVMList.Add(newReferral);
            }

            return ReferralVMList;
        }

        public ViewReferralVM GetReferral(int referralid)
        {
            var referralQuery = (from referrals in _context.Referrals
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
                                 })
                                  .Where(s => s.ReferralId == referralid).FirstOrDefault();

            ViewReferralVM referralToView = new ViewReferralVM
            {
                ReferralId = referralQuery.ReferralId,
                StudentId = referralQuery.StudentId,
                StudentFirstName = referralQuery.StudentFirstName,
                StudentLastName = referralQuery.StudentLastName,
                TeacherId = referralQuery.TeacherId,
                TeacherFirstName = referralQuery.TeacherFirstName,
                TeacherLastName = referralQuery.TeacherLastName,
                DateIssued = referralQuery.DateIssued,
                Description = referralQuery.Description,
                Handled = referralQuery.Handled
            };

            var otherReferralsQuery = (from referrals in _context.Referrals
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
                                       })
                                       .Where(s => s.StudentId == referralToView.StudentId && s.ReferralId != referralid).ToList();

            List<ReferralVM> otherReferrals = new List<ReferralVM>();
            foreach (var referral in otherReferralsQuery)
            {
                ReferralVM curReferral = new ReferralVM
                {
                    ReferralId = referral.ReferralId,
                    StudentId = referral.StudentId,
                    StudentFirstName = referral.StudentFirstName,
                    StudentLastName = referral.StudentLastName,
                    TeacherId = referral.TeacherId,
                    TeacherFirstName = referral.TeacherFirstName,
                    TeacherLastName = referral.TeacherLastName,
                    DateIssued = referral.DateIssued,
                    Description = referral.Description,
                    Handled = referral.Handled
                };

                otherReferrals.Add(curReferral);
            }

            referralToView.OtherReferrals = otherReferrals;

            return referralToView;
        }

        public async Task MarkReferralAsHandled(int? referralid)
        {
            var referralToUpdate = _context.Referrals.FirstOrDefault(s => s.ReferralId == referralid);

            referralToUpdate.Handled = true;

            _context.Update(referralToUpdate);
            await _context.SaveChangesAsync();                
        }
        #endregion
    }
}