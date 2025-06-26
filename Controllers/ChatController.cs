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
                .OrderByDescending(m => m.Cas)
                .Take(20)
                .OrderBy(m => m.Cas)
                .ToList();

            return View(messages);
        }

        
    }
}