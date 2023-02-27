using FirebaseAdmin;
using FirebaseAdmin.Auth;
using LANPartyAPI_DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using LANPartyAPI_Utils;
using LANPartyAPI_Core.Models;
using Google.Apis.Util;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace LANPartyAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //https://firebase.google.com/docs/auth/admin/custom-claims?hl=fr#c_1
        [HttpPost("GetClaims")]
        public async Task<IActionResult> GetClaims([FromBody] FirebaseTokenModel firebaseToken)
        {

            FirebaseApp firebaseApp = FirebaseApp.DefaultInstance;
            FirebaseAuth firebaseAuth = FirebaseAuth.GetAuth(firebaseApp);

            try
            {
                // Verifier que le firebase token recu par le client est valide
                FirebaseToken decodedToken = await firebaseAuth.VerifyIdTokenAsync(idToken: firebaseToken.IdToken);
                string uid = decodedToken.Uid;

                //decodedToken.Claims.TryGetValue("test-refresh-claims", out object value);

                var user = await _userManager.FindByIdAsync(uid);
                if (user == null)
                    return NotFound();

                var roles = await _userManager.GetRolesAsync(user);
                var claims = new Dictionary<string, object>()
                {
                    { "gamertag", user.UserName },
                    { "firstname", user.FirstName },
                    { "lastname", user.LastName},
                    { "roles", string.Join(",", roles.Select(r => r).ToList()) },
                    //{ "test-refresh-claims", !Convert.ToBoolean(value)},
                };

                // The new custom claims will propagate to the user's ID token the next time a new one is issued.
                await firebaseAuth.SetCustomUserClaimsAsync(uid, claims);

                // Tell client to refresh token on user.
                return Ok("Success");
            }
            catch (Exception e)
            {
                return BadRequest($"Inelegible: {e.Message}");
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerDTO)
        {

            FirebaseApp firebaseApp = FirebaseApp.DefaultInstance;
            FirebaseAuth firebaseAuth = FirebaseAuth.GetAuth(firebaseApp);

            try
            {
                // Verifier que le firebase token recu par le client est valide
                FirebaseToken decodedToken = await firebaseAuth.VerifyIdTokenAsync(idToken: registerDTO.IdToken);
                string uid = decodedToken.Uid;

                // Optionaly get the user of uid
                UserRecord firebaseUser = await firebaseAuth.GetUserAsync(uid);

                ApplicationUser user = new ApplicationUser();
                user.Id = firebaseUser.Uid;
                user.Email = firebaseUser.Email;
                user.UserName = registerDTO.GamerTag;
                user.FirstName = registerDTO.FirstName;
                user.LastName = registerDTO.LastName;

                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    // Errors array
                    return BadRequest(result.Errors.Select(e => e.Description));
                }

                // Add user to Role 
                await _userManager.AddToRoleAsync(user, Utils.Roles.Joueur);

                var roles = await _userManager.GetRolesAsync(user);
                var claims = new Dictionary<string, object>()
                {
                    { "gamertag", user.UserName },
                    { "firstname", user.FirstName },
                    { "lastname", user.LastName},
                    { "roles", string.Join(",", roles.Select(r => r).ToList()) },
                };

                // The new custom claims will propagate to the user's ID token the next time a new one is issued.
                await firebaseAuth.SetCustomUserClaimsAsync(uid, claims);

                // Tell client to refresh token on user.
                return Ok("Success");

            }
            catch (Exception e)
            {
                return BadRequest($"Inelegible: {e.Message}");
            }
        }

        public record FirebaseTokenModel
        {
            public string IdToken { get; set; }
        }
        public record RegisterModel
        {
            public string IdToken { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string GamerTag { get; set; }
        }
    }
}
