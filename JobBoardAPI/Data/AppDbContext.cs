using JobBoardAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace JobBoardAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Job> jobs { get; set; }
        public DbSet<Application> applications { get; set; }
    }
}
