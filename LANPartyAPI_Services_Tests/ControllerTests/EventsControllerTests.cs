using LANPartyAPI.Controllers;
using LANPartyAPI_Core.Models;
using LANPartyAPI_DataAccess.Data;
using LANPartyAPI_Services;
using LANPartyAPI_Services.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LANPartyAPI_Services_Tests.ControllerTests
{
	public class EventsControllerTests
	{
		private Mock<IEventService> _eventServiceMock;
		private Mock<ISeatService> _seatServiceMock;
		private EventsController _controller;

		private ApplicationDbContext _context;

		public void SeedDatabase()
		{
			_context.Events.Add(new Event()
			{
				Id = 1,
				Name = "test",
				StartDate = DateTime.Now.AddDays(1),
				EndDate = DateTime.Now.AddDays(3),
			});
			_context.Seats.AddRange(
			new Seat()
			{
				Id = 2,
				Prefix = "A",
				Position = 1,
				EventId = 1,
			},
			new Seat()
			{
				Id = 3,
				Prefix = "A",
				Position = 2,
				EventId = 1,
			},
			new Seat()
			{
				Id = 4,
				Prefix = "A",
				Position = 3,
				EventId = 1,
			});
			_context.Pictures.Add(new Picture()
			{
				Id = 1,
				Path = "637bde6c-124f-4ea6-83c2-1b52b186e1c6.jpg",
				MimeType = "image/jpeg",
				EventId = 1,
			});
			_context.SaveChanges();
		}

		[SetUp]
		public void Setup()
		{
			_context = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(databaseName: "InMemoryDatabase")
				.Options
				);
			if (!_context.Database.IsInMemory()) return;
			_context.Database.EnsureDeleted();
			_context.Database.EnsureCreated();
			SeedDatabase();

			_eventServiceMock = new Mock<IEventService>();
			_seatServiceMock = new Mock<ISeatService>();
			_controller = new EventsController(_eventServiceMock.Object, _seatServiceMock.Object);
		}

		/*[Test]
		public async Task UpsertPlanLayout_UpdateSeatsOnly_WhenSuccess_ReturnsOK()
		{
			//Arrange
			SeatsUpsertDTO seatsUps = new SeatsUpsertDTO()
			{
				EventId = 1,
				Seats = new List<SeatUpsertDTO>()
				{
					new SeatUpsertDTO()
					{
						Id = 2,
						Prefix = "B",
						Position = 2
					},
					new SeatUpsertDTO()
					{
						Id= 3,
						Prefix = "B",
						Position = 3
					},
					new SeatUpsertDTO()
					{
						Id = 4,
						Prefix = "B",
						Position = 4
					},
				}.ToList()
			};

			_eventServiceMock.Setup(e => e.UpsertPlanLayout(seatsUps)).Verifiable();

			//Act
			OkResult result = (await _controller.UpsertSeats(seatsUps)) as OkResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(200, result.StatusCode);

			
		}*/

		[Test]
		public async Task UpsertPlanLayout_Update_Seats_Image()
		{
			//Arrange
			_eventServiceMock.Setup(s => s.UpsertPlanLayout(It.IsAny<SeatsUpsertDTO>())).Verifiable();

			//Act
			OkResult result = (await _controller.UpsertSeats(null)) as OkResult;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(200, result.StatusCode);
		}
	}
}
