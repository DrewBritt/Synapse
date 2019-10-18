using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Synapse.Data.Models;

namespace Synapse.Controllers
{
    public class HomeController : Controller
    {
        private readonly Data.SynapseContext _context;

        public HomeController(Data.SynapseContext _context)
        {
            this._context = _context;
        }

        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => View());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
