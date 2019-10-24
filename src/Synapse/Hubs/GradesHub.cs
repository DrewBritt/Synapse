using Synapse.Data;
using Synapse.Data.Repositories;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Synapse.Hubs
{
    public class GradesHub : Hub
    {
        private TeacherRepository _teacherRepository;

        public GradesHub(SynapseContext context)
        {
            _teacherRepository = new TeacherRepository(context);
        }

        public async Task UpdateGrade(int gradeid, string gradevalue, int studentid, int classid)
        {
            //Server side truncation of gradevalue if greater than 3 characters
            if(gradevalue.Length > 3)
            {
                gradevalue = gradevalue.Substring(0, 3);
            }

            await _teacherRepository.SubmitGrade(gradeid, gradevalue);
            await UpdateGradeFinished(studentid, classid);
        }

        public async Task UpdateGradeFinished(int studentid, int classid)
        {
            int average = _teacherRepository.GetStudentAverageForClass(studentid, classid);
            await Clients.All.SendAsync("UpdateGradeFinished", average);
        }
    }
}
