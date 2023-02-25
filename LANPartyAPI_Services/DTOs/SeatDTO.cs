using LANPartyAPI_Core.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LANPartyAPI_Services.DTOs
{
    public class SeatUpsertDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = ExceptionErrorMessages.SeatExceptions.SeatPrefixRequired)]
        public string Prefix { get; set; }

        [Required(ErrorMessage = ExceptionErrorMessages.SeatExceptions.SeatPositionRequired)]
        public int? Position { get; set; }

    }

    public class SeatsUpsertDTO
    {

        
        public IFormFile? File { get; set; }

        [Required(ErrorMessage = ExceptionErrorMessages.SeatExceptions.SeatEventIdRequired)]
        public int? EventId { get; set; }

        public List<SeatUpsertDTO> Seats { get; set; } = new();
    }



    public class SeatResponseDTO
    {
        public int Id { get; set; }

        public string Prefix { get; set; }

        public int Position { get; set; }

        public int EventId { get; set; }

        public bool IsFree { get; set; }
    }

   
}
