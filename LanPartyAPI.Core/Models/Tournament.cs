using LANPartyAPI_Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LANPartyAPI_Core.Models
{
    public class Tournament
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [MaxLength(60)]
        public string Name { get; set; }


        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        [MaxLength(100)]
        public string Game { get; set; }

        [Required]
        [Range(1, 5)]
        public int MaxPlayersPerTeam { get; set; }

        [Required]
        [Range(1, 100)]
        public int MaxTeamNumber { get; set; }

        public string Url { get; set; }

        public bool hasStarted { get; set; }

        public List<Team> Teams { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }

        [Required]
        public TournamentType TournamentType { get; set; }

     
    }
}
