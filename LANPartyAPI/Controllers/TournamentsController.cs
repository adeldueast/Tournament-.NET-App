using LANPartyAPI_Core.Enums;
using LANPartyAPI_Core.Exceptions;
using LANPartyAPI_Services;
using LANPartyAPI_Services.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LANPartyAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TournamentsController : UserIdController
    {
        private readonly ITournamentsService _tournamentsService;

        public TournamentsController(ITournamentsService tournamentsService) => _tournamentsService = tournamentsService;

        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetEventTournaments(int eventId)
        {
            try
            {
                var userId = await GetUserIdAsync();
                List<TournamentResponseDTO> tournamentsDtos = await _tournamentsService.GetEventTournaments(eventId, userId);
                return Ok(tournamentsDtos);
            }
            catch (EventNotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.EventExceptions.EventNotFound);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTournaments()
        {
            var userId = await GetUserIdAsync();
            List<TournamentResponseDTO> tournamentsDtos = await _tournamentsService.GetAllTournaments(userId);
            return Ok(tournamentsDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTournament(int id)
        {
            try
            {
                var userId = await GetUserIdAsync();
                TournamentResponseDTO tournamentDto = await _tournamentsService.GetTournament(id, userId);
                return Ok(tournamentDto);
            }
            catch (TournamentNotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.TournamentExceptions.TournamentNotFound);
            }
        }

        [HttpPost("upsert")]
        public async Task<IActionResult> UpsertTournament(TournamentUpsertDTO tournamentDto)
        {
            try
            {
                TournamentResponseDTO upsertedTournamentDto = await _tournamentsService.Upsert(tournamentDto);

                if (tournamentDto.Id == 0)
                {
                    //retourne un 201 et l'endroit ou retrouver la ressource (dans le header)
                    return CreatedAtAction(nameof(GetTournament), new { id = upsertedTournamentDto.Id }, upsertedTournamentDto);
                }
                //No content for Update, (Convention??)
                return NoContent();
            }
            catch (TournamentNameTakenException)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest, ExceptionErrorMessages.TournamentExceptions.TournamentNameTaken);
            }
            catch (EventNotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.EventExceptions.EventNotFound);
            }
            catch (TournamentNotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.TournamentExceptions.TournamentNotFound);
            }
        }

        [Authorize(AuthenticationSchemes =  JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("join")]
        public async Task<IActionResult> JoinTournamentCreateTeam(TeamUpsertDTO teamDto)
        {
            try
            {
                var userId = GetUserIdWhenAuthorize();
                await _tournamentsService.JoinTournamentCreateTeam(teamDto, userId);
                return Ok();
            }
            catch (TournamentNotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.TournamentExceptions.TournamentNotFound);
            }
            catch (TeamLimitReachedException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.TeamExceptions.TeamLimitReached);
            }
            catch (UserNotInEventException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.UserExceptions.UserNotInEvent);
            }
            catch (UserAlreadyJoinedTournamentException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.UserExceptions.UserAlreadyJoinedTournament);
            }
            catch (TeamNameAlreadyTakenException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.TeamExceptions.TeamNameAlreadyTaken);
            }

        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("join/{teamId}")]
        public async Task<IActionResult> JoinTournamentExistingTeam(int teamId)
        {

            try
            {
                var userId = GetUserIdWhenAuthorize();
                await _tournamentsService.JoinTournamentExistingTeam(teamId, userId);
                return Ok();
            }
            catch (TeamNotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.TeamExceptions.TeamNotFound);
            }
            catch (UserNotInEventException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.UserExceptions.UserNotFound);

            }
            catch (UserAlreadyJoinedTournamentException)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest, ExceptionErrorMessages.UserExceptions.UserAlreadyJoinedEvent);

            }
            catch (TeamPlayersLimitReachedException)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest, ExceptionErrorMessages.TeamExceptions.TeamPlayersLimitReached);
            }

        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("{tournamentId}/leave")]
        public async Task<IActionResult> QuitTournament(int tournamentId)
        {
            try
            {
                var userId = GetUserIdWhenAuthorize();
                await _tournamentsService.QuitTournament(userId, tournamentId);
                return Ok();
            }
            catch (TournamentNotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.TournamentExceptions.TournamentNotFound);

            }
            catch (UserNotInTournamentException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.UserExceptions.UserNotInTournament);

            }

        }
    }
}
