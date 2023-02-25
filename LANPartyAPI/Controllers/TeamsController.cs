using LANPartyAPI_Core.Enums;
using LANPartyAPI_Core.Exceptions;
using LANPartyAPI_Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LANPartyAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class TeamsController : ControllerBase
	{
		private readonly TeamService _teamService;

		public TeamsController(TeamService teamService)
		{
			_teamService = teamService;
		}

		[HttpGet("{tournamentId}")]
		public async Task<IActionResult> GetAllTeamsOfTournament( int tournamentId)
		{
			try
			{
				var teams = await _teamService.GetAllTeams(tournamentId);

				return Ok(teams);
			}
			catch (TournamentNotFoundException)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.TournamentExceptions.TournamentNotFound);
			}
		}
	}
}
