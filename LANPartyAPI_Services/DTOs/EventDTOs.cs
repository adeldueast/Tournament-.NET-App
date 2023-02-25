using LANPartyAPI_Core.Enums;

using System.ComponentModel.DataAnnotations;


namespace LANPartyAPI_Services.DTOs
{
    public class EventUpsertDTO
    {

        public int Id { get; set; }

        [Required(ErrorMessage = $"{ExceptionErrorMessages.EventExceptions.EventNameRequired}")]
        [MaxLength(2500,ErrorMessage =ExceptionErrorMessages.EventExceptions.EventNameTooLong)]
        public string Name { get; set; }

        [Required(ErrorMessage = $"{ExceptionErrorMessages.EventExceptions.EventStartDateRequired}")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = $"{ExceptionErrorMessages.EventExceptions.EventEndDateRequired}")]
        public DateTime? EndDate { get; set; }

        [Range(1, 9999, ErrorMessage = $"{ExceptionErrorMessages.EventExceptions.EventMaxPlayersNumberNotInRange}")]
        public int? MaxPlayerNumber { get; set; }

        [MaxLength(1000, ErrorMessage = ExceptionErrorMessages.EventExceptions.EventDescriptionTooLong)]
        public string Description { get; set; }
    }
  
    public class EventResponseDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int? MaxPlayerNumber { get; set; }

		public bool Joined { get; set; } 

		public string Description { get; set; }


    }
}
