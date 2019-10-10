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
        private AdminRepository _adminRepository;

        public AdminController(SynapseContext dbContext)
        {
            _adminRepository = new AdminRepository(dbContext);
        }

        public async Task<IActionResult> Students()
        {
            return View(_adminRepository.GetAllStudents());
        }

        public async Task<IActionResult> ViewStudent(int studentid)
        {
            ViewStudentVM StudentToView = _adminRepository.GetStudent(studentid);
            StudentToView.Classes = _adminRepository.GetStudentSchedule(studentid);

            return await Task.Run(() => View(StudentToView));
        }

        [HttpPost]
        public async Task<IActionResult> EditStudentInfo(int? studentid, string name, string email, string gradelevel)
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

        public async Task<IActionResult> Classes()
        {
            return await Task.Run(() => View(_adminRepository.GetAllClasses()));
        }

        public async Task<IActionResult> ViewClass(int classid)
        {
            ViewClassVM classToView = _adminRepository.GetClass(classid);
            classToView.EnrolledStudents = _adminRepository.GetEnrolledStudents(classid);
            classToView.AllTeachers = _adminRepository.GetTeachers();

            return await Task.Run(() => View(classToView));
        }

        [HttpPost]
        public async Task<IActionResult> ViewClass(int? classid, int teacherid, string location, string period)
        {
            if(classid == null)
            {
                return NotFound();
            }

            await _adminRepository.UpdateClassInfo(classid, teacherid, location, period);

            return RedirectToAction("ViewClass", new { classid });
        }

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
    }
}