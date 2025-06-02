using Microsoft.EntityFrameworkCore;

namespace JobBoardAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }      
    }
}
