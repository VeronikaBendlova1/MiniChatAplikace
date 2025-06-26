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

            // Získání portu z environmentální promìnné "PORT"
            // Railway nastavuje port zde, pokud není nastavena, použije 5000 jako výchozí
            var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";

            // Pøidání služeb do kontejneru DI (Dependency Injection)
            builder.Services.AddControllersWithViews();

            // Registrace SignalR služby pro realtime komunikaci
            builder.Services.AddSignalR();

            // Pøipojení k databázi PostgreSQL pomocí connection stringu z appsettings.json
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Vytvoøení aplikace
            var app = builder.Build();

            // Nastavení aplikace, aby naslouchala na zvoleném portu (všech IP adresách)
            app.Urls.Add($"http://*:{port}");

            // Povolení statických souborù (napø. CSS, JS, obrázky)
            app.UseStaticFiles();

            // Mapa pro SignalR hub na URL /chatHub
            app.MapHub<ChatHub>("/chatHub");

            // Pokud nejde o vývojové prostøedí, nastavíme chytání výjimek a HSTS
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // Pøesmìrování HTTP na HTTPS
            app.UseHttpsRedirection();

            // Nastavení smìrování HTTP požadavkù
            app.UseRouting();

            // Povolení autorizace (pokud ji nìkde používáš)
            app.UseAuthorization();

            // Nastavení výchozí trasy pro MVC kontrolery
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Chat}/{action=Index}/{id?}");

            // Spuštìní aplikace
            app.Run();
        }
    }
}

