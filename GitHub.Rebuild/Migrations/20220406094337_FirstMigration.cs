using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GitHub.Rebuild.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contributors",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepoId = table.Column<long>(type: "bigint", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contributions = table.Column<int>(type: "int", nullable: false),
                    NumberOfContributors = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contributors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Repos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GitHubId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HtmlUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    License = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StargazersCount = table.Column<int>(type: "int", nullable: false),
                    GitHubOwnerId = table.Column<long>(type: "bigint", nullable: false),
                    GitHubOwnerLogin = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GitHubUserId = table.Column<long>(type: "bigint", nullable: false),
                    Avatar_url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Html_url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Followers = table.Column<int>(type: "int", nullable: false),
                    Following = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contributors");

            migrationBuilder.DropTable(
                name: "Repos");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
