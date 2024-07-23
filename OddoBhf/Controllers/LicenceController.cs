using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OddoBhf.Interfaces;
using OddoBhf.Models;

namespace OddoBhf.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LicenceController : Controller
    {
        private readonly ILicenceRepository _licenceRepository;
        private readonly ISessionRepository _sessionRepository;
        public LicenceController(ILicenceRepository licenceRepository, ISessionRepository sessionRepository)
        {
            _licenceRepository = licenceRepository;
            _sessionRepository = sessionRepository;
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
            _licenceRepository.Save();
            return CreatedAtAction("GetLicences", new { id = licence.Id }, licence);
        }

        //GET: take licence
        [HttpGet("{id}/take")]
        [ProducesResponseType(200, Type=typeof(Licence))]
        public IActionResult TakeLicence(int id)
        {
//            return Ok(_licences.FirstOrDefault(l => l.Id == id));
            var licence = _licenceRepository.GetLicenceById(id);
            if (licence == null)
            {
                return NotFound();
            }
            if (licence.IsAvailable)
            {
                licence.IsAvailable = false;
                _licenceRepository.UpdateLicence(licence);
                _licenceRepository.Save();

                _sessionRepository.AddSession(new Session { Licence = licence, StartTime = DateTime.Now });
                _sessionRepository.Save();

                return Ok(licence);
            }
            return BadRequest("Licence is not available");
        }

        //GET: return licence
        [HttpGet("{id}/return")]
        [ProducesResponseType(200, Type=typeof(Licence))]
        public IActionResult ReturnLicence(int id)
        {
            var licence = _licenceRepository.GetLicenceById(id);
            if (licence == null)
            {
                return NotFound();
            }
            if (!licence.IsAvailable)
            {
                licence.IsAvailable = true;
                _licenceRepository.UpdateLicence(licence);
                _licenceRepository.Save();

                var session = _sessionRepository.GetSessionByLicenceId(id);
                session.EndTime = DateTime.Now;
                _sessionRepository.UpdateSession(session);
                _sessionRepository.Save();

                return Ok(licence);
            }
            return BadRequest("Licence is already available");
        }

    }
}
