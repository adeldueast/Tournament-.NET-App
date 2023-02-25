using LANPartyAPI_Core.Enums;
using LANPartyAPI_Core.Models;
using LANPartyAPI_DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace LANPartyAPI.DbSeeder
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Look for any movies.
                if (!context.Events.Any())
                {
                    context.Events.AddRange(
                    new Event
                    {
                        Name = "LAN01",
                        StartDate = DateTime.Now.AddDays(1),
                        EndDate = DateTime.Now.AddDays(3),
                        MaxPlayerNumber = 15,
                        Description = "A description.."
                    },
                    new Event
                    {
                        Name = "LAN02",
                        StartDate = DateTime.Now.AddDays(4),
                        EndDate = DateTime.Now.AddDays(6),
                        MaxPlayerNumber = 15,
                        Description = "A description.."
                    },
                    new Event
                    {
                        Name = "LAN03",
                        StartDate = DateTime.Now.AddDays(7),
                        EndDate = DateTime.Now.AddDays(9),
                        MaxPlayerNumber = 15,
                        Description = "A description.."
                    },
                    new Event
                    {
                        Name = "LAN04",
                        StartDate = DateTime.Now.AddDays(10),
                        EndDate = DateTime.Now.AddDays(12),
                        MaxPlayerNumber = 15,
                        Description = "A description.."
                    }, new Event
                    {
                        Name = "LAN05",
                        StartDate = DateTime.Now.AddDays(13),
                        EndDate = DateTime.Now.AddDays(15),
                        MaxPlayerNumber = 15,
                        Description = "A description.."
                    });
                }
                if (!context.Tournaments.Any())
                {
                    context.Tournaments.AddRange(
                    new Tournament
                    {
                        Id = 500,
                        EventId = 2,
                        Name = "Tournament01",
                        EliminationMode = EliminationTypes.Simple,
                        Description = "A random description...",
                        Game = "Mario smash brothers",
                        MaxTeamNumber = 10,
                        PlayersPerTeamNumber = 2,


                    },
                     new Tournament
                     {
                         Id = 501,
                         EventId = 2,
                         Name = "Tournament02",
                         EliminationMode = EliminationTypes.Double,
                         Description = "A random description...",
                         Game = "Valorant",
                         MaxTeamNumber = 10,
                         PlayersPerTeamNumber = 2,


                     },
                      new Tournament
                      {
                          Id = 502,
                          EventId = 2,
                          Name = "Tournament03",
                          EliminationMode = EliminationTypes.Robin,
                          Description = "A random description...",
                          Game = "CS:GO",
                          MaxTeamNumber = 10,
                          PlayersPerTeamNumber = 2,
                      }
                    );
                }
                if (!context.Teams.Any())
                {
                    context.Teams.AddRange(
                    new Team
                    {
                        Id = 1,
                        TournamentId = 501,
                    },
                    new Team
                    {
                        Id = 2,
                        TournamentId = 501,
                    });
                }
                if (!context.Matches.Any())
                {
                    context.Matches.AddRange(
                    new Match
                    {
                        Id = 4,
                        TournamentId = 501,
                        Round = 1,
                        MatchNumber = 1,
                    },
                    new Match
                    {
                        Id = 5,
                        TournamentId = 501,
                        Round = 1,
                        MatchNumber = 2,

                    });
                }

                if (!context.Matches_Teams.Any())
                {
                    context.Matches_Teams.AddRange(
                    new Match_Team
                    {
                        MatchId = 4,
                        TeamId = 1
                    },
                    new Match_Team
                    {
                        MatchId = 4,
                        TeamId = 2
                    },
                    new Match_Team
                    {
                        MatchId = 5,
                        TeamId = 1
                    },
                    new Match_Team
                    {
                        MatchId = 5,
                        TeamId = 2
                    });
                }

                context.SaveChanges();
            }
        }
    }
}
