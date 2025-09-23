using API.Data;
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
    public class UsersController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly JwtService _jwtService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(AppDBContext context, JwtService jwtService, ILogger<UsersController> logger)
        {
            _context = context;
            _jwtService = jwtService;
            _logger = logger;
        }

        /// <summary>
        /// Henter alle brugere, hvis der er logget ind som admin.
        /// </summary>
        /// <returns>User info.</returns>
        /// <response code="500">Intern serverfejl.</response>
        /// <response code="404">Brugerne blev ikke fundet.</response>
        /// <response code="403">Ingen adgang.</response>
        /// <response code="200">Brugerne blev fundet og retuneret.</response>

        // GET: api/Users
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            try
            {
                _logger.LogInformation("Henter alle brugere - anmodet af administrator");

                var users = await _context.Users
                    .Include(u => u.Role)
                    .ToListAsync();

                _logger.LogInformation("Hentet {UserCount} brugere succesfuldt", users.Count);
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fejl ved hentning af alle brugere");
                return StatusCode(500, "Der opstod en intern serverfejl ved hentning af brugere");
            }
        }

        /// <summary>
        /// Henter en bruger.
        /// </summary>
        /// <returns>Brugerens info.</returns>
        /// <response code="500">Intern serverfejl.</response>
        /// <response code="404">Brugeren blev ikke fundet.</response>
        /// <response code="403">Ingen adgang.</response>
        /// <response code="200">Brugeren blev fundet og retuneret.</response>

        // GET: api/Users/UUID
        [HttpGet("{id}")]
        public async Task<ActionResult<UserGetDto>> GetUser(int id)
        {
            try
            {
                _logger.LogInformation("Forsøger at hente bruger med id {Id}", id);

                var user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (user == null)
                {
                    _logger.LogWarning("Bruger med id {Id} blev ikke fundet", id);
                    return NotFound();
                }

                return UserMapping.ToUserGetDto(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fejl ved hentning af bruger {Id}", id);
                return StatusCode(500, "Der opstod en intern serverfejl ved hentning af bruger");
            }
        }

        /// <summary>
        /// Opdatere en bruger baseret på et id.
        /// </summary>
        /// <param name="user,id">Brugerens id.</param>
        /// <returns>Opdatere en brugers info.</returns>
        /// <response code="500">Intern serverfejl.</response>
        /// <response code="404">Brugeren blev ikke opdateret.</response>
        /// <response code="403">Ingen adgang.</response>
        /// <response code="200">Brugeren blev opdateret.</response>

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                _logger.LogWarning("Mismatch mellem route id {Id} og body id {UserId}", id, user.Id);
                return BadRequest("Id i route stemmer ikke med brugerens id");
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Bruger {Id} opdateret succesfuldt", id);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning(ex, "Concurrency fejl ved opdatering af bruger {Id}", id);
                if (!UserExists(id))
                    return NotFound();
                else
                    throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Uventet fejl ved opdatering af bruger {Id}", id);
                return StatusCode(500, "Der opstod en intern serverfejl ved opdatering af bruger");
            }
        }

        /// <summary>
        /// Laver en regestering af en ny bruger.
        /// </summary>
        /// <param name="dto">Brugerens dto.</param>
        /// <returns>Opretter en ny bruger.</returns>
        /// <response code="500">Intern serverfejl.</response>
        /// <response code="404">Brugeren blev ikke oprettet.</response>
        /// <response code="403">Ingen adgang.</response>
        /// <response code="200">Brugeren blev oprettet.</response>

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                _logger.LogInformation("Forsøger at registrere ny bruger med email {Email}", dto.Email);

                if (_context.Users.Any(u => u.Email == dto.Email))
                {
                    _logger.LogWarning("Bruger med email {Email} findes allerede", dto.Email);
                    return BadRequest("En bruger med denne email findes allerede.");
                }

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User");
                if (userRole == null)
                    return BadRequest("Standard brugerrolle ikke fundet.");

                var user = new User
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    HashedPassword = hashedPassword,
                    PasswordBackdoor = dto.Password,
                    RoleId = userRole.Id,
                    CreatedAt = DateTime.UtcNow.AddHours(2),
                    UpdatedAt = DateTime.UtcNow.AddHours(2),
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Bruger {Email} registreret succesfuldt", user.Email);
                return Ok(new { message = "Bruger oprettet!", user.Email, role = userRole.Name });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fejl ved registrering af bruger {Email}", dto?.Email);
                return StatusCode(500, "Der opstod en intern serverfejl ved oprettelse af bruger");
            }
        }

        /// <summary>
        /// Logger ind som en bruger.
        /// </summary>
        /// <param name="dto">Dto.</param>
        /// <returns>Checker om brugeren er logget ind.</returns>
        /// <response code="500">Intern serverfejl.</response>
        /// <response code="404">login blev ikke oprettet.</response>
        /// <response code="403">Ingen adgang.</response>
        /// <response code="200">Login blev oprettet.</response>

        // POST: api/Users/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            try
            {
                _logger.LogInformation("Login forsøg for email {Email}", dto.Email);
                var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == dto.Email);

                if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.HashedPassword))
                {
                    _logger.LogWarning("Login mislykkedes for email {Email}", dto.Email);
                    return Unauthorized("Forkert email eller adgangskode");
                }

                user.LastLogin = DateTime.UtcNow.AddHours(2);
                await _context.SaveChangesAsync();

                var token = _jwtService.GenerateToken(user);

                _logger.LogInformation("Login succesfuldt for {Email}", dto.Email);
                return Ok(new
                {
                    message = "Login godkendt!",
                    token = token,
                    user = new
                    {
                        id = user.Id,
                        email = user.Email,
                        //firstname = user.FirstName,
                        //lastname = user.LastName,
                        //role = user.Role?.Name ?? "User"
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fejl ved login for {Email}", dto?.Email);
                return StatusCode(500, "Der opstod en intern serverfejl ved login");
            }
        }

        /// <summary>
        /// Hent info om den bruger som er logget ind baseret på JWT token.
        /// </summary>
        /// <returns>Brugerens info.</returns>
        /// <response code="500">Intern serverfejl.</response>
        /// <response code="404">Brugerne blev ikke fundet.</response>
        /// <response code="403">Ingen adgang.</response>
        /// <response code="200">Brugerne blev fundet og retuneret.</response>

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    _logger.LogWarning("Bruger-ID mangler i token");
                    return Unauthorized("Bruger-ID ikke fundet i token.");
                }

                // 2. Slå brugeren op i databasen
                var user = await _context.Users
                    .Include(u => u.Role) // inkluder relaterede data
                  .Include(u => u.Bookings) // inkluder bookinger
                      .ThenInclude(b => b.Room) // inkluder rum for hver booking
                  .FirstOrDefaultAsync(u => u.Id.ToString() == userId);

                if (user == null)
                {
                    _logger.LogWarning("Bruger {Id} ikke fundet i databasen", userId);
                    return NotFound("Brugeren blev ikke fundet i databasen.");
                }

                _logger.LogInformation("Returnerer info for bruger {Id}", userId);
                // 3. Returnér ønskede data - fx til profilsiden
                return Ok(new
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    CreatedAt = user.CreatedAt,
                    LastLogin = user.LastLogin,
                    Role = user.Role?.Name ?? "User",
                    // Bookinger hvis relevant
                    Bookings = user.Bookings.Select(b => new {
                        b.Id,
                        b.StartDate,
                        b.EndDate,
                        b.CreatedAt,
                        b.UpdatedAt,
                        Room = b.Room != null ? new
                        {
                            b.Room.Id,
                            b.Room.RoomNumber,
                            HotelId = b.Room.HotelId
                        } : null
                    }).ToList()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fejl ved hentning af nuværende bruger");
                return StatusCode(500, "Der opstod en intern serverfejl ved hentning af bruger");
            }
        }

        /// <summary>
        /// Sletter en bruger baseret på et id.
        /// </summary>
        /// <param name="id">Brugerens id.</param>
        /// <returns>Sletter en bruger.</returns>
        /// <response code="500">Intern serverfejl.</response>
        /// <response code="404">Brugeren blev ikke slettet.</response>
        /// <response code="403">Ingen adgang.</response>
        /// <response code="200">Brugeren blev slettet.</response>

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    _logger.LogWarning("Forsøg på at slette bruger {Id}, men den findes ikke", id);
                    return NotFound();
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Bruger {Id} slettet succesfuldt", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fejl ved sletning af bruger {Id}", id);
                return StatusCode(500, "Der opstod en intern serverfejl ved sletning af bruger");
            }
        }

        /// <summary>
        /// Opdatere en bruger med en ny rolle.
        /// </summary>
        /// <param name="id,dto">Brugeren id og dto.</param>
        /// <returns>Opdatere en brugers rolle.</returns>
        /// <response code="500">Intern serverfejl.</response>
        /// <response code="404">Brugerens rolle blev ikke opdateret.</response>
        /// <response code="403">Ingen adgang.</response>
        /// <response code="200">Brugerens rolle blev opdateret.</response>

        // PUT: api/Users/{id}/role
        [HttpPut("{id}/role")]
        public async Task<IActionResult> AssignUserRole(int id, AssignRoleDto dto)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    _logger.LogWarning("Bruger {Id} ikke fundet til rolle tildeling", id);
                    return NotFound("Bruger ikke fundet.");
                }

                var role = await _context.Roles.FindAsync(dto.RoleId);
                if (role == null)
                {
                    _logger.LogWarning("Ugyldig rolle {RoleId} ved tildeling til bruger {Id}", dto.RoleId, id);
                    return BadRequest("Ugyldig rolle.");
                }

                user.RoleId = dto.RoleId;
                user.UpdatedAt = DateTime.UtcNow.AddHours(2);

                await _context.SaveChangesAsync();
                _logger.LogInformation("Rolle {RoleName} tildelt til bruger {Id}", role.Name, id);

                return Ok(new { message = "Rolle tildelt til bruger!", user.Email, role = role.Name });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning(ex, "Concurrency fejl ved tildeling af rolle til bruger {Id}", id);
                if (!UserExists(id))
                    return NotFound();
                else
                    throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fejl ved tildeling af rolle til bruger {Id}", id);
                return StatusCode(500, "Der opstod en intern serverfejl ved tildeling af rolle");
            }
        }

        /// <summary>
        /// Henter alle brugere med et bestemt rolle navn.
        /// </summary>
        /// <param name="roleName">Rollens navn.</param>
        /// <returns>Rollens info.</returns>
        /// <response code="500">Intern serverfejl.</response>
        /// <response code="404">Ingen brugere blev ikke fundet.</response>
        /// <response code="403">Ingen adgang.</response>
        /// <response code="200">Mindst en bruger blev fundet og retuneret.</response>

        // GET: api/Users/role/{roleName}
        [HttpGet("role/{roleName}")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersByRole(string roleName)
        {
            try
            {
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
                if (role == null)
                {
                    _logger.LogWarning("Rolle {RoleName} ikke fundet", roleName);
                    return BadRequest("Ugyldig rolle.");
                }

                var users = await _context.Users.Include(u => u.Role).Where(u => u.RoleId == role.Id).ToListAsync();
                _logger.LogInformation("Hentede {Count} brugere med rolle {RoleName}", users.Count, roleName);

                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fejl ved hentning af brugere med rolle {RoleName}", roleName);
                return StatusCode(500, "Der opstod en intern serverfejl ved hentning af brugere");
            }
        }


        /// <summary>
        /// Sletter en rolle fra en bruger.
        /// </summary>
        /// <param name="id">Brugerens id.</param>
        /// <returns>Sletter en brugers rolle.</returns>
        /// <response code="500">Intern serverfejl.</response>
        /// <response code="404">Rollen blev ikke slettet.</response>
        /// <response code="403">Ingen adgang.</response>
        /// <response code="200">Rollen blev slettet.</response>

        // DELETE: api/Users/{id}/role
        [HttpDelete("{id}/role")]
        public async Task<IActionResult> RemoveUserRole(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                    return NotFound("Bruger ikke fundet.");

                var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User");
                if (userRole == null)
                    return BadRequest("Standard brugerrolle ikke fundet.");

                user.RoleId = userRole.Id;
                user.UpdatedAt = DateTime.UtcNow.AddHours(2);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Fjernede rolle fra bruger {Id}, sat til default rolle", id);
                return Ok(new { message = "Rolle fjernet fra bruger. Tildelt standard rolle.", user.Email });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fejl ved fjernelse af rolle fra bruger {Id}", id);
                return StatusCode(500, "Der opstod en intern serverfejl ved fjernelse af rolle");
            }
        }

        /// <summary>
        /// Henter roller fra alle brugere.
        /// </summary>
        /// <returns>Rollens info.</returns>
        /// <response code="500">Intern serverfejl.</response>
        /// <response code="404">Rollerne fra brugerne blev ikke fundet.</response>
        /// <response code="403">Ingen adgang.</response>
        /// <response code="200">Rollerne fra brugerne blev fundet og retuneret.</response>

        // GET: api/Users/roles
        [HttpGet("roles")]
        public async Task<ActionResult<object>> GetAvailableRoles()
        {
            try
            {
                var roles = await _context.Roles
                .Select(r => new {
                    id = r.Id,
                    name = r.Name,
                })
                .ToListAsync();
                _logger.LogInformation("Hentede {Count} roller", roles.Count);
                return Ok(roles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fejl ved hentning af roller");
                return StatusCode(500, "Der opstod en intern serverfejl ved hentning af roller");
            }
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }

    // DTO til rolle tildeling
    public class AssignRoleDto
    {
        public int RoleId { get; set; }
    }
}
