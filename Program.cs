using Microsoft.EntityFrameworkCore;
using MiniChatApp.Data;
using MiniChatApp.Hubs;

namespace MiniChatApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddSignalR(); // Registrace služby

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            // MUSÍ BÝT PØED Build();

            var app = builder.Build();

            // Pøidání podpory pro statické soubory (css, js, obrázky atd.)
            app.UseStaticFiles();

            app.MapHub<ChatHub>("/chatHub");

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Chat}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

