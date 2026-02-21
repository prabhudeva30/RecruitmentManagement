using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Database
{
    public class RadancyDbContext : DbContext
    {
        public DbSet<CandidateEntity> Candidates { get; set; }

        public RadancyDbContext(DbContextOptions<RadancyDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CandidateEntity>().ToTable("Candidate").HasKey(x => x.Id);
        }
    }
}