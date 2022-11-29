using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using WebService.Entities;

namespace WebService.DbContexts
{
    public class PayloadContext : DbContext
    {
        public PayloadContext(DbContextOptions<PayloadContext> options)
            : base(options)
        {
        }

        public DbSet<Payload> Payloads { get; set; } = null!;

        public DbSet<Message> Message { get; set; } = null!;
    }
}
