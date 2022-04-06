namespace GitHub.Rebuild.Models
{
    public class DetailsModel
    {
        public RepositoryModel Repository { get; set; }
        public List<ContributorsModel> Contributors { get; set; }
    }
}
