using Synapse.Data;
using Synapse.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Synapse.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private AdminRepository _adminRepository;
        private StudentRepository _studentRepository;

        public StudentController(SynapseContext dbContext)
        {
            _adminRepository = new AdminRepository(dbContext);
            _studentRepository = new StudentRepository(dbContext);
        }
        public async Task<IActionResult> Grades()
        {
            var userEmail = this.User.Identity.Name;
            int studentid = _studentRepository.GetStudentIdWithEmail(userEmail);

            return await Task.Run(() => View(_adminRepository.GetStudentSchedule(studentid)));
        }

        public async Task<IActionResult> Attendance()
        {
            return await Task.Run(() => View());
        }
    }
}