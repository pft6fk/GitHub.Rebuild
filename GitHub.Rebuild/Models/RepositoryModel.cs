using System.ComponentModel.DataAnnotations;

namespace GitHub.Rebuild.Models
{
    public class RepositoryModel
    {
        [Key]
        public long Id { get; set; }
        public long GitHubId { get; set; }
        public string Name { get; set; }
        public string HtmlUrl { get; set; }
        public string FullName { get; set; }
        public string? Language { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public string? License { get; set; }
        public int StargazersCount { get; set; }
        public long GitHubOwnerId { get; set; }
        public string GitHubOwnerLogin { get; set; }
    }
    public class RepositoryDetailModel
    {
        public long Id { get; set; }
        public long ReposId { get; set; }
        public string RepositoryName { get; set; }
        public string? License { get; set; }
        public int StargazersCount { get; set; }
        public long GitHubOwnerId { get; set; }
        public string OwnerLogin { get; set; }
    }
    public class RepositoryRequest
    {
        public int total_count { get; set; }
        public GitHubRepositoryModel[] items { get; set; }
    }
}
