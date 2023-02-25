using LANPartyAPI_Core;
using LANPartyAPI_Core.Enums;
using LANPartyAPI_Core.Exceptions;
using LANPartyAPI_Core.Models;
using LANPartyAPI_Services;
using LANPartyAPI_Services.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LANPartyAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventsController : UserIdController
    {

        private readonly IEventService _eventService;
        private readonly ISeatService _seatService;

        public EventsController(IEventService eventService, ISeatService seatService)
        {
            _eventService = eventService;
            _seatService = seatService;
        }



        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            var userId = await GetUserIdAsync();
            List<EventResponseDTO> eventsDtos = await _eventService.GetAllEvents(userId);
            return Ok(eventsDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent(int id)
        {
			try
			{
				var userId = await GetUserIdAsync();
				EventResponseDTO e = await _eventService.GetEvent(id, userId);
				return Ok(e);
			}
			catch (EventNotFoundException)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.EventExceptions.EventNotFound);
			}
        }

        [HttpPost("upsert")]
        public async Task<IActionResult> UpsertEvent(EventUpsertDTO eventDto)
        {
			try
			{
				EventResponseDTO upsertedEventDTO = await _eventService.Upsert(eventDto);
				if (eventDto.Id == 0)
				{
					//retourne un 201 et l'endroit ou retrouver la ressource (dans le header)
					return CreatedAtAction(nameof(GetEvent), new { id = upsertedEventDTO.Id }, upsertedEventDTO);
				}
				//No content for Update, (Convention??)
				return NoContent();
			}
			catch (EventNameTakenException)
			{
				throw new HttpResponseException(HttpStatusCode.BadRequest, ExceptionErrorMessages.EventExceptions.EventNameTaken);
			}
			catch (EventInvalidDatesException)
			{
				throw new HttpResponseException(HttpStatusCode.BadRequest, ExceptionErrorMessages.EventExceptions.EventInvalidDates);
			}
			catch (EventNotFoundException)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.EventExceptions.EventNotFound);
			}

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
			try
			{
				await _eventService.DeleteEvent(id);
				return NoContent();
			}
			catch (EventNotFoundException)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.EventExceptions.EventNotFound);
			}
		}

        [HttpGet("{id}/seats")]
        public async Task<IActionResult> GetAllSeats(int id)
        {
			try
			{
				List<SeatResponseDTO> seatsDtos = await _seatService.GetAllSeats(id);
				return Ok(seatsDtos);
			}
			catch (EventNotFoundException)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.EventExceptions.EventNotFound);
			}
			
        }

        [HttpPost("seats/upsert")]
        public async Task<IActionResult> UpsertSeats([FromForm] SeatsUpsertDTO seatsUpsertDTO)
        {
			try
			{
				await _eventService.UpsertPlanLayout(seatsUpsertDTO);
				return Ok();
			}
			catch (EventNotFoundException)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.EventExceptions.EventNotFound);
			}
			catch (NoSeatsGivenException)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.SeatExceptions.NoSeatsGiven);
			}
			catch (SeatDuplicateIdException)
			{
				throw new HttpResponseException(HttpStatusCode.BadRequest, ExceptionErrorMessages.SeatExceptions.SeatDuplicateId);
			}
			catch (SeatDuplicatePositionException)
			{
				throw new HttpResponseException(HttpStatusCode.BadRequest, ExceptionErrorMessages.SeatExceptions.SeatDuplicatePosition);
			}
			catch (SeatAlreadyExistException)
			{
				throw new HttpResponseException(HttpStatusCode.BadRequest, ExceptionErrorMessages.SeatExceptions.SeatAlreadyExist);
			}
			catch (SeatNotFoundException)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.SeatExceptions.SeatNotFound);
			}
			catch (NoFileGivenException)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.PictureExceptions.NoFileGiven);
			}
		}


		

        [HttpPost("{eventId}/join")]
        [Authorize]
        public async Task<IActionResult> JoinEvent(int eventId)
        {
			try
			{
				var userId = GetUserIdWhenAuthorize();
				await _eventService.JoinEvent(userId, eventId);
				return NoContent();
			}
			catch (EventNotFoundException)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.EventExceptions.EventNotFound);
			}
			catch (UserNotFoundException)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.UserExceptions.UserNotFound);
			}
			catch (UserAlreadyJoinedEventException)
			{
				throw new HttpResponseException(HttpStatusCode.BadRequest, ExceptionErrorMessages.UserExceptions.UserAlreadyJoinedEvent);
			}
			catch (EventMaxPlayersNumberReachedException)
			{
				throw new HttpResponseException(HttpStatusCode.BadRequest, ExceptionErrorMessages.EventExceptions.EventMaxPlayersNumberReached);
			}
		}


        [Authorize]
        [HttpPost("seats/Reserve/{seatId}")]
        public async Task<IActionResult> ReserveSeat(int seatId, string userId)
        {
			try
			{
				userId = GetUserIdWhenAuthorize();
				await _seatService.ReserveSeat(seatId, userId);
				//Todo return new user seat info??
				return Ok();
			}
			catch (SeatNotFoundException)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.SeatExceptions.SeatNotFound);
			}
			catch (UserNotFoundException)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.UserExceptions.UserNotFound);
			}
			catch (UserNotOwnerOfSeatException)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.UserExceptions.UserNotOwnerOfSeat);
			}
			catch (UserNotInEventException)
			{
				throw new HttpResponseException(HttpStatusCode.BadRequest, ExceptionErrorMessages.UserExceptions.UserNotInEvent);
			}
			catch (SeatAlreadyReservedException)
			{
				throw new HttpResponseException(HttpStatusCode.BadRequest, ExceptionErrorMessages.SeatExceptions.SeatAlreadyReserved);
			}
        }

        [Authorize]
        [HttpPost("seats/Remove/{seatId}")]
        public async Task<IActionResult> RemoveSeat(int seatId)//Todo rename method name if its confusing (Opposite of reserve)
        {
			try
			{
				var userId = GetUserIdWhenAuthorize();
				await _seatService.RemoveSeat(seatId, userId);
				return Ok();
			}
			catch (UserNotFoundException)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.UserExceptions.UserNotFound);
			}
			catch (SeatNotFoundException)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.SeatExceptions.SeatNotFound);
			}
        }

    }
}