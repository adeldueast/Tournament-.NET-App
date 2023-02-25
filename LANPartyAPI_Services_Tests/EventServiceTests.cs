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
using NuGet.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LANPartyAPI_Services_Tests
{
    public class EventServiceTests
    {
        public EventService _eventService;
        public SeatService _seatService;
        public PictureService _pictureService;
        public EventsController _controller;

        public ApplicationDbContext _applicationDbContext;


        Mock<IEventService> _mockEventService;
        Mock<ISeatService> _mockSeatService;


        public void SeedDatabase()
        {
            _applicationDbContext.Events.AddRange(
            new Event()
            {

                Name = "Event01",
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(4),
            },
            new Event()
            {

                Name = "Event02",
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(4),
            },
            new Event()
            {

                Name = "Event03",
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(4),
            }
            );
            _applicationDbContext.SaveChanges();
        }
        [SetUp]
        public void Setup()
        {
            _applicationDbContext = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options);

            _applicationDbContext.Database.EnsureDeleted();

            SeedDatabase();
            _seatService = new SeatService(_applicationDbContext);
            _eventService = new EventService(_applicationDbContext, _seatService, _pictureService);

           

            _mockEventService = new Mock<IEventService>();

            _mockSeatService = new Mock<ISeatService>();

            _controller = new EventsController(_mockEventService.Object, _mockSeatService.Object);

            
        }

        [Category("Upsert")]
        [Test]
        public async Task Upsert_Controller_SuccesfullUpdate()
        {
            EventUpsertDTO upsertDTO = new EventUpsertDTO()
            {
                Id = 1,
                Name = "ControllerEvent01",
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(4),

            };


            _mockEventService.Setup(s => s.Upsert(upsertDTO))
                .ReturnsAsync(new EventResponseDTO() { Id = 1 });


            //Act
            NoContentResult result = await _controller.UpsertEvent(upsertDTO) as NoContentResult;



            //Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.NoContent));
           

            

        }

        [Category("Upsert")]
        [Test]
        public async Task Upsert_Controller_SuccesfullInsert()
        {
            EventUpsertDTO upsertDTO = new EventUpsertDTO()
            {
                Id = 0,
                Name = "ControllerEvent01",
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(4),

            };



           

            _mockEventService.Setup(s => s.Upsert(upsertDTO))
                .ReturnsAsync(new EventResponseDTO() { Id = 1 });

            //Act
            CreatedAtActionResult result = await _controller.UpsertEvent(upsertDTO) as CreatedAtActionResult;
            EventResponseDTO resultValue = result.Value as EventResponseDTO;


            //Assert
            Assert.IsNotNull(result);
            Assert.That(resultValue.Id, Is.EqualTo(1));

        }

        [Category("Upsert")]
        [Test]
        public async Task Upsert_Create_NameAlreadyTaken_ShouldThrowHttpResponseException()
        {

            //Arrange
            EventUpsertDTO upsertDTO = new EventUpsertDTO()
            {
                Id = 0,
                Name = "Event01",
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(4),

            };


            //Act && Assert
            EventNameTakenException exception = Assert.ThrowsAsync<EventNameTakenException>(async () => await _eventService.Upsert(upsertDTO));

            //Assert
            //Assert.That(exception, Is.EqualTo(EventNameTakenException));
            //Assert.That(exception.Value, Is.EqualTo(ExceptionErrorMessages.EventExceptions.EventNameTaken));
            await Task.CompletedTask;

        }

        [Category("Upsert")]
        [Test]
        public async Task Upsert_Update_NameAlreadyTaken_ShouldThrowHttpResponseException()
        {

            //Arrange
            EventUpsertDTO upsertDTO = new EventUpsertDTO()
            {
                Id = 1,
                Name = "Event02",
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(4),

            };


            //Act && Assert
            EventNameTakenException exception = Assert.ThrowsAsync<EventNameTakenException>(async () => await _eventService.Upsert(upsertDTO));

            //Assert
           // Assert.That(exception.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
            //Assert.That(exception.Value, Is.EqualTo(ExceptionErrorMessages.EventExceptions.EventNameTaken));
            await Task.CompletedTask;

        }

        [Category("Upsert")]
        [Test]
        public async Task Upsert_endDateBeforeStartDate_ShouldThrowInvalidDatesException()
        {

            //Arrange
            EventUpsertDTO upsertDTO = new EventUpsertDTO()
            {
                Id = 1,
                Name = "NewEvent01",
                StartDate = DateTime.Now.AddDays(4),
                EndDate = DateTime.Now.AddDays(1),

            };


            //Act
            EventInvalidDatesException exception = Assert.ThrowsAsync<EventInvalidDatesException>(async () => await _eventService.Upsert(upsertDTO));

            //Assert
           // Assert.That(exception.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
            //Assert.That(exception.Value, Is.EqualTo(ExceptionErrorMessages.EventExceptions.EventInvalidDates));
            await Task.CompletedTask;

        }

        [Category("Upsert")]
        [Test]
        public async Task Upsert_Update_NonExistentId_ShouldThrowHttpResponseException()
        {

            //Arrange
            EventUpsertDTO upsertDTO = new EventUpsertDTO()
            {
                Id = 18,
                Name = "NonExistingEvent01",
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(4),

            };

            //Erreur lors du lancement en batch mais la 2eme exécution fonctionne
            //Act
            EventNotFoundException exception = Assert.ThrowsAsync<EventNotFoundException>(async () => await _eventService.Upsert(upsertDTO));

            //Assert
           // Assert.That(exception.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
           // Assert.That(exception.Value, Is.EqualTo(ExceptionErrorMessages.EventExceptions.EventNotFound));
            await Task.CompletedTask;

        }

        [Category("Upsert")]
        [Test]
        public async Task Upsert_SuccessfulUpdate_ShouldUpdate()
        {

            //Arrange
            EventUpsertDTO upsertDTO = new EventUpsertDTO()
            {
                Id = 1,
                Name = "NewEvent01",
                StartDate = DateTime.Now.AddDays(4),
                EndDate = DateTime.Now.AddDays(8),
                MaxPlayerNumber = 25,
                Description = "pipo"

            };


            //Act
            EventResponseDTO responseDTO = await _eventService.Upsert(upsertDTO);

            ////Assert
            Assert.That(responseDTO.Name, Is.EqualTo(upsertDTO.Name));
            Assert.That(responseDTO.StartDate, Is.EqualTo(upsertDTO.StartDate));
            Assert.That(responseDTO.EndDate, Is.EqualTo(upsertDTO.EndDate));
            Assert.That(responseDTO.MaxPlayerNumber, Is.EqualTo(upsertDTO.MaxPlayerNumber));
            Assert.That(responseDTO.Description, Is.EqualTo(upsertDTO.Description));


            //Assert.That(exception.Value, Is.EqualTo(ExceptionErrorMessages.EventExceptions.EventNotFound));
            //await Task.CompletedTask;

        }
        [Category("Upsert")]
        [Test]
        public async Task Upsert_SuccessfulInsert_ShouldInsert()
        {

            //Arrange
            EventUpsertDTO upsertDTO = new EventUpsertDTO()
            {
                Id = 0,
                Name = "NewEvent",
                StartDate = DateTime.Now.AddDays(4),
                EndDate = DateTime.Now.AddDays(8),
                MaxPlayerNumber = 25,
                Description = "pipo"

            };


            //Act
            EventResponseDTO responseDTO = await _eventService.Upsert(upsertDTO);

            ////Assert
            Assert.That(responseDTO.Name, Is.EqualTo(upsertDTO.Name));
            Assert.That(responseDTO.StartDate, Is.EqualTo(upsertDTO.StartDate));
            Assert.That(responseDTO.EndDate, Is.EqualTo(upsertDTO.EndDate));
            Assert.That(responseDTO.MaxPlayerNumber, Is.EqualTo(upsertDTO.MaxPlayerNumber));
            Assert.That(responseDTO.Description, Is.EqualTo(upsertDTO.Description));


            //Assert.That(exception.Value, Is.EqualTo(ExceptionErrorMessages.EventExceptions.EventNotFound));
            //await Task.CompletedTask;

        }


    }
}
