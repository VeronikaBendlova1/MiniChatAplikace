using Microsoft.AspNetCore.Mvc;
using MiniChatApp.Data;
using MiniChatApp.Models;

namespace MiniChat.Controllers
{
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChatController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var messages = _context.Zpravy
                .OrderBy(m => m.Cas)
                .Take(20)
                .ToList();

            return View(messages);
        }

        
    }
}