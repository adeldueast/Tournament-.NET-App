using Challonge.Objects;
using LANPartyAPI_Core.Enums;
using System.ComponentModel.DataAnnotations;
using TournamentType = LANPartyAPI_Core.Enums.TournamentType;

namespace LANPartyAPI_Services.DTOs
{
    public class TournamentUpsertDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = ExceptionErrorMessages.TournamentExceptions.TournamentNameRequired)]
        [MaxLength(60, ErrorMessage = ExceptionErrorMessages.TournamentExceptions.TournamentNameTooLong)]
        public string Name { get; set; }

        [MaxLength(1000, ErrorMessage = ExceptionErrorMessages.TournamentExceptions.TournamentDescriptionTooLong)]
        public string Description { get; set; }

        [Required(ErrorMessage = ExceptionErrorMessages.TournamentExceptions.TournamentGameRequired)]
        [MaxLength(100, ErrorMessage = ExceptionErrorMessages.TournamentExceptions.TournamentGameTooLong)]
        public string Game { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = ExceptionErrorMessages.TournamentExceptions.TournamentPlayersPerTeamNumberNotInRange)]
        public int? MaxPlayersPerTeam { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = ExceptionErrorMessages.TournamentExceptions.TournamentMaxTeamNumberNotInRange)]
        public int? MaxTeamNumber { get; set; }

        [Required(ErrorMessage = ExceptionErrorMessages.TournamentExceptions.TournamentEliminationModeRequired)]
        public TournamentType? TournamentType { get; set; }

        [Required(ErrorMessage = ExceptionErrorMessages.TournamentExceptions.TournamentEventIdRequired)]
        public int? EventId { get; set; }
    }

    public class TournamentResponseDTO
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Game { get; set; }

        public int MaxPlayersPerTeam { get; set; }

        public int MaxTeamNumber { get; set; }

        public TournamentType TournamentType { get; set; }

        public int EventId { get; set; }

        public bool hasJoined { get; set; }
    }
}
