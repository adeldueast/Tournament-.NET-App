using LANPartyAPI_Core.Enums;
using LANPartyAPI_Core.Exceptions;
using LANPartyAPI_Core.Models;
using LANPartyAPI_DataAccess.Data;
using LANPartyAPI_Services;
using LANPartyAPI_Services.DTOs;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LANPartyAPI_Services_Tests
{
	public class SeatServiceTests
	{
		public SeatService _sut;

		public ApplicationDbContext _context;

		public void SeedDatabase()
		{
			_context.Events.Add(new Event()
			{
				Id = 1,
				Name= "test",
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
			
			_sut = new SeatService(_context);
		}

		//[Test]
		//public async Task Upsert_Create_WhenEventNotFound_ShouldThrowHttpResponseException()
		//{

		//	//Arrange
		//	SeatsUpsertDTO seatsUps = new SeatsUpsertDTO()
		//	{
		//		EventId = 2,
		//		Seats = new List<SeatUpsertDTO>()
		//		{
		//			new SeatUpsertDTO()
		//			{
		//				Prefix = "A",
		//				Position = 3
		//			},
		//			new SeatUpsertDTO()
		//			{
		//				Prefix = "A",
		//				Position = 4
		//			}
		//		}.ToList()
		//	};


		//	//Act
		//	HttpResponseException exception = Assert.ThrowsAsync<HttpResponseException>(async () => await _sut.Upsert(seatsUps));

		//	//Assert
		//	Assert.That(exception.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
		//	Assert.That(exception.Value, Is.EqualTo(ExceptionErrorMessages.EventExceptions.EventNotFound));
		//	await Task.CompletedTask;
		//}

		//[Test]
		//public async Task Upsert_Create_WhenEventIdNull_ShouldThrowHttpResponseException()
		//{
		//	//Arrange
		//	SeatsUpsertDTO seatsUps = new SeatsUpsertDTO()
		//	{
		//		EventId = null,
		//		Seats = new List<SeatUpsertDTO>()
		//		{
		//			new SeatUpsertDTO()
		//			{
		//				Prefix = "A",
		//				Position = 3
		//			},
		//			new SeatUpsertDTO()
		//			{
		//				Prefix = "A",
		//				Position = 4
		//			}
		//		}
		//	};

		//	//Act
		//	HttpResponseException exception = Assert.ThrowsAsync<HttpResponseException>(async () => await _sut.Upsert(seatsUps));

		//	//Assert
		//	Assert.That(exception.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
		//	Assert.That(exception.Value, Is.EqualTo(ExceptionErrorMessages.EventExceptions.EventNotFound));
		//	await Task.CompletedTask;
		//}


		//[Test]
		//public async Task Upsert_Update_WhenDuplicateIds_ShouldThrowHttpResponseException()
		//{
		//	//Arrange
		//	SeatsUpsertDTO seatsUps = new SeatsUpsertDTO()
		//	{
		//		EventId = 1,
		//		Seats = new List<SeatUpsertDTO>()
		//		{
		//			new SeatUpsertDTO()
		//			{
		//				Id = 2,
		//				Prefix = "A",
		//				Position = 3
		//			},
		//			new SeatUpsertDTO()
		//			{
		//				Id = 2,
		//				Prefix = "A",
		//				Position = 4
		//			}
		//		}.ToList()
		//	};

		//	//Act
		//	HttpResponseException exception = Assert.ThrowsAsync<HttpResponseException>(async () => await _sut.Upsert(seatsUps));

		//	//Assert
		//	Assert.That(exception.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
		//	Assert.That(exception.Value, Is.EqualTo(ExceptionErrorMessages.SeatExceptions.SeatDuplicateId));
		//	await Task.CompletedTask;
		//}

		//[Test]
		//public async Task Upsert_Create_WhenDuplicatePositions_ShouldThrowHttpResponseException()
		//{
		//	//Arrange
		//	SeatsUpsertDTO seatsUps = new SeatsUpsertDTO()
		//	{
		//		EventId = 1,
		//		Seats = new List<SeatUpsertDTO>()
		//		{
		//			new SeatUpsertDTO()
		//			{
		//				Prefix = "A",
		//				Position = 3
		//			},
		//			new SeatUpsertDTO()
		//			{
		//				Prefix = "A",
		//				Position = 3
		//			}
		//		}.ToList()
		//	};

		//	//Act
		//	HttpResponseException exception = Assert.ThrowsAsync<HttpResponseException>(async () => await _sut.Upsert(seatsUps));

		//	//Assert
		//	Assert.That(exception.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
		//	Assert.That(exception.Value, Is.EqualTo(ExceptionErrorMessages.SeatExceptions.SeatDuplicatePosition));
		//	await Task.CompletedTask;
		//}

		//[Test]
		//public async Task Upsert_Create_WhenSeatAlreadyExists_ShouldThrowHttpResponseException()
		//{
		//	//Arrange
		//	SeatsUpsertDTO seatsUps = new SeatsUpsertDTO()
		//	{
		//		EventId = 1,
		//		Seats = new List<SeatUpsertDTO>()
		//		{
		//			new SeatUpsertDTO()
		//			{
		//				Id = 2,
		//				Prefix = "A",
		//				Position = 1
		//			},
		//			new SeatUpsertDTO()
		//			{
		//				Id = 3,
		//				Prefix = "A",
		//				Position = 2
		//			},
		//			new SeatUpsertDTO()
		//			{
		//				Prefix = "A",
		//				Position = 1
		//			},
		//		}.ToList()
		//	};

		//	//Act
		//	HttpResponseException exception = Assert.ThrowsAsync<HttpResponseException>(async () => await _sut.Upsert(seatsUps));

		//	//Assert
		//	Assert.That(exception.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
		//	Assert.That(exception.Value, Is.EqualTo(ExceptionErrorMessages.SeatExceptions.SeatAlreadyExist));
		//	await Task.CompletedTask;
		//}

		//[Test]
		//public async Task Upsert_Update_WhenSeatAlreadyExists_ShouldThrowHttpResponseException()
		//{
		//	//Arrange
		//	SeatsUpsertDTO seatsUps = new SeatsUpsertDTO()
		//	{
		//		EventId = 1,
		//		Seats = new List<SeatUpsertDTO>()
		//		{
		//			new SeatUpsertDTO()
		//			{
		//				Id = 2,
		//				Prefix = "A",
		//				Position = 2
		//			},
		//			new SeatUpsertDTO()
		//			{
		//				Id = 3,
		//				Prefix = "A",
		//				Position = 2
		//			}
		//		}.ToList()
		//	};

		//	//Act
		//	HttpResponseException exception = Assert.ThrowsAsync<HttpResponseException>(async () => await _sut.Upsert(seatsUps));

		//	//Assert
		//	Assert.That(exception.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
		//	Assert.That(exception.Value, Is.EqualTo(ExceptionErrorMessages.SeatExceptions.SeatAlreadyExist));
		//	await Task.CompletedTask;
		//}

		//[Test]
		//public async Task Upsert_Update_WhenSeatNotFound_ShouldThrowHttpResponseException()
		//{
		//	//Arrange
		//	SeatsUpsertDTO seatsUps = new SeatsUpsertDTO()
		//	{
		//		EventId = 1,
		//		Seats = new List<SeatUpsertDTO>()
		//		{
		//			new SeatUpsertDTO()
		//			{
		//				Id = 2,
		//				Prefix = "A",
		//				Position = 1
		//			},
		//			new SeatUpsertDTO()
		//			{
		//				Id = 3,
		//				Prefix = "A",
		//				Position = 2
		//			},
		//			new SeatUpsertDTO()
		//			{
		//				Id = 5,
		//				Prefix = "A",
		//				Position = 3
		//			}
		//		}.ToList()
		//	};

		//	//Act
		//	HttpResponseException exception = Assert.ThrowsAsync<HttpResponseException>(async () => await _sut.Upsert(seatsUps));

		//	//Assert
		//	Assert.That(exception.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
		//	Assert.That(exception.Value, Is.EqualTo(ExceptionErrorMessages.SeatExceptions.SeatNotFound));
		//	await Task.CompletedTask;
		//}

		//[Test]
		//public async Task Upsert_Create_WhenSuccess_ShouldReturnDTOsInList()
		//{
		//	//Arrange
		//	SeatsUpsertDTO seatsUps = new SeatsUpsertDTO()
		//	{
		//		EventId = 1,
		//		Seats = new List<SeatUpsertDTO>()
		//		{
		//			new SeatUpsertDTO()
		//			{
		//				Id = 2,
		//				Prefix = "A",
		//				Position = 1
		//			},
		//			new SeatUpsertDTO()
		//			{
		//				Id = 3,
		//				Prefix = "A",
		//				Position = 2
		//			},
		//			new SeatUpsertDTO()
		//			{
		//				Prefix = "A",
		//				Position = 3
		//			}
		//		}.ToList()
		//	};

		//	//Act
		//	List<SeatResponseDTO> response = await _sut.Upsert(seatsUps);

		//	//Assert
		//	Assert.That(response.Count, Is.EqualTo(seatsUps.Seats.Count));
		//	await Task.CompletedTask;
		//}

		//[Test]
		//public async Task Upsert_CreateDelete_WhenSuccess_ShouldReturnDTOsInList()
		//{
		//	//Arrange
		//	SeatsUpsertDTO seatsUps = new SeatsUpsertDTO()
		//	{
		//		EventId = 1,
		//		Seats = new List<SeatUpsertDTO>()
		//		{
		//			new SeatUpsertDTO()
		//			{
		//				Prefix = "A",
		//				Position = 3
		//			},
		//			new SeatUpsertDTO()
		//			{
		//				Id = 2,
		//				Prefix = "A",
		//				Position = 1
		//			}
		//		}.ToList()
		//	};

		//	//Act
		//	List<SeatResponseDTO> response = await _sut.Upsert(seatsUps);

		//	//Assert
		//	Assert.That(response.Count, Is.EqualTo(seatsUps.Seats.Count));
		//	await Task.CompletedTask;
		//}

		//[Test]
		//public async Task Upsert_Update_WhenSuccess_ShouldReturnDTOsInList()
		//{
		//	//Arrange
		//	SeatsUpsertDTO seatsUps = new SeatsUpsertDTO()
		//	{
		//		EventId = 1,
		//		Seats = new List<SeatUpsertDTO>()
		//		{
		//			new SeatUpsertDTO()
		//			{
		//				Id = 2,
		//				Prefix = "B",
		//				Position = 1
		//			},
		//			new SeatUpsertDTO()
		//			{
		//				Id = 3,
		//				Prefix = "A",
		//				Position = 2
		//			}
		//		}.ToList()
		//	};

		//	//Act
		//	List<SeatResponseDTO> response = await _sut.Upsert(seatsUps);

		//	//Assert
		//	Assert.That(response.Count, Is.EqualTo(seatsUps.Seats.Count));
		//	await Task.CompletedTask;
		//}

		//[Test]
		//public async Task Upsert_UpdateDelete_WhenSuccess_ShouldReturnDTOsInList()
		//{
		//	//Arrange
		//	SeatsUpsertDTO seatsUps = new SeatsUpsertDTO()
		//	{
		//		EventId = 1,
		//		Seats = new List<SeatUpsertDTO>()
		//		{
		//			new SeatUpsertDTO()
		//			{
		//				Id = 2,
		//				Prefix = "A",
		//				Position = 2
		//			},
		//			new SeatUpsertDTO()
		//			{
		//				Id = 4,
		//				Prefix = "A",
		//				Position = 4
		//			},
		//		}.ToList()
		//	};

		//	//Act
		//	List<SeatResponseDTO> response = await _sut.Upsert(seatsUps);

		//	//Assert
		//	Assert.That(response.Count, Is.EqualTo(seatsUps.Seats.Count));
		//	await Task.CompletedTask;
		//}
	}
}
