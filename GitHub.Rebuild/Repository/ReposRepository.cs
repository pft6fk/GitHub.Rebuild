using GitHub.Rebuild.Data;
using GitHub.Rebuild.Models;
using GitHub.Rebuild.Repository.IRepository;

namespace GitHub.Rebuild.Repository
{
    public class ReposRepository: Repository<RepositoryModel>, IReposRepository
    {
        private readonly AppDbContext _context;
        public ReposRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
