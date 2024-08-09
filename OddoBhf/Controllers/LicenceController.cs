using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OddoBhf.Interfaces;
using OddoBhf.Models;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using OddoBhf.Dto;


namespace OddoBhf.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LicenceController : Controller
    {
        private readonly ILicenceRepository _licenceRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IUserRepository _userRepository;
        private readonly HttpClient _httpClient;

        public LicenceController(ILicenceRepository licenceRepository, ISessionRepository sessionRepository, IUserRepository userRepository, HttpClient httpClient)
        {
            _licenceRepository = licenceRepository;
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
            _httpClient = httpClient;
        }

        // Get licence by id:
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type=typeof(Licence))]
        public IActionResult GetLicenceById(int id)
        {
            return Ok(_licenceRepository.GetLicenceById(id));
        }


        // GET: LicenceController
        [HttpGet]
        [ProducesResponseType(200, Type=typeof(IEnumerable<Licence>))]
        public IActionResult GetLicences()
        {
            return Ok(_licenceRepository.GetAllLicences());
        }

        //POST: LicenceController 
        [HttpPost]
        [ProducesResponseType(201, Type=typeof(Licence))]
        public IActionResult CreateLicence([FromBody] Licence licence)
        {
            _licenceRepository.AddLicence(licence);
            return CreatedAtAction("GetLicences", new { id = licence.Id }, licence);
        }

        //PUT: LicenceController
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        public IActionResult UpdateLicence(int id, [FromBody] Licence licence)
        {
            if (id != licence.Id)
            {
                return BadRequest();
            }
            _licenceRepository.UpdateLicence(licence);
            return NoContent();
        }

        //POST: take licence
        [HttpPost("{id}/take")]
        [ProducesResponseType(200, Type=typeof(Licence))]
        public async Task<IActionResult> TakeLicence(int id, [FromBody] OpenPluralsightDto dto)
        {
            var licence = _licenceRepository.GetLicenceById(id);
            if (licence == null)
            {
                return NotFound();
            }
            if (licence.CurrentSession == null)
            {
                try
                {
                    var url = "http://127.0.0.1:5000/get_cookie";
                    if (dto.NgorkUrl != null)
                    {
                        url = dto.NgorkUrl + "/get_cookie";
                    }
                    var email = "sami.belhadj@oddo-bhf.com";
                    var password = "7cB3MP.6y9.Z?c?"; // Replace with actual password
                    var user = _userRepository.GetUserById(dto.UserId);
                    var startTime = DateTime.Now;
/*                    var endTime = DateTime.Now.AddHours(2);
*/                    var endTime = DateTime.Now.AddMinutes(1);
                    var formattedEndTime = endTime;

                    var licenceId = licence.Id;

                    if (user == null)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new { message = "user not provided" });
                    }

                    // Create the payload
                    var payload = new
                    {
                        email,
                        password,
                        formattedEndTime,
                        licenceId
                    };

                    // Serialize the payload to JSON
                    var jsonPayload = JsonSerializer.Serialize(payload);

                    // Create the StringContent with the JSON payload and set the content type
                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    // Send the POST request
                    var response = await _httpClient.PostAsync(url, content);

                    if (!response.IsSuccessStatusCode)
                    {
                        return (IActionResult)response;
                    }

                    Session session = new Session
                    {
                        StartTime = startTime,
                        EndTime = endTime,
                        Licence = licence,
                        User = user,
                        UserNotes = ""
                    };
                    Console.WriteLine("licence: ", licence);
                    Console.WriteLine("Session: ", session);
                    _sessionRepository.AddSession(session);

                    licence.CurrentSession = session;

                    _licenceRepository.UpdateLicence(licence);

                    return Ok(licence);


                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = ex.Message });
                }


            }
            return BadRequest("Licence is not available");
        }

        //GET: return licence
        [HttpPost("{id}/return")]
        [ProducesResponseType(200, Type = typeof(Licence))]
        public async Task<IActionResult> ReturnLicence(int id, [FromBody]ReturnLicenceDto dto)
        {
            var licence = _licenceRepository.GetLicenceById(id);
            if (licence == null)
            {
                return NotFound();
            }

            if (licence.CurrentSession != null)
            {
                var currentSession = _sessionRepository.GetSessionById(licence.CurrentSession.Id);

                try
                {
                    if (dto.isBrowserClosed == false)
                    {
                        var response = await _httpClient.GetAsync("http://127.0.0.1:5000/close");


                        if (!response.IsSuccessStatusCode)
                        {
                            //      return StatusCode((int)response.StatusCode, new { message = "Error fetching data" });
                        }
                    }
                    currentSession.EndTime = DateTime.Now;
                    currentSession.Licence = licence;

                    _sessionRepository.UpdateSession(currentSession);


                    licence.CurrentSession = null;
                    _licenceRepository.UpdateLicence(licence);
                    return Ok(_sessionRepository.GetSessionById(currentSession.Id));


//                    return Ok(licence);

                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = ex.Message });
                }

            }
                return BadRequest("Licence is already available");
            
        }

    }
}
