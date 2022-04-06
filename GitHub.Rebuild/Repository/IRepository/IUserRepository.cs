using GitHub.Rebuild.Models;
using GitHub.Rebuild.Repository.Repository;

namespace GitHub.Rebuild.Repository.IRepository
{
    public interface IUserRepository: IRepository<UserModel>
    {
        public IEnumerable<RepositoryModel> GetAllUserRepository(long userId);
    }
}
