using LANPartyAPI_Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LANPartyAPI_Services.DTOs
{
    public class TournamentUpsertDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = ExceptionErrorMessages.TournamentExceptions.TournamentNameRequired)]
        [MaxLength(250, ErrorMessage = ExceptionErrorMessages.TournamentExceptions.TournamentNameTooLong)]
        public string Name { get; set; }

        [MaxLength(1000, ErrorMessage = ExceptionErrorMessages.TournamentExceptions.TournamentDescriptionTooLong)]
        public string Description { get; set; }

        [Required(ErrorMessage = ExceptionErrorMessages.TournamentExceptions.TournamentGameRequired)]
        [MaxLength(250, ErrorMessage = ExceptionErrorMessages.TournamentExceptions.TournamentGameTooLong)]
        public string Game { get; set; }

        [Range(1, 5, ErrorMessage = ExceptionErrorMessages.TournamentExceptions.TournamentPlayersPerTeamNumberNotInRange)]
        public int? PlayersPerTeamNumber { get; set; }

        [Range(1, 100, ErrorMessage = ExceptionErrorMessages.TournamentExceptions.TournamentMaxTeamNumberNotInRange)]
        public int? MaxTeamNumber { get; set; }

        [Required(ErrorMessage = ExceptionErrorMessages.TournamentExceptions.TournamentEliminationModeRequired)]
        [Range(0, 2, ErrorMessage = ExceptionErrorMessages.TournamentExceptions.TournamentEliminationModeNotInRange)]
        public EliminationTypes? EliminationMode { get; set; }

        [Required(ErrorMessage = ExceptionErrorMessages.TournamentExceptions.TournamentEventIdRequired)]
        public int? EventId { get; set; }
    }

    public class TournamentResponseDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Game { get; set; }

        public int? PlayersPerTeamNumber { get; set; }

        public int? MaxTeamNumber { get; set; }

        public EliminationTypes EliminationMode { get; set; }

        public int EventId { get; set; }

        public bool hasJoined { get; set; }
    }
}
