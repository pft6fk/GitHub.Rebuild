namespace GitHub.Rebuild.Models
{
    public class ContributorsModel
    {
        public long Id { get; set; }
        public long RepoId { get; set; }
        public string Login { get; set; }
        public int Contributions { get; set; }
        public int NumberOfContributors { get; set; }
    }

    public class Contributors
    {
        public ContributorsModel[] Contributor { get; set; }
    }
}
