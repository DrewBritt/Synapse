using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AGGS.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherController : Controller
    {
        public IActionResult Classes()
        {
            return View();
        }

        public IActionResult ViewClass()
        {
            return View();
        }
    }
}