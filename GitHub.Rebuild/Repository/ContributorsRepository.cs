using GitHub.Rebuild.Data;
using GitHub.Rebuild.Models;
using GitHub.Rebuild.Repository.IRepository;

namespace GitHub.Rebuild.Repository
{
    public class ContributorsRepository: Repository<ContributorsModel>, IContributorsRepository
    {
        public ContributorsRepository(AppDbContext context): base(context)
        {

        }
    }
}
