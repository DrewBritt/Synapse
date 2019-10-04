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
            //Server side truncation of gradevalue if greater than 3 characters
            if(gradevalue.Length > 3)
            {
                gradevalue = gradevalue.Substring(0, 3);
            }

            await _teacherRepository.SubmitGrade(gradeid, gradevalue);
        }
    }
}
