using AGGS.Data;
using AGGS.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
            //Name is always email
            var userEmail = this.User.Identity.Name;

            return await Task.Run(() => View(_teacherRepository.GetTeacherClasses(userEmail)));
        }

        public async Task<IActionResult> ViewClass(int classid)
        {
            return await Task.Run(() => View());
        }

        public async Task<IActionResult> Grades(int classid)
        {
            return await Task.Run(() => View());
        }
    }
}