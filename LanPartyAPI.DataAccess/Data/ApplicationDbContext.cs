using LANPartyAPI_Utils;
using Microsoft.AspNetCore.Identity;
using LANPartyAPI_Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LANPartyAPI_Core.Enums;

namespace LANPartyAPI_DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {

        }

        public DbSet<Event> Events { get; set; }

        public DbSet<Tournament> Tournaments { get; set; }

        public DbSet<Match> Matches { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Match_Team> Matches_Teams { get; set; }

        public DbSet<Seat> Seats { get; set; }

        public DbSet<Picture> Pictures { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Match_Team>().HasKey(mt => new { mt.MatchId, mt.TeamId });

            // Si tu vx delete une TEAM, tu vas devoir défaire le relation où la team est utilisée (dans Matches_Teams),
            // sinon tu es restricted from performing that action
            modelBuilder.Entity<Match_Team>()
                .HasOne<Match>(m => m.Match)
                .WithMany(m => m.Matches_Teams)
                .HasForeignKey(mt => mt.MatchId);
            modelBuilder.Entity<Match_Team>()
               .HasOne<Team>(t => t.Team)
               .WithMany(t => t.Matches_Teams)
               .HasForeignKey(mt => mt.TeamId)
               .OnDelete(DeleteBehavior.Restrict);

            //Dont want to seed during testing
            if (Database.IsInMemory())
            {
                return;
            }

            //SEED EVENTS
            modelBuilder.Entity<Event>()
               .HasData(
               new Event
               {
                   Id = 1,
                   Name = "LAN01",
                   StartDate = new DateTime(2023, 02, 20),
                   EndDate = new DateTime(2023, 02, 25),
                   MaxPlayerNumber = 500,
                   Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer turpis lorem, rhoncus at lacus ut, consectetur vehicula tellus. Integer at imperdiet mi, in feugiat nisl. Praesent a sem nec enim blandit cursus in nec velit. Vivamus accumsan tincidunt metus, ac commodo nibh ultrices at. Pellentesque id metus quis libero ultricies iaculis. Vivamus posuere gravida feugiat. Phasellus et sagittis velit. Curabitur eleifend sed ipsum eget congue. Cras augue libero, placerat pharetra sollicitudin quis, faucibus ut nibh.",
               }, new Event
               {
                   Id = 2,
                   Name = "LAN02",
                   StartDate = new DateTime(2023, 02, 25),
                   EndDate = new DateTime(2023, 02, 28),
                   MaxPlayerNumber = 100,
                   Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer turpis lorem, rhoncus at lacus ut, consectetur vehicula tellus. Integer at imperdiet mi, in feugiat nisl. Praesent a sem nec enim blandit cursus in nec velit. Vivamus accumsan tincidunt metus, ac commodo nibh ultrices at. Pellentesque id metus quis libero ultricies iaculis. Vivamus posuere gravida feugiat. Phasellus et sagittis velit. Curabitur eleifend sed ipsum eget congue. Cras augue libero, placerat pharetra sollicitudin quis, faucibus ut nibh.",
               },
                new Event
                {
                    Id = 3,
                    Name = "LAN03",
                    StartDate = new DateTime(2023, 03, 1),
                    EndDate = new DateTime(2023, 03, 6),
                    MaxPlayerNumber = 300,
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer turpis lorem, rhoncus at lacus ut, consectetur vehicula tellus. Integer at imperdiet mi, in feugiat nisl. Praesent a sem nec enim blandit cursus in nec velit. Vivamus accumsan tincidunt metus, ac commodo nibh ultrices at. Pellentesque id metus quis libero ultricies iaculis. Vivamus posuere gravida feugiat. Phasellus et sagittis velit. Curabitur eleifend sed ipsum eget congue. Cras augue libero, placerat pharetra sollicitudin quis, faucibus ut nibh.",
                },
                new Event
                {
                    Id = 4,
                    Name = "LAN04",
                    StartDate = new DateTime(2023, 03, 6),
                    EndDate = new DateTime(2023, 03, 11),
                    MaxPlayerNumber = 450,
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer turpis lorem, rhoncus at lacus ut, consectetur vehicula tellus. Integer at imperdiet mi, in feugiat nisl. Praesent a sem nec enim blandit cursus in nec velit. Vivamus accumsan tincidunt metus, ac commodo nibh ultrices at. Pellentesque id metus quis libero ultricies iaculis. Vivamus posuere gravida feugiat. Phasellus et sagittis velit. Curabitur eleifend sed ipsum eget congue. Cras augue libero, placerat pharetra sollicitudin quis, faucibus ut nibh.",
                },
                new Event
                {
                    Id = 5,
                    Name = "LAN05",
                    StartDate = new DateTime(2023, 03, 11),
                    EndDate = new DateTime(2023, 03, 16),
                    MaxPlayerNumber = 9500,
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer turpis lorem, rhoncus at lacus ut, consectetur vehicula tellus. Integer at imperdiet mi, in feugiat nisl. Praesent a sem nec enim blandit cursus in nec velit. Vivamus accumsan tincidunt metus, ac commodo nibh ultrices at. Pellentesque id metus quis libero ultricies iaculis. Vivamus posuere gravida feugiat. Phasellus et sagittis velit. Curabitur eleifend sed ipsum eget congue. Cras augue libero, placerat pharetra sollicitudin quis, faucibus ut nibh.",
                });

            //SEED TOURNAMENTS
            modelBuilder.Entity<Tournament>().HasData(
                new Tournament
                {
                    Id = 1,
                    EventId = 1,
                    Name = "Tournament01",
                    EliminationMode = EliminationTypes.Simple,
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer turpis lorem, rhoncus at lacus ut, consectetur vehicula tellus. Integer at imperdiet mi, in feugiat nisl. Praesent a sem nec enim blandit cursus in nec velit. Vivamus accumsan tincidunt metus, ac commodo nibh ultrices at. Pellentesque id metus quis libero ultricies iaculis. Vivamus posuere gravida feugiat. Phasellus et sagittis velit. Curabitur eleifend sed ipsum eget congue. Cras augue libero, placerat pharetra sollicitudin quis, faucibus ut nibh. Curabitur sagittis diam massa, vel gravida nisi dignissim non. Phasellus semper in dui ac luctus. Duis tincidunt purus enim, sit amet porta libero iaculis id. Donec volutpat, lacus non condimentum pretium, ex arcu scelerisque nisi, eget suscipit erat neque eget neque. Nunc dictum a justo quis pretium. Sed nisl massa, fringilla non facilisis molestie, malesuada semper nisi aliquam.",
                    Game = "Minecraft",
                    MaxTeamNumber = 20,
                    PlayersPerTeamNumber = 2,
                },
                new Tournament
                {
                    Id = 2,
                    EventId = 1,
                    Name = "Tournament02",
                    EliminationMode = EliminationTypes.Double,
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer turpis lorem, rhoncus at lacus ut, consectetur vehicula tellus. Integer at imperdiet mi, in feugiat nisl. Praesent a sem nec enim blandit cursus in nec velit. Vivamus accumsan tincidunt metus, ac commodo nibh ultrices at. Pellentesque id metus quis libero ultricies iaculis. Vivamus posuere gravida feugiat. Phasellus et sagittis velit. Curabitur eleifend sed ipsum eget congue. Cras augue libero, placerat pharetra sollicitudin quis, faucibus ut nibh. Curabitur sagittis diam massa, vel gravida nisi dignissim non. Phasellus semper in dui ac luctus. Duis tincidunt purus enim, sit amet porta libero iaculis id. Donec volutpat, lacus non condimentum pretium, ex arcu scelerisque nisi, eget suscipit erat neque eget neque. Nunc dictum a justo quis pretium. Sed nisl massa, fringilla non facilisis molestie, malesuada semper nisi aliquam.",
                    Game = "Valorant",
                    MaxTeamNumber = 10,
                    PlayersPerTeamNumber = 5,
                },
                new Tournament
                {
                    Id = 3,
                    EventId = 1,
                    Name = "Tournament03",
                    EliminationMode = EliminationTypes.Double,
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer turpis lorem, rhoncus at lacus ut, consectetur vehicula tellus. Integer at imperdiet mi, in feugiat nisl. Praesent a sem nec enim blandit cursus in nec velit. Vivamus accumsan tincidunt metus, ac commodo nibh ultrices at. Pellentesque id metus quis libero ultricies iaculis. Vivamus posuere gravida feugiat. Phasellus et sagittis velit. Curabitur eleifend sed ipsum eget congue. Cras augue libero, placerat pharetra sollicitudin quis, faucibus ut nibh. Curabitur sagittis diam massa, vel gravida nisi dignissim non. Phasellus semper in dui ac luctus. Duis tincidunt purus enim, sit amet porta libero iaculis id. Donec volutpat, lacus non condimentum pretium, ex arcu scelerisque nisi, eget suscipit erat neque eget neque. Nunc dictum a justo quis pretium. Sed nisl massa, fringilla non facilisis molestie, malesuada semper nisi aliquam.",
                    Game = "CS:GO",
                    MaxTeamNumber = 10,
                    PlayersPerTeamNumber = 5,
                },
                new Tournament
                {
                    Id = 4,
                    EventId = 1,
                    Name = "Tournament03",
                    EliminationMode = EliminationTypes.Double,
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer turpis lorem, rhoncus at lacus ut, consectetur vehicula tellus. Integer at imperdiet mi, in feugiat nisl. Praesent a sem nec enim blandit cursus in nec velit. Vivamus accumsan tincidunt metus, ac commodo nibh ultrices at. Pellentesque id metus quis libero ultricies iaculis. Vivamus posuere gravida feugiat. Phasellus et sagittis velit. Curabitur eleifend sed ipsum eget congue. Cras augue libero, placerat pharetra sollicitudin quis, faucibus ut nibh. Curabitur sagittis diam massa, vel gravida nisi dignissim non. Phasellus semper in dui ac luctus. Duis tincidunt purus enim, sit amet porta libero iaculis id. Donec volutpat, lacus non condimentum pretium, ex arcu scelerisque nisi, eget suscipit erat neque eget neque. Nunc dictum a justo quis pretium. Sed nisl massa, fringilla non facilisis molestie, malesuada semper nisi aliquam.",
                    Game = "Destiny 2",
                    MaxTeamNumber = 5,
                    PlayersPerTeamNumber = 5,
                },
                new Tournament
                {
                    Id = 5,
                    EventId = 1,
                    Name = "Tournament03",
                    EliminationMode = EliminationTypes.Simple,
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer turpis lorem, rhoncus at lacus ut, consectetur vehicula tellus. Integer at imperdiet mi, in feugiat nisl. Praesent a sem nec enim blandit cursus in nec velit. Vivamus accumsan tincidunt metus, ac commodo nibh ultrices at. Pellentesque id metus quis libero ultricies iaculis. Vivamus posuere gravida feugiat. Phasellus et sagittis velit. Curabitur eleifend sed ipsum eget congue. Cras augue libero, placerat pharetra sollicitudin quis, faucibus ut nibh. Curabitur sagittis diam massa, vel gravida nisi dignissim non. Phasellus semper in dui ac luctus. Duis tincidunt purus enim, sit amet porta libero iaculis id. Donec volutpat, lacus non condimentum pretium, ex arcu scelerisque nisi, eget suscipit erat neque eget neque. Nunc dictum a justo quis pretium. Sed nisl massa, fringilla non facilisis molestie, malesuada semper nisi aliquam.",
                    Game = "Fornite",
                    MaxTeamNumber = 100,
                    PlayersPerTeamNumber = 1,
                },


                new Tournament
                {
                    Id = 6,
                    EventId = 2,
                    Name = "Tournament03",
                    EliminationMode = EliminationTypes.Double,
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer turpis lorem, rhoncus at lacus ut, consectetur vehicula tellus. Integer at imperdiet mi, in feugiat nisl. Praesent a sem nec enim blandit cursus in nec velit. Vivamus accumsan tincidunt metus, ac commodo nibh ultrices at. Pellentesque id metus quis libero ultricies iaculis. Vivamus posuere gravida feugiat. Phasellus et sagittis velit. Curabitur eleifend sed ipsum eget congue. Cras augue libero, placerat pharetra sollicitudin quis, faucibus ut nibh. Curabitur sagittis diam massa, vel gravida nisi dignissim non. Phasellus semper in dui ac luctus. Duis tincidunt purus enim, sit amet porta libero iaculis id. Donec volutpat, lacus non condimentum pretium, ex arcu scelerisque nisi, eget suscipit erat neque eget neque. Nunc dictum a justo quis pretium. Sed nisl massa, fringilla non facilisis molestie, malesuada semper nisi aliquam.",
                    Game = "Call of Duty: Warzone",
                    MaxTeamNumber = 50,
                    PlayersPerTeamNumber = 2,
                },
                new Tournament
                {
                    Id = 7,
                    EventId = 3,
                    Name = "Tournament03",
                    EliminationMode = EliminationTypes.Double,
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer turpis lorem, rhoncus at lacus ut, consectetur vehicula tellus. Integer at imperdiet mi, in feugiat nisl. Praesent a sem nec enim blandit cursus in nec velit. Vivamus accumsan tincidunt metus, ac commodo nibh ultrices at. Pellentesque id metus quis libero ultricies iaculis. Vivamus posuere gravida feugiat. Phasellus et sagittis velit. Curabitur eleifend sed ipsum eget congue. Cras augue libero, placerat pharetra sollicitudin quis, faucibus ut nibh. Curabitur sagittis diam massa, vel gravida nisi dignissim non. Phasellus semper in dui ac luctus. Duis tincidunt purus enim, sit amet porta libero iaculis id. Donec volutpat, lacus non condimentum pretium, ex arcu scelerisque nisi, eget suscipit erat neque eget neque. Nunc dictum a justo quis pretium. Sed nisl massa, fringilla non facilisis molestie, malesuada semper nisi aliquam.",
                    Game = "Apex Legends",
                    MaxTeamNumber = 25,
                    PlayersPerTeamNumber = 4,
                },
                new Tournament
                {
                    Id = 8,
                    EventId = 4,
                    Name = "Tournament03",
                    EliminationMode = EliminationTypes.Double,
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer turpis lorem, rhoncus at lacus ut, consectetur vehicula tellus. Integer at imperdiet mi, in feugiat nisl. Praesent a sem nec enim blandit cursus in nec velit. Vivamus accumsan tincidunt metus, ac commodo nibh ultrices at. Pellentesque id metus quis libero ultricies iaculis. Vivamus posuere gravida feugiat. Phasellus et sagittis velit. Curabitur eleifend sed ipsum eget congue. Cras augue libero, placerat pharetra sollicitudin quis, faucibus ut nibh. Curabitur sagittis diam massa, vel gravida nisi dignissim non. Phasellus semper in dui ac luctus. Duis tincidunt purus enim, sit amet porta libero iaculis id. Donec volutpat, lacus non condimentum pretium, ex arcu scelerisque nisi, eget suscipit erat neque eget neque. Nunc dictum a justo quis pretium. Sed nisl massa, fringilla non facilisis molestie, malesuada semper nisi aliquam.",
                    Game = "Rocket League",
                    MaxTeamNumber = 10,
                    PlayersPerTeamNumber = 3,
                },
                new Tournament
                {
                    Id = 9,
                    EventId = 5,
                    Name = "Tournament03",
                    EliminationMode = EliminationTypes.Double,
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer turpis lorem, rhoncus at lacus ut, consectetur vehicula tellus. Integer at imperdiet mi, in feugiat nisl. Praesent a sem nec enim blandit cursus in nec velit. Vivamus accumsan tincidunt metus, ac commodo nibh ultrices at. Pellentesque id metus quis libero ultricies iaculis. Vivamus posuere gravida feugiat. Phasellus et sagittis velit. Curabitur eleifend sed ipsum eget congue. Cras augue libero, placerat pharetra sollicitudin quis, faucibus ut nibh. Curabitur sagittis diam massa, vel gravida nisi dignissim non. Phasellus semper in dui ac luctus. Duis tincidunt purus enim, sit amet porta libero iaculis id. Donec volutpat, lacus non condimentum pretium, ex arcu scelerisque nisi, eget suscipit erat neque eget neque. Nunc dictum a justo quis pretium. Sed nisl massa, fringilla non facilisis molestie, malesuada semper nisi aliquam.",
                    Game = "League of legends",
                    MaxTeamNumber = 10,
                    PlayersPerTeamNumber = 2,
                });


            modelBuilder.Entity<Team>().HasData(
                new Team
                {
                    Id = 1,
                    TournamentId = 1,
                    Name = "TSM",
                },
                new Team
                {
                    Id = 2,
                    TournamentId = 1,
                    Name = "FAZE",
                },
                new Team
                {
                    Id = 3,
                    TournamentId = 1,
                    Name = "100 Thieves",
                },
                new Team
                {
                    Id = 4,
                    TournamentId = 1,
                    Name = "Evil Geniuses",
                },
                new Team
                {
                    Id = 5,
                    TournamentId = 1,
                    Name = "Cloud9",
                },

                new Team
                {
                    Id = 6,
                    TournamentId = 2,
                    Name = "Fnatic",
                },
                new Team
                {
                    Id = 7,
                    TournamentId = 2,
                    Name = "CLG",
                },
                new Team
                {
                    Id = 8,
                    TournamentId = 2,
                    Name = "Team Envy",
                });

            modelBuilder.Entity<Match>().HasData(
                new Match
                {
                    Id = 1,
                    TournamentId = 1,
                    Round = 1,
                    MatchNumber = 1,
                    Date = new DateTime(2023, 02, 23, 16, 10, 0)

                },
                new Match
                {
                    Id = 2,
                    TournamentId = 1,
                    Round = 1,
                    MatchNumber = 2,
                    Date = new DateTime(2023, 02, 23, 16, 15, 0)
                },
                new Match
                {
                    Id = 3,
                    TournamentId = 1,
                    Round = 1,
                    MatchNumber = 3,
                    Date = new DateTime(2023, 02, 23, 16, 20, 0)
                },
                new Match
                {
                    Id = 4,
                    TournamentId = 1,
                    Round = 1,
                    MatchNumber = 4,
                    Date = new DateTime(2023, 02, 23, 16, 25, 0)
                },
                new Match
                {
                    Id = 5,
                    TournamentId = 1,
                    Round = 1,
                    MatchNumber = 5,
                    Date = new DateTime(2023, 02, 23, 16, 30, 0)
                },
                new Match
                {
                    Id = 6,
                    TournamentId = 1,
                    Round = 1,
                    MatchNumber = 6,

                },
                new Match
                {
                    Id = 7,
                    TournamentId = 1,
                    Round = 1,
                    MatchNumber = 7,

                });

            modelBuilder.Entity<Match_Team>().HasData(
                 new Match_Team
                 {
                     MatchId = 1,
                     TeamId = 1,
                     IsWinner = true,
                     Score = "5"
                 },
                 new Match_Team
                 {
                     MatchId = 1,
                     TeamId = 2,
                     IsWinner = false,
                     Score = "3"
                 },
                 new Match_Team
                 {
                     MatchId = 1,
                     TeamId = 3,
                     IsWinner = false,
                     Score = "4"
                 },
                 new Match_Team
                 {
                     MatchId = 2,
                     TeamId = 4,
                     IsWinner = true,
                     Score = "5"
                 },
                 new Match_Team
                 {
                     MatchId = 2,
                     TeamId = 5,
                     IsWinner = false,
                     Score = "4"
                 });



            //SEED ROLES AND ADMIN USER
            #region
           // modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "2c5e174e-3b0e-446f-86af-483d56fd7210", Name = Utils.Roles.Admin, NormalizedName = Utils.Roles.Admin.ToUpper() });
           // modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "32ffcfa8-7337-4d05-a17e-99d6280a12d5", Name = Utils.Roles.Joueur, NormalizedName = Utils.Roles.Joueur.ToUpper() });


           // //a hasher to hash the password before seeding the user to the db
           // var hasher = new PasswordHasher<ApplicationUser>();


           // //Seeding the User to AspNetUsers table
           // modelBuilder.Entity<ApplicationUser>().HasData(
           //     new ApplicationUser
           //     {
           //         Id = "8e445865-a24d-4543-a6c6-9443d048cdb9", // primary key
           //         Email = "admin@hotmail.com",
           //         NormalizedEmail = "admin@hotmail.com".ToUpper(),
           //         FirstName = "admin",
           //         LastName = "admin",
           //         UserName = "admin",
           //         NormalizedUserName = "replace-by-firebaseUid0".ToUpper(),
           //         PasswordHash = hasher.HashPassword(null, "123456")
           //     }
           // );

           // //Seeding the User to AspNetUsers table
           // modelBuilder.Entity<ApplicationUser>().HasData(
           //     new ApplicationUser
           //     {
           //         Id = "c036ad57-128c-4d61-94ef-ceb8a7c8a8df", // primary key
           //         Email = "player1@hotmail.com",
           //         NormalizedEmail = "player1@hotmail.com".ToUpper(),
           //         FirstName = "Adel",
           //         LastName = "Kouaou",
           //         UserName = "s1mple",
           //         NormalizedUserName = "replace-by-firebaseUid1".ToUpper(),
           //         PasswordHash = hasher.HashPassword(null, "123456")
           //     }
           // );
           // modelBuilder.Entity<ApplicationUser>().HasData(
           //    new ApplicationUser
           //    {
           //        Id = "039e1e4b-fbd0-4638-b3ab-3567e6c944b6", // primary key
           //        Email = "player2@hotmail.com",
           //        NormalizedEmail = "player2@hotmail.com".ToUpper(),
           //        FirstName = "Nathan",
           //        LastName = "Bilodeau",
           //        UserName = "Subroza",
           //        NormalizedUserName = "replace-by-firebaseUid2".ToUpper(),
           //        PasswordHash = hasher.HashPassword(null, "123456")
           //    }
           //);

           // modelBuilder.Entity("ApplicationUserEvent").HasData(
           //   new
           //   {
           //       PlayersId = "c036ad57-128c-4d61-94ef-ceb8a7c8a8df",
           //       EventsId = 1,
           //   },
           //   new
           //   {
           //       PlayersId = "c036ad57-128c-4d61-94ef-ceb8a7c8a8df",
           //       EventsId = 2,
           //   },
           //    new
           //    {
           //        PlayersId = "039e1e4b-fbd0-4638-b3ab-3567e6c944b6",
           //        EventsId = 1,
           //    },
           //   new
           //   {
           //       PlayersId = "039e1e4b-fbd0-4638-b3ab-3567e6c944b6",
           //       EventsId = 2,
           //   });

           // modelBuilder.Entity("ApplicationUserTeam").HasData(
           //     new
           //     {
           //         PlayersId = "c036ad57-128c-4d61-94ef-ceb8a7c8a8df",
           //         TeamsId = 1,
           //     },
           //     new
           //     {
           //         PlayersId = "c036ad57-128c-4d61-94ef-ceb8a7c8a8df",
           //         TeamsId = 2,
           //     },
           //      new
           //      {
           //          PlayersId = "039e1e4b-fbd0-4638-b3ab-3567e6c944b6",
           //          TeamsId = 1,
           //      },
           //     new
           //     {
           //         PlayersId = "039e1e4b-fbd0-4638-b3ab-3567e6c944b6",
           //         TeamsId = 2,
           //     });

           // //Seeding the relation between our user and role to AspNetUserRoles table
           // modelBuilder.Entity<IdentityUserRole<string>>().HasData(
           //     new IdentityUserRole<string>
           //     {
           //         RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7210",
           //         UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
           //     }
           // );
            #endregion

        }
    }
}
