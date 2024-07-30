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

        /*        protected override void OnModelCreating(ModelBuilder modelBuilder)
                {
                    modelBuilder.Entity<Licence>()
                        .HasMany(l => l.Sessions)
                        .WithOne(s => s.Licence)
                        .HasForeignKey(s => s.LicenceId);

                    modelBuilder.Entity<Licence>()
                        .HasOne(l => l.CurrentSession)
                        .WithOne()
                        .HasForeignKey<Licence>(l => l.CurrentSessionId);

                    modelBuilder.Entity<Session>()
                        .HasOne(s => s.User)
                        .WithMany(u => u.Sessions)
                        .HasForeignKey(s => s.UserId);


                    base.OnModelCreating(modelBuilder);
                }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Licence>()
                .HasOne(l => l.CurrentSession)
                .WithOne()
                .HasForeignKey<Session>("LicenceId"); // Shadow property

            modelBuilder.Entity<Session>()
                .HasOne(s=> s.Licence)
                .WithOne()
                .HasForeignKey<Licence>("CurrentSessionId"); // Shadow property

            modelBuilder.Entity<Session>()
                .HasOne(s => s.User)
                .WithMany(u => u.Sessions);

            base.OnModelCreating(modelBuilder);
        }
    }
}
