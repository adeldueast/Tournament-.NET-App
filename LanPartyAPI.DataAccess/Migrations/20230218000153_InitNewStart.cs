using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LANPartyAPI_DataAccess.Migrations
{
    public partial class InitNewStart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaxPlayerNumber = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserEvent",
                columns: table => new
                {
                    EventsId = table.Column<int>(type: "int", nullable: false),
                    PlayersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserEvent", x => new { x.EventsId, x.PlayersId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserEvent_AspNetUsers_PlayersId",
                        column: x => x.PlayersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserEvent_Events_EventsId",
                        column: x => x.EventsId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pictures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MimeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pictures_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Seats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Prefix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<int>(type: "int", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seats_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Seats_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tournaments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Game = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    PlayersPerTeamNumber = table.Column<int>(type: "int", nullable: true),
                    MaxTeamNumber = table.Column<int>(type: "int", nullable: true),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    EliminationMode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournaments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tournaments_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Round = table.Column<int>(type: "int", nullable: false),
                    MatchNumber = table.Column<int>(type: "int", nullable: false),
                    TournamentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TournamentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teams_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserTeam",
                columns: table => new
                {
                    PlayersId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TeamsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserTeam", x => new { x.PlayersId, x.TeamsId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserTeam_AspNetUsers_PlayersId",
                        column: x => x.PlayersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserTeam_Teams_TeamsId",
                        column: x => x.TeamsId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Matches_Teams",
                columns: table => new
                {
                    MatchId = table.Column<int>(type: "int", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    IsWinner = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches_Teams", x => new { x.MatchId, x.TeamId });
                    table.ForeignKey(
                        name: "FK_Matches_Teams_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Matches_Teams_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Description", "EndDate", "MaxPlayerNumber", "Name", "StartDate" },
                values: new object[,]
                {
                    { 1, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer turpis lorem, rhoncus at lacus ut, consectetur vehicula tellus. Integer at imperdiet mi, in feugiat nisl. Praesent a sem nec enim blandit cursus in nec velit. Vivamus accumsan tincidunt metus, ac commodo nibh ultrices at. Pellentesque id metus quis libero ultricies iaculis. Vivamus posuere gravida feugiat. Phasellus et sagittis velit. Curabitur eleifend sed ipsum eget congue. Cras augue libero, placerat pharetra sollicitudin quis, faucibus ut nibh.", new DateTime(2023, 2, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 500, "LAN01", new DateTime(2023, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer turpis lorem, rhoncus at lacus ut, consectetur vehicula tellus. Integer at imperdiet mi, in feugiat nisl. Praesent a sem nec enim blandit cursus in nec velit. Vivamus accumsan tincidunt metus, ac commodo nibh ultrices at. Pellentesque id metus quis libero ultricies iaculis. Vivamus posuere gravida feugiat. Phasellus et sagittis velit. Curabitur eleifend sed ipsum eget congue. Cras augue libero, placerat pharetra sollicitudin quis, faucibus ut nibh.", new DateTime(2023, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 100, "LAN02", new DateTime(2023, 2, 25, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer turpis lorem, rhoncus at lacus ut, consectetur vehicula tellus. Integer at imperdiet mi, in feugiat nisl. Praesent a sem nec enim blandit cursus in nec velit. Vivamus accumsan tincidunt metus, ac commodo nibh ultrices at. Pellentesque id metus quis libero ultricies iaculis. Vivamus posuere gravida feugiat. Phasellus et sagittis velit. Curabitur eleifend sed ipsum eget congue. Cras augue libero, placerat pharetra sollicitudin quis, faucibus ut nibh.", new DateTime(2023, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 300, "LAN03", new DateTime(2023, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer turpis lorem, rhoncus at lacus ut, consectetur vehicula tellus. Integer at imperdiet mi, in feugiat nisl. Praesent a sem nec enim blandit cursus in nec velit. Vivamus accumsan tincidunt metus, ac commodo nibh ultrices at. Pellentesque id metus quis libero ultricies iaculis. Vivamus posuere gravida feugiat. Phasellus et sagittis velit. Curabitur eleifend sed ipsum eget congue. Cras augue libero, placerat pharetra sollicitudin quis, faucibus ut nibh.", new DateTime(2023, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 450, "LAN04", new DateTime(2023, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer turpis lorem, rhoncus at lacus ut, consectetur vehicula tellus. Integer at imperdiet mi, in feugiat nisl. Praesent a sem nec enim blandit cursus in nec velit. Vivamus accumsan tincidunt metus, ac commodo nibh ultrices at. Pellentesque id metus quis libero ultricies iaculis. Vivamus posuere gravida feugiat. Phasellus et sagittis velit. Curabitur eleifend sed ipsum eget congue. Cras augue libero, placerat pharetra sollicitudin quis, faucibus ut nibh.", new DateTime(2023, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 9500, "LAN05", new DateTime(2023, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Tournaments",
                columns: new[] { "Id", "Description", "EliminationMode", "EventId", "Game", "MaxTeamNumber", "Name", "PlayersPerTeamNumber" },
                values: new object[,]
                {
                    { 1, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer turpis lorem, rhoncus at lacus ut, consectetur vehicula tellus. Integer at imperdiet mi, in feugiat nisl. Praesent a sem nec enim blandit cursus in nec velit. Vivamus accumsan tincidunt metus, ac commodo nibh ultrices at. Pellentesque id metus quis libero ultricies iaculis. Vivamus posuere gravida feugiat. Phasellus et sagittis velit. Curabitur eleifend sed ipsum eget congue. Cras augue libero, placerat pharetra sollicitudin quis, faucibus ut nibh. Curabitur sagittis diam massa, vel gravida nisi dignissim non. Phasellus semper in dui ac luctus. Duis tincidunt purus enim, sit amet porta libero iaculis id. Donec volutpat, lacus non condimentum pretium, ex arcu scelerisque nisi, eget suscipit erat neque eget neque. Nunc dictum a justo quis pretium. Sed nisl massa, fringilla non facilisis molestie, malesuada semper nisi aliquam.", 0, 1, "Minecraft", 20, "Tournament01", 2 },
                    { 2, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer turpis lorem, rhoncus at lacus ut, consectetur vehicula tellus. Integer at imperdiet mi, in feugiat nisl. Praesent a sem nec enim blandit cursus in nec velit. Vivamus accumsan tincidunt metus, ac commodo nibh ultrices at. Pellentesque id metus quis libero ultricies iaculis. Vivamus posuere gravida feugiat. Phasellus et sagittis velit. Curabitur eleifend sed ipsum eget congue. Cras augue libero, placerat pharetra sollicitudin quis, faucibus ut nibh. Curabitur sagittis diam massa, vel gravida nisi dignissim non. Phasellus semper in dui ac luctus. Duis tincidunt purus enim, sit amet porta libero iaculis id. Donec volutpat, lacus non condimentum pretium, ex arcu scelerisque nisi, eget suscipit erat neque eget neque. Nunc dictum a justo quis pretium. Sed nisl massa, fringilla non facilisis molestie, malesuada semper nisi aliquam.", 1, 1, "Valorant", 10, "Tournament02", 5 },
                    { 3, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer turpis lorem, rhoncus at lacus ut, consectetur vehicula tellus. Integer at imperdiet mi, in feugiat nisl. Praesent a sem nec enim blandit cursus in nec velit. Vivamus accumsan tincidunt metus, ac commodo nibh ultrices at. Pellentesque id metus quis libero ultricies iaculis. Vivamus posuere gravida feugiat. Phasellus et sagittis velit. Curabitur eleifend sed ipsum eget congue. Cras augue libero, placerat pharetra sollicitudin quis, faucibus ut nibh. Curabitur sagittis diam massa, vel gravida nisi dignissim non. Phasellus semper in dui ac luctus. Duis tincidunt purus enim, sit amet porta libero iaculis id. Donec volutpat, lacus non condimentum pretium, ex arcu scelerisque nisi, eget suscipit erat neque eget neque. Nunc dictum a justo quis pretium. Sed nisl massa, fringilla non facilisis molestie, malesuada semper nisi aliquam.", 1, 1, "CS:GO", 10, "Tournament03", 5 },
                    { 4, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer turpis lorem, rhoncus at lacus ut, consectetur vehicula tellus. Integer at imperdiet mi, in feugiat nisl. Praesent a sem nec enim blandit cursus in nec velit. Vivamus accumsan tincidunt metus, ac commodo nibh ultrices at. Pellentesque id metus quis libero ultricies iaculis. Vivamus posuere gravida feugiat. Phasellus et sagittis velit. Curabitur eleifend sed ipsum eget congue. Cras augue libero, placerat pharetra sollicitudin quis, faucibus ut nibh. Curabitur sagittis diam massa, vel gravida nisi dignissim non. Phasellus semper in dui ac luctus. Duis tincidunt purus enim, sit amet porta libero iaculis id. Donec volutpat, lacus non condimentum pretium, ex arcu scelerisque nisi, eget suscipit erat neque eget neque. Nunc dictum a justo quis pretium. Sed nisl massa, fringilla non facilisis molestie, malesuada semper nisi aliquam.", 1, 1, "Destiny 2", 5, "Tournament03", 5 },
                    { 5, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer turpis lorem, rhoncus at lacus ut, consectetur vehicula tellus. Integer at imperdiet mi, in feugiat nisl. Praesent a sem nec enim blandit cursus in nec velit. Vivamus accumsan tincidunt metus, ac commodo nibh ultrices at. Pellentesque id metus quis libero ultricies iaculis. Vivamus posuere gravida feugiat. Phasellus et sagittis velit. Curabitur eleifend sed ipsum eget congue. Cras augue libero, placerat pharetra sollicitudin quis, faucibus ut nibh. Curabitur sagittis diam massa, vel gravida nisi dignissim non. Phasellus semper in dui ac luctus. Duis tincidunt purus enim, sit amet porta libero iaculis id. Donec volutpat, lacus non condimentum pretium, ex arcu scelerisque nisi, eget suscipit erat neque eget neque. Nunc dictum a justo quis pretium. Sed nisl massa, fringilla non facilisis molestie, malesuada semper nisi aliquam.", 0, 1, "Fornite", 100, "Tournament03", 1 },
                    { 6, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer turpis lorem, rhoncus at lacus ut, consectetur vehicula tellus. Integer at imperdiet mi, in feugiat nisl. Praesent a sem nec enim blandit cursus in nec velit. Vivamus accumsan tincidunt metus, ac commodo nibh ultrices at. Pellentesque id metus quis libero ultricies iaculis. Vivamus posuere gravida feugiat. Phasellus et sagittis velit. Curabitur eleifend sed ipsum eget congue. Cras augue libero, placerat pharetra sollicitudin quis, faucibus ut nibh. Curabitur sagittis diam massa, vel gravida nisi dignissim non. Phasellus semper in dui ac luctus. Duis tincidunt purus enim, sit amet porta libero iaculis id. Donec volutpat, lacus non condimentum pretium, ex arcu scelerisque nisi, eget suscipit erat neque eget neque. Nunc dictum a justo quis pretium. Sed nisl massa, fringilla non facilisis molestie, malesuada semper nisi aliquam.", 1, 2, "Call of Duty: Warzone", 50, "Tournament03", 2 },
                    { 7, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer turpis lorem, rhoncus at lacus ut, consectetur vehicula tellus. Integer at imperdiet mi, in feugiat nisl. Praesent a sem nec enim blandit cursus in nec velit. Vivamus accumsan tincidunt metus, ac commodo nibh ultrices at. Pellentesque id metus quis libero ultricies iaculis. Vivamus posuere gravida feugiat. Phasellus et sagittis velit. Curabitur eleifend sed ipsum eget congue. Cras augue libero, placerat pharetra sollicitudin quis, faucibus ut nibh. Curabitur sagittis diam massa, vel gravida nisi dignissim non. Phasellus semper in dui ac luctus. Duis tincidunt purus enim, sit amet porta libero iaculis id. Donec volutpat, lacus non condimentum pretium, ex arcu scelerisque nisi, eget suscipit erat neque eget neque. Nunc dictum a justo quis pretium. Sed nisl massa, fringilla non facilisis molestie, malesuada semper nisi aliquam.", 1, 3, "Apex Legends", 25, "Tournament03", 4 },
                    { 8, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer turpis lorem, rhoncus at lacus ut, consectetur vehicula tellus. Integer at imperdiet mi, in feugiat nisl. Praesent a sem nec enim blandit cursus in nec velit. Vivamus accumsan tincidunt metus, ac commodo nibh ultrices at. Pellentesque id metus quis libero ultricies iaculis. Vivamus posuere gravida feugiat. Phasellus et sagittis velit. Curabitur eleifend sed ipsum eget congue. Cras augue libero, placerat pharetra sollicitudin quis, faucibus ut nibh. Curabitur sagittis diam massa, vel gravida nisi dignissim non. Phasellus semper in dui ac luctus. Duis tincidunt purus enim, sit amet porta libero iaculis id. Donec volutpat, lacus non condimentum pretium, ex arcu scelerisque nisi, eget suscipit erat neque eget neque. Nunc dictum a justo quis pretium. Sed nisl massa, fringilla non facilisis molestie, malesuada semper nisi aliquam.", 1, 4, "Rocket League", 10, "Tournament03", 3 },
                    { 9, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer turpis lorem, rhoncus at lacus ut, consectetur vehicula tellus. Integer at imperdiet mi, in feugiat nisl. Praesent a sem nec enim blandit cursus in nec velit. Vivamus accumsan tincidunt metus, ac commodo nibh ultrices at. Pellentesque id metus quis libero ultricies iaculis. Vivamus posuere gravida feugiat. Phasellus et sagittis velit. Curabitur eleifend sed ipsum eget congue. Cras augue libero, placerat pharetra sollicitudin quis, faucibus ut nibh. Curabitur sagittis diam massa, vel gravida nisi dignissim non. Phasellus semper in dui ac luctus. Duis tincidunt purus enim, sit amet porta libero iaculis id. Donec volutpat, lacus non condimentum pretium, ex arcu scelerisque nisi, eget suscipit erat neque eget neque. Nunc dictum a justo quis pretium. Sed nisl massa, fringilla non facilisis molestie, malesuada semper nisi aliquam.", 1, 5, "League of legends", 10, "Tournament03", 2 }
                });

            migrationBuilder.InsertData(
                table: "Matches",
                columns: new[] { "Id", "Date", "MatchNumber", "Round", "TournamentId" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 2, 23, 16, 10, 0, 0, DateTimeKind.Unspecified), 1, 1, 1 },
                    { 2, new DateTime(2023, 2, 23, 16, 15, 0, 0, DateTimeKind.Unspecified), 2, 1, 1 },
                    { 3, new DateTime(2023, 2, 23, 16, 20, 0, 0, DateTimeKind.Unspecified), 3, 1, 1 },
                    { 4, new DateTime(2023, 2, 23, 16, 25, 0, 0, DateTimeKind.Unspecified), 4, 1, 1 },
                    { 5, new DateTime(2023, 2, 23, 16, 30, 0, 0, DateTimeKind.Unspecified), 5, 1, 1 },
                    { 6, null, 6, 1, 1 },
                    { 7, null, 7, 1, 1 }
                });

            migrationBuilder.InsertData(
                table: "Teams",
                columns: new[] { "Id", "Name", "TournamentId" },
                values: new object[,]
                {
                    { 1, "TSM", 1 },
                    { 2, "FAZE", 1 },
                    { 3, "100 Thieves", 1 },
                    { 4, "Evil Geniuses", 1 },
                    { 5, "Cloud9", 1 },
                    { 6, "Fnatic", 2 },
                    { 7, "CLG", 2 },
                    { 8, "Team Envy", 2 }
                });

            migrationBuilder.InsertData(
                table: "Matches_Teams",
                columns: new[] { "MatchId", "TeamId", "IsWinner", "Score" },
                values: new object[,]
                {
                    { 1, 1, true, "5" },
                    { 1, 2, false, "3" },
                    { 1, 3, false, "4" },
                    { 2, 4, true, "5" },
                    { 2, 5, false, "4" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserEvent_PlayersId",
                table: "ApplicationUserEvent",
                column: "PlayersId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserTeam_TeamsId",
                table: "ApplicationUserTeam",
                column: "TeamsId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_TournamentId",
                table: "Matches",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_Teams_TeamId",
                table: "Matches_Teams",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_EventId",
                table: "Pictures",
                column: "EventId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Seats_EventId",
                table: "Seats",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_UserId",
                table: "Seats",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_TournamentId",
                table: "Teams",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_EventId",
                table: "Tournaments",
                column: "EventId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserEvent");

            migrationBuilder.DropTable(
                name: "ApplicationUserTeam");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Matches_Teams");

            migrationBuilder.DropTable(
                name: "Pictures");

            migrationBuilder.DropTable(
                name: "Seats");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Tournaments");

            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
