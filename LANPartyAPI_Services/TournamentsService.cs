using LANPartyAPI_Core.Enums;
using LANPartyAPI_Core.Exceptions;
using LANPartyAPI_Core.Models;
using LANPartyAPI_DataAccess.Data;
using LANPartyAPI_Services.DTOs;
using Microsoft.EntityFrameworkCore;

namespace LANPartyAPI_Services
{
    public interface ITournamentsService
    {
        public Task<List<TournamentResponseDTO>> GetEventTournaments(int eventId, string userId);

        public Task<List<TournamentResponseDTO>> GetAllTournaments(string userId);

        public Task<TournamentResponseDTO> GetTournament(int id, string userId);

        public Task<TournamentResponseDTO> Upsert(TournamentUpsertDTO tournamentToUpsert);


        /// <exception cref="TournamentNotFoundException"></exception>
        /// <exception cref="TeamLimitReachedException"></exception>
        /// <exception cref="UserNotInEventException"></exception>
        /// <exception cref="UserAlreadyJoinedTournamentException"></exception>
        /// <exception cref="TeamNameAlreadyTakenException"></exception>
        public Task JoinTournamentCreateTeam(TeamUpsertDTO teamUpsert, string userId);


        /// <exception cref="TeamNotFoundException"></exception>
        /// <exception cref="UserNotInEventException"></exception>
        /// <exception cref="UserAlreadyJoinedTournamentException"></exception>
        /// <exception cref="TeamPlayersLimitReachedException"></exception>
        public Task JoinTournamentExistingTeam(int teamId, string userId);


        /// <exception cref="TournamentNotFoundException"></exception>
        /// <exception cref="UserNotInTournamentException"></exception>
        public Task QuitTournament(string userId, int tournamentId);
    }
    public class TournamentsService : ITournamentsService
    {

        private readonly ApplicationDbContext _dbContext;

        public TournamentsService(ApplicationDbContext dbContext) => _dbContext = dbContext;

        /// <exception cref="EventNotFoundException"></exception>
        public async Task<List<TournamentResponseDTO>> GetEventTournaments(int eventId, string userId)
        {
            var e = await _dbContext.Events
                .AsNoTracking()
                .Include(e => e.Tournaments.OrderBy(t => t.Name))
                .ThenInclude(t => t.Teams)
                .ThenInclude(t => t.Players)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (e == null)
            {
                throw new EventNotFoundException();
            }


            List<TournamentResponseDTO> tournamentsDtos = e.Tournaments.Select(t => new TournamentResponseDTO
            {
                Id = t.Id,
                Name = t.Name,
                Game = t.Game,
                MaxTeamNumber = t.MaxTeamNumber,
                PlayersPerTeamNumber = t.PlayersPerTeamNumber,
                Description = t.Description,
                EliminationMode = t.EliminationMode,
                EventId = t.EventId,
                hasJoined = string.IsNullOrEmpty(userId) ? false : t.Teams.SelectMany(t => t.Players).Select(p => p.Id).Any(p => p == userId)

            }).ToList();


            return tournamentsDtos;
        }

        public async Task<List<TournamentResponseDTO>> GetAllTournaments(string userId)
        {
            var tournaments = await _dbContext.Tournaments
                .OrderBy(t => t.Name)
                .Include(t => t.Teams)
                .ThenInclude(t => t.Players)
                .AsNoTracking()
                .ToListAsync();

            List<TournamentResponseDTO> tournamentsDtos = tournaments.Select(t => new TournamentResponseDTO
            {
                Id = t.Id,
                Name = t.Name,
                Game = t.Game,
                MaxTeamNumber = t.MaxTeamNumber,
                PlayersPerTeamNumber = t.PlayersPerTeamNumber,
                Description = t.Description,
                EliminationMode = t.EliminationMode,
                EventId = t.EventId,
                hasJoined = string.IsNullOrEmpty(userId) ? false : t.Teams.SelectMany(t => t.Players).Select(p => p.Id).Any(p => p == userId)

            }).ToList();


            return tournamentsDtos;
        }

