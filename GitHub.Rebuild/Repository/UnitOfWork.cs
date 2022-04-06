using GitHub.Rebuild.Data;
using GitHub.Rebuild.Repository;
using GitHub.Rebuild.Repository.IRepository;

namespace GitHub.Rebuild.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            //DetailsRepository = new DetailsRepository(context);
            ReposRepository = new ReposRepository(context);
            UserRepository = new UserRepository(context);
            ContributorsRepository = new ContributorsRepository(context);
        }

        //public IDetailsRepository DetailsRepository { get; private set; }
        public IReposRepository ReposRepository { get; private set; }
        public IUserRepository UserRepository { get; private set; }
        public IContributorsRepository ContributorsRepository { get; private set; }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
    