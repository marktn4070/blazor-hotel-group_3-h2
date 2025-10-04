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
public class RoomsController : ControllerBase
{
    private readonly AppDBContext _context;
    private readonly JwtService _jwtService;
    private readonly ILogger<RoomsController> _logger;

    public RoomsController(AppDBContext context, JwtService jwtService, ILogger<RoomsController> logger)
    {
        _context = context;
        _jwtService = jwtService;
        _logger = logger;
    }

    /// <summary>
    /// Henter alle rum.
    /// </summary>
    /// <returns>Rummets info.</returns>
    /// <response code="500">Intern serverfejl.</response>
    /// <response code="404">Rummet blev ikke fundet.</response>
    /// <response code="403">Ingen adgang.</response>
    /// <response code="200">Rummet blev fundet og retuneret.</response>
    // GET: api/Rooms
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoomGetDto>>> GetRooms()
    {
        try
        {
            _logger.LogInformation("Henter alle rum");
            var rooms = await _context.Rooms.ToListAsync();
            var result = RoomMapping.ToRoomGetDtos(rooms);

            _logger.LogInformation("Hentet {Count} rum succesfuldt", result.Count);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fejl ved hentning af rum");
            return StatusCode(500, "Der opstod en intern serverfejl ved hentning af rum");
        }
    }


    /// <summary>
    /// Henter roomlet baseret på id.
    /// </summary>
    /// <param name="id">Rummets id.</param>
    /// <returns>Rummets info.</returns>
    /// <response code="500">Intern serverfejl.</response>
    /// <response code="404">Rummet blev ikke fundet.</response>
    /// <response code="403">Ingen adgang.</response>
    /// <response code="200">Rummet blev fundet og retuneret.</response>

    // GET: api/Rooms/5
    [HttpGet("{id}")]
    public async Task<ActionResult<RoomGetDto>> GetRoom(int id)
    {
        try
        {
            _logger.LogInformation("Henter room med id {Id}", id);
            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
            {
                _logger.LogWarning("Room med id {Id} blev ikke fundet", id);
                return NotFound();
            }

            return Ok(RoomMapping.ToRoomGetDto(room));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fejl ved hentning af room {Id}", id);
            return StatusCode(500, "Der opstod en intern serverfejl ved hentning af room");
        }
    }

    /// <summary>
    /// Opdatere roomlet baseret på et id.
    /// </summary>
    /// <param name="room">Rummets id.</param>
    /// <returns>Opdatere roomlets info.</returns>
    /// <response code="500">Intern serverfejl.</response>
    /// <response code="404">Rummet blev ikke opdateret.</response>
    /// <response code="403">Ingen adgang.</response>
    /// <response code="200">Rummet blev opdateret.</response>

    // PUT: api/Rooms/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutRoom(int id, RoomPutDto room)
    {
        if (id != room.Id)
        {
            _logger.LogWarning("Mismatch mellem route id {Id} og body id {RoomId}", id, room.Id);
            return BadRequest("Id i route stemmer ikke med roomlets id");
        }

        _context.Entry(room).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Room {Id} opdateret succesfuldt", id);
            return NoContent();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Concurrency fejl ved opdatering af room {Id}", id);
            if (!await RoomExists(id))
                return NotFound();
            else
                throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fejl ved opdatering af room {Id}", id);
            return StatusCode(500, "Der opstod en intern serverfejl ved opdatering af room");
        }
    }

    /// <summary>
    /// Opretter et nyt room.
    /// </summary>
    /// <param name="roomDto">Rummets dto.</param>
    /// <returns>opretter et nyt room.</returns>
    /// <response code="500">Intern serverfejl.</response>
    /// <response code="404">Rummet blev ikke oprettet.</response>
    /// <response code="403">Ingen adgang.</response>
    /// <response code="200">Rummet blev oprettet.</response>

    // POST: api/Rooms
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Room>> PostRoom(RoomPostDto roomDto)
    {
        try
        {
            Room room = RoomMapping.PostRoomFromDto(roomDto);
            _context.Rooms.Add(room);

            await _context.SaveChangesAsync();
            _logger.LogInformation("Room {Id} oprettet succesfuldt", room.Id);

            return CreatedAtAction("GetRoom", new { id = room.Id }, room);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "DbUpdateException ved oprettelse af room {RoomNumber}", roomDto.RoomNumber);
            if (!await HotelExists(roomDto.HotelId))
                return Conflict("Det angivne hotel eksisterer ikke");
            else if (await RoomExistsInHotel(roomDto.HotelId, roomDto.RoomNumber))
                return Conflict("Et rum med dette roomNumber eksisterer allerede i hotellet");
            else
                throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fejl ved oprettelse af room");
            return StatusCode(500, "Der opstod en intern serverfejl ved oprettelse af room");
        }
    }

    /// <summary>
    /// Sletter et room.
    /// </summary>
    /// <param name="id">Rummets id.</param>
    /// <returns>Sletter et room.</returns>
    /// <response code="500">Intern serverfejl.</response>
    /// <response code="404">Rummet blev ikke slettet.</response>
    /// <response code="403">Ingen adgang.</response>
    /// <response code="200">Rummet blev slettet.</response>

    // DELETE: api/Rooms/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoom(int id)
    {
        try
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                _logger.LogWarning("Forsøg på at slette room {Id}, men det blev ikke fundet", id);
                return NotFound();
            }

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Room {Id} slettet succesfuldt", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fejl ved sletning af room {Id}", id);
            return StatusCode(500, "Der opstod en intern serverfejl ved sletning af room");
        }
    }

    private async Task<bool> RoomExists(int id)
    {
        return await _context.Rooms.AnyAsync(e => e.Id == id);
    }
    private async Task<bool> HotelExists(int hotelid)
    {
        return await _context.Hotels.AnyAsync(h => h.Id == hotelid);
    }
    private async Task<bool> RoomExistsInHotel(int hotelid, int roomnumber)
    {
        return await _context.Rooms.AnyAsync(r => r.HotelId == hotelid && r.RoomNumber == roomnumber);
    }

}
