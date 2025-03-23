using Microsoft.EntityFrameworkCore;
namespace task6.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Presentation> Presentations { get; set; }
        public DbSet<Slide> Slides { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Slide>()
                .HasOne<Presentation>()
                .WithMany(p => p.Slides)
                .HasForeignKey(s => s.PresentationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
