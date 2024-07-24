using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OddoBhf.Interfaces;
using OddoBhf.Models;
using System.Net.Http;


namespace OddoBhf.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LicenceController : Controller
    {
        private readonly ILicenceRepository _licenceRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly HttpClient _httpClient;

        public LicenceController(ILicenceRepository licenceRepository, ISessionRepository sessionRepository, HttpClient httpClient)
        {
            _licenceRepository = licenceRepository;
            _sessionRepository = sessionRepository;
            _httpClient = httpClient;
        }


        /*private ICollection<Licence>? _licences = new List<Licence>
        {
            new Licence { Id = 1, Email= "sami.belhadj@oddo-bhf.com", Password= "7cB3MP.6y9.Z?c?", IsAvailable=true },
            new Licence { Id = 2, Email= "aziz@oddo-bhf.com", Password="7cB3MP.6y9.Z?c?", IsAvailable=false},
            new Licence { Id = 3, Email= "ahmed@oddo-bhf.com", Password="7cB3MP.6y9.Z?c?", IsAvailable=false}
        };*/



        // GET: LicenceController
        [HttpGet]
        [ProducesResponseType(200, Type=typeof(IEnumerable<Licence>))]
        public IActionResult GetLicences()
        {
            return Ok(_licenceRepository.GetAllLicences());
//            return Ok(_licences);
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

        //GET: take licence
        [HttpGet("{id}/take")]
        [ProducesResponseType(200, Type=typeof(Licence))]
        public async Task<IActionResult> TakeLicence(int id)
        {
//            return Ok(_licences.FirstOrDefault(l => l.Id == id));
            var licence = _licenceRepository.GetLicenceById(id);
            if (licence == null)
            {
                return NotFound();
            }
            if (licence.IsAvailable)
            {

                try
                {
                    var response = await _httpClient.GetAsync("http://127.0.0.1:5000/get_cookie");

                    if (!response.IsSuccessStatusCode)
                    {
                        return StatusCode((int)response.StatusCode, new { message = "Error fetching data" });
                    }

                    Session session = new Session
                    {
                        StartTime = DateTime.Now,
                        LicenceId = licence.Id,
                        Licence = licence,
                        UserNotes = ""
                    };
                    _sessionRepository.AddSession(session);

                    licence.IsAvailable = false;
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
        [HttpGet("{id}/return")]
        [ProducesResponseType(200, Type=typeof(Licence))]
        public async Task<IActionResult> ReturnLicence(int id)
        {
            var licence = _licenceRepository.GetLicenceById(id);
            if (licence == null)
            {
                return NotFound();
            }

            if (!licence.IsAvailable)

                try
                {
                    var response = await _httpClient.GetAsync("http://127.0.0.1:5000/close");

                    if (!response.IsSuccessStatusCode)
                    {
                  //      return StatusCode((int)response.StatusCode, new { message = "Error fetching data" });
                    }

                    licence.CurrentSession.EndTime = DateTime.Now;
                    _sessionRepository.UpdateSession(licence.CurrentSession);

                    licence.IsAvailable = true;
                    licence.CurrentSession = null;
                    _licenceRepository.UpdateLicence(licence);

                    return Ok(licence);

                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = ex.Message });
                }

            
            return BadRequest("Licence is already available");
        }

    }
}
