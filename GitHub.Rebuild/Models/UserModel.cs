using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Globalization;

namespace GitHub.Rebuild.Models
{
    public class UserModel
    {
        public string Login { get; set; }
        public long ID { get; set; }
        public long GitHubUserId { get; set; }
        public string Avatar_url { get; set; }
        public string Html_url { get; set; }
        public string Name { get; set; }
        public string? Company { get; set; }
        public string? Location { get; set; }
        public string? Bio { get; set; }
        public int Followers { get; set; }
        public int Following { get; set; }
    }
}
