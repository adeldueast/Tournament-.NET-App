using Challonge.Api;
using Challonge.Objects;
using LANPartyAPI_Core.Exceptions;
using LANPartyAPI_DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace LANPartyAPI_Services
{
    public class MatchesService
    {
        public readonly ApplicationDbContext _dbContext;
        private readonly IChallongeClient _client;

        public MatchesService(ApplicationDbContext dbContext, IChallongeClient client)
        {
            _dbContext = dbContext;
            _client = client;
        }

        public async Task<object> GetTournamentMatches(int tournamentId)
        {
            var tournament = await _dbContext.Tournaments
                .Include(t => t.Teams).FirstOrDefaultAsync(t => t.Id == tournamentId);

            if (tournament == null)
                throw new TournamentNotFoundException();

            var challongeTournament = await _client.GetTournamentByIdAsync(tournament.Id);

            var matches = await _client.GetMatchesAsync(challongeTournament);

            List<object> matches_response = new();
            foreach (var match in matches)
            {
                var teamParticipant1 = tournament.Teams.First(team => team.Id == match.Player1Id);
                var teamParticipant2 = tournament.Teams.First(team => team.Id == match.Player2Id);

                var dto = new
                {
                    match.Id,
                    match.State,
                    match.Round,
                    match.Identifier,
                    match.Scores,
                    Team1 = new { teamParticipant1.Id,  Name = teamParticipant1.Name},  
                    Team2 = new { teamParticipant2.Id,  Name = teamParticipant2.Name},  
                    match.WinnerId,
                    match.LoserId,
                };
                matches_response.Add(dto);
            }

            return matches_response;
        }

        public async Task<object> GetTeamMatches(int tournamentId, string userId)
        {

            var tournament = await _dbContext.Tournaments.FindAsync(tournamentId);
            if (tournament == null)
                throw new TournamentNotFoundException();

            var userTeam = tournament.Teams.First(t => t.Players.Any(p => p.Id == userId));


            var challongeTournament = await _client.GetTournamentByIdAsync(tournament.Id);

            var participantTeam = await _client.GetParticipantAsync(challongeTournament, userTeam.Id);

            var userMatches = await _client.GetMatchesAsync(challongeTournament, MatchState.All, participantTeam);

            var response = userMatches.Select(m => new
            {
                m.Id,
                m.State,
                m.Round,
                m.Identifier,
                m.Scores,
                m.WinnerId,
                m.LoserId,
                m.Player1Id,
                m.Player2Id,
            });

            return response;
        }

    }
}
