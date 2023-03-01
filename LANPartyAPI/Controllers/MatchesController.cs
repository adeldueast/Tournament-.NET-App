using LANPartyAPI_Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LANPartyAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly MatchesService _matchesService;
        public MatchesController(MatchesService matchesService)
        {
            _matchesService = matchesService;
        }

        [HttpGet("{tournamentId}")]
        //[Authorize]
        public async Task<IActionResult> Index([FromRoute]int tournamentId)
        {
            try
            {
                var matches = await _matchesService.GetTournamentMatches(tournamentId);
                return Ok(matches);
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex);
            }
        }
    }
}
