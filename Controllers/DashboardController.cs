using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tracker.Data;
using tracker.Models;
using static tracker.Controller.UserController;

namespace tracker.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ColorService _colorService;

        public DashboardController(ApplicationDbContext context, ColorService colorService)
        {
            _context = context;
            _colorService = colorService;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<DashboardDto>>> GetDashboardsByUserId(int userId)
        {
            var user = await _context.Users
                .Include(u => u.Dashboards)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var dashboardDtos = user.Dashboards.Select(d => new DashboardDto
            {
                Id = d.Id,
                Name = d.Name,
                Color = d.Color,
                Invite = d.Invite,
            }).ToList();

            return Ok(dashboardDtos);
        }

        [HttpGet("users/{dashboardId}")]
        public async Task<ActionResult<IEnumerable<DashboardDto>>> GetDashboardsUsers(int dashboardId)
        {
            var dashboard = await _context.Dashboards
        .Include(d => d.Users)
        .FirstOrDefaultAsync(d => d.Id == dashboardId);

            if (dashboard == null)
            {
                return NotFound("Dashboard not found");
            }

            var userDtos = dashboard.Users.Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Avatar = u.Avatar
            }).ToList();

            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DashboardDto>> GetDashboardById(int id)
        {
            var dashboard = await _context.Dashboards.FindAsync(id);

            if (dashboard == null)
            {
                return NotFound();
            }

            var dashboardDto = new DashboardDto
            {
                Id = dashboard.Id,
                Name = dashboard.Name,
                Color = dashboard.Color,
                Invite = dashboard.Invite,
            };

            return Ok(dashboardDto);
        }

        [HttpPost]
        public async Task<ActionResult<DashboardDto>> CreateDashboard(CreateDashboardDto createDashboardDto)
        {
            var user = await _context.Users.FindAsync(createDashboardDto.UserId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var dashboard = new Dashboard
            {
                Name = createDashboardDto.Name,
                Color = createDashboardDto.Color,
                Invite = GenerateInviteCode(),
                Users = new List<User> { user },
            };

            _context.Dashboards.Add(dashboard);
            await _context.SaveChangesAsync();

            var dashboardDto = new DashboardDto
            {
                Id = dashboard.Id,
                Name = dashboard.Name,
                Color = dashboard.Color,
                Invite = dashboard.Invite,
            };

            return CreatedAtAction(nameof(GetDashboardById), new { id = dashboard.Id }, dashboardDto);
        }

        [HttpPost("addUserToDashboard")]
        public async Task<IActionResult> AddUserToDashboard(AddUserToDashboardDto dto)
        {
            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null)
            {
                return NotFound("Ошибка - пользователь не найден");
            }

            var dashboard = await _context.Dashboards
                                          .Include(d => d.Users) 
                                          .FirstOrDefaultAsync(d => d.Invite == dto.InviteCode);

            if (dashboard == null)
            {
                return BadRequest("Дашборд с таким кодом не найден");
            }

            if (dashboard.Users.Any(u => u.Id == dto.UserId))
            {
                return BadRequest("Вы уже присоединились к этому дашборду");
            }

            dashboard.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Вы присоединились к дашборду");
        }

        private string GenerateInviteCode()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                .Replace("=", "")
                .Replace("+", "")
                .Replace("/", "");
        }

        public class DashboardDto
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public string Color { get; set; }
            public string Invite { get; set; }
        }

        public class CreateDashboardDto
        {
            public string Name { get; set; }
            public string Color { get; set; }
            public int UserId { get; set; } 
        }

        public class AddUserToDashboardDto
        {
            public string InviteCode { get; set; }
            public int UserId { get; set; }
        }
    }
}