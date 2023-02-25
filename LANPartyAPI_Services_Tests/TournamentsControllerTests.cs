using LANPartyAPI.Controllers;
using LANPartyAPI_Core.Enums;
using LANPartyAPI_Core.Exceptions;
using LANPartyAPI_Core.Models;
using LANPartyAPI_DataAccess.Data;
using LANPartyAPI_Services;
using LANPartyAPI_Services.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LANPartyAPI_Services_Tests
{
    public class TournamentsControllerTests
    {

        private Mock<ITournamentsService> _mockTournamentsService;
        private TournamentsController _controller;


        [SetUp]
        public void Setup()
        {
            _mockTournamentsService = new Mock<ITournamentsService>();
            _controller = new TournamentsController(_mockTournamentsService.Object);
        }

        [Test]
        public async Task UpsertTournament_Create_OnSuccess_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var upsertDto = new TournamentUpsertDTO()
            {
                Id = 0,
                EventId = 1,
                Name = "Volks",
                Game = "Rust",
                Description = "Description",
                EliminationMode = EliminationTypes.Simple,
                MaxTeamNumber = 10,
                PlayersPerTeamNumber = 10,
            };

            _mockTournamentsService.Setup(s => s.Upsert(upsertDto))
                .ReturnsAsync(new TournamentResponseDTO() { Id = 1 });

            // Act
            CreatedAtActionResult result = await _controller.UpsertTournament(upsertDto) as CreatedAtActionResult;
            TournamentResponseDTO resultValue = result.Value as TournamentResponseDTO;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(resultValue.Id, Is.EqualTo(1));
        }

        [Test]
        public async Task UpsertTournament_Update_OnSuccess_ShouldReturnNoContent()
        {
            // Arrange
            var upsertDto = new TournamentUpsertDTO()
            {
                Id = 1,
                EventId = 1,
                Name = "Volks",
                Game = "Rust",
                Description = "Description",
                EliminationMode = EliminationTypes.Simple,
                MaxTeamNumber = 10,
                PlayersPerTeamNumber = 10,
            };

            _mockTournamentsService.Setup(s => s.Upsert(upsertDto))
                .ReturnsAsync(new TournamentResponseDTO() { Id = 1, });

            // Act
            NoContentResult result = await _controller.UpsertTournament(upsertDto) as NoContentResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.NoContent));
        }


        //Exceptions

        [Test]
        public async Task UpsertTournament_WhenTournamentNameTaken_ShouldShouldThrowHttpResponseException()
        {
            // Arrange
            var upsertDto = new TournamentUpsertDTO()
            {
                Id = 1,
                EventId = 1,
                Name = "Volks",
                Game = "Rust",
                Description = "Description",
                EliminationMode = EliminationTypes.Simple,
                MaxTeamNumber = 10,
                PlayersPerTeamNumber = 10,
            };

            _mockTournamentsService.Setup(s => s.Upsert(upsertDto))
                .ThrowsAsync(new TournamentNameTakenException());

            // Act
            HttpResponseException exception = Assert.ThrowsAsync<HttpResponseException>(async () => await _controller.UpsertTournament(upsertDto));

            // Assert
            Assert.That(exception.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
            Assert.That(exception.Value, Is.EqualTo(ExceptionErrorMessages.TournamentExceptions.TournamentNameTaken));
            await Task.CompletedTask;

        }
        [Test]
        public async Task UpsertTournament_WhenEventNotFound_ShouldThrowHttpResponseException()
        {
            // Arrange
            var upsertDto = new TournamentUpsertDTO()
            {
                Id = 1,
                EventId = 1,
                Name = "Volks",
                Game = "Rust",
                Description = "Description",
                EliminationMode = EliminationTypes.Simple,
                MaxTeamNumber = 10,
                PlayersPerTeamNumber = 10,
            };

            _mockTournamentsService.Setup(s => s.Upsert(upsertDto))
                .ThrowsAsync(new EventNotFoundException());

            // Act
            HttpResponseException exception = Assert.ThrowsAsync<HttpResponseException>(async () => await _controller.UpsertTournament(upsertDto));

            // Assert
            Assert.That(exception.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
            Assert.That(exception.Value, Is.EqualTo(ExceptionErrorMessages.EventExceptions.EventNotFound));
            await Task.CompletedTask;
        }
        [Test]
        public async Task UpsertTournament_WhenTournamentNotFoundException_ShouldThrowHttpResponseException()
        {
            // Arrange
            var upsertDto = new TournamentUpsertDTO()
            {
                Id = 1,
                EventId = 1,
                Name = "Volks",
                Game = "Rust",
                Description = "Description",
                EliminationMode = EliminationTypes.Simple,
                MaxTeamNumber = 10,
                PlayersPerTeamNumber = 10,
            };

            _mockTournamentsService.Setup(s => s.Upsert(upsertDto))
                .ThrowsAsync(new TournamentNotFoundException());

            // Act
            HttpResponseException exception = Assert.ThrowsAsync<HttpResponseException>(async () => await _controller.UpsertTournament(upsertDto));

            // Assert
            Assert.That(exception.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
            Assert.That(exception.Value, Is.EqualTo(ExceptionErrorMessages.TournamentExceptions.TournamentNotFound));
            await Task.CompletedTask;

        }


    }
}
