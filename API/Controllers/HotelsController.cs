﻿using API.Data;
using API.Services;
using DomainModels;
using DomainModels.Mapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly JwtService _jwtService;
        private readonly ILogger<HotelsController> _logger;

        public HotelsController(AppDBContext context, JwtService jwtService, ILogger<HotelsController> logger)
        {
            _context = context;
            _jwtService = jwtService;
            _logger = logger;
        }

		/// <summary>
		/// Henter alle hoteller.
		/// </summary>
		/// <returns>Hotellets info.</returns>
		/// <response code="500">Intern serverfejl.</response>
		/// <response code="404">Hotellet blev ikke fundet.</response>
		/// <response code="403">Ingen adgang.</response>
		/// <response code="200">Hotellet blev fundet og retuneret.</response>
		// GET: api/Hotels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HotelGetDto>>> GetHotels()
        {
            try
            {
                _logger.LogInformation("Henter alle hoteller");
                var hotels = await _context.Hotels.ToListAsync();
                var result = HotelMapping.ToHotelGetDtos(hotels);

                _logger.LogInformation("Hentet {Count} hoteller succesfuldt", result.Count);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fejl ved hentning af hoteller");
                return StatusCode(500, "Der opstod en intern serverfejl ved hentning af hoteller");
            }
        }


        /// <summary>
        /// Henter hotellet baseret på id.
        /// </summary>
        /// <param name="id"> Hotellets id.</param>
        /// <returns>Hotellets info.</returns>
        /// <response code="500">Intern serverfejl.</response>
        /// <response code="404">Hotellet blev ikke fundet.</response>
        /// <response code="403">Ingen adgang.</response>
        /// <response code="200">Hotellet blev fundet og retuneret.</response>

        // GET: api/Hotels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HotelGetDto>> GetHotel(int id)
        {
            try
            {
                _logger.LogInformation("Henter hotel med id {Id}", id);
                var hotel = await _context.Hotels.FindAsync(id);

                if (hotel == null)
                {
                    _logger.LogWarning("Hotel med id {Id} blev ikke fundet", id);
                    return NotFound();
                }

                return Ok(HotelMapping.ToHotelGetDto(hotel));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fejl ved hentning af hotel {Id}", id);
                return StatusCode(500, "Der opstod en intern serverfejl ved hentning af hotel");
            }
        }

        /// <summary>
        /// Opdatere hotellet baseret på et id.
        /// </summary>
        /// <param name="hotel"> Hotellets id.</param>
        /// <returns>Opdatere hotellets info.</returns>
        /// <response code="500">Intern serverfejl.</response>
        /// <response code="404">Hotellet blev ikke opdateret.</response>
        /// <response code="403">Ingen adgang.</response>
        /// <response code="200">Hotellet blev opdateret.</response>
        
        // PUT: api/Hotels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, HotelPutDto hotelDto)
        {
            if (id != hotelDto.Id)
            {
                _logger.LogWarning("Mismatch mellem route id {Id} og body id {HotelId}", id, hotelDto.Id);
                return BadRequest("Id i route stemmer ikke med hotellets id");
            }

            //_context.Entry(hotel).State = EntityState.Modified;
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }

            HotelMapping.PutHotelFromDto(hotel, hotelDto);

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Hotel {Id} opdateret succesfuldt", id);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning(ex, "Concurrency fejl ved opdatering af hotel {Id}", id);
                if (!HotelExists(id))
                    return NotFound();
                else
                    throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fejl ved opdatering af hotel {Id}", id);
                return StatusCode(500, "Der opstod en intern serverfejl ved opdatering af hotel");
            }
        }

        /// <summary>
        /// Opretter et nyt hotel.
        /// </summary>
        /// <param name="hotelDto"> Hotellets dto.</param>
        /// <returns>opretter et nyt hotel.</returns>
        /// <response code="500">Intern serverfejl.</response>
        /// <response code="404">Hotellet blev ikke oprettet.</response>
        /// <response code="403">Ingen adgang.</response>
        /// <response code="200">Hotellet blev oprettet.</response>

        // POST: api/Hotels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Hotel>> PostHotel(HotelPostDto hotelDto)
        {
           try
            {
                Hotel hotel = HotelMapping.PostHotelFromDto(hotelDto);
                _context.Hotels.Add(hotel);

                await _context.SaveChangesAsync();
                _logger.LogInformation("Hotel {Id} oprettet succesfuldt", hotel.Id);

                return CreatedAtAction("GetHotel", new { id = hotel.Id }, hotel);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogWarning(ex, "DbUpdateException ved oprettelse af hotel {Id}", hotelDto.Id);
                if (HotelExists(hotelDto.Id))
                    return Conflict("Hotel med dette id findes allerede");
                else
                    throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fejl ved oprettelse af hotel");
                return StatusCode(500, "Der opstod en intern serverfejl ved oprettelse af hotel");
            }
        }

		/// <summary>
		/// Sletter et hotel.
		/// </summary>
		/// <param name="id"> Hotellets id.</param>
		/// <returns>Sletter et hotel.</returns>
		/// <response code="500">Intern serverfejl.</response>
		/// <response code="404">Hotellet blev ikke slettet.</response>
		/// <response code="403">Ingen adgang.</response>
		/// <response code="200">Hotellet blev slettet.</response>
        
        // DELETE: api/Hotels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            try
            {
                var hotel = await _context.Hotels.FindAsync(id);
                if (hotel == null)
                {
                    _logger.LogWarning("Forsøg på at slette hotel {Id}, men det blev ikke fundet", id);
                    return NotFound();
                }

                _context.Hotels.Remove(hotel);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Hotel {Id} slettet succesfuldt", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fejl ved sletning af hotel {Id}", id);
                return StatusCode(500, "Der opstod en intern serverfejl ved sletning af hotel");
            }
        }

        private bool HotelExists(int id)
        {
            return _context.Hotels.Any(e => e.Id == id);
        }
    }
}
