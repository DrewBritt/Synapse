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
        private readonly StudentRepository _studentRepository;

        public AdminController(SynapseContext dbContext)
        {
            _adminRepository = new AdminRepository(dbContext);
            _teacherRepository = new TeacherRepository(dbContext);
            _studentRepository = new StudentRepository(dbContext);
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
            StudentToView.Classes = _studentRepository.GetStudentSchedule(studentid);

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
                CurrentClasses = _studentRepository.GetStudentSchedule(studentid),
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

        /// <summary>
        /// Action called to delete student and all attached information from database
        /// </summary>
        /// <param name="studentid">ID of student to delete data of</param>
        /// <returns>View: Students.cshtml</returns>
        public async Task<IActionResult> DeleteStudent(int studentid)
        {
            await _adminRepository.DeleteStudent(studentid);
            await _adminRepository.RemoveStudentFromAllClasses(studentid);
            await _adminRepository.DeleteStudentsReferrals(studentid);
            await _adminRepository.DeleteStudentsGrades(studentid);
            //await _adminRepository.DeleteUser(this.User.Identity.Name);

            return RedirectToAction("Students");
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
        /// PartialView of SelectTeacher modal (found on Admin/AddClass page)
        /// </summary>
        /// <returns>PartialView: _SelectTeacher.cshtml</returns>
        [HttpGet]
        public async Task<IActionResult> _SelectTeacher()
        {
            return await Task.Run(() => PartialView(_adminRepository.GetAllTeachers()));
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

        /// <summary>
        /// Deletes teacher and all associated information (classes and associated data)
        /// </summary>
        /// <param name="teacherid">ID of teacher to delete</param>
        /// <returns>View: Classes.cshtml</returns>
        public async Task<IActionResult> DeleteTeacher(int teacherid)
        {
            await _adminRepository.DeleteClassesAssignments(teacherid);
            await _adminRepository.DeleteClassesAssignmentCategories(teacherid);
            await _adminRepository.DeleteClassesGrades(teacherid);
            await _adminRepository.RemoveStudentsFromClasses(teacherid);
            await _adminRepository.DeleteTeacherReferrals(teacherid);
            await _adminRepository.DeleteClasses(teacherid);
            await _adminRepository.DeleteTeacher(teacherid);

            return RedirectToAction("Classes");
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

        /// <summary>
        /// Action page to delete class from database and all attached data
        /// </summary>
        /// <param name="classid">ID of class to delete</param>
        /// <returns>View: Classes.cshtml</returns>
        public async Task<IActionResult> DeleteClass(int classid)
        {
            await _adminRepository.DeleteClass(classid);
            await _adminRepository.DeleteClassAssignments(classid);
            await _adminRepository.DeleteClassAssignmentCategories(classid);
            await _adminRepository.DeleteClassGrades(classid);
            await _adminRepository.RemoveStudentsFromClass(classid);

            return RedirectToAction("Classes");
        }

        /// <summary>
        /// Grades management page for classid (includes setting grades, and adding assignments)
        /// </summary>
        /// <param name="classid">ID of class to update grades</param>
        /// <returns>View: Grades?classid</returns>
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
            await _teacherRepository.AddAssignment(classid, assignmentname, categoryid, duedate);

            return RedirectToAction("Grades", "Admin", new { classid });
        }

        /// <summary>
        /// Function to delete assignment mapped to assignmentid from database
        /// </summary>
        /// <param name="assignmentid">ID of assignment to delete</param>
        /// <param name="classid">ID of class to redirect view to</param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteAssignment(int assignmentid, int classid)
        {
            await _teacherRepository.DeleteAssignment(assignmentid);
            await _teacherRepository.DeleteGrades(assignmentid);

            return RedirectToAction("Grades", "Admin", new { classid });
        }

        /// <summary>
        /// Form post to add assignment to database attached to classid
        /// </summary>
        /// <param name="classid"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddAssignmentCategory(int classid, string categoryname, int categoryweight)
        {
            await _teacherRepository.AddAssignmentCategory(classid, categoryname, categoryweight);

            return RedirectToAction("Grades", "Admin", new { classid });
        }

        /// <summary>
        /// Action to delete assignment categories attached to classid
        /// </summary>
        /// <param name="categoryid">ID of category to delete</param>
        /// <param name="classid">ID of class to get category from</param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteAssignmentCategory(int categoryid, int classid)
        {
            await _teacherRepository.DeleteAssignmentCategory(categoryid);

            return RedirectToAction("Grades", "Admin", new { classid });
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
        /// Action to delete referral attached to referralid.
        /// </summary>
        /// <param name="referralid">ID of referral to delete</param>
        /// <returns>View: Referrals.cshtml</returns>
        public async Task<IActionResult> DeleteReferral(int referralid)
        {
            await _adminRepository.DeleteReferral(referralid);

            return RedirectToAction("Referrals");
        }

        /// <summary>
        /// Marks referral mapped to referralid as "handled"
        /// </summary>
        /// <param name="referralid">ID of referral to mark</param>
        /// <returns>View: Referrals.cshtml</returns>
        public async Task<IActionResult> MarkAsHandled (int referralid)
        {
            await _adminRepository.MarkReferralAsHandled(referralid);

            return RedirectToAction(nameof(Referrals));
        }
        #endregion
    }
}