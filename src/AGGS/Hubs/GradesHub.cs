using AGGS.Data;
using AGGS.Data.Repositories;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;

namespace AGGS.Hubs
{
    public class GradesHub : Hub
    {
        private TeacherRepository _teacherRepository;

        public GradesHub(AGGSContext context)
        {
            _teacherRepository = new TeacherRepository(context);
        }

        public async Task UpdateGrade(int gradeid, string gradevalue)
        {
            await _teacherRepository.SubmitGrade(gradeid, gradevalue);
        }
    }
}
