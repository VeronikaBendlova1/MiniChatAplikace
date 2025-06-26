using Microsoft.EntityFrameworkCore;
using MiniChatApp.Models;


namespace MiniChatApp.Data;
   

    public class ApplicationDbContext : DbContext
     {

            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                   : base(options) { }

    public DbSet<Zprava> Zpravy { get; set; }
    }

