using System.Threading.Tasks;
using Application.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Application.Controllers 
{
//    [Route("/home")]
    [Controller]
    public class HomeController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index() 
        {
            return View();
        }
    }
}