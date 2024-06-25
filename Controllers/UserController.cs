using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using tracker.Data;
using tracker.Migrations;
using tracker.Models;
using static tracker.Controller.BriefcaseController;
using static tracker.Controller.TaskController;

namespace tracker.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public UserController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<User>> GetUsers()
        {
            var user = await _context.Users
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Avatar = u.Avatar,
                })
                .ToListAsync();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<User>> GetUserById(int id) 
        { 
            var user = await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new User
                { 
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Password = u.Password,
                    Avatar = u.Avatar,
                })
                .FirstOrDefaultAsync();

            if(user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }


        [HttpPost("login")]
        public async Task<ActionResult<User>> GetUserByEmailAndPassword([FromBody] LoginModel loginModel)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == loginModel.Email);

            if (user == null || user.Password != loginModel.Password)
            {
                return NotFound("Ќеверный email или пароль");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                Token = tokenString,
                Id = user.Id,
                Name = user.Name,
                Avatar = user.Avatar,
            });
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserNotId userNotId)
        {
            if (await EmailExists(userNotId.Email))
            {
                return Conflict("Email уже используетс€.");
            }

            User user = new User()
            {
                Name = userNotId.Name,
                Avatar = userNotId.Avatar,
                Email = userNotId.Email,
                Password = userNotId.Password,
            };

            _context.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserById", new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserDto user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            var userDb = await _context.Users.FindAsync(id);

            if (userDb == null)
            {
                return NotFound();
            }

            userDb.Name = user.Name;
            userDb.Avatar = user.Avatar;
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserById", new { id = user.Id }, userDb);
        }

        public class LoginModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        private async Task<bool> EmailExists(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public class UserDto
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public string Avatar { get; set; }
        }

        public class UserNotId
        {
            public string Name { get; set; }

            public string Avatar { get; set; }

            public string Email { get; set; }

            public string Password { get; set; }
        }
    }
}