using AGGS.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using AGGS.ViewModels;
using System.Collections.Generic;

namespace AGGS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AGGSContext _context;

        public AdminController(AGGSContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Students()
        {
            //Pull list of students from Students DbSet
            var students = from s in _context.Students
                           select s;

            //Sort students by last name
            students = students.OrderBy(s => s.StudentLastName);

            return View(await students.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult> ViewStudent()
        {
            return await Task.Run(() => View());
        }

        public async Task<IActionResult> Classes()
        {
            List<ClassVM> ClassVMList = new List<ClassVM>();

            //LINQ Query to pull Classes + associated Teacher data
            var classList = (from classes in _context.Classes
                             join teachers in _context.Teachers on classes.TeacherId equals teachers.TeacherId 
                             select new { classes.ClassId, teachers.TeacherFirstName, teachers.TeacherLastName, teachers.Email, classes.ClassName, classes.Period }).ToList();

            //Order classes by teacher last name
            classList = classList.OrderBy(classes => classes.TeacherLastName).ToList();

            //Add query results to List<ClassVM>
            foreach(var item in classList)
            {
                ClassVM newClass = new ClassVM();
                newClass.ClassId = item.ClassId;
                newClass.TeacherFirstName = item.TeacherFirstName;
                newClass.TeacherLastName = item.TeacherLastName;
                newClass.Email = item.Email;
                newClass.ClassName = item.ClassName;
                newClass.Period = item.Period;

                ClassVMList.Add(newClass);
            }

            return await Task.Run(() => View(ClassVMList));
        }

        public async Task<IActionResult> ViewClass()
        {
            return await Task.Run(() => View());
        }

        public async Task<IActionResult> Referrals()
        {
            List<ReferralVM> ReferralVMList = new List<ReferralVM>();

            //LINQ Query to pull Referrals + associated Student/Teacher data
            var referralList = (from referrals in _context.Referrals
                                join students in _context.Students on referrals.StudentId equals students.StudentId
                                join teachers in _context.Teachers on referrals.TeacherId equals teachers.TeacherId
                                select new { referrals.ReferralId, students.StudentId, students.StudentFirstName, students.StudentLastName,
                                    teachers.TeacherId, teachers.TeacherFirstName, teachers.TeacherLastName, referrals.DateIssued, referrals.Description}).ToList();

            //Order referrals by date
            referralList = referralList.OrderBy(referrals => referrals.DateIssued).ToList();

            //Add query results to List<ReferralVM>
            foreach(var item in referralList)
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

                ReferralVMList.Add(newReferral);
            }

            return await Task.Run(() => View(ReferralVMList));
        }

        public async Task<IActionResult> ViewReferral()
        {
            return await Task.Run(() => View());
        }
    }
}