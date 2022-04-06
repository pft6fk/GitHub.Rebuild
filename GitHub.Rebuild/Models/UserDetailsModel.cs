namespace GitHub.Rebuild.Models
{
    public class UserDetailsModel
    {
        public UserModel User { get; set; }
        public IEnumerable<RepositoryModel> Repo { get; set; }
    }
}
