using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LANPartyAPI_Core.Models
{
    public class Team
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }

        public List<Match_Team> Matches_Teams { get; set; } = new();

        public List<ApplicationUser> Players { get; set; } = new();


    }
}
