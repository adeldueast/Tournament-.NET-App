using LANPartyAPI_Core.Enums;
using LANPartyAPI_Core.Exceptions;
using LANPartyAPI_DataAccess.Data;
using LANPartyAPI_Services.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LANPartyAPI_Services
{
	public class TeamService
	{
		private readonly ApplicationDbContext _dbContext;
		public TeamService(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		//public List<TeamResponseDTO> Teams { get; set; }

		/// <exception cref="TournamentNotFoundException"></exception>
		public async Task<object> GetAllTeams(int tournamentId)
		{

			var tournament = await _dbContext.Tournaments
				.AsNoTracking()
				.Include(t => t.Teams)
				.ThenInclude(team=>team.Players)
				.FirstOrDefaultAsync(t => t.Id == tournamentId);

			if (tournament == null)
			{
				throw new TournamentNotFoundException();
			}


			var teams = tournament.Teams.Select(t => new TeamMatchScoreResponseDTO
			{
				Id = t.Id,
				Name = t.Name,
				TournamentId = t.TournamentId,
				Players = t.Players.Select(p=> new ApplicationUserDTO
				{
					Id = p.Id,
					FirstName  = p.FirstName,
					LastName = p.LastName,
					GamerTag = p.UserName,

				}).ToList(),
				isFull = t.Players.Count == t.Tournament.MaxPlayersPerTeam
				
			}).ToList();

		  
			return teams;
		}
	}
}
