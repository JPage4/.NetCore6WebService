using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace WebService.Models
{
    public class PayloadContext : DbContext
    {
        public PayloadContext(DbContextOptions<PayloadContext> options)
            : base(options) 
        {
        }

        public DbSet<Payload> Payloads { get; set; } = null!;
    }
}
