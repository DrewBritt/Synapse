using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AGGS.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherController : Controller
    {
        public async Task<IActionResult> Classes()
        {
            return View();
        }

        public async Task<IActionResult> ViewClass()
        {
            return View();
        }
    }
}