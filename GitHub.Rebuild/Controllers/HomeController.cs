using GitHub.Rebuild.Models;
using GitHub.Rebuild.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace GitHub.Rebuild.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private const string UserRequest = "https://api.github.com/users/";
        private const string SearchRepoRequest = "https://api.github.com/search/repositories?q="; 
        private const string BaseAddress = "https://api.github.com/";
        private readonly IUnitOfWork _unitOfWork;


        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> GetRepo(string search)
        {
            var repoRequest = new RepositoryRequest();
            if()

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");

                repoRequest = await client.GetFromJsonAsync<RepositoryRequest>(SearchRepoRequest + search);
            }

            var obj = GitHubToDbParserList(repoRequest.items);

            return View(obj);
        }
        public bool IfRepoExists(string search)
        {
            IEnumerable<RepositoryModel> RepoDb = _unitOfWork.ReposRepository.GetAll();
            //IEnumerable<ReposModel> FoundRepos = 
            //    from repo in RepoDb
            //    where repo.Name.Contains(search)
            //    select repo;

            foreach (var item in RepoDb)
            {
                if (item.Name.Contains(search))
                {
                    return true;
                }
            }
            return false;

        }
        /// <summary>
        /// Parses the Input which comes in GithubRepositoryModel to RepositoryModel 
        /// </summary>
        /// <param name="githubRepo"></param>
        /// <returns>List of RepositoryModel</returns>
        private RepositoryModel GitHubToDbParser(GitHubRepositoryModel githubRepo)
        {
            var obj = new RepositoryModel(); //temprorary obj to save into list

            obj.GitHubId = githubRepo.ID;
            obj.Name = githubRepo.Name;
            obj.HtmlUrl = githubRepo.Html_url;
            obj.UpdatedAt = githubRepo.Updated_at;
            obj.GitHubOwnerId = githubRepo.Owner.ID;
            obj.GitHubOwnerLogin = githubRepo.Owner.Login;
            obj.Language = githubRepo.Language;
            obj.StargazersCount = githubRepo.Stargazers_count;
            obj.FullName = githubRepo.Full_name;

            if (githubRepo.License != null)
                obj.License = githubRepo.License.Name;


            return obj;
        }
        /// <summary>
        /// Parses the Input which comes in List from GithubRepositoryModel to RepositoryModel 
        /// </summary>
        /// <param name="githubRepos"></param>
        /// <returns>List of RepositoryModel</returns>
        private IList<RepositoryModel> GitHubToDbParserList(IEnumerable<GitHubRepositoryModel> githubRepos)
        {
            List<RepositoryModel> repo = new List<RepositoryModel>();
            foreach (var item in githubRepos)
            {
                var obj = GitHubToDbParser(item);

                repo.Add(obj);

            }

            return repo;
        }

        public async Task<IActionResult> Details(int id)
        {
            var obj = new DetailsModel();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");

                obj.Repository = GitHubToDbParser(await client.GetFromJsonAsync<GitHubRepositoryModel> (BaseAddress + "repositories/" + id));
                obj.Contributors = JsonConvert.DeserializeObject<List<ContributorsModel>>(await client.GetStringAsync(BaseAddress + "repositories/" + id + "/contributors"));

                //var response = await client.GetAsync(BaseAddress + "repositories/" + id + "/contributors");
                //var result = await response.Content.ReadAsStringAsync();
                //obj.Contributors = JsonConvert.DeserializeObject<List<ContributorsModel>>(result);

                //obj.Contributors = await client.GetFromJsonAsync<ContributorsModel>(BaseAddress + "repositories/" + id + "/contributors"); //did not work

            }

            return View(obj);
        }

        public async Task<IActionResult> UserDetails(string login)
        {
            var obj = new UserDetailsModel();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");

                var tempRepo = JsonConvert.DeserializeObject<List<GitHubRepositoryModel>>(await client.GetStringAsync(UserRequest + login + "/repos"));
                obj.Repo = GitHubToDbParserList(tempRepo);

                var tempUser = await client.GetFromJsonAsync<GithubUserModel>(UserRequest + login);
                obj.User = GithubUserParser(tempUser);
            }

            return View(obj);

        }

        private UserModel GithubUserParser(GithubUserModel githubUserModel)
        {
            var user = new UserModel();

            user.Login = githubUserModel.login;
            user.Name = githubUserModel.name;
            user.Followers = githubUserModel.followers;
            user.Following = githubUserModel.following;
            user.Bio = githubUserModel.bio;
            user.Avatar_url = githubUserModel.avatar_url;
            user.GitHubUserId = githubUserModel.id;
            user.Company = githubUserModel.company;
            user.Location = githubUserModel.location;
            user.Html_url = githubUserModel.html_url;

            return user;
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}