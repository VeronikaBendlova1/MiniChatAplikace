using Microsoft.AspNetCore.SignalR;
using MiniChatApp.Data; // namespace pro ApplicationDbContext
using MiniChatApp.Models;

namespace MiniChatApp.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _dbContext;
        private static readonly Dictionary<string, string> TypingUsers = new();

        public ChatHub(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Poslání zprávy od klienta
        public async Task SendMessage(string user, string message)
        {
            var zprava = new Zprava
            {
                UzivatelskeJmeno = user,
                Text = message,
                Cas = DateTime.UtcNow // PostgreSQL očekává UTC
            };

            _dbContext.Zpravy.Add(zprava);
            await _dbContext.SaveChangesAsync();

            // Všem klientům odešli zprávu
            await Clients.All.SendAsync("ReceiveMessage", user, message, zprava.Cas);
        }

        // Když uživatel začne psát
        public async Task Typing(string username)
        {
            TypingUsers[Context.ConnectionId] = username;

            await Clients.Others.SendAsync("UserTyping", username);

            _ = Task.Run(async () =>
            {
                await Task.Delay(3000);
                TypingUsers.Remove(Context.ConnectionId);
                await Clients.All.SendAsync("UserStoppedTyping", username);
            });
        }

        // Odstraníme uživatele, když se odpojí
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (TypingUsers.TryGetValue(Context.ConnectionId, out string username))
            {
                TypingUsers.Remove(Context.ConnectionId);
                await Clients.All.SendAsync("UserStoppedTyping", username);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
