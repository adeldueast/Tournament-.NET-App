using LANPartyAPI_Core.Enums;
using LANPartyAPI_Core.Exceptions;
using LANPartyAPI_Core.Models;
using LANPartyAPI_Services;
using LANPartyAPI_Services.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace LANPartyAPI.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class MatchesController : UserIdController
	{
		private readonly IMatchService _matchService;

		public MatchesController(IMatchService matchService) => _matchService = matchService;

		[HttpGet("GetAll/{tournamentId}")]
		public async Task<IActionResult> GetAllTournamentMatches(int tournamentId)
		{
			try
			{

				return Ok(await _matchService.GetAllMatches(tournamentId));
			}
			catch (TournamentNotFoundException)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.TournamentExceptions.TournamentNotFound);
			}
		}

		[HttpGet("{matchId}")]
		public async Task<IActionResult> GetMatch(int matchId)
		{
			try
			{
				
				return Ok(await _matchService.GetMatch(matchId));
			}
			catch (MatchNotFoundException)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.MatchExceptions.MatchNotFound);
			}
		}

		[HttpPost("upsert")]
		public async Task<IActionResult> UpsertMatch(MatchUpsertDTO matchDto)
		{
			try
			{
				MatchResponseDTO upsertedMatchDTO = await _matchService.UpsertMatch(matchDto);
				if (matchDto.Id == 0)
				{
					//retourne un 201 et l'endroit ou retrouver la ressource (dans le header)
					return CreatedAtAction(nameof(GetMatch), new { matchId = upsertedMatchDTO.Id }, upsertedMatchDTO);
				}
				//No content for Update, (Convention??)
				return NoContent();
			}
			catch (TournamentNotFoundException)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.TournamentExceptions.TournamentNotFound);
			}
			catch (TeamNotFoundException)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.TeamExceptions.TeamNotFound);
			}
			catch (TeamAlreadySetInRoundException)
			{
				throw new HttpResponseException(HttpStatusCode.BadRequest, ExceptionErrorMessages.TeamExceptions.TeamAlreadySetInRound);
			}
			catch (MatchInvalidStartDateException)
			{
				throw new HttpResponseException(HttpStatusCode.BadRequest, ExceptionErrorMessages.MatchExceptions.MatchInvalidStartDate);
			}
			 catch (MatchAlreadyExistException)
			{
				throw new HttpResponseException(HttpStatusCode.BadRequest, ExceptionErrorMessages.MatchExceptions.MatchAlreadyExist);
			}
			catch (MatchNotFoundException)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.MatchExceptions.MatchNotFound);
			}
		}
		
		[HttpGet("MatchTeams/{matchID}")]
		public async Task<IActionResult> GetMatchTeams(int matchID)
		{
			try
			{
				
				return Ok(await _matchService.GetMatchTeams(matchID));
			}
			catch (MatchNotFoundException)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.MatchExceptions.MatchNotFound);
			}
		}

		[HttpPost("RegisterScores")]
		public async Task<IActionResult> RegisterScores(MatchTeamsResultsDTO matchTeamsResultsDTO)
		{
			try
			{
				await _matchService.RegisterScore(matchTeamsResultsDTO);
				return Ok();
			}
			catch (MatchNotFoundException)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.MatchExceptions.MatchNotFound);
			}
			catch (TeamNotFoundException)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.TeamExceptions.TeamNotFound);
			}
		}

		[HttpGet("GetUserMatchs")]
		[Authorize]
		public async Task<IActionResult> GetUserMAtchs()
		{
            try
            {
                var userId = GetUserIdWhenAuthorize();
                return Ok(await _matchService.GetUserMatches(userId));
            }
            catch (UserNotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.UserExceptions.UserNotFound);
            }


        }
	}
}
