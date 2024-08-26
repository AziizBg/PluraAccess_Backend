using Microsoft.EntityFrameworkCore;
using OddoBhf.Models;


namespace OddoBhf.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Licence> Licences { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Queue> Queue { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Licence has current session
            modelBuilder.Entity<Licence>()
                .HasOne(l => l.CurrentSession)
                .WithOne()
                .HasForeignKey<Licence>("CurrentSessionId"); // Shadow property

            //Session has Licence
            modelBuilder.Entity<Session>()
                .HasOne(s => s.Licence)
                .WithMany();

            //Session has User
            modelBuilder.Entity<Session>()
                .HasOne(s => s.User)
                .WithMany(u => u.Sessions);

            //Queue has User
            modelBuilder.Entity<Queue>()
                .HasOne(q => q.User)
                .WithOne()
                .HasForeignKey<Queue>("UserId");

            base.OnModelCreating(modelBuilder);
        }
    }
}
