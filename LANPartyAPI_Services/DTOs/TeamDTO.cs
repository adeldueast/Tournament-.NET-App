using LANPartyAPI_Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LANPartyAPI_Services.DTOs
{
    public class JoinTournamentTeamDTO
    {
        public int TournamentId { get; set; }
    }

    public class TeamUpsertDTO
    {
        [Required(ErrorMessage = ExceptionErrorMessages.TeamExceptions.TeamNameRequired)]
        [MaxLength(50, ErrorMessage = ExceptionErrorMessages.TeamExceptions.TeamNameTooLong)]
        public string Name { get; set; }

        public int TournamentId { get; set; }
    }

    public class TeamMatchScoreResponseDTO
    {
        public int Id { get; set; }

        public int TournamentId { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// This property is only used for web(admin) when upserting a match, indicate if the team is part of the match
        /// </summary>
        public bool isInMatch { get; set; }

        public string Score { get; set; }

        public bool isWinner { get; set; }

        public List<ApplicationUserDTO> Players { get; set; }

        public bool isFull { get; set; }
    }
}
