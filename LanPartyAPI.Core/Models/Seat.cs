using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LANPartyAPI_Core.Models
{
	public class Seat
	{
		public int Id { get; set; }

		public string Prefix { get; set; }

		public int Position	{ get; set; }

		public int EventId { get; set; }
		public Event Event { get; set; }

		
		
		public ApplicationUser User { get; set; }
	}
}
