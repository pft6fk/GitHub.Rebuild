using GitHub.Rebuild.Data;
using GitHub.Rebuild.Models;
using GitHub.Rebuild.Repository.IRepository;

namespace GitHub.Rebuild.Repository
{
    public class UserRepository: Repository<UserModel>, IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context): base(context)
        {
            _context = context;
        }

        public IEnumerable<RepositoryModel> GetAllUserRepository(long userId)
        {
            var repository = from db in _context.Repos
                             where db.GitHubOwnerId == userId
                             select db;
            return repository;
        }
    }
}
