using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AGGS.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        public async Task<IActionResult> Grades()
        {
            return await Task.Run(() => View());
        }

        public async Task<IActionResult> Attendance()
        {
            return await Task.Run(() => View());
        }
    }
}