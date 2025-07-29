using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Interfaces;
using api.Models;
using api.Services;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly IArtistService _artistService;
        private readonly ILogger<ArtistsController> _logger;

        public ArtistsController(IArtistService artistService, ILogger<ArtistsController> logger)
        {
            _artistService = artistService ?? throw new ArgumentNullException(nameof(artistService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/Artists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Artist>>> GetArtists()
        {
            try
            {
                _logger.LogInformation("Fetching all artists from the database.");
                var artists = await _artistService.GetAllArtistsAsync();
                if (artists == null || !artists.Any())
                {
                    _logger.LogWarning("No artists found in the database.");
                    return NotFound("No artists found.");
                }
                _logger.LogInformation($"Found {artists.Count()} artists.");
                return Ok(artists);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An error occurred while fetching artists.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching artists.");
            }
        }

        // GET: api/Artists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Artist>> GetArtist(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching artist with ID {id} from the database.");
                var artist = await _artistService.GetArtistByIdAsync(id);
                if (artist == null)
                {
                    _logger.LogWarning($"Artist with ID {id} not found.");
                    return NotFound($"Artist with ID {id} not found.");
                }
                _logger.LogInformation($"Artist with ID {id} found.");
                return Ok(artist);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"An error occurred while fetching artist with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the artist.");
            }
        }

        // PUT: api/Artists/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArtist(int id, Artist artist)
        {
            try
            {
                if (id != artist.Id)
                {
                    _logger.LogWarning($"Artist ID mismatch: {id} does not match {artist.Id}.");
                    return BadRequest("Artist ID mismatch.");
                }
                _logger.LogInformation($"Updating artist with ID {id}.");
                await _artistService.UpdateArtistAsync(id, artist);
                _logger.LogInformation($"Artist with ID {id} updated successfully.");
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _artistService.GetArtistByIdAsync(id) == null)
                {
                    _logger.LogWarning($"Artist with ID {id} not found for update.");
                    return NotFound($"Artist with ID {id} not found.");
                }
                else
                {
                    _logger.LogError($"Concurrency error occurred while updating artist with ID {id}.");
                    throw;
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"An error occurred while updating artist with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the artist.");
            }
        }

        // POST: api/Artists
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Artist>> PostArtist(Artist artist)
        {
            try
            {
                if (artist == null)
                {
                    _logger.LogWarning("Recieved empty student object");
                    return BadRequest("Artist object cannot be null.");
                }
                await _artistService.AddArtistAsync(artist);
                _logger.LogInformation($"Artist with ID {artist.Id} created successfully.");
                return CreatedAtAction("GetArtist", new { id = artist.Id }, artist);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An error occurred while creating the artist.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the artist.");
            }
        }

        // DELETE: api/Artists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtist(int id)
        {
            try
            {
                var artist = await _artistService.GetArtistByIdAsync(id);
                if (artist == null)
                {
                    _logger.LogWarning($"Artist with ID {id} not found for deletion.");
                    return NotFound($"Artist with ID {id} not found.");
                }

                await _artistService.DeleteArtistAsync(id);
                _logger.LogInformation($"Artist with ID {id} deleted successfully.");
                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"An error occurred while deleting artist with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the artist.");
            }
        }
    }
}


