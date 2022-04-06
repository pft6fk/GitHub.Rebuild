using GitHub.Rebuild.Models;
using Microsoft.EntityFrameworkCore;

namespace GitHub.Rebuild.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }

        public DbSet<RepositoryModel> Repos { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<ContributorsModel> Contributors { get; set; }
    }
}