        /// <exception cref="TournamentNotFoundException"></exception>
        public async Task<TournamentResponseDTO> GetTournament(int id, string userId)
        {
            var existingTournament = await _dbContext.Tournaments
                .AsNoTracking()
                .Include(t => t.Teams)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (existingTournament == null)
            {
                throw new TournamentNotFoundException();
            }

            TournamentResponseDTO existingTournamentDto = new TournamentResponseDTO
            {
                Id = existingTournament.Id,
                Name = existingTournament.Name,
                Game = existingTournament.Game,
                MaxTeamNumber = existingTournament.MaxTeamNumber,
                PlayersPerTeamNumber = existingTournament.PlayersPerTeamNumber,
                Description = existingTournament.Description,
                EliminationMode = existingTournament.EliminationMode,
                EventId = existingTournament.EventId,
                hasJoined = string.IsNullOrEmpty(userId) ? false : existingTournament.Teams.SelectMany(t => t.Players).Select(p => p.Id).Any(p => p == userId)

            };

            return existingTournamentDto;
        }

        /// <exception cref="TournamentNameTakenException"></exception>
        /// <exception cref="EventNotFoundException"></exception>
        /// <exception cref="TournamentNotFoundException"></exception>
        public async Task<TournamentResponseDTO> Upsert(TournamentUpsertDTO tournamentToUpsert)
        {

            //TODO: Check if name condition is only within the same Event or no duplicate at ALL??
            var tournamentAlreadyExist = tournamentToUpsert.Id == 0
                ? await _dbContext.Tournaments.AnyAsync(t => t.Name == tournamentToUpsert.Name && t.EventId == tournamentToUpsert.EventId)
                : await _dbContext.Tournaments.AnyAsync(t => t.Name == tournamentToUpsert.Name && t.EventId == tournamentToUpsert.EventId && t.Id != tournamentToUpsert.Id);

            if (tournamentAlreadyExist)
            {
                throw new TournamentNameTakenException();
            }

            Tournament upsertTournamentDomain;
            //Create
            if (tournamentToUpsert.Id == 0)
            {
                var eventExist = await _dbContext.Events.AnyAsync(t => t.Id == tournamentToUpsert.EventId);
                if (!eventExist)
                {
                    throw new EventNotFoundException();
                }

                upsertTournamentDomain = new Tournament()
                {
                    Name = tournamentToUpsert.Name,
                    Game = tournamentToUpsert.Game,
                    MaxTeamNumber = tournamentToUpsert.MaxTeamNumber,
                    PlayersPerTeamNumber = tournamentToUpsert.PlayersPerTeamNumber,
                    Description = tournamentToUpsert.Description,
                    EliminationMode = (EliminationTypes)tournamentToUpsert.EliminationMode,
                    EventId = (int)tournamentToUpsert.EventId
                };

                _dbContext.Tournaments.Add(upsertTournamentDomain);
            }
            else
            {
                //Update
                upsertTournamentDomain = await _dbContext.Tournaments.FindAsync(tournamentToUpsert.Id);

                if (upsertTournamentDomain == null)
                {
                    throw new TournamentNotFoundException();
                }

                upsertTournamentDomain.Name = tournamentToUpsert.Name;
                upsertTournamentDomain.Game = tournamentToUpsert.Game;
                upsertTournamentDomain.MaxTeamNumber = tournamentToUpsert.MaxTeamNumber;
                upsertTournamentDomain.PlayersPerTeamNumber = tournamentToUpsert.PlayersPerTeamNumber;
                upsertTournamentDomain.Description = tournamentToUpsert.Description;
                upsertTournamentDomain.EliminationMode = (EliminationTypes)tournamentToUpsert.EliminationMode;

            }

            await _dbContext.SaveChangesAsync();

            TournamentResponseDTO responseDTO = new TournamentResponseDTO()
            {
                Id = upsertTournamentDomain.Id,
                Name = upsertTournamentDomain.Name,
                Game = upsertTournamentDomain.Game,
                MaxTeamNumber = upsertTournamentDomain.MaxTeamNumber,
                PlayersPerTeamNumber = upsertTournamentDomain.PlayersPerTeamNumber,
                Description = upsertTournamentDomain.Description,
                EliminationMode = upsertTournamentDomain.EliminationMode,
                EventId = upsertTournamentDomain.EventId
            };

            return responseDTO;
        }



