using Synapse.Data.ViewModels;
using Synapse.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

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
        /// <summary>
        /// Gets list of all students in database
        /// </summary>
        /// <returns>List(Student) of all students</returns>
        public List<Student> GetAllStudents()
        {
            var students = from s in _context.Students
                           select s;

            students = students.OrderBy(s => s.StudentLastName);

            return students.AsNoTracking().ToList();
        }

        /// <summary>
        /// Gets record of student mapped to studentid.
        /// </summary>
        /// <param name="studentid">ID of studentid to get record of</param>
        /// <returns>ViewStudentVM with data of student</returns>
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

        /// <summary>
        /// Edit record of student mapped to studentid.
        /// </summary>
        /// <param name="studentid">ID of student to edit info of</param>
        /// <param name="name">New name of student</param>
        /// <param name="email">New email of student</param>
        /// <param name="gradelevel">New grade level of student</param>
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

        /// <summary>
        /// Adds new student to database
        /// </summary>
        /// <param name="firstname">First Name of new student</param>
        /// <param name="lastname">Last Name of new student</param>
        /// <param name="email">Email of new student</param>
        /// <param name="gradelevel">Grade Level of new student</param>
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

        /// <summary>
        /// Removes student from class in "studentsclasses" table
        /// </summary>
        /// <param name="studentid">ID of student to remove from class</param>
        /// <param name="classid">ID of class to remove student from</param>
        public async Task RemoveStudentFromClass(int studentid, int classid)
        {
            var studentClassToRemove = _context.StudentsClasses.FirstOrDefault(c => c.StudentId == studentid && c.ClassId == classid);

            _context.StudentsClasses.Remove(studentClassToRemove);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Adds student to a class in "studentsclasses" table
        /// </summary>
        /// <param name="studentid">ID of student to add to class</param>
        /// <param name="classid">ID of class to add student to</param>
        public async Task AddStudentToClass(int studentid, int classid)
        {
            StudentsClass newClass = new StudentsClass()
            {
                StudentId = studentid,
                ClassId = classid
            };

            _context.StudentsClasses.Add(newClass);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes student from database
        /// </summary>
        /// <param name="studentid">ID of student to delete</param>
        public async Task DeleteStudent(int studentid)
        {
            Student student = new Student() { StudentId = studentid };
            _context.Students.Attach(student);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Removes student from all classes attached to student
        /// </summary>
        /// <param name="studentid">ID of student to remove from classes</param>
        public async Task RemoveStudentFromAllClasses(int studentid)
        {
            var studentsClasses = (from studentsclasses in _context.StudentsClasses
                                   select new
                                   {
                                       studentsclasses.StudentId,
                                       studentsclasses.ClassId
                                   }).Where(sc => sc.StudentId == studentid);

            List<StudentsClass> studentsClassesList = new List<StudentsClass>();
            foreach(var sc in studentsClasses)
            {
                StudentsClass newStudentClass = new StudentsClass()
                {
                    StudentId = sc.StudentId,
                    ClassId = sc.ClassId
                };

                studentsClassesList.Add(newStudentClass);
            }

            _context.StudentsClasses.RemoveRange(studentsClassesList);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Removes all referrals attached to student
        /// </summary>
        /// <param name="studentid">ID of student to delete referrals for</param>
        public async Task DeleteStudentsReferrals(int studentid)
        {
            var studentReferrals = (from referrals in _context.Referrals
                                    select new
                                    {
                                        referrals.ReferralId,
                                        referrals.StudentId
                                    }).Where(r => r.StudentId == studentid);

            List<Referral> studentReferralsList = new List<Referral>();
            foreach(var r in studentReferrals)
            {
                Referral newReferral = new Referral()
                {
                    ReferralId = r.ReferralId,
                    StudentId = r.StudentId
                };

                studentReferralsList.Add(newReferral);
            }

            _context.Referrals.RemoveRange(studentReferralsList);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Removes all grades attached to student
        /// </summary>
        /// <param name="studentid">ID of student to delete grades for</param>
        public async Task DeleteStudentsGrades(int studentid)
        {
            var studentGrades = (from grades in _context.Grades
                                  select new
                                  {
                                      grades.GradeId,
                                      grades.StudentId
                                  }).Where(g => g.StudentId == studentid);

            List<Grade> studentsGradesList = new List<Grade>();
            foreach(var g in studentGrades)
            {
                Grade studentGrade = new Grade()
                {
                    GradeId = g.GradeId,
                    StudentId = g.StudentId
                };

                studentsGradesList.Add(studentGrade);
            }

            _context.Grades.RemoveRange(studentsGradesList);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes Identity user account attached to email
        /// </summary>
        /// <param name="email">Email of account to delete</param>\
        public async Task DeleteUser(string email)
        {
            var account = (from aspnetusers in _context.AspNetUsers
                           select new
                           {
                               aspnetusers.Email,
                               aspnetusers.Id
                           }).Where(u => u.Email == email).FirstOrDefault();

            IdentityUser user = new IdentityUser()
            {
                Email = account.Email,
                Id = account.Id
            };

            _context.AspNetUsers.Remove(user);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Removes students from classes attached to teacherid.
        /// </summary>
        /// <param name="teacherid">ID of teacher to get classes of</param>
        /// <returns></returns>
        public async Task RemoveStudentsFromClasses(int teacherid)
        {
            var classesQuery = (from studentclasses in _context.StudentsClasses
                                join classes in _context.Classes on studentclasses.ClassId equals classes.ClassId
                                select new
                                {
                                    studentclasses.StudentId,
                                    studentclasses.ClassId,
                                    classes.TeacherId
                                }).Where(c => c.TeacherId == teacherid);

            List<StudentsClass> connectionsToDelete = new List<StudentsClass>();
            foreach(var c in classesQuery)
            {
                StudentsClass sc = new StudentsClass()
                {
                    StudentId = c.StudentId,
                    ClassId = c.ClassId
                };

                connectionsToDelete.Add(sc);
            }

            _context.StudentsClasses.AttachRange(connectionsToDelete);
            _context.StudentsClasses.RemoveRange(connectionsToDelete);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Teacher Functions
        /// <summary>
        /// Gets list of all teachers in database
        /// </summary>
        /// <returns>List(Teacher) of all teachers in database</returns>
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

        /// <summary>
        /// Gets data of teacher mapped to teacherid
        /// </summary>
        /// <param name="teacherid">ID of teacher to get data of</param>
        /// <returns>ViewTeacherVM with data of teacher</returns>
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
        
        /// <summary>
        /// Gets schedule of teacher mapped to teacherid.
        /// </summary>
        /// <param name="teacherid">ID of teacher to get schedule of</param>
        /// <returns>List(Class) of classes in teacher's schedule</returns>
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

        /// <summary>
        /// Edit data of teacher mapped to teacherid.
        /// </summary>
        /// <param name="teacherid">ID of teacher to edit data of</param>
        /// <param name="name">New name of teacher</param>
        /// <param name="email">New email of teacher</param>
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

        /// <summary>
        /// Add new teacher to database.
        /// </summary>
        /// <param name="firstname">First Name of new teacher</param>
        /// <param name="lastname">Last Name of new teacher</param>
        /// <param name="email">Email of new teacher</param>
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

        /// <summary>
        /// Deletes teacher record from "teachers" table attached to teacherid
        /// </summary>
        /// <param name="teacherid">ID of teacher to delete</param>
        public async Task DeleteTeacher(int teacherid)
        {
            Teacher teacherToDelete = new Teacher() { TeacherId = teacherid };

            _context.Teachers.Attach(teacherToDelete);
            _context.Teachers.Remove(teacherToDelete);

            await _context.SaveChangesAsync();
        }
        #endregion

        #region Class Functions
        /// <summary>
        /// Gets list of all classes in database.
        /// </summary>
        /// <returns>List(ClassWithTeacherInfo) of all classes in database</returns>
        public List<ClassWithTeacherInfo> GetAllClasses()
        {
            List<ClassWithTeacherInfo> AllClasses = new List<ClassWithTeacherInfo>();

            //LINQ Query to pull Classes + associated Teacher data
            var classList = (from classes in _context.Classes
                             join teachers in _context.Teachers on classes.TeacherId equals teachers.TeacherId
                             select new
                             {
                                 classes.ClassId,
                                 teachers.TeacherId,
                                 teachers.TeacherFirstName,
                                 teachers.TeacherLastName,
                                 teachers.Email,
                                 classes.ClassName,
                                 classes.Period,
                                 classes.Location
                             }).OrderBy(c => c.TeacherLastName).ThenBy(c => c.Period).ToList();

            //Add query results to List<ClassWithTeacherInfo>
            foreach (var item in classList)
            {
                ClassWithTeacherInfo newClass = new ClassWithTeacherInfo()
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

                AllClasses.Add(newClass);
            }

            return AllClasses;
        }

        /// <summary>
        /// Gets data of class mapped to classid.
        /// </summary>
        /// <param name="classid">ID of class to get data of</param>
        /// <returns>ViewClassVM with data of class mapped to classid</returns>
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

        /// <summary>
        /// Edit data of class mapped to classid.
        /// </summary>
        /// <param name="classid">ID of class to edit data of</param>
        /// <param name="teacherid">ID of new teacher for class</param>
        /// <param name="location">New Location of class</param>
        /// <param name="period">New Period of class</param>
        public async Task EditClassInfo(int? classid, int teacherid, string location, string period)
        {
            var classToUpdate = _context.Classes.FirstOrDefault(s => s.ClassId == classid);

            classToUpdate.TeacherId = teacherid;
            classToUpdate.Location = location;
            classToUpdate.Period = period;

            _context.Update(classToUpdate);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Add new class to database.
        /// </summary>
        /// <param name="classname">Name of new class</param>
        /// <param name="teacherid">ID of teacher of new class</param>
        /// <param name="period">Period of new class</param>
        /// <param name="location">Location of new class</param>
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

        /// <summary>
        /// Deletes class attached to classid.
        /// </summary>
        /// <param name="classid">ID of class to delete</param>
        public async Task DeleteClass(int classid)
        {
            Class classToDelete = new Class() { ClassId = classid };

            _context.Classes.Attach(classToDelete);
            _context.Classes.Remove(classToDelete);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes all classes attached to teacherid.
        /// </summary>
        /// <param name="teacherid">ID of teacher to delete attached classes of</param>
        public async Task DeleteClasses(int teacherid)
        {
            var classesQuery = (from classes in _context.Classes
                                select new
                                {
                                    classes.ClassId,
                                    classes.TeacherId
                                }).Where(c => c.TeacherId == teacherid);

            List<Class> classesToDelete = new List<Class>();
            foreach(var c in classesQuery)
            {
                Class newClass = new Class()
                {
                    ClassId = c.ClassId,
                    TeacherId = c.TeacherId
                };

                classesToDelete.Add(newClass);
            }

            _context.Classes.AttachRange(classesToDelete);
            _context.Classes.RemoveRange(classesToDelete);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes all assignments attached to classid.
        /// </summary>
        /// <param name="classid">ID of class to delete assignments of</param>
        public async Task DeleteClassAssignments(int classid)
        {
            var assignmentsQuery = (from assignments in _context.Assignments
                                    select new
                                    {
                                        assignments.AssignmentId,
                                        assignments.ClassId
                                    }).Where(a => a.ClassId == classid);

            List<Assignment> assignmentsToDelete = new List<Assignment>();
            foreach(var a in assignmentsQuery)
            {
                Assignment newAssignment = new Assignment()
                {
                    AssignmentId = a.AssignmentId,
                    ClassId = a.ClassId
                };

                assignmentsToDelete.Add(newAssignment);
            }

            _context.Assignments.RemoveRange(assignmentsToDelete);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes all assignment categories attached to classid.
        /// </summary>
        /// <param name="classid">ID of class to delete assignment categories of</param>
        public async Task DeleteClassAssignmentCategories(int classid)
        {
            var assignmentCategoriesQuery = (from assignmentcategories in _context.AssignmentCategories
                                             select new
                                             {
                                                 assignmentcategories.CategoryId,
                                                 assignmentcategories.ClassId
                                             }).Where(ac => ac.ClassId == classid);

            List<AssignmentCategory> categoriesToDelete = new List<AssignmentCategory>();
            foreach(var ac in assignmentCategoriesQuery)
            {
                AssignmentCategory category = new AssignmentCategory()
                {
                    CategoryId = ac.CategoryId,
                    ClassId = ac.ClassId
                };

                categoriesToDelete.Add(category);
            }

            _context.AssignmentCategories.RemoveRange(categoriesToDelete);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes all grades attached to classid.
        /// </summary>
        /// <param name="classid">ID of class to delete grades of.</param>
        public async Task DeleteClassGrades(int classid)
        {
            var gradesQuery = (from grades in _context.Grades
                               select new
                               {
                                   grades.GradeId,
                                   grades.ClassId
                               }).Where(g => g.ClassId == classid);

            List<Grade> gradesToDelete = new List<Grade>();
            foreach(var g in gradesQuery)
            {
                Grade grade = new Grade()
                {
                    GradeId = g.GradeId,
                    ClassId = g.ClassId
                };

                gradesToDelete.Add(grade);
            }

            _context.Grades.RemoveRange(gradesToDelete);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Removes all students from class attached to classid.
        /// </summary>
        /// <param name="classid">ID of class to remove students from.</param>
        public async Task RemoveStudentsFromClass(int classid)
        {
            var studentsClassesQuery = (from studentsclasses in _context.StudentsClasses
                                        select new
                                        {
                                            studentsclasses.ClassId,
                                            studentsclasses.StudentId
                                        }).Where(sc => sc.ClassId == classid);

            List<StudentsClass> connectionsToDelete = new List<StudentsClass>();
            foreach(var sc in studentsClassesQuery)
            {
                StudentsClass connection = new StudentsClass()
                {
                    ClassId = sc.ClassId,
                    StudentId = sc.StudentId
                };

                connectionsToDelete.Add(connection);
            }

            _context.StudentsClasses.RemoveRange(connectionsToDelete);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes all assignments of classes attached to teacherid
        /// </summary>
        /// <param name="teacherid">ID of teacher to delete classes assignments</param>
        public async Task DeleteClassesAssignments(int teacherid)
        {
            var assignmentsQuery = (from assignments in _context.Assignments
                                    join classes in _context.Classes on assignments.ClassId equals classes.ClassId
                                    select new
                                    {
                                        classes.ClassId,
                                        classes.TeacherId,
                                        assignments.AssignmentId
                                    }).Where(a => a.TeacherId == teacherid);

            List<Assignment> assignmentsToDelete = new List<Assignment>();
            foreach(var a in assignmentsQuery)
            {
                Assignment newAssignment = new Assignment()
                {
                    ClassId = a.ClassId,
                    AssignmentId = a.AssignmentId
                };

                assignmentsToDelete.Add(newAssignment);
            }

            _context.Assignments.AttachRange(assignmentsToDelete);
            _context.Assignments.RemoveRange(assignmentsToDelete);

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes all assignment categories of classes attached to teacherid
        /// </summary>
        /// <param name="teacherid">ID of teacher to delete assignment categories from classes</param>
        public async Task DeleteClassesAssignmentCategories(int teacherid)
        {
            var categoriesQuery = (from categories in _context.AssignmentCategories
                                   join classes in _context.Classes on categories.ClassId equals classes.ClassId
                                   select new
                                   {
                                       categories.CategoryId,
                                       classes.ClassId,
                                       classes.TeacherId
                                   }).Where(c => c.TeacherId == teacherid);

            List<AssignmentCategory> categoriesToDelete = new List<AssignmentCategory>();
            foreach(var c in categoriesQuery)
            {
                AssignmentCategory ac = new AssignmentCategory()
                {
                    CategoryId = c.CategoryId,
                    ClassId = c.ClassId,
                };

                categoriesToDelete.Add(ac);
            }

            _context.AssignmentCategories.AttachRange(categoriesToDelete);
            _context.AssignmentCategories.RemoveRange(categoriesToDelete);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes all grades attached to all classes attached to teacherid
        /// </summary>
        /// <param name="teacherid">ID of teacher to delete grades of classes</param>
        public async Task DeleteClassesGrades(int teacherid)
        {
            var gradesQuery = (from grades in _context.Grades
                               join classes in _context.Classes on grades.ClassId equals classes.ClassId
                               select new
                               {
                                   grades.GradeId,
                                   classes.ClassId,
                                   classes.TeacherId
                               }).Where(g => g.TeacherId == teacherid);

            List<Grade> gradesToDelete = new List<Grade>();
            foreach(var g in gradesQuery)
            {
                Grade grade = new Grade()
                {
                    GradeId = g.GradeId,
                    ClassId = g.ClassId
                };

                gradesToDelete.Add(grade);
            }

            _context.Grades.AttachRange(gradesToDelete);
            _context.Grades.RemoveRange(gradesToDelete);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Referral Functions
        /// <summary>
        /// Gets list of all referrals in database.
        /// </summary>
        /// <returns>List(ReferralVM) of all referrals in database</returns>
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

        /// <summary>
        /// Gets record of referral mapped to referralid
        /// </summary>
        /// <param name="referralid">ID of referral to get record of</param>
        /// <returns>ViewReferralVM with data of referral</returns>
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

        /// <summary>
        /// Deletes referral record attached to referralid
        /// </summary>
        /// <param name="referralid">ID of referral to delete</param>
        public async Task DeleteReferral(int referralid)
        {
            Referral referralToDelete = new Referral()
            {
                ReferralId = referralid
            };

            _context.Referrals.Attach(referralToDelete);
            _context.Referrals.Remove(referralToDelete);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Marks referral mapped to referralid as "handled"
        /// </summary>
        /// <param name="referralid">ID of referral to mark as "handled"</param>
        public async Task MarkReferralAsHandled(int? referralid)
        {
            var referralToUpdate = _context.Referrals.FirstOrDefault(s => s.ReferralId == referralid);

            referralToUpdate.Handled = true;

            _context.Update(referralToUpdate);
            await _context.SaveChangesAsync();                
        }

        /// <summary>
        /// Deletes all referrals attached to teacherid.
        /// </summary>
        /// <param name="teacherid">ID of teacher to delete referrals of.</param>
        public async Task DeleteTeacherReferrals(int teacherid)
        {
            var referralsQuery = (from referrals in _context.Referrals
                                  select new
                                  {
                                      referrals.TeacherId,
                                      referrals.ReferralId
                                  }).Where(r => r.TeacherId == teacherid);

            List<Referral> referralsToDelete = new List<Referral>();
            foreach(var r in referralsQuery)
            {
                Referral referral = new Referral()
                {
                    TeacherId = r.TeacherId,
                    ReferralId = r.ReferralId
                };
                referralsToDelete.Add(referral);
            }

            _context.Referrals.AttachRange(referralsToDelete);
            _context.Referrals.RemoveRange(referralsToDelete);
            await _context.SaveChangesAsync();
        }
        #endregion
    }
}