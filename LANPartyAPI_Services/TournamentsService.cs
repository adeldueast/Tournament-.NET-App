using Challonge.Api;
using Challonge.Objects;
using LANPartyAPI_Core.Exceptions;
using LANPartyAPI_Core.Models;
using LANPartyAPI_DataAccess.Data;
using LANPartyAPI_Services.DTOs;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using challongeTournament = Challonge.Objects.Tournament;
using Tournament = LANPartyAPI_Core.Models.Tournament;

namespace LANPartyAPI_Services
{
    public interface ITournamentsService
    {
        public Task<object> UpsertTournament(TournamentUpsertDTO tournamentUpsertDTO);
        public Task<object> GetTournament(int tournamentId);
        public Task<object> GetAllEventTournaments(int eventId, string userId);
        public Task DeleteTournament(int tournamentId);
        public Task JoinTournamentCreateTeam(TeamUpsertDTO teamUpsert, string userId);
        public Task JoinTournamentExistingTeam(int teamId, string userId);
        public Task QuitTournament(string userId, int tournamentId);

    }
    public class TournamentsService : ITournamentsService
    {

        public readonly ApplicationDbContext _dbContext;
        private readonly IChallongeClient _client;

        public TournamentsService(ApplicationDbContext dbContext, IChallongeClient client)
        {
            _dbContext = dbContext;
            _client = client;
        }

        public async Task<object> UpsertTournament(TournamentUpsertDTO tournamentUpsertDTO)
        {

            var eventExist = await _dbContext.Events.AnyAsync(e => e.Id == tournamentUpsertDTO.EventId);
            if (!eventExist)
                throw new EventNotFoundException();

            var domainTournament = tournamentUpsertDTO.Id == 0
                ? await _dbContext.Tournaments.FirstOrDefaultAsync(t => t.Name == tournamentUpsertDTO.Name)
                : await _dbContext.Tournaments.FirstOrDefaultAsync(t => t.Name == tournamentUpsertDTO.Name && t.Id != tournamentUpsertDTO.Id);

            if (domainTournament != null)
                throw new TournamentNameTakenException();


            if (tournamentUpsertDTO.Id == 0)
            {
                TournamentInfo info = new()
                {
                    Name = tournamentUpsertDTO.Name,
                    TournamentType = (TournamentType)tournamentUpsertDTO.TournamentType,
                    Description = tournamentUpsertDTO.Description,
                    SignupCap = tournamentUpsertDTO.MaxTeamNumber,
                    Private = true,
                    ShowRounds = true,
                    AcceptAttachments = true,


                };

                challongeTournament tournament = await _client.CreateTournamentAsync(info);

                if (tournament == null)
                    throw new Exception();

                domainTournament = new Tournament()
                {
                    Id = Convert.ToInt32(tournament.Id),
                    Url = tournament.Url,
                    EventId = (int)tournamentUpsertDTO.EventId,
                    Name = tournament.Name,
                    Description = tournament.Description,
                    Game = tournamentUpsertDTO.Game,
                    MaxTeamNumber = (int)tournament.SignupCap,
                    MaxPlayersPerTeam = (int)tournamentUpsertDTO.MaxPlayersPerTeam,
                    TournamentType = (LANPartyAPI_Core.Enums.TournamentType)tournament.TournamentType,
                };


                _dbContext.Tournaments.Add(domainTournament);

            }
            else
            {

                domainTournament = await _dbContext.Tournaments.FindAsync(tournamentUpsertDTO.Id);
                if (domainTournament == null)
                    throw new TournamentNotFoundException();

                TournamentInfo info = new()
                {
                    Name = tournamentUpsertDTO.Name,
                    TournamentType = (TournamentType)tournamentUpsertDTO.TournamentType,
                    Description = tournamentUpsertDTO.Description,
                    SignupCap = tournamentUpsertDTO.MaxTeamNumber,
                    Private = true,
                    ShowRounds = true,
                    AcceptAttachments = true,
                };

                challongeTournament tournament = await _client.GetTournamentByIdAsync(tournamentUpsertDTO.Id);
                if (tournament == null)
                    throw new Exception();

                challongeTournament updatedTournament = await _client.UpdateTournamentAsync(tournament, info);


                domainTournament.Name = updatedTournament.Name;
                domainTournament.Description = updatedTournament.Description;
                domainTournament.Game = tournamentUpsertDTO.Game;
                domainTournament.MaxTeamNumber = (int)updatedTournament.SignupCap;
                domainTournament.MaxPlayersPerTeam = (int)tournamentUpsertDTO.MaxPlayersPerTeam;
                domainTournament.TournamentType = (LANPartyAPI_Core.Enums.TournamentType)updatedTournament.TournamentType;


            }

            await _dbContext.SaveChangesAsync();

            var response = new TournamentResponseDTO()
            {
                Id = domainTournament.Id,
                Description = domainTournament.Description,
                TournamentType = domainTournament.TournamentType,
                Game = domainTournament.Game,
                EventId = (int)tournamentUpsertDTO.EventId,
                Name = domainTournament.Name,
                MaxTeamNumber = domainTournament.MaxTeamNumber,
                MaxPlayersPerTeam = domainTournament.MaxPlayersPerTeam,
                Url = domainTournament.Url,
            };

            return response;
        }

