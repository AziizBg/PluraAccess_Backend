using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OddoBhf.Interfaces;
using OddoBhf.Models;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using OddoBhf.Dto;
using OddoBhf.Services;


namespace OddoBhf.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LicenceController : Controller
    {

        private readonly ILicenceService _licenceService;

        public LicenceController(ILicenceService licenceService)
        {
            _licenceService = licenceService;
        }

        // Get licence by id:
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type=typeof(Licence))]
        public IActionResult GetLicenceById(int id)
        {
            return Ok(_licenceService.GetLicenceById(id));
        }


        // GET: LicenceController
        [HttpGet]
        [ProducesResponseType(200, Type=typeof(IEnumerable<Licence>))]
        public IActionResult GetLicences()
        {
            return Ok(_licenceService.GetLicences());
        }

        //POST: LicenceController 
        [HttpPost]
        [ProducesResponseType(201, Type=typeof(Licence))]
        public IActionResult CreateLicence([FromBody] Licence licence)
        {
            _licenceService.CreateLicence(licence);
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
            _licenceService.UpdateLicence(licence);
            return NoContent();
        }

        
        [HttpPost("{id}/take")]
        [ProducesResponseType(200, Type = typeof(Licence))]
        public async Task<IActionResult> TakeLicence(int id, [FromBody] OpenPluralsightDto dto)
        {
            try
            {
                var licence = await _licenceService.TakeLicence(id, dto);
                if (licence == null)
                {
                    return BadRequest("Licence is not available or user not found");
                }

                return Ok(licence);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("{id}/return")]
        [ProducesResponseType(200, Type = typeof(Session))]
        public async Task<IActionResult> ReturnLicence(int id, [FromBody] ReturnLicenceDto dto)
        {
            try
            {
                var session = await _licenceService.ReturnLicence(id, dto);
                if (session == null)
                {
                    return BadRequest("Licence is already available or not found");
                }

                return Ok(session);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
