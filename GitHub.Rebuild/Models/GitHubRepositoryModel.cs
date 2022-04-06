namespace GitHub.Rebuild.Models
{
    public class GitHubRepositoryModel
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Full_name { get; set; }
        public Owner Owner { get; set; }
        public string Html_url { get; set; }
        public DateTime Updated_at { get; set; }
        public int Stargazers_count { get; set; }
        public int Watchers_count { get; set; }
        public string Language { get; set; }
        public License License { get; set; }
    }

    public class Owner
    {
        public string Login { get; set; }
        public long ID { get; set; }
    }

    public class License
    {
        public string Name { get; set; }
    }

    public class GithubUserModel
    {
        public string login { get; set; }
        public int id { get; set; }
        public string node_id { get; set; }
        public string avatar_url { get; set; }
        public string gravatar_id { get; set; }
        public string url { get; set; }
        public string html_url { get; set; }
        public string followers_url { get; set; }
        public string following_url { get; set; }
        public string gists_url { get; set; }
        public string starred_url { get; set; }
        public string subscriptions_url { get; set; }
        public string organizations_url { get; set; }
        public string repos_url { get; set; }
        public string events_url { get; set; }
        public string received_events_url { get; set; }
        public string type { get; set; }
        public bool site_admin { get; set; }
        public string name { get; set; }
        public string company { get; set; }
        public string blog { get; set; }
        public string location { get; set; }
        public string email { get; set; }
        public string hireable { get; set; }
        public string bio { get; set; }
        public string twitter_username { get; set; }
        public int public_repos { get; set; }
        public int public_gists { get; set; }
        public int followers { get; set; }
        public int following { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }

}
