using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AGGS.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        public IActionResult Grades()
        {
            return View();
        }

        public IActionResult Attendance()
        {
            return View();
        }
    }
}