
using System.ComponentModel.DataAnnotations;


namespace LANPartyAPI_Core.Models
{
    public class Match
    {
        public int Id { get; set; }

        public DateTime? Date { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Round { get; set; }

        [Range(1,int.MaxValue)]
        [Required]
        public int MatchNumber { get; set; }

        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }

        public List<Match_Team> Matches_Teams { get; set; } = new();

    }
}
