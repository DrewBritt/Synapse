using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Synapse.Data.ViewModels;
using Synapse.Data.Repositories;
using Synapse.Data;
using System;

namespace Synapse.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AdminRepository _adminRepository;
        private readonly TeacherRepository _teacherRepository;

        public AdminController(SynapseContext dbContext)
        {
            _adminRepository = new AdminRepository(dbContext);
            _teacherRepository = new TeacherRepository(dbContext);
        }

        #region Student Pages
        public async Task<IActionResult> Students()
        {
            return await Task.Run(() => View(_adminRepository.GetAllStudents()));
        }

        public async Task<IActionResult> ViewStudent(int studentid)
        {
            ViewStudentVM StudentToView = _adminRepository.GetStudent(studentid);
            StudentToView.Classes = _adminRepository.GetStudentSchedule(studentid);

            return await Task.Run(() => View(StudentToView));
        }

        [HttpPost]
        public async Task<IActionResult> EditStudentInfo(int? studentid, string name, string email, int gradelevel)
        {
            if(studentid == null)
            {
                return NotFound();
            }

            await _adminRepository.EditStudentInfo(studentid, name, email, Convert.ToByte(gradelevel));

            return RedirectToAction("ViewStudent", new { studentid });
        }

        public async Task<IActionResult> AddStudent()
        {
            return await Task.Run(() => View());
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent(string firstname, string lastname, string email, int gradelevel)
        {
            await _adminRepository.AddStudent(firstname, lastname, email, Convert.ToByte(gradelevel));

            return RedirectToAction("Students");
        }
        #endregion

        #region Teacher Pages
        public async Task<IActionResult> Teachers()
        {
            return await Task.Run(() => View(_adminRepository.GetAllTeachers()));
        }

        public async Task<IActionResult> ViewTeacher(int teacherid)
        {
            ViewTeacherVM TeacherToView = _adminRepository.GetTeacher(teacherid);
            TeacherToView.Classes = _adminRepository.GetTeacherSchedule(teacherid);

            return await Task.Run(() => View(TeacherToView));
        }

        [HttpPost]
        public async Task<IActionResult> EditTeacherInfo(int? teacherid, string name, string email)
        {
            if(teacherid == null)
            {
                return NotFound();
            }

            await _adminRepository.EditTeacherInfo(teacherid, name, email);

            return RedirectToAction("ViewTeacher", new { teacherid });
        }

        public async Task<IActionResult> AddTeacher()
        {
            return await Task.Run(() => View());
        }

        [HttpPost]
        public async Task<IActionResult> AddTeacher(string firstname, string lastname, string email)
        {
            await _adminRepository.AddTeacher(firstname, lastname, email);

            return RedirectToAction("Teachers");
        }
        #endregion

        #region Class Pages
        public async Task<IActionResult> Classes()
        {
            return await Task.Run(() => View(_adminRepository.GetAllClasses()));
        }

        public async Task<IActionResult> ViewClass(int classid)
        {
            ViewClassVM classToView = _adminRepository.GetClass(classid);
            classToView.EnrolledStudents = _teacherRepository.GetEnrolledStudents(classid);
            classToView.StudentAverages = _teacherRepository.GetStudentAverages(classid);
            classToView.AllTeachers = _adminRepository.GetAllTeachers();

            return await Task.Run(() => View(classToView));
        }

        [HttpPost]
        public async Task<IActionResult> EditClassInfo(int? classid, int teacherid, string location, string period)
        {
            if(classid == null)
            {
                return NotFound();
            }

            await _adminRepository.UpdateClassInfo(classid, teacherid, location, period);

            return RedirectToAction("ViewClass", new { classid });
        }
        #endregion

        #region Referral Pages
        public async Task<IActionResult> Referrals()
        {
            return await Task.Run(() => View(_adminRepository.GetAllReferrals()));
        }

        public async Task<IActionResult> ViewReferral(int referralid)
        {
            return await Task.Run(() => View(_adminRepository.GetReferral(referralid)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsHandled (int? referralid)
        {
            if (referralid == null)
            {
                return NotFound();
            }

            await _adminRepository.MarkReferralAsHandled(referralid);

            return RedirectToAction(nameof(Referrals));
        }
        #endregion
    }
}