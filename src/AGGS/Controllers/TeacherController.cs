using AGGS.Data;
using AGGS.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AGGS.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherController : Controller
    {
        private TeacherRepository _teacherRepository;

        public TeacherController(AGGSContext dbContext)
        {
            _teacherRepository = new TeacherRepository(dbContext);
        }

        public async Task<IActionResult> Classes()
        {
            return await Task.Run(() => View());
        }

        public async Task<IActionResult> ViewClass()
        {
            return await Task.Run(() => View());
        }
    }
}