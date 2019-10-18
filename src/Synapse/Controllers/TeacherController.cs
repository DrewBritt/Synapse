using Synapse.Data;
using Synapse.Data.Repositories;
using Synapse.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Synapse.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherController : Controller
    {
        private readonly TeacherRepository _teacherRepository;
        private readonly AdminRepository _adminRepository;

        public TeacherController(SynapseContext dbContext)
        {
            _teacherRepository = new TeacherRepository(dbContext);
            _adminRepository = new AdminRepository(dbContext);
        }

        #region Class Pages
        public async Task<IActionResult> Classes()
        {
            //Name is always email
            var userEmail = this.User.Identity.Name;

            return await Task.Run(() => View(_teacherRepository.GetTeacherClasses(userEmail)));
        }

        public async Task<IActionResult> ViewClass(int classid)
        {
            ViewClassVM classToView = _adminRepository.GetClass(classid);
            classToView.EnrolledStudents = _teacherRepository.GetEnrolledStudents(classid);
            classToView.StudentAverages = _teacherRepository.GetStudentAverages(classid);

            return await Task.Run(() => View(classToView));
        }
        #endregion

        #region Grades/Assignment Pages
        public async Task<IActionResult> Grades(int classid)
        {
            GradesVM gradeVM = _teacherRepository.GetGradesForClass(classid);

            gradeVM.AssignmentCategories = _teacherRepository.GetAssignmentCategories(classid);
            gradeVM.ClassAssignments = _teacherRepository.GetClassAssignments(classid);
            gradeVM.EnrolledStudents = _teacherRepository.GetEnrolledStudents(classid);
            gradeVM.StudentGrades = _teacherRepository.GetEnrolledStudentsGrades(classid);
            gradeVM.PopulateStudentAverages();

            return await Task.Run(() => View(gradeVM));
        }

        [HttpPost]
        public async Task<IActionResult> AddAssignment(int classid, string assignmentname, int categoryid, string duedate)
        {
            await _teacherRepository.AddAssignment(classid, assignmentname, categoryid, duedate);

            return RedirectToAction("Grades", new { classid });
        }

        public async Task<IActionResult> DeleteAssignment(int assignmentid, int classid)
        {
            await _teacherRepository.DeleteAssignment(assignmentid);
            await _teacherRepository.DeleteGrades(assignmentid);

            return RedirectToAction("Grades", new { classid });
        }
        #endregion

        public async Task<IActionResult> ViewStudent(int studentid)
        {
            return await Task.Run(() => View());
        }
    }
}