        public async Task<object> GetAllEventTournaments(int eventId, string userId)
        {
            var e = await _dbContext.Events
                .Include(e => e.Tournaments)
                .ThenInclude(tourn => tourn.Teams)
                .ThenInclude(team => team.Players)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            var tournaments = e.Tournaments.Select(tourn => new TournamentResponseDTO()
            {
                Id = tourn.Id,
                Name = tourn.Name,
                Description = tourn.Description,
                Game = tourn.Game,
                TournamentType = tourn.TournamentType,
                MaxTeamNumber = tourn.MaxTeamNumber,
                MaxPlayersPerTeam = tourn.MaxPlayersPerTeam,
                EventId = (int)tourn.EventId,
                hasJoined = tourn.Teams.SelectMany(t => t.Players).Any(p => p.Id == userId),
                Url = tourn.Url
            }).ToList();

            return tournaments;
        }

        public async Task<object> GetTournament(int tournamentId)
        {
            var tournament = await _dbContext.Tournaments.FindAsync(tournamentId);

            if (tournament == null)
                throw new TournamentNotFoundException();

            var response = new TournamentResponseDTO()
            {
                Id = tournament.Id,
                Description = tournament.Description,
                TournamentType = tournament.TournamentType,
                Game = tournament.Game,
                EventId = (int)tournament.EventId,
                Name = tournament.Name,
                MaxTeamNumber = tournament.MaxTeamNumber,
                MaxPlayersPerTeam = tournament.MaxPlayersPerTeam,
                Url = tournament.Url
            };

            return response;
        }

        public async Task JoinTournamentCreateTeam(TeamUpsertDTO teamUpsert, string userId)
        {

            Tournament tournament = await _dbContext.Tournaments
                .Include(t => t.Event)
                .Include(t => t.Teams.Where(t => t.TournamentId == teamUpsert.TournamentId))
                .ThenInclude(team => team.Players)
                .FirstOrDefaultAsync(t => t.Id == teamUpsert.TournamentId);

            //Check if tournament exist
            if (tournament == null)
                throw new TournamentNotFoundException();


            //Check if tournament team limit is reached
            if (tournament.Teams.Count == tournament.MaxTeamNumber)
                throw new TeamLimitReachedException();


            //Check if user has already joined a tournament (team)
            bool userAlreadyInTeam = tournament.Teams
                .SelectMany(t => t.Players)
                .Any(player => player.Id == userId);
            if (userAlreadyInTeam)
                throw new UserAlreadyJoinedTournamentException();

            //Check if new team's name is already taken
            bool teamNameAlreadyTaken = tournament.Teams
                .Any(team => team.Name.ToUpper() == teamUpsert.Name.ToUpper());
            if (teamNameAlreadyTaken)
                throw new TeamNameAlreadyTakenException();



            var challongeTournament = await _client.GetTournamentByIdAsync((long)tournament.Id);
            Participant teamParticipant = await _client.CreateParticipantAsync(challongeTournament, new ParticipantInfo { Name = teamUpsert.Name, });


            //Check if user is register to the event
            ApplicationUser user = await _dbContext.Users
                .Include(u => u.Events)
                .FirstOrDefaultAsync(user => user.Id == userId);

            var userIsInEvent = user.Events.Any(ev => ev.Id == tournament.EventId);
            if (!userIsInEvent)
            {
                user.Events.Add(tournament.Event);
                //tournament.Event.Players.Add(user);
            }


            Team team = new Team()
            {
                Id = (int)teamParticipant.Id,
                Name = teamUpsert.Name,
                Players = new List<ApplicationUser> { user },
                //TournamentId = teamUpsert.TournamentId,
            };

            tournament.Teams.Add(team);
            await _dbContext.SaveChangesAsync();


        }

