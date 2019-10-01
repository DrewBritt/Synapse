using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace AGGS.Hubs
{
    public class GradesHub : Hub
    {
        public async Task UpdateGrade(int gradeid, string newgradevalue)
        {

        }
    }
}
