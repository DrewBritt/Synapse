using AGGS.Data;
using AGGS.Data.Repositories;
using AGGS.Data.ViewModels;
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
        private AdminRepository _adminRepository;

        public TeacherController(AGGSContext dbContext)
        {
            _teacherRepository = new TeacherRepository(dbContext);
            _adminRepository = new AdminRepository(dbContext);
        }

        public async Task<IActionResult> Classes()
        {
            //Name is always email
            var userEmail = this.User.Identity.Name;

            return await Task.Run(() => View(_teacherRepository.GetTeacherClasses(userEmail)));
        }

        public async Task<IActionResult> ViewClass(int classid)
        {
            ViewClassVM classToView = _adminRepository.GetClass(classid);
            classToView.EnrolledStudents = _adminRepository.GetEnrolledStudents(classid);

            return await Task.Run(() => View(classToView));
        }

        public async Task<IActionResult> Grades(int classid)
        {
            return await Task.Run(() => View());
        }

        public async Task<IActionResult> ViewStudent(int studentid)
        {
            return await Task.Run(() => View());
        }
    }
}