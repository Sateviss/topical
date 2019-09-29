using System.Threading.Tasks;
using Application.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Application.Controllers
{
    [Route("/api")]
    [ApiController]
    public class ApiController : Controller
    {
        [HttpGet]
        public async Task<ActionResult<User>> Index()
        {
            User model = new User();
//            Chat chat = new Chat();
//            chat.UserA = 1;
//            chat.UserB = 2;
//            chat.Topic = "politics";
//            model.Chats.Add(chat);
//            model.Name = "Marvin";
            return model;
        }
    }
    
}