        public async Task JoinTournamentExistingTeam(int teamId, string userId)
        {
            Team team = await _dbContext.Teams
                .Include(t => t.Tournament)
                .ThenInclude(tour => tour.Event)
                .ThenInclude(e => e.Players)
                .FirstOrDefaultAsync(t => t.Id == teamId);


            //Check if team exist
            if (team == null)
                throw new TeamNotFoundException();


            //Check if user is already in a team
            bool userAlreadyInTeam = team.Tournament.Teams
                .SelectMany(t => t.Players)
                .Any(player => player.Id == userId);

            if (userAlreadyInTeam)
                throw new UserAlreadyJoinedTournamentException();

            //Check if tournament team limit is reached
            if (team.Players.Count == team.Tournament.MaxPlayersPerTeam)
                throw new TeamPlayersLimitReachedException();


            //Check if user is register to the event
            ApplicationUser user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == userId);

            var isUserInEvent = team.Tournament.Event.Players.Any(p => p.Id == userId);
            if (!isUserInEvent)
            {

                team.Tournament.Event.Players.Add(user);
            }


            team.Players.Add(user);

            await _dbContext.SaveChangesAsync();

        }

        public async Task QuitTournament(string userId, int tournamentId)
        {
            var tournamentToQuit = await _dbContext.Tournaments
                .Include(t => t.Teams)
                .ThenInclude(team => team.Players)
                //.Include(t => t.Teams).ThenInclude(team => team.Matches_Teams)
                .FirstOrDefaultAsync(tour => tour.Id == tournamentId);

            if (tournamentToQuit == null)
                throw new TournamentNotFoundException();

            var userTeam = tournamentToQuit.Teams
                .Where(team => team.Players.Any(p => p.Id == userId))
                .FirstOrDefault();

            if (userTeam == null)
                throw new UserNotInTournamentException();




            if (userTeam.Players.Count == 1)
            {
                //Remove tout les match_teams (matches relations) de la team
                //userTeam.Matches_Teams.RemoveAll(m => m != null);


                challongeTournament tournament = await _client.GetTournamentByIdAsync(tournamentId);

                var participant = await _client.GetParticipantAsync(tournament, userTeam.Id);

                await _client.DeleteParticipantAsync(participant);

                tournamentToQuit.Teams.Remove(userTeam);
            }
            else
            {
                ApplicationUser user = userTeam.Players.FirstOrDefault(p => p.Id == userId);

                userTeam.Players.Remove(user);
            }



            await _dbContext.SaveChangesAsync();

        }

        public async Task DeleteTournament(int tournamentId)
        {

            var tournamentToDelete = await _dbContext.Tournaments.FindAsync(tournamentId);

            if (tournamentToDelete == null)
                throw new TournamentNotFoundException();

            var challengeTournament = await _client.GetTournamentByIdAsync(tournamentToDelete.Id);

            await _client.DeleteTournamentAsync(challengeTournament);

            _dbContext.Tournaments.Remove(tournamentToDelete);
            await _dbContext.SaveChangesAsync();
        }

      
    }
}
