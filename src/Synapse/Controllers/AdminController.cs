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
        /// <summary>
        /// Admin page with list of all records in "students" table
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Students()
        {
            return await Task.Run(() => View(_adminRepository.GetAllStudents()));
        }

        /// <summary>
        /// Admin page with data about student mapped to "studentid".
        /// Data includes: Name, email, grade, class schedule
        /// </summary>
        /// <param name="studentid">ID of student in "students" table</param>
        /// <returns></returns>
        public async Task<IActionResult> ViewStudent(int studentid)
        {
            ViewStudentVM StudentToView = _adminRepository.GetStudent(studentid);
            StudentToView.Classes = _adminRepository.GetStudentSchedule(studentid);

            return await Task.Run(() => View(StudentToView));
        }

        /// <summary>
        /// Edits data of student record in database that maps to "studentid"
        /// </summary>
        /// <param name="studentid">ID of Student to edit information of</param>
        /// <param name="name">New (full)name of student, name is split into firstname and lastname in Repository</param>
        /// <param name="email">New email of student</param>
        /// <param name="gradelevel">New grade level of student</param>
        /// <returns>Redirects user to ViewStudent?studentid</returns>
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

        /// <summary>
        /// Form page to add a new student record to database
        /// </summary>
        /// <returns>View()</returns>
        public async Task<IActionResult> AddStudent()
        {
            return await Task.Run(() => View());
        }

        /// <summary>
        /// Form Post to add a new student record to database
        /// </summary>
        /// <param name="firstname">First Name of new student</param>
        /// <param name="lastname">Last Name of new student</param>
        /// <param name="email">Email of new student</param>
        /// <param name="gradelevel">Grade Level of new student</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddStudent(string firstname, string lastname, string email, int gradelevel)
        {
            await _adminRepository.AddStudent(firstname, lastname, email, Convert.ToByte(gradelevel));

            return RedirectToAction("Students");
        }

        /// <summary>
        /// Removes a class from a student's schedule (dictated in studentsclasses table)
        /// </summary>
        /// <param name="studentid">StudentID of student to remove from class</param>
        /// <param name="classid">ClassId of class to remove student from</param>
        /// <returns>View: ViewStudent?studentid</returns>
        public async Task<IActionResult> RemoveStudentFromClass(int studentid, int classid)
        {
            await _adminRepository.RemoveStudentFromClass(studentid, classid);

            return RedirectToAction("ViewStudent", new { studentid });
        }

        /// <summary>
        /// PartialView of AddStudentToClass modal (found on Admin/ViewStudent page)
        /// </summary>
        /// <param name="studentid">StudentID of student to add to class</param>
        /// <returns>PartialView: _AddStudentToClass.cshtml</returns>
        [HttpGet]
        public async Task<IActionResult> _AddStudentToClass(int studentid)
        {
            _AddStudentToClassVM model = new _AddStudentToClassVM()
            {
                AllClasses = _adminRepository.GetAllClasses(),
                CurrentClasses = _adminRepository.GetStudentSchedule(studentid),
                StudentId = studentid
            };

            return await Task.Run(() => PartialView(model));
        }

        /// <summary>
        /// Action called to add student to class in database
        /// </summary>
        /// <param name="studentid">StudentID of student to add to class</param>
        /// <param name="classid">ClassID of class to add student to</param>
        /// <returns>View: ViewStudent?studentid</returns>
        public async Task<IActionResult> AddStudentToClass(int studentid, int classid)
        {
            await _adminRepository.AddStudentToClass(studentid, classid);

            return RedirectToAction("ViewStudent", new { studentid });
        }
        #endregion

        #region Teacher Pages
        /// <summary>
        /// Admin page with list of all teachers in "teachers" table
        /// </summary>
        /// <returns>View: Teachers.cshtml</returns>
        public async Task<IActionResult> Teachers()
        {
            return await Task.Run(() => View(_adminRepository.GetAllTeachers()));
        }

        /// <summary>
        /// Admin page with data mapped to TeacherID in "teachers" table
        /// Data includes: name, email, teacher's classes schedule
        /// </summary>
        /// <param name="teacherid">TeacherID of teacher to pull data of</param>
        /// <returns>View: ViewTeacher?teacherid</returns>
        public async Task<IActionResult> ViewTeacher(int teacherid)
        {
            ViewTeacherVM TeacherToView = _adminRepository.GetTeacher(teacherid);
            TeacherToView.Classes = _adminRepository.GetTeacherSchedule(teacherid);

            return await Task.Run(() => View(TeacherToView));
        }

        /// <summary>
        /// Edit data of teacher mapped to teacherid
        /// </summary>
        /// <param name="teacherid">ID of teacher record to update</param>
        /// <param name="name">New name of teacher</param>
        /// <param name="email">New email of teacher</param>
        /// <returns>View: ViewTeacher?teacherid</returns>
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

        /// <summary>
        /// Form page to add new teacher to database
        /// </summary>
        /// <returns>View: AddTeacher.cshtml</returns>
        public async Task<IActionResult> AddTeacher()
        {
            return await Task.Run(() => View());
        }

        /// <summary>
        /// Form Post to add new teacher to database
        /// </summary>
        /// <param name="firstname">First Name of new teacher</param>
        /// <param name="lastname">Last Name of new teacher</param>
        /// <param name="email">Email of new teacher</param>
        /// <returns>View: Teachers.cshtml</returns>
        [HttpPost]
        public async Task<IActionResult> AddTeacher(string firstname, string lastname, string email)
        {
            await _adminRepository.AddTeacher(firstname, lastname, email);

            return RedirectToAction("Teachers");
        }
        #endregion

        #region Class Pages
        /// <summary>
        /// Admin page with list of all classes in "classes" table
        /// </summary>
        /// <returns>View: Classes.cshtml</returns>
        public async Task<IActionResult> Classes()
        {
            return await Task.Run(() => View(_adminRepository.GetAllClasses()));
        }

        /// <summary>
        /// Admin page with data of class tied to classid.
        /// Data includes: Class Name, Teacher information, Class Period and Location, enrolled students
        /// </summary>
        /// <param name="classid">ClassID of class to view</param>
        /// <returns>View: ViewClass?classid</returns>
        public async Task<IActionResult> ViewClass(int classid)
        {
            ViewClassVM classToView = _adminRepository.GetClass(classid);
            classToView.EnrolledStudents = _teacherRepository.GetEnrolledStudents(classid);
            classToView.StudentAverages = _teacherRepository.GetStudentAverages(classid);
            classToView.AllTeachers = _adminRepository.GetAllTeachers();

            return await Task.Run(() => View(classToView));
        }

        /// <summary>
        /// Form Post to edit information of class tied to classid
        /// </summary>
        /// <param name="classid">ID of class to edit information of</param>
        /// <param name="teacherid">ID of new Teacher for class</param>
        /// <param name="location">New Location of class</param>
        /// <param name="period">New Period of class</param>
        /// <returns>View: ViewClass?classid</returns>
        [HttpPost]
        public async Task<IActionResult> EditClassInfo(int? classid, int teacherid, string location, string period)
        {
            if(classid == null)
            {
                return NotFound();
            }

            await _adminRepository.EditClassInfo(classid, teacherid, location, period);

            return RedirectToAction("ViewClass", new { classid });
        }
        
        /// <summary>
        /// Form page to add new class to database
        /// </summary>
        /// <returns>View: AddClass.cshtml</returns>
        public async Task<IActionResult> AddClass()
        {
            return await Task.Run(() => View());
        }

        /// <summary>
        /// Form Post to add new class to database
        /// </summary>
        /// <param name="classname">Name of new class</param>
        /// <param name="teacherid">ID of teacher for new class</param>
        /// <param name="period">Period of new class</param>
        /// <param name="location">Location of new class</param>
        /// <returns>View: Classes.cshtml</returns>
        [HttpPost]
        public async Task<IActionResult> AddClass(string classname, int teacherid, string period, string location)
        {
            await _adminRepository.AddClass(classname, teacherid, period, location);

            return RedirectToAction("Classes");
        }
        #endregion

        #region Referral Pages
        /// <summary>
        /// Admin page of all referrals in database
        /// </summary>
        /// <returns>View: Referrals.cshtml</returns>
        public async Task<IActionResult> Referrals()
        {
            return await Task.Run(() => View(_adminRepository.GetAllReferrals()));
        }

        /// <summary>
        /// Admin page to view data of referral mapped to referralid
        /// </summary>
        /// <param name="referralid">ID of referral to view</param>
        /// <returns></returns>
        public async Task<IActionResult> ViewReferral(int referralid)
        {
            return await Task.Run(() => View(_adminRepository.GetReferral(referralid)));
        }

        /// <summary>
        /// Marks referral mapped to referralid as "handled"
        /// </summary>
        /// <param name="referralid">ID of referral to mark</param>
        /// <returns>View: Referrals.cshtml</returns>
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