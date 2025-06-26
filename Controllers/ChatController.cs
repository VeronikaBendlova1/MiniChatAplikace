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
        .OrderByDescending(m => m.Cas)  // Nejnovější jako první
        .Take(20)                        // Vezmi 20
        .ToList()                        // Přepni na paměť
        .OrderBy(m => m.Cas)             // Zase seřaď chronologicky
        .ToList();                       // Výsledek


            return View(messages);
        }

        
    }
}