using API.Data;
using API.Services;
using DomainModels;
using DomainModels.Mapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomtypesController : ControllerBase
{
    private readonly AppDBContext _context;
    private readonly JwtService _jwtService;
    private readonly ILogger<RoomtypesController> _logger;

    public RoomtypesController(AppDBContext context, JwtService jwtService, ILogger<RoomtypesController> logger)
    {
        _context = context;
        _jwtService = jwtService;
        _logger = logger;
    }

    /// <summary>
    /// Henter alle rumtyper.
    /// </summary>
    /// <returns>Rumtypens info.</returns>
    /// <response code="500">Intern serverfejl.</response>
    /// <response code="404">Rumtypen blev ikke fundet.</response>
    /// <response code="403">Ingen adgang.</response>
    /// <response code="200">Rumtypen blev fundet og retuneret.</response>
    // GET: api/Roomtypes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoomtypeGetDto>>> GetRoomtypes()
    {
        try
        {
            _logger.LogInformation("Henter alle rumtyper");
            var roomtypes = await _context.Roomtypes.ToListAsync();
            var result = RoomtypeMapping.ToRoomtypeGetDtos(roomtypes);

            _logger.LogInformation("Hentet {Count} rumtyper succesfuldt", result.Count);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fejl ved hentning af rumtyper");
            return StatusCode(500, "Der opstod en intern serverfejl ved hentning af rumtyper");
        }
    }


    /// <summary>
    /// Henter roomtypelet baseret på id.
    /// </summary>
    /// <param name="id"> Rumtypens id.</param>
    /// <returns>Rumtypens info.</returns>
    /// <response code="500">Intern serverfejl.</response>
    /// <response code="404">Rumtypen blev ikke fundet.</response>
    /// <response code="403">Ingen adgang.</response>
    /// <response code="200">Rumtypen blev fundet og retuneret.</response>

    // GET: api/Roomtypes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<RoomtypeGetDto>> GetRoomtype(int id)
    {
        try
        {
            _logger.LogInformation("Henter roomtype med id {Id}", id);
            var roomtype = await _context.Roomtypes.FindAsync(id);

            if (roomtype == null)
            {
                _logger.LogWarning("Roomtype med id {Id} blev ikke fundet", id);
                return NotFound();
            }

            return Ok(RoomtypeMapping.ToRoomtypeGetDto(roomtype));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fejl ved hentning af roomtype {Id}", id);
            return StatusCode(500, "Der opstod en intern serverfejl ved hentning af roomtype");
        }
    }

    /// <summary>
    /// Opdatere roomtypelet baseret på et id.
    /// </summary>
    /// <param name="roomtype"> Rumtypens id.</param>
    /// <returns>Opdatere roomtypelets info.</returns>
    /// <response code="500">Intern serverfejl.</response>
    /// <response code="404">Rumtypen blev ikke opdateret.</response>
    /// <response code="403">Ingen adgang.</response>
    /// <response code="200">Rumtypen blev opdateret.</response>

    // PUT: api/Roomtypes/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutRoomtype(int id, RoomtypePutDto roomtypeDto)
    {
        if (id != roomtypeDto.Id)
        {
            _logger.LogWarning("Mismatch mellem route id {Id} og body id {RoomtypeId}", id, roomtypeDto.Id);
            return BadRequest("Id i route stemmer ikke med roomtypelets id");
        }

        //_context.Entry(roomtype).State = EntityState.Modified;
        var roomtype = await _context.Roomtypes.FindAsync(id);
        if (roomtype == null)
        {
            return NotFound();
        }

        RoomtypeMapping.PutRoomtypeFromDto(roomtype, roomtypeDto);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Rumtype med id'et {Id} opdateret succesfuldt", id);
            return NoContent();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Concurrency fejl ved opdatering af roomtype {Id}", id);
            if (!RoomtypeExists(id))
                return NotFound();
            else
                throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fejl ved opdatering af roomtype {Id}", id);
            return StatusCode(500, "Der opstod en intern serverfejl ved opdatering af roomtype");
        }
    }

    /// <summary>
    /// Opretter et nyt roomtype.
    /// </summary>
    /// <param name="roomtypePostDto"> Rumtypens dto.</param>
    /// <returns>opretter et nyt roomtype.</returns>
    /// <response code="500">Intern serverfejl.</response>
    /// <response code="404">Rumtypen blev ikke oprettet.</response>
    /// <response code="403">Ingen adgang.</response>
    /// <response code="200">Rumtypen blev oprettet.</response>

    // POST: api/Roomtypes
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Roomtype>> PostRoomtype(RoomtypePostDto roomtypePostDto)
    {
        try
        {
            Roomtype roomtype = RoomtypeMapping.PostRoomtypeFromDto(roomtypePostDto);
            _context.Roomtypes.Add(roomtype);

            // Tjek om rumtype navn allerede eksisterer
            var roomtypeExists = await _context.Roomtypes.AnyAsync(rt => rt.Name == roomtypePostDto.Name);

            if (roomtypeExists)
            {
                _logger.LogWarning("Rumtype navn {Name} eksisterer allerede", roomtypePostDto.Name);
                return Conflict("En Rumtype med dette navn eksisterer allerede");
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Rumtype med id'et {Id} oprettet succesfuldt", roomtype.Id);

            return CreatedAtAction("GetRoomtype", new { id = roomtype.Id }, roomtype);

        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Fejl ved oprettelse af roomtype");
            return StatusCode(500, "Der opstod en intern serverfejl ved oprettelse af roomtype");
        }
    }

    /// <summary>
    /// Sletter et roomtype.
    /// </summary>
    /// <param name="id"> Rumtypens id.</param>
    /// <returns>Sletter et roomtype.</returns>
    /// <response code="500">Intern serverfejl.</response>
    /// <response code="404">Rumtypen blev ikke slettet.</response>
    /// <response code="403">Ingen adgang.</response>
    /// <response code="200">Rumtypen blev slettet.</response>

    // DELETE: api/Roomtypes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoomtype(int id)
    {
        try
        {
            var roomtype = await _context.Roomtypes.FindAsync(id);
            if (roomtype == null)
            {
                _logger.LogWarning("Forsøg på at slette roomtype {Id}, men det blev ikke fundet", id);
                return NotFound();
            }

            _context.Roomtypes.Remove(roomtype);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Rumtype med id'et {Id} slettet succesfuldt", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fejl ved sletning af roomtype {Id}", id);
            return StatusCode(500, "Der opstod en intern serverfejl ved sletning af roomtype");
        }
    }

    private bool RoomtypeExists(int id)
    {
        return _context.Roomtypes.Any(e => e.Id == id);
    }
}
