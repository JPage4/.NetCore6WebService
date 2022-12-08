using Microsoft.EntityFrameworkCore;
using WebService.Entities;
using WebService.Models;

namespace WebService.DbContexts
{
    public class PayloadContext : DbContext
    {
        public PayloadContext(DbContextOptions<PayloadContext> options)
            : base(options)
        {
        }

        public DbSet<Payload> Payloads { get; set; } = null!;

        public DbSet<Message> Messages { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payload>()
                .HasData(
                new Payload()
                {
                    Id = new Guid("f0411596-95f7-4553-b5bc-f90b91534506"),
                    TS = 1530228282,
                    Sender = "testy-test-service",
                    MessageId = new Guid("1ce1fb75-d186-4700-a44a-b15382ca22ea"),
                    SentFromIp = "1.2.3.4",
                    Priority = 2
                },

                new Payload()
                {
                    Id = new Guid("77194d53-0f5d-4868-bcf4-5adc693b6e62"),
                    TS = 1684234873,
                    Sender = "whoa very test",
                    MessageId = new Guid("a5f0285e-8739-4b60-887d-d5b829164dd0"),
                    SentFromIp = "4.3.2.1",
                    Priority = 0
                });

            modelBuilder.Entity<Message>()
                .HasData(
                new Message()
                {
                    Id = new Guid("1ce1fb75-d186-4700-a44a-b15382ca22ea"),
                    Foo = "bar",
                    Baz = "bang"
                },
                 new Message()
                 {
                     Id = new Guid("a5f0285e-8739-4b60-887d-d5b829164dd0"),
                     Foo = "big",
                     Baz = "whoop"
                 });

            base.OnModelCreating(modelBuilder);
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite("connectionstring");
        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}
