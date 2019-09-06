using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AGGS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public IActionResult Students()
        {
            return View();
        }

        public IActionResult ViewStudent()
        {
            return View();
        }

        public IActionResult Classes()
        {
            return View();
        }

        public IActionResult ViewClass()
        {
            return View();
        }

        public IActionResult Referrals()
        {
            return View();
        }

        public IActionResult ViewReferral()
        {
            return View();
        }
    }
}