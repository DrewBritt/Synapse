using AGGS.Data;
using AGGS.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AGGS.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private AdminRepository _adminRepository;
        private StudentRepository _studentRepository;

        public StudentController(AGGSContext dbContext)
        {
            _adminRepository = new AdminRepository(dbContext);
            _studentRepository = new StudentRepository(dbContext);
        }
       /*public async Task<IActionResult> Grades()
        {
            return await Task.Run(() => View(_adminRepository.GetStudentSchedule()));
        }*/

        public async Task<IActionResult> Attendance()
        {
            return await Task.Run(() => View());
        }
    }
}