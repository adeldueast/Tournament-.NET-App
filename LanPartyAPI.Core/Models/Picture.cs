using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LANPartyAPI_Core.Models
{
    public class Picture
    {
        public int Id { get; set; }

        [Required]
        public string Path { get; set; }

        public string MimeType { get; set; }

        public int EventId { get; set; }

        public Event Event { get; set; }
    }
}
