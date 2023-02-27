
using System.ComponentModel.DataAnnotations;


namespace LANPartyAPI_Core.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Range(1, 9999)]
        public int? MaxPlayerNumber { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public List<Tournament> Tournaments { get; set; } = new();
        
        public List<ApplicationUser> Players { get; set; } = new();


    }
}
