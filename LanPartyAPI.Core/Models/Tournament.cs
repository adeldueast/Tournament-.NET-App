using LANPartyAPI_Core.Enums;
using System.ComponentModel.DataAnnotations;


namespace LANPartyAPI_Core.Models
{
    public class Tournament
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(250, ErrorMessage = ExceptionErrorMessages.TournamentExceptions.TournamentNameTooLong)]
        public string Name { get; set; }

        [MaxLength(1000, ErrorMessage = ExceptionErrorMessages.TournamentExceptions.TournamentDescriptionTooLong)]
        public string Description { get; set; }

        [Required]
        [MaxLength(250, ErrorMessage = ExceptionErrorMessages.TournamentExceptions.TournamentGameTooLong)]
        public string Game { get; set; }

        [Required]
        [Range(1, 5)]
        public int? PlayersPerTeamNumber { get; set; }

        [Required]
        [Range(1, 100)]
        public int? MaxTeamNumber { get; set; }

        public bool hasStarted { get; set; }

        public List<Match> Matches { get; set; }

        public List<Team> Teams { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }

        [Required]
        public EliminationTypes EliminationMode { get; set; }
    }
}
