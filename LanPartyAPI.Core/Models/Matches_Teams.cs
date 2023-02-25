using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LANPartyAPI_Core.Models
{
    public class Match_Team
    {
        public int MatchId { get; set; }
        public Match Match { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }

        [MaxLength(10)]
        public string Score { get; set; }

        public bool IsWinner { get; set; }
    }
}
