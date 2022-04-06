using GitHub.Rebuild.Models;
using GitHub.Rebuild.Repository.Repository;

namespace GitHub.Rebuild.Repository.IRepository
{
    public interface IContributorsRepository: IRepository<ContributorsModel>
    {
        //public void AddToDb(RepositoryContributor contributors, long repoId, int numberOfContributors);
    }
}
