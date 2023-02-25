using LANPartyAPI_Core.Enums;
using LANPartyAPI_Core.Exceptions;
using LANPartyAPI_Core.Models;
using LANPartyAPI_Services;
using LANPartyAPI_Services.DTOs;
using System.Net;

namespace LANPartyAPI_Services_Tests
{
    public class TournamentServiceTests : ServiceTestBase<TournamentsService>
    {
        public override void SeedDatabase()
        {
            var events = new List<Event>()
            {
                new Event{ Id = 1,Name="LAN01",Description ="A random description..",StartDate = DateTime.Now , EndDate = DateTime.Now.AddDays(5), MaxPlayerNumber = 100}
            };
            _context.AddRange(events);
            var tournaments = new List<Tournament>
            {
                new Tournament { Id = 1, EventId =1, Name = "IEM Katowice Major", Game = "Valorant",EliminationMode = EliminationTypes.Simple,MaxTeamNumber= 10,PlayersPerTeamNumber=5 },
                new Tournament { Id = 2, EventId =1, Name = "FACEIT Major", Game = "CS:GO",EliminationMode = EliminationTypes.Double,MaxTeamNumber= 10,PlayersPerTeamNumber=5 }
            };
            _context.AddRange(tournaments);
        }

        #region Create Tournament
        [Category("Create")]
        [Test]
        public async Task Upsert_Create_WhenNameAlreadyTaken_ShouldThrowTournamentNameTakenException()
        {
            // Arrange
            TournamentUpsertDTO upsertDto = new TournamentUpsertDTO()
            {
                Id = 0,
                EventId = 1,
                Name = "FACEIT Major",
            };

            // Act && Assert
            TournamentNameTakenException exception = Assert.ThrowsAsync<TournamentNameTakenException>(async () => await _sut.Upsert(upsertDto));

            // Assert
            //Assert.That(exception.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
            //Assert.That(exception.Value, Is.EqualTo(ExceptionErrorMessages.TournamentExceptions.TournamentNameTaken));
            await Task.CompletedTask;
        }

        [Category("Create")]
        [Test]
        public async Task Upsert_Create_WhenSuccess_ShouldReturnCreatedRessourceDto()
        {
            // Arrange
            TournamentUpsertDTO upsertDto = new TournamentUpsertDTO()
            {
                Id = 0,
                EventId = 1,
                Name = "Created Tournament",
                Game = "Rust",
                Description = "Description",
                EliminationMode = EliminationTypes.Robin,
                MaxTeamNumber = 10,
                PlayersPerTeamNumber = 30,
            };

            // Act
            TournamentResponseDTO tournamentDto = await _sut.Upsert(upsertDto);

            //Assert against dto model
            Assert.That(tournamentDto.Id, Is.EqualTo(3));

        }

        [Category("Create")]
        [TestCase(42)]
        public async Task Upsert_Create_WhenEventDoesNotExist_ShouldThrowEventNotFoundException(int eventId)
        {
            // Arrange
            TournamentUpsertDTO upsertDto = new TournamentUpsertDTO()
            {
                Id = 0,
                EventId = eventId,
                Name = "A new tournament",
            };

            // Act && Assert
            EventNotFoundException exception = Assert.ThrowsAsync<EventNotFoundException>(async () => await _sut.Upsert(upsertDto));

            // Assert
            //Assert.That(exception.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
            //Assert.That(exception.Value, Is.EqualTo(ExceptionErrorMessages.EventExceptions.EventNotFound));
            await Task.CompletedTask;
        }

        #endregion

        #region Update Tournament

        [Category("Update")]
        [TestCase(1)]
        public async Task Upsert_Update_WhenNameAlreadyTaken_ShouldThrowTournamentNameTakenException(int tournamentId)
        {
            // Arrange
            TournamentUpsertDTO upsertDto = new TournamentUpsertDTO()
            {
                Id = tournamentId,
                EventId = 1,
                Name = "FACEIT Major",
                EliminationMode = EliminationTypes.Simple
            };

            // Act && Assert
            TournamentNameTakenException exception = Assert.ThrowsAsync<TournamentNameTakenException>(async () => await _sut.Upsert(upsertDto));

            // Assert
            //Assert.That(exception.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
            //Assert.That(exception.Value, Is.EqualTo(ExceptionErrorMessages.TournamentExceptions.TournamentNameTaken));
            await Task.CompletedTask;
        }

        [Category("Update")]
        [TestCase(2)]
        public async Task Upsert_Update_WhenNameAlreadyTakenByItSelf_ShouldNotThrowAnyException(int tournamentId)
        {
            // Arrange
            TournamentUpsertDTO upsertDto = new TournamentUpsertDTO()
            {
                Id = tournamentId,
                Name = "FACEIT Major",
                Description = "Description",
                EliminationMode = EliminationTypes.Robin,

            };

            //TODO: assert dto is same instead of DoesNotThrowAsync
            // Act
            // Assert
            Assert.DoesNotThrowAsync(async () => await _sut.Upsert(upsertDto));
            await Task.CompletedTask;
        }

        [Category("Update")]
        [TestCase(939)]
        public async Task Upsert_Update_WhenTournamentDoesNotExist_ShouldThrowTournamentNotFoundException(int tournamentId)
        {
            // Arrange
            TournamentUpsertDTO upsertDto = new TournamentUpsertDTO()
            {
                Id = tournamentId,
                Name = "Updated Name"
            };

            // Act && Assert
            TournamentNotFoundException exception = Assert.ThrowsAsync<TournamentNotFoundException>(async () => await _sut.Upsert(upsertDto));

            // Assert
            //Assert.That(exception.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
            //Assert.That(exception.Value, Is.EqualTo(ExceptionErrorMessages.TournamentExceptions.TournamentNotFound));
            await Task.CompletedTask;
        }

        [Category("Update")]
        [TestCase(1)]
        public async Task Upsert_Update_WhenSuccess_ShouldReturnUpdatedRessourceDto(int tournamentId)
        {
            // Arrange
            TournamentUpsertDTO upsertDto = new TournamentUpsertDTO()
            {
                Id = tournamentId,
                Name = "Updated Tournament",
                Game = "Valorant",
                Description = "Description",
                EliminationMode = EliminationTypes.Robin,
                MaxTeamNumber = 10,
                PlayersPerTeamNumber = 30,
                EventId = 1,
            };

            // Act
            TournamentResponseDTO tournamentDto = await _sut.Upsert(upsertDto);
            Tournament tournamentDomain = await _context.Tournaments.FindAsync(tournamentDto.Id);

            // Assert against domain model
            Assert.IsNotNull(tournamentDomain);
            Assert.That(tournamentDomain.Id, Is.EqualTo(upsertDto.Id));
            Assert.That(tournamentDomain.Name, Is.EqualTo(upsertDto.Name));
          
        }

        #endregion



    }
}