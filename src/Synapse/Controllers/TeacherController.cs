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
        /// <summary>
        /// Teacher page to view all classes in teacher's schedule
        /// </summary>
        /// <returns>View: Classes.cshtml</returns>
        public async Task<IActionResult> Classes()
        {
            //Name is always email
            var userEmail = this.User.Identity.Name;

            return await Task.Run(() => View(_teacherRepository.GetTeacherClasses(userEmail)));
        }

        /// <summary>
        /// Teacher page to view data of class mapped to classid
        /// </summary>
        /// <param name="classid">ID of class to view</param>
        /// <returns>View: ViewClass?classid</returns>
        public async Task<IActionResult> ViewClass(int classid)
        {
            //Verification to ensure that teacher is tied to class
            int teacherid = _teacherRepository.GetTeacherIdFromEmail(this.User.Identity.Name);
            if(!_teacherRepository.IsTeacherForClass(teacherid, classid))
            {
                return RedirectToAction(nameof(Classes));
            }

            ViewClassVM classToView = _adminRepository.GetClass(classid);
            classToView.EnrolledStudents = _teacherRepository.GetEnrolledStudents(classid);
            classToView.StudentAverages = _teacherRepository.GetStudentAverages(classid);

            return await Task.Run(() => View(classToView));
        }
        #endregion

        #region Grades/Assignment Pages
        /// <summary>
        /// Grades management page for teacher's class (includes setting grades, and adding assignments)
        /// </summary>
        /// <param name="classid">ID of class to update grades</param>
        /// <returns>View: Grades?classid</returns>
        public async Task<IActionResult> Grades(int classid)
        {
            //Verification to ensure that teacher is tied to class
            int teacherid = _teacherRepository.GetTeacherIdFromEmail(this.User.Identity.Name);
            if (!_teacherRepository.IsTeacherForClass(teacherid, classid))
            {
                return RedirectToAction(nameof(Classes));
            }

            GradesVM gradeVM = _teacherRepository.GetGradesForClass(classid);

            gradeVM.AssignmentCategories = _teacherRepository.GetAssignmentCategories(classid);
            gradeVM.ClassAssignments = _teacherRepository.GetClassAssignments(classid);
            gradeVM.EnrolledStudents = _teacherRepository.GetEnrolledStudents(classid);
            gradeVM.StudentGrades = _teacherRepository.GetEnrolledStudentsGrades(classid);
            gradeVM.PopulateStudentAverages();

            return await Task.Run(() => View(gradeVM));
        }

        /// <summary>
        /// Form Post to add new assignment for class mapped to classid
        /// </summary>
        /// <param name="classid">ID of class to add assignment to</param>
        /// <param name="assignmentname">Name of new assignment</param>
        /// <param name="categoryid">ID of AssignmentCategory to put assignment under (manages grade weights)</param>
        /// <param name="duedate">Date that new assignment is due</param>
        /// <returns>View: Grades?classid</returns>
        [HttpPost]
        public async Task<IActionResult> AddAssignment(int classid, string assignmentname, int categoryid, string duedate)
        {
            //Verification to ensure that teacher is tied to class
            int teacherid = _teacherRepository.GetTeacherIdFromEmail(this.User.Identity.Name);
            if (!_teacherRepository.IsTeacherForClass(teacherid, classid))
            {
                return RedirectToAction(nameof(Classes));
            }

            await _teacherRepository.AddAssignment(classid, assignmentname, categoryid, duedate);

            return RedirectToAction("Grades", new { classid });
        }

        /// <summary>
        /// Function to delete assignment mapped to assignmentid from database
        /// </summary>
        /// <param name="assignmentid">ID of assignment to delete</param>
        /// <param name="classid">ID of class to redirect view to</param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteAssignment(int assignmentid, int classid)
        {
            //Verification to ensure that teacher is tied to class
            int teacherid = _teacherRepository.GetTeacherIdFromEmail(this.User.Identity.Name);
            if (!_teacherRepository.IsTeacherForClass(teacherid, classid))
            {
                return RedirectToAction(nameof(Classes));
            }

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