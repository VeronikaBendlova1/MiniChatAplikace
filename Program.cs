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

            // Pøidání služeb do kontejneru Dependency Injection (DI)
            builder.Services.AddControllersWithViews();

            // Registrace SignalR služby pro real-time komunikaci (websockets apod.)
            builder.Services.AddSignalR();

            // Konfigurace Entity Framework Core pro pøipojení k PostgreSQL databázi
            // Connection string se naèítá z appsettings.json v sekci "ConnectionStrings:DefaultConnection"
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Vytvoøení instance webové aplikace se všemi nakonfigurovanými službami
            var app = builder.Build();

            // Nastavení, na jakém URL a portu aplikace poslouchá
            // Naslouchá na všech IP adresách a na portu, který poskytne Railway pøes env. promìnnou
            app.Urls.Add($"http://*:{port}");

            // Povolení servírování statických souborù (CSS, JS, obrázky)
            app.UseStaticFiles();

            // Mapa (endpoint) pro SignalR hub - zde mùže klient pøistupovat k /chatHub pro realtime komunikaci
            app.MapHub<ChatHub>("/chatHub");

            // V produkèním režimu pøesmìrování na vlastní chybovou stránku a zapnutí HSTS (bezpeènost)
            if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            var errorFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
            var exception = errorFeature?.Error;

            if (exception != null)
            {
                Console.WriteLine($"Exception handled: {exception}");
            }

            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("Internal Server Error");
        });
    });

    app.UseHsts();
}


            // Pøesmìrování HTTP na HTTPS
            // Pokud chceš, mùžeš tady pro Railway proxy HTTPS vypnout (napø. zakomentovat), pokud zpùsobuje problémy
            app.UseHttpsRedirection();

            // Zapnutí smìrování požadavkù (routing)
            app.UseRouting();

            // Zapnutí autorizace (pokud používáš nìjaké zabezpeèení)
            app.UseAuthorization();

            // Nastavení výchozí MVC trasy: pokud není jinak zadáno, použije se kontroler Chat a akce Index
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Chat}/{action=Index}/{id?}");

            // Spuštìní aplikace
            app.Run();
        }
    }
}
