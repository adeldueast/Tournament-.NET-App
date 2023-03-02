using Challonge.Api;
using LANPartyAPI_Core.Enums;
using LANPartyAPI_Core.Exceptions;
using LANPartyAPI_Services;
using LANPartyAPI_Services.DTOs;
using LANPartyAPI_Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LANPartyAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TournamentsController : UserIdController
    {

        private readonly IChallongeClient _client;
        private readonly ITournamentsService _tournamentsService;
        public TournamentsController(ITournamentsService tournamentsService, IChallongeClient client)
        {
            _tournamentsService = tournamentsService;
            _client = client;
        }



        [Authorize]
        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetEventTournaments(int eventId)
        {
            try
            {
                var userId = GetUserIdWhenAuthorize();
                var response = await _tournamentsService.GetAllEventTournaments(eventId, userId);
                return Ok(response);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{tournamentId}")]
        public async Task<IActionResult> GetTournament(int tournamentId)
        {
            try
            {
                var response = await _tournamentsService.GetTournament(tournamentId);
                return Ok(response);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("upsert")]
        public async Task<IActionResult> UpsertTournament(TournamentUpsertDTO tournamentDto)
        {
            try
            {
                var response = await _tournamentsService.UpsertTournament(tournamentDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpDelete("{tournamentId}")]
        public async Task<IActionResult> DeleteTournament(int tournamentId)
        {
            try
            {
                await _tournamentsService.DeleteTournament(tournamentId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("start/{tournamentId}")]
        public async Task<IActionResult> StartTournament(int tournamentId)
        {
            try
            {
                await _tournamentsService.StartTournament(tournamentId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("finalize/{tournamentId}")]
        public async Task<IActionResult> FinalizeTournament(int tournamentId)
        {
            try
            {
                await _tournamentsService.FinalizeTournament(tournamentId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        #region
        [Authorize]
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
            catch (Exception e)
            {
                return StatusCode(500, e);
            }

        }

        [Authorize]
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

        [Authorize]
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
            catch (Exception e)
            {
                return StatusCode(500, e);
            }

        }
        #endregion
    }
}
