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
            IList<RepositoryModel> obj = new List<RepositoryModel>();

            if (IfRepoExists(search))
            {
                obj = TakeRepoFromDb(search);
            }
            else
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");

                    repoRequest = await client.GetFromJsonAsync<RepositoryRequest>(SearchRepoRequest + search);
                }

                obj = GitHubToDbParserList(repoRequest.items);
                
                foreach (var item in obj)
                {
                    _unitOfWork.ReposRepository.Add(item);
                }
                _unitOfWork.Save();
            }

            return View(obj);
        }
        public async Task<IActionResult> UserDetails(string login)
        {
            var obj = new UserDetailsModel();

            if (IfUserExists(login))
            {
                obj.User = TakeUserFromDb(login);
                obj.Repo = TakeUserRepos(login);
            }
            else
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");

                    var tempRepo = JsonConvert.DeserializeObject<List<GitHubRepositoryModel>>(await client.GetStringAsync(UserRequest + login + "/repos"));
                    obj.Repo = GitHubToDbParserList(tempRepo);

                    var tempUser = await client.GetFromJsonAsync<GithubUserModel>(UserRequest + login);
                    obj.User = GithubUserParser(tempUser);
                }

                _unitOfWork.UserRepository.Add(obj.User);
                foreach (var item in obj.Repo)
                {
                    _unitOfWork.ReposRepository.Add(item);
                }
                _unitOfWork.Save();

            }


            return View(obj);

        }
        public async Task<IActionResult> Details(int id)
        {
            var obj = new DetailsModel();

            var repos = _unitOfWork.ReposRepository.GetAll();
            obj.Repository = repos.Where(n => n.GitHubId == id).FirstOrDefault();

            if (IfContributorsExists(id))
            {
                obj.Contributors = TakeContributorsFromDb(id);
            }
            else
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");

                    obj.Contributors = JsonConvert.DeserializeObject<List<ContributorsModel>>(await client.GetStringAsync(BaseAddress + "repositories/" + id + "/contributors"));

                    //var response = await client.GetAsync(BaseAddress + "repositories/" + id + "/contributors");
                    //var result = await response.Content.ReadAsStringAsync();
                    //obj.Contributors = JsonConvert.DeserializeObject<List<ContributorsModel>>(result);

                    //obj.Contributors = await client.GetFromJsonAsync<ContributorsModel>(BaseAddress + "repositories/" + id + "/contributors"); //did not work

                }

                foreach (var item in obj.Contributors)
                {
                    _unitOfWork.ContributorsRepository.AddToDb(item, id, obj.Contributors.Count);
                }

                _unitOfWork.Save();

            }

            return View(obj);
        }
        public IActionResult Privacy()
        {
            return View();
        }


        /// <summary>
        /// Searches from db for repositories
        /// </summary>
        /// <param name="search">repository name</param>
        /// <returns>True if any repo contains any repo, otherwise false</returns>
        public bool IfRepoExists(string search)
        {
            IEnumerable<RepositoryModel> RepoDb = _unitOfWork.ReposRepository.GetAll();

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
        /// Takes from db repositories which name is similar to search
        /// </summary>
        /// <param name="search"></param>
        /// <returns>List of repositories</returns>
        public IList<RepositoryModel> TakeRepoFromDb(string search)
        {
            IEnumerable<RepositoryModel> RepoDb = _unitOfWork.ReposRepository.GetAll();

            List<RepositoryModel> reposModels = new List<RepositoryModel>();
            foreach (var item in RepoDb)
            {
                if (item.Name.Contains(search))
                {
                    reposModels.Add(item);
                }
            }
            return reposModels;
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

        /// <summary>
        /// Searches from db for contributors who has been contributed in repository which id is equal to repoId
        /// </summary>
        /// <param name="repoId"></param>
        /// <returns>True if contributors exists in particular repository, otherwise false</returns>
        public bool IfContributorsExists(long repoId)
        {
            var contributorsDb = _unitOfWork.ContributorsRepository.GetAll();

            foreach (var item in contributorsDb)
            {
                if (item.RepoId == repoId)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Takes from db Contributors who has contributed in repository with id equal to repoId
        /// </summary>
        /// <param name="repoId"></param>
        /// <returns>List of Contributors</returns>
        public List<ContributorsModel> TakeContributorsFromDb (long repoId)
        {
            var contributorsDb = _unitOfWork.ContributorsRepository.GetAll();

            List<ContributorsModel> Contributors = new List<ContributorsModel>();

            foreach (var item in contributorsDb)
            {
                if (item.RepoId == repoId)
                    Contributors.Add(item);
            }

            return Contributors;

        }

        /// <summary>
        /// Searches for user from db
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public bool IfUserExists(string login)
        {
            IEnumerable<UserModel> UserDb = _unitOfWork.UserRepository.GetAll();

            foreach (var item in UserDb)
            {
                if (item.Login == login)
                    return true;
            }
            
            return false;
        }

        /// <summary>
        /// Gets user from db 
        /// </summary>
        /// <param name="login"></param>
        /// <returns>User from db</returns>
        private UserModel TakeUserFromDb(string login)
        {
            UserModel user = new UserModel();

            IEnumerable<UserModel> UserDb = _unitOfWork.UserRepository.GetAll();

            foreach (var item in UserDb)
            {
                if (item.Login == login)
                {
                    user = item;
                    break;
                }
            }

            return user;
        }

        /// <summary>
        /// Takes all user repos from db
        /// </summary>
        /// <param name="OwnerLogin"></param>
        /// <returns>Repositories</returns>
        public IList<RepositoryModel> TakeUserRepos(string OwnerLogin)
        {
            IEnumerable<RepositoryModel> RepoDb = _unitOfWork.ReposRepository.GetAll();

            List<RepositoryModel> reposModels = new List<RepositoryModel>();
            foreach (var item in RepoDb)
            {
                if (item.GitHubOwnerLogin.Equals(OwnerLogin))
                {
                    reposModels.Add(item);
                }
            }

            return reposModels;

        }

        /// <summary>
        /// Gets as input Github api and parses to local created UserModel
        /// </summary>
        /// <param name="githubUserModel"></param>
        /// <returns>Object of UserModel class</returns>
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


    }
}