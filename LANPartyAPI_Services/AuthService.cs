using FirebaseAdmin.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LANPartyAPI_Services
{
    public class FirebaseAuthService
    {

        private readonly FirebaseAuth _auth;

        public FirebaseAuthService()
        {
            _auth = FirebaseAuth.DefaultInstance;
        }

        public async Task AddCustomClaimsAsync(string idToken, Dictionary<string,object> customClaims)
        {
            //Verify the ID token
            FirebaseToken decodedToken = await _auth.VerifyIdTokenAsync(idToken);

            //Add custom claims to the ID Token
            await _auth.SetCustomUserClaimsAsync(decodedToken.Uid, customClaims);

        }
    }
}
