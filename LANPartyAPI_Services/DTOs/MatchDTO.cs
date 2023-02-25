
using LANPartyAPI_Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace LANPartyAPI_Services.DTOs
{
    public class MatchUpsertDTO
    {
        public int Id { get; set; }

        public DateTime? Date { get; set; }

        [Required(ErrorMessage = ExceptionErrorMessages.MatchExceptions.MatchRoundRequired)]
        [Range(1, int.MaxValue, ErrorMessage = ExceptionErrorMessages.MatchExceptions.MatchRoundInvalid)]
        public int? Round { get; set; }

        [Required(ErrorMessage = ExceptionErrorMessages.MatchExceptions.MatchNumberRequired)]
        [Range(1, int.MaxValue, ErrorMessage = ExceptionErrorMessages.MatchExceptions.MatchNumberInvalid)]
        public int? MatchNumber { get; set; }

        [Required(ErrorMessage = ExceptionErrorMessages.MatchExceptions.MatchTournamentIdRequired)]
        public int? TournamentId { get; set; }

        public List<int> TeamIds { get; set; } = new();
    }

    public class MatchResponseDTO
    {
        public int Id { get; set; }

        public DateTime? Date { get; set; }

        public int Round { get; set; }

        public int MatchNumber { get; set; }

        public int TournamentId { get; set; }

		public string TournamentName { get; set; }

        public List<TeamMatchScoreResponseDTO> Teams { get; set; }

    }

    public class MatchTeamsResultsDTO
    {
        public int MatchId { get; set; }

        public List<TeamsResultsDTO> Results { get; set; } = new List<TeamsResultsDTO>();
    }

    public class TeamsResultsDTO
    {

        public int TeamId { get; set; }

        [MaxLength(10, ErrorMessage = ExceptionErrorMessages.MatchExceptions.MatchResultInvalidScore)]
        public string Score { get; set; }

        public bool IsWinner { get; set; }

        public string Name { get; set; }
    }

}
