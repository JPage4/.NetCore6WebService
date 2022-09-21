using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using WebService.Models;
using DbContext = System.Data.Entity.DbContext;

namespace WebService.Helpers
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server with connection string from app settings
            options.UseSqlServer(Configuration.GetConnectionString("WebApiDatabase"));
        }

        public System.Data.Entity.DbSet<Payload> Payloads { get; set; }
    }
}
