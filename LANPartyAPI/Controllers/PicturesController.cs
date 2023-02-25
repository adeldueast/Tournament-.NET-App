using LANPartyAPI_Core.Enums;
using LANPartyAPI_Core.Exceptions;
using LANPartyAPI_Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LANPartyAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        private readonly PictureService _context;

        public PicturesController(PictureService context)
        {
            _context = context;
        }

        [HttpGet("{id}/{height}")]
        public async Task<IActionResult> GetImage(int id, int height)
        {
			try
			{
				var file = await _context.GetImage(id, height);
				return File(file.Item1, file.Item2);
			}
			catch (PictureNotFoundException)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound, ExceptionErrorMessages.PictureExceptions.PictureNotFound);
			}
        }
    }
}
