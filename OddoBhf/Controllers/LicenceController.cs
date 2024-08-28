using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OddoBhf.Interfaces;
using OddoBhf.Models;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using OddoBhf.Dto;
using OddoBhf.Services;
using OddoBhf.Dto.Licence;
using Microsoft.AspNetCore.Authorization;


namespace OddoBhf.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class LicenceController : Controller
    {

        private readonly ILicenceService _licenceService;

        public LicenceController(ILicenceService licenceService)
        {
            _licenceService = licenceService;
        }


        // GET: LicenceController
        [HttpGet]

        [ProducesResponseType(200, Type = typeof(IEnumerable<GetLicenceDto>))]
        public IActionResult GetLicences()
        {
            return Ok(_licenceService.GetLicences());
        }

        // Get licence by id:
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type=typeof(GetLicenceDto))]
        public IActionResult GetLicenceById(int id)
        {
            var licence = _licenceService.GetLicenceById(id);
            var result = new GetLicenceDto()
            {
                Email = licence.Email,
                BookedByUserId = licence.BookedByUserId,
                BookedUntil = licence.BookedUntil,
                CurrentSession = licence.CurrentSession,
            };
            return Ok(result);
        }



        //POST: LicenceController 
        [HttpPost]
        [ProducesResponseType(201, Type=typeof(Licence))]
        public IActionResult CreateLicence([FromBody] CreateLicenceDto dto)
        {
            var licence = new Licence { Email = dto.Email, Password=dto.Password };
            _licenceService.CreateLicence(licence);
            return CreatedAtAction("GetLicences", new { id = licence.Id }, licence);
        }

        //PUT: LicenceController
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        public IActionResult UpdateLicence(int id, [FromBody] CreateLicenceDto dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }
            var licence = new Licence { Id=id,Email = dto.Email, Password = dto.Password };
            _licenceService.UpdateLicence(licence);
            return NoContent();
        }

        [HttpGet("extend/{id}")]
        public async Task<IActionResult> ExtendLicence(int id)
        {
            try
            {
                await _licenceService.ExtendLicence(id);
                return Ok(new {message = "Licence Extended"});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("take/{id}")]
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

        [HttpPost("return/{id}")]
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

        [HttpGet("cancelBookLicence/{id}")]
        public async Task<IActionResult> CancelBookLicence(int id)
        {
            try
            {
                await _licenceService.CancelRequestLicence(id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Deletelicence(int id)
        {
            var licence = _licenceService.GetLicenceById(id);
            if (licence == null)
            {
                return NotFound(); // 404 if the licence doesn't exist
            }
            _licenceService.DeleteLicence(id);
            return NoContent(); // 204 if the licence was successfully deleted
        }
    }
}
