using System;
using System.Net;
using LANPartyAPI.Controllers;
using LANPartyAPI_Core.Enums;
using LANPartyAPI_Core.Exceptions;
using LANPartyAPI_Core.Models;
using LANPartyAPI_DataAccess.Data;
using LANPartyAPI_Services;
using LANPartyAPI_Services.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LANPartyAPI_Services_Tests
{
    public class MatchsControllerTests
    {
        public MatchesController _matchController;
        public MatchService _matchService;

        public ApplicationDbContext _applicationDbContext;

        [SetUp]
        public void Setup()
        {
            _applicationDbContext = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options);


            SeedDatabase();

            _matchService = new MatchService(_applicationDbContext);
            _matchController = new MatchesController(_matchService);
        }

        #region GetMatch

        [Category("GetMatch")]
        [Test]//Get
        public async Task Get_Match_Exist_Controller()
        {        
            
            //Act
            var Actualresult = await _matchController.GetMatch(1);
            OkObjectResult okResult = Actualresult as OkObjectResult;             
            
            //Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Category("GetMatch")]
        [Test]//Get
        public async Task Get_Match_Not_Found_Controller()
        {
            //Arrange


            //Act
            HttpResponseException exception = Assert.ThrowsAsync<HttpResponseException>(async () => await _matchController.GetMatch(0));


            //Assert
            Assert.That(exception.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
            Assert.That(exception.Value, Is.EqualTo("MatchNotFound"));
            await Task.CompletedTask;
        }


        #endregion


        public void SeedDatabase()
        {

            Event ev = new Event
            {
                Id = 1,
                Name = "LAN01",
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(3),
                MaxPlayerNumber = 15,
                Description = "A description.."
            };

            Tournament tour = new Tournament
            {
                Id = 1,
                EventId = 1,
                Name = "Tournament01",
                EliminationMode = EliminationTypes.Simple,
                Description = "A random description...",
                Game = "Mario smash brothers",
                MaxTeamNumber = 10,
                PlayersPerTeamNumber = 2,


            };
            var tm = new List<Team>
            {
                 new Team{
                 Id = 1,
                 TournamentId = 1,
                } ,
                 new Team{
                 Id = 2,
                 TournamentId = 1,
                }
            };
            var matchs = new List<Match>()
            {
                new Match{
                Id = 1,
                TournamentId = 1,
                Round = 1,
                MatchNumber = 1, },
                new Match
                {
                Id = 2,
                TournamentId = 1,
                Round = 1,
                MatchNumber = 2,
                }
            };

            var matTeam = new List<Match_Team>()
            {
                new Match_Team
                {
                    MatchId=1,
                    TeamId=1
                },
                new Match_Team
                {
                    MatchId=1,
                    TeamId=2
                },
            };

            _applicationDbContext.Events.Add(ev);
            _applicationDbContext.Tournaments.Add(tour);

            foreach (var item in matTeam)
            {
                _applicationDbContext.Matches_Teams.Add(item);
            };

            foreach (Team item in tm)
            {
                _applicationDbContext.Teams.Add(item);
            }

            foreach (Match mat in matchs)
            {
                _applicationDbContext.Matches.Add(mat);
            }

            _applicationDbContext.SaveChanges();
        }

    }
}

