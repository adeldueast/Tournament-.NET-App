using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LANPartyAPI_Services.DTOs
{
    public class ApplicationUserDTO 
    {
        public string Id { get; set; }

        //public string FirebaseUid { get; set; }

        public string FirstName { get; set; }    

        public string LastName { get; set; }

        public string GamerTag { get; set; }

    }
}
