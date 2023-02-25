using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LANPartyAPI_Core.Models
{
    public class ApplicationUser : IdentityUser
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        public List<Event> Events { get; set; } = new();

        public List<Team> Teams { get; set; } = new();

        public List<Seat> Seats { get; set; } = new();
    }
}
