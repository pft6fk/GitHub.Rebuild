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
        public void AddToDb(ContributorsModel contributors, long repoId, int numberOfContributors)
        {
            var obj = new ContributorsModel();

            obj.Contributions = contributors.Contributions;
            obj.RepoId = repoId;
            obj.Login = contributors.Login;
            obj.NumberOfContributors = numberOfContributors;

            Add(obj);
        }
    }
}
