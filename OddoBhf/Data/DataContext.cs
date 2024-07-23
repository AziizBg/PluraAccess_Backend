using Microsoft.EntityFrameworkCore;
using OddoBhf.Models;


namespace OddoBhf.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options)
        {
        }

        public DbSet<Licence> Licences { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<User> Users { get; set; }

/*        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Licence>()
                .HasOne(l => l.CurrentSession)
                .WithOne(s => s.Licence)
                .HasForeignKey<Session>(s => s.LicenceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Session>()
                .HasOne(s => s.Licence)
                .WithOne(l => l.CurrentSession)
                .HasForeignKey<Licence>(l => l.CurrentSessionId)
                .OnDelete(DeleteBehavior.Cascade);
        }*/
    }
}
