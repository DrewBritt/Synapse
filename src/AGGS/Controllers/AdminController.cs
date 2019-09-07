using AGGS.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
            return View();
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