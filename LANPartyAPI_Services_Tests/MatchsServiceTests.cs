using System;
using System.Net;
using LANPartyAPI_Core.Enums;
using LANPartyAPI_Core.Exceptions;
using LANPartyAPI_Core.Models;
using LANPartyAPI_Services;
using LANPartyAPI_Services.DTOs;
using Microsoft.EntityFrameworkCore;

namespace LANPartyAPI_Services_Tests
{
	public class MatchsServiceTests : ServiceTestBase<MatchService>
	{
        #region GetMatchTeams

        [Category("GetMatchTeams")]
        [TestCase(0)]//create
        public async Task Get_When_Match_does_not_exist(int matchID)
        {
            //Arrange

            //Act
            HttpResponseException exception=Assert.ThrowsAsync<HttpResponseException>(async ()=> await _sut.GetMatchTeams(matchID));


            //Assert
            Assert.That(exception.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
            Assert.That(exception.Value,Is.EqualTo("MatchNotFound"));
            await Task.CompletedTask;
        }

        [Category("GetMatchTeams")]
        [TestCase(1)]//create
        public async Task Get_When_Match_does_exist(int matchID)
        {
            //Arrange
            int idMatch= 1;
            //Act
            MatchTeamsResultsDTO mt = await _sut.GetMatchTeams(matchID);


            //Assert
            Assert.That(mt.MatchId, Is.EqualTo(idMatch));
            await Task.CompletedTask;
        }

        [Category("GetMatchTeams")]
        [TestCase(1)]//create
        public async Task Get_Match_With_List_Of_Teams(int matchID)
        {
            //Arrange
            int[] idMatch =new int[] { 1,2 };
            //Act
            MatchTeamsResultsDTO mt = await _sut.GetMatchTeams(matchID);
            int[] teamIds = mt.Results.Select(t => t.TeamId).ToArray();

            //Assert
            CollectionAssert.AreEqual(idMatch,teamIds);
            await Task.CompletedTask;
        }
        #endregion

        #region RegisterScore

        [Category("RegisterScore")]
        [Test]
        public async Task Register_Score_Match_Exist()
        {
            //create
            MatchTeamsResultsDTO mtr = new MatchTeamsResultsDTO()
            {
                MatchId = 1,
                Results = new List<TeamsResultsDTO>()
                {
                    new TeamsResultsDTO
                    {
                        TeamId=1,
                        Score="1000",
                        IsWinner=true,
                    },
                    new TeamsResultsDTO
                    {
                        TeamId=2,
                        Score="9999",
                        IsWinner=false,
                    },
                }
            };

            //Arrange
            List<string> scoreToAssert = new List<string>() { "1000", "9999" };
            List<string> scoreDB = new List<string>();
            //Act
            await _sut.RegisterScore(mtr);

            List<Team> assert = await _context.Teams.Include(m=>m.Matches_Teams).AsNoTracking().ToListAsync();

            foreach (var item in assert)
            {
                foreach (var teamListMatche in item.Matches_Teams)
                {
                  if (teamListMatche.MatchId!=mtr.MatchId)
                  {
                        assert.Remove(item);
                  }
                    else
                    {
                        scoreDB.Add(teamListMatche.Score);
                    }
                }

            }


            //Assert
            CollectionAssert.AreEqual(scoreToAssert, scoreDB);
            await Task.CompletedTask;
        }


        #endregion

        #region GetMatch

        [Category("GetMatch")]
        [TestCase(1)]//Get
        public async Task Get_Match_Exist(int matchId)
        {
            //Arrange
            var domaine = await _context.Matches.FindAsync(matchId);

            //Act
            var m = await _sut.GetMatch(matchId);


            //Assert
            Assert.That(m.Id, Is.EqualTo(domaine.Id));
            Assert.That(m.MatchNumber, Is.EqualTo(domaine.MatchNumber));
            Assert.That(m.TournamentId, Is.EqualTo(domaine.TournamentId));
            Assert.That(m.Date, Is.EqualTo(domaine.Date));
            Assert.That(m.Round, Is.EqualTo(domaine.Round));
        }

        [Category("GetMatch")]
        [TestCase(0)]//Get
        public async Task Get_Match_Not_Found(int matchID)
        {
            //Arrange

      
            //Act
            HttpResponseException exception = Assert.ThrowsAsync<HttpResponseException>(async () => await _sut.GetMatch(matchID));


            //Assert
            Assert.That(exception.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
            Assert.That(exception.Value, Is.EqualTo("MatchNotFound"));
            await Task.CompletedTask;
        }


        #endregion


        public override void SeedDatabase()
        {

            Event ev=new Event
            {
                Id = 1,
                Name = "LAN01",
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(3),
                MaxPlayerNumber = 15,
                Description = "A description.." 
            };

            Tournament tour=new Tournament
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
            var tm=new List<Team>
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

            _context.Events.Add(ev);
            _context.Tournaments.Add(tour);

            foreach (var item in matTeam)
            {
                _context.Matches_Teams.Add(item);
            };
            
            foreach (Team item in tm)
            {
              _context.Teams.Add(item);
            }
            
            foreach (Match mat in matchs)
            {
              _context.Matches.Add(mat);
            }

            _context.SaveChanges();

        }

    }
}

