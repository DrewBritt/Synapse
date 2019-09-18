﻿using AGGS.Models;
using AGGS.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            ViewStudentVM studentToView = new ViewStudentVM();

            studentToView.StudentId = student.StudentId;
            studentToView.StudentFirstName = student.StudentFirstName;
            studentToView.StudentLastName = student.StudentLastName;
            studentToView.Email = student.Email;
            studentToView.GradeLevel = student.GradeLevel;

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
                            }).Where(s => s.StudentId == studentid)
                            .ToList();

            List<ClassWithTeacherInfo> EnrolledClasses = new List<ClassWithTeacherInfo>();

            foreach (var item in schedule)
            {
                ClassWithTeacherInfo studentClass = new ClassWithTeacherInfo();

                studentClass.ClassId = item.ClassId;
                studentClass.TeacherId = item.TeacherId;
                studentClass.TeacherFirstName = item.TeacherFirstName;
                studentClass.TeacherLastName = item.TeacherLastName;
                studentClass.ClassName = item.ClassName;
                studentClass.Period = item.Period;
                studentClass.Location = item.Location;

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
            string studentlastname = name.Substring(name.IndexOf(' ') + 1, name.Length - name.IndexOf(' '));

            //Update info from page
            studentToUpdate.StudentFirstName = studentfirstname;
            studentToUpdate.StudentLastName = studentlastname;
            studentToUpdate.Email = email;
            studentToUpdate.GradeLevel = Convert.ToByte(gradelevel);

            //Change student data in local database instance
            _context.Update(studentToUpdate);

            //Grab user with same email as student
            var userStudentToUpdate = _context.Users.First(u => u.Email == studentCurrentEmail);

            //Update user email to inputted email
            userStudentToUpdate.Email = email;

            //Save user data in local database instance
            _context.Update(userStudentToUpdate);

            //Flush changes to database
            await _context.SaveChangesAsync();


        }

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
                             })
                             .ToList();

            //Order classes by teacher last name
            classList = classList.OrderBy(classes => classes.TeacherLastName).ToList();

            //Add query results to List<ClassVM>
            foreach (var item in classList)
            {
                ClassVM newClass = new ClassVM();
                newClass.ClassId = item.ClassId;
                newClass.TeacherFirstName = item.TeacherFirstName;
                newClass.TeacherLastName = item.TeacherLastName;
                newClass.Email = item.Email;
                newClass.ClassName = item.ClassName;
                newClass.Period = item.Period;
                newClass.Location = item.Location;

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
                                         });

            List<Student> ListOfStudentsEnrolled = new List<Student>();
            foreach (var item in listOfStudentsInClass)
            {
                if (item.ClassId == classid)
                {
                    Student newStudent = new Student();

                    newStudent.StudentId = item.StudentId;
                    newStudent.StudentFirstName = item.StudentFirstName;
                    newStudent.StudentLastName = item.StudentLastName;
                    newStudent.Email = item.Email;
                    newStudent.GradeLevel = item.GradeLevel;

                    ListOfStudentsEnrolled.Add(newStudent);
                }

            }

            return ListOfStudentsEnrolled;
        }

        public List<Teacher> GetTeachers()
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
                Teacher newTeacher = new Teacher();

                newTeacher.TeacherId = teacher.TeacherId;
                newTeacher.TeacherFirstName = teacher.TeacherFirstName;
                newTeacher.TeacherLastName = teacher.TeacherLastName;
                newTeacher.Email = teacher.Email;

                TeachersList.Add(newTeacher);
            }

            return TeachersList;
        }

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
                ReferralVM newReferral = new ReferralVM();
                newReferral.ReferralId = item.ReferralId;
                newReferral.StudentId = item.StudentId;
                newReferral.StudentFirstName = item.StudentFirstName;
                newReferral.StudentLastName = item.StudentLastName;
                newReferral.TeacherId = item.TeacherId;
                newReferral.TeacherFirstName = item.TeacherFirstName;
                newReferral.TeacherLastName = item.TeacherLastName;
                newReferral.DateIssued = item.DateIssued;
                newReferral.Description = item.Description;
                newReferral.Handled = item.Handled;

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

            ViewReferralVM referralToView = new ViewReferralVM();
            referralToView.ReferralId = referralQuery.ReferralId;
            referralToView.StudentId = referralQuery.StudentId;
            referralToView.StudentFirstName = referralQuery.StudentFirstName;
            referralToView.StudentLastName = referralQuery.StudentLastName;
            referralToView.TeacherId = referralQuery.TeacherId;
            referralToView.TeacherFirstName = referralQuery.TeacherFirstName;
            referralToView.TeacherLastName = referralQuery.TeacherLastName;
            referralToView.DateIssued = referralQuery.DateIssued;
            referralToView.Description = referralQuery.Description;
            referralToView.Handled = referralQuery.Handled;

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
                ReferralVM curReferral = new ReferralVM();
                curReferral.ReferralId = referral.ReferralId;
                curReferral.StudentId = referral.StudentId;
                curReferral.StudentFirstName = referral.StudentFirstName;
                curReferral.StudentLastName = referral.StudentLastName;
                curReferral.TeacherId = referral.TeacherId;
                curReferral.TeacherFirstName = referral.TeacherFirstName;
                curReferral.TeacherLastName = referral.TeacherLastName;
                curReferral.DateIssued = referral.DateIssued;
                curReferral.Description = referral.Description;
                curReferral.Handled = referral.Handled;

                otherReferrals.Add(curReferral);
            }

            referralToView.OtherReferrals = otherReferrals;

            return referralToView;
        }

        public async Task UpdateClassInfo(int? classid, int teacherid, string location, string period)
        {
            var classToUpdate = _context.Classes.FirstOrDefault(s => s.ClassId == classid);

            classToUpdate.TeacherId = teacherid;
            classToUpdate.Location = location;
            classToUpdate.Period = period;

            _context.Update(classToUpdate);
            await _context.SaveChangesAsync();
        }

        public async Task MarkReferralAsHandled(int? referralid)
        {
            var referralToUpdate = _context.Referrals.FirstOrDefault(s => s.ReferralId == referralid);

            referralToUpdate.Handled = true;

            _context.Update(referralToUpdate);
            await _context.SaveChangesAsync();                
        }
    }
}
