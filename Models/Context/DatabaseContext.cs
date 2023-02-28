using Microsoft.EntityFrameworkCore;
using MyNoteSampleApp.Models.Entities;

namespace MyNoteSampleApp.Models.Context
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<LikedNote> LikedNotes { get; set; }
        public DbSet<EmailMembership> EmailMemberships { get; set; }
        public DbSet<EBulletin> EBulletins { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-M8JLN9L\\SQLEXPRESS;Database=MyNotesDB;Trusted_Connection=true");
                optionsBuilder.UseLazyLoadingProxies();
            }
        }
    }
}