        /// <exception cref="TournamentNotFoundException"></exception>
        /// <exception cref="TeamLimitReachedException"></exception>
        /// <exception cref="UserNotInEventException"></exception>
        /// <exception cref="UserAlreadyJoinedTournamentException"></exception>
        /// <exception cref="TeamNameAlreadyTakenException"></exception>
        public async Task JoinTournamentCreateTeam(TeamUpsertDTO teamUpsert, string userId)
        {

            Tournament tournament = await _dbContext.Tournaments
                .Include(t => t.Teams.Where(t => t.TournamentId == teamUpsert.TournamentId))
                .ThenInclude(team => team.Players)
                .FirstOrDefaultAsync(t => t.Id == teamUpsert.TournamentId);

            //Check if tournament exist
            if (tournament == null)
                throw new TournamentNotFoundException();


            //Check if tournament team limit is reached
            if (tournament.Teams.Count == tournament.MaxTeamNumber)
                throw new TeamLimitReachedException();


            //Check if user is register to the event
            ApplicationUser user = await _dbContext.Users
                .FirstOrDefaultAsync(user => user.Id == userId && user.Events.Any(e => e.Id == tournament.EventId));

            if (user == null)
                throw new UserNotInEventException();


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


            Team team = new Team()
            {
                Name = teamUpsert.Name,
                Players = new List<ApplicationUser> { user },
                TournamentId = teamUpsert.TournamentId,
            };

            tournament.Teams.Add(team);

            await _dbContext.SaveChangesAsync();

        }


        /// <exception cref="TeamNotFoundException"></exception>
        /// <exception cref="UserNotInEventException"></exception>
        /// <exception cref="UserAlreadyJoinedTournamentException"></exception>
        /// <exception cref="TeamPlayersLimitReachedException"></exception>
        public async Task JoinTournamentExistingTeam(int teamId, string userId)
        {
            Team team = await _dbContext.Teams
                .Include(t => t.Tournament)
                .FirstOrDefaultAsync(t => t.Id == teamId);


            //Check if team exist
            if (team == null)
                throw new TeamNotFoundException();

            //Check if user is register to the event
            ApplicationUser user = await _dbContext.Users
                .FirstOrDefaultAsync(user => user.Id == userId && user.Events.Any(e => e.Id == team.Tournament.EventId));

            if (user == null)
                throw new UserNotInEventException();

            //Check if user is already in a team
            bool userAlreadyInTeam = team.Tournament.Teams
                .SelectMany(t => t.Players)
                .Any(player => player.Id == userId);

            if (userAlreadyInTeam)
                throw new UserAlreadyJoinedTournamentException();

            //Check if tournament team limit is reached
            if (team.Players.Count == team.Tournament.PlayersPerTeamNumber)
                throw new TeamPlayersLimitReachedException();

            team.Players.Add(user);

            await _dbContext.SaveChangesAsync();

        }

        /// <exception cref="TournamentNotFoundException"></exception>
        /// <exception cref="UserNotInTournamentException"></exception>
        public async Task QuitTournament(string userId, int tournamentId)
        {
            Tournament tournamentToQuit = await _dbContext.Tournaments
                .Include(t => t.Teams).ThenInclude(team => team.Players)
                .Include(t => t.Teams).ThenInclude(team => team.Matches_Teams)
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
                userTeam.Matches_Teams.RemoveAll(m => m != null);
                tournamentToQuit.Teams.Remove(userTeam);
            }
            else
            {
                ApplicationUser user = userTeam.Players.FirstOrDefault(p => p.Id == userId);

                userTeam.Players.Remove(user);
            }



            await _dbContext.SaveChangesAsync();

        }
    }
}
