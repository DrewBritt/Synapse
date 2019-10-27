using Synapse.Data;
using Synapse.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Synapse.Data.ViewModels;

namespace Synapse.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private AdminRepository _adminRepository;
        private StudentRepository _studentRepository;
        private TeacherRepository _teacherRepository;

        public StudentController(SynapseContext dbContext)
        {
            _adminRepository = new AdminRepository(dbContext);
            _studentRepository = new StudentRepository(dbContext);
            _teacherRepository = new TeacherRepository(dbContext);
        }

        public async Task<IActionResult> Grades()
        {
            var userEmail = this.User.Identity.Name;
            int studentid = _studentRepository.GetStudentIdWithEmail(userEmail);

            StudentGradesVM model = new StudentGradesVM()
            {
                StudentId = studentid,
                StudentSchedule = _studentRepository.GetStudentSchedule(studentid),
                ClassAverages = _studentRepository.CalculateStudentAverages(studentid)
            };

            return await Task.Run(() => View(model));
        }

        public async Task<IActionResult> _AssignmentsTable(int studentid, int classid)
        {
            AssignmentsTableVM model = new AssignmentsTableVM()
            {
                ClassId = classid,
                classAssignments = _teacherRepository.GetClassAssignments(classid),
                classAssignmentCategories = _teacherRepository.GetAssignmentCategories(classid),
                studentGrades = _teacherRepository.GetStudentsGrades(studentid, classid)
            };

            return await Task.Run(() => PartialView(model));
        }
    }
}