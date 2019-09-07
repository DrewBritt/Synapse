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
            students = students.OrderBy(s => s.LastName);

            return View(await students.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult> ViewStudent()
        {
            return View();
        }

        public async Task<IActionResult> Classes()
        {
            List<ClassVM> ClassVMList = new List<ClassVM>();

            //LINQ Query to pull Classes + associated Teacher data
            var classList = (from classes in _context.Classes
                             join teachers in _context.Teachers on classes.TeacherId equals teachers.TeacherId 
                             select new { classes.ClassId, teachers.FirstName, teachers.LastName, teachers.Email, classes.ClassName, classes.Period }).ToList();

            //Add query results to List<ClassVM>
            foreach(var item in classList)
            {
                ClassVM newClass = new ClassVM();
                newClass.ClassId = item.ClassId;
                newClass.FirstName = item.FirstName;
                newClass.LastName = item.LastName;
                newClass.Email = item.Email;
                newClass.ClassName = item.ClassName;
                newClass.Period = item.Period;

                ClassVMList.Add(newClass);
            }

            return View(ClassVMList);
        }

        public async Task<IActionResult> ViewClass()
        {
            return View();
        }

        public async Task<IActionResult> Referrals()
        {
            return View();
        }

        public async Task<IActionResult> ViewReferral()
        {
            return View();
        }
    }
}