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

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
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





    }
}