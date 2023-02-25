using LANPartyAPI_Core.Enums;
using LANPartyAPI_Core.Exceptions;
using LANPartyAPI_Core.Models;
using LANPartyAPI_DataAccess.Data;
using LANPartyAPI_Services.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace LANPartyAPI_Services
{
	public class MatchService : IMatchService
	{

		private readonly ApplicationDbContext _dbContext;

		public MatchService(ApplicationDbContext dbContext) => _dbContext = dbContext;

		/// <exception cref="TournamentNotFoundException"></exception>
		public async Task<List<MatchResponseDTO>> GetAllMatches(int tournamentId)
		{
			var tournament = await _dbContext.Tournaments
				.AsNoTracking()
				.Include(t => t.Matches)
				.ThenInclude(m => m.Matches_Teams)
				.ThenInclude(mt => mt.Team)
				.ThenInclude(t => t.Players)
				.FirstOrDefaultAsync(t => t.Id == tournamentId);

			if (tournament == null)
				throw new TournamentNotFoundException();

			List<MatchResponseDTO> listMatchDTO = new();

			foreach (var match in tournament.Matches)
			{
				MatchResponseDTO matchDTO = new MatchResponseDTO()
				{
					Id = match.Id,
					Date = match.Date,
					Round = match.Round,
					MatchNumber = match.MatchNumber,
					TournamentId = match.TournamentId,
					Teams = match.Matches_Teams
						.Select(m_t => new { m_t.Team, isWinner = m_t.IsWinner, Score = m_t.Score })
						.Select(t => new TeamMatchScoreResponseDTO()
						{
							Id = t.Team.Id,
							TournamentId = t.Team.TournamentId,
							Score = t.Score,
							isWinner = t.isWinner,
							Name = t.Team.Name,
							Players = t.Team.Players.Select(player => new ApplicationUserDTO()
							{
								Id = player.Id,
								FirstName = player.FirstName,
								LastName = player.LastName,
								GamerTag = player.UserName
							}).ToList()

						}).ToList()

				};

				listMatchDTO.Add(matchDTO);
			}

			return listMatchDTO;
		}

		/// <exception cref="MatchNotFoundException"></exception>
		public async Task<MatchResponseDTO> GetMatch(int matchId)
		{
			var match = await _dbContext.Matches
				.AsNoTracking()
				.Include(m => m.Tournament)
				.ThenInclude(t => t.Teams)
				.Include(m => m.Matches_Teams)
				.ThenInclude(mt => mt.Team)
				.ThenInclude(t => t.Players)
				.FirstOrDefaultAsync(m => m.Id == matchId);

			if (match == null)
				throw new MatchNotFoundException();

			var matchTeamsIds = match.Matches_Teams.Select(mt => mt.TeamId);
			var teams = match.Tournament.Teams.Select(t => new TeamMatchScoreResponseDTO
			{
				Id = t.Id,
				Name = t.Name,
				TournamentId = t.TournamentId,
				isInMatch = matchTeamsIds.Contains(t.Id),
				Players = t.Players.Select(p => new ApplicationUserDTO
				{
					Id = p.Id,
					FirstName = p.FirstName,
					LastName = p.LastName,
					GamerTag = p.UserName,

				}).ToList()

			}).ToList();

			MatchResponseDTO matchDTO = new MatchResponseDTO()
			{
				Id = match.Id,
				Date = match.Date,
				Round = match.Round,
				MatchNumber = match.MatchNumber,
				TournamentId = match.TournamentId,
				Teams = teams,
				//TODO: Retrieve all tournament's teams but differenciate match's teams and others 
				//Teams = match.Matches_Teams.Select(m_t => m_t.Team).Select(t => new TeamResponseDTO()
				//{
				//    Id = t.Id,
				//    TournamentId = t.TournamentId,
				//    Players = t.Players.Select(player => new ApplicationUserDTO()
				//    {
				//        Id = player.Id,
				//        FirstName = player.FirstName,
				//        LastName = player.LastName,
				//        GamerTag = player.UserName
				//    }).ToList()

				//}).ToList()


			};


			return matchDTO;
		}

		/// <exception cref="TournamentNotFoundException"></exception>
		/// <exception cref="TeamNotFoundException"></exception>
		/// <exception cref="TeamAlreadySetInRoundException"></exception>
		/// <exception cref="MatchInvalidStartDateException"></exception>
		/// <exception cref="MatchAlreadyExistException"></exception>
		/// <exception cref="MatchNotFoundException"></exception>
		public async Task<MatchResponseDTO> UpsertMatch(MatchUpsertDTO matchToUpsertDto)
		{
			//Check if tournament exist
			Tournament existingTournament = await _dbContext.Tournaments
					.Include(t => t.Event)
					.Include(t => t.Matches)
					.ThenInclude(m => m.Matches_Teams)
					.Include(t => t.Teams)
					.ThenInclude(team => team.Players)
					.FirstOrDefaultAsync(t => t.Id == matchToUpsertDto.TournamentId);

			if (existingTournament == null)
			{
				throw new TournamentNotFoundException();
			}

			//Check if provided team Ids are all valid
			var teamIds = matchToUpsertDto.TeamIds.Distinct().ToArray();
			var tournamentTeamsIds = existingTournament.Teams.Select(t => t.Id).ToArray();
			bool isSubset = teamIds.All(elem => tournamentTeamsIds.Contains(elem));
			if (!isSubset)
				throw new TeamNotFoundException();


			//Check if provided teams already exist in a match for the same provided match.Round 
			var matches = existingTournament.Matches.Where(m => m.Round == matchToUpsertDto.Round && m.Id != matchToUpsertDto.Id).ToList();
			foreach (var match in matches)
			{
				var teamAlreadySet = match.Matches_Teams.Any(mt => teamIds.Contains(mt.TeamId));
				if (teamAlreadySet)
				{
					//TODO add error devops
					throw new TeamAlreadySetInRoundException();
				}

			}

			//Check if match date is BEFORE || AFTER event start/end date .. then throw exception
			if (matchToUpsertDto.Date != null)
			{
				if (DateTime.Compare((DateTime)matchToUpsertDto.Date, (DateTime)existingTournament.Event.StartDate) < 0 ||
					DateTime.Compare((DateTime)matchToUpsertDto.Date, (DateTime)existingTournament.Event.EndDate) > 0)
				{
					throw new MatchInvalidStartDateException();
				}

			}

			//Check Round and MatchNumber
			//Cannot upsert a match if an existing match with the same MatchNumber exist in a (Any)Round;
			var matchExist = matchToUpsertDto.Id == 0
				? existingTournament.Matches.Any(m => m.Round == matchToUpsertDto.Round && m.MatchNumber == matchToUpsertDto.MatchNumber)
				: existingTournament.Matches.Any(m => m.Round == matchToUpsertDto.Round && m.MatchNumber == matchToUpsertDto.MatchNumber && m.Id != matchToUpsertDto.Id);

			if (matchExist)
				throw new MatchAlreadyExistException();


			Match matchToUpsertDomain;

			//Create match
			if (matchToUpsertDto.Id == 0)
			{
				matchToUpsertDomain = new Match()
				{
					Date = matchToUpsertDto.Date,
					Round = matchToUpsertDto.Round.Value,
					MatchNumber = matchToUpsertDto.MatchNumber.Value,
					TournamentId = (int)matchToUpsertDto.TournamentId,
					Matches_Teams = teamIds.Select(teamId => new Match_Team { TeamId = teamId }).ToList()
			};

				//Save entity...
				_dbContext.Matches.Add(matchToUpsertDomain);
			}
			else
			{

				//Update, so check if the entity exist in tournament.matches loaded in memory (used Include)
				matchToUpsertDomain = existingTournament.Matches
					.FirstOrDefault(m => m.Id == matchToUpsertDto.Id);

				if (matchToUpsertDomain == null)
				{
					throw new MatchNotFoundException();
				}

				//TODO  Check unlink relations breaks something (ex clearing all seats will remove all relation seat-player )
				foreach (var match_team in matchToUpsertDomain.Matches_Teams.ToList())
				{
					matchToUpsertDomain.Matches_Teams.Remove(match_team);
				}

				matchToUpsertDomain.Date = matchToUpsertDto.Date;
				matchToUpsertDomain.Round = matchToUpsertDto.Round.Value;
				matchToUpsertDomain.MatchNumber = matchToUpsertDto.MatchNumber.Value;
				//matchToUpsertDomain.TournamentId = matchToUpsertDto.TournamentId;
				matchToUpsertDomain.Matches_Teams = teamIds.Select(teamId => new Match_Team { TeamId = teamId }).ToList();

				_dbContext.Matches.Update(matchToUpsertDomain);
			}


			await _dbContext.SaveChangesAsync();


			MatchResponseDTO matchDto = new MatchResponseDTO()
			{
				Id = matchToUpsertDomain.Id,
				Date = matchToUpsertDomain.Date,
				MatchNumber = matchToUpsertDomain.MatchNumber,
				Round = matchToUpsertDomain.Round,
				TournamentId = matchToUpsertDomain.TournamentId,
				Teams = existingTournament.Teams.Where(t => teamIds.Contains(t.Id)).Select(t => new TeamMatchScoreResponseDTO()
				{
					Id = t.Id,
					TournamentId = t.TournamentId,
					Players = t.Players.Select(player => new ApplicationUserDTO()
					{
						Id = player.Id,
						FirstName = player.FirstName,
						LastName = player.LastName,
						GamerTag = player.UserName
					}).ToList()

				}).ToList()

			};

			return matchDto;
		}

		/// <exception cref="MatchNotFoundException"></exception>
		public async Task<MatchTeamsResultsDTO> GetMatchTeams(int matchID)
		{
			Match match = await _dbContext.Matches.
				Include(m => m.Matches_Teams).
				ThenInclude(mt => mt.Team).
				FirstOrDefaultAsync(m => m.Id == matchID);
			if (match == null)
			{
				throw new MatchNotFoundException();
			}

			MatchTeamsResultsDTO TM = new MatchTeamsResultsDTO();
			TM.MatchId = matchID;


			foreach (Match_Team e in match.Matches_Teams)
			{
				var Result = new TeamsResultsDTO()
				{
					TeamId = e.TeamId,
					Score = e.Score,
					IsWinner = e.IsWinner,
					Name = e.Team.Name

				};

				TM.Results.Add(Result);
			};
			return TM;
		}

		/// <exception cref="MatchNotFoundException"></exception>
		/// <exception cref="TeamNotFoundException"></exception>
		public async Task RegisterScore(MatchTeamsResultsDTO matchTeamsResultsDTO)
		{
			Match m = await _dbContext.Matches.FindAsync(matchTeamsResultsDTO.MatchId);
			if (m == null)
			{
				throw new MatchNotFoundException();

			}


			foreach (var resultDTO in matchTeamsResultsDTO.Results)
			{
				Team team = await _dbContext.Teams
					.Include(m => m.Matches_Teams)
					.FirstOrDefaultAsync(m => m.Id == resultDTO.TeamId);

				if (team == null)
				{
					throw new TeamNotFoundException();
				}

				foreach (var match in team.Matches_Teams)
				{
					if (match.MatchId == matchTeamsResultsDTO.MatchId)
					{
						match.Score = resultDTO.Score;
						match.IsWinner = resultDTO.IsWinner;
					}
					await _dbContext.SaveChangesAsync();

				}

			};


		}

		public async Task<List<MatchResponseDTO>> GetUserMatches(string userID)
		{
			var player = await _dbContext.Users.FindAsync(userID);
            if (player == null)
            {
                throw new UserNotFoundException();
            }

    //        var matches = await _dbContext.Matches
				//.AsNoTracking()
				//.Include(m=>m.Matches_Teams)
				//.ThenInclude(mt=>mt.Team)
				//.ThenInclude(p=>p.Players.Contains(player))
				//.ToListAsync();

			var matches_Teams = await _dbContext.Matches_Teams
				.Include(mT => mT.Match)
				.ThenInclude(m => m.Tournament)
				.Include(mT => mT.Team)
				.ThenInclude(t => t.Players)
				.ToListAsync();

			List<Match_Team> userMatchTeams = new();

			foreach(var match_team in matches_Teams)
			{
				if (match_team.Team.Players.Contains(player))
				{
					userMatchTeams.Add(match_team);
				}
			}

            List<MatchResponseDTO> listMatchDTO = new();

            foreach (var match in userMatchTeams)
            {
                MatchResponseDTO matchDTO = new MatchResponseDTO()
                {
                    Id = match.MatchId,
                    Date = match.Match.Date,
                    Round = match.Match.Round,
                    MatchNumber = match.Match.MatchNumber,
                    TournamentId = match.Match.TournamentId,
					TournamentName = match.Match.Tournament.Name,
                    Teams = userMatchTeams
                        .Select(m_t => new { m_t.Team, isWinner = m_t.IsWinner, Score = m_t.Score })
                        .Select(t => new TeamMatchScoreResponseDTO()
                        {
                            Id = t.Team.Id,
                            TournamentId = t.Team.TournamentId,
                            Score = t.Score,
                            isWinner = t.isWinner,
                            Name = t.Team.Name,
                            Players = t.Team.Players.Select(player => new ApplicationUserDTO()
                            {
                                Id = player.Id,
                                FirstName = player.FirstName,
                                LastName = player.LastName,
                                GamerTag = player.UserName
                            }).ToList()

                        }).ToList()

                };

                listMatchDTO.Add(matchDTO);
            }

            return listMatchDTO;
		}

	}

	public interface IMatchService
	{
		public Task<List<MatchResponseDTO>> GetAllMatches(int tournamentId);

		public Task<MatchResponseDTO> GetMatch(int matchId);

		public Task<MatchResponseDTO> UpsertMatch(MatchUpsertDTO matchToUpsertDto);

		public Task<MatchTeamsResultsDTO> GetMatchTeams(int matchID);

		public Task RegisterScore(MatchTeamsResultsDTO matchTeamsResultsDTO);

        public Task<List<MatchResponseDTO>> GetUserMatches(string userID);
    }
}
