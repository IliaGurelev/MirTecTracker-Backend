using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tracker.Data;
using tracker.Models;

namespace tracker.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class BriefcaseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ColorService _colorService;

        public BriefcaseController(ApplicationDbContext context, ColorService colorService)
        {
            _context = context;
            _colorService = colorService;
        }

        [HttpGet("{dashboardId}")]
        public async Task<ActionResult<IEnumerable<BriefcaseDto>>> GetBriefcase(int dashboardId)
        {
            return await _context.Briefcases
                .Where(d => d.DashboardId == dashboardId)
                .Select(d => new BriefcaseDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Color = d.Color.Name,
                    DashboardId = d.DashboardId,
                }).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<BriefcaseDto>> PostBriefcase(BriefcaseDto briefcaseDto)
        {
            var color = await _colorService.GetColorByNameAsync(briefcaseDto.Color);
            if (color == null)
            {
                return BadRequest("Invalid color");
            }

            var briefcase = new Briefcase
            {
                Name = briefcaseDto.Name,
                ColorId = color.Id,
                DashboardId = briefcaseDto.DashboardId,
            };

            _context.Briefcases.Add(briefcase);
            await _context.SaveChangesAsync();

            var responseDto = new BriefcaseDto()
            {
                Id =briefcase.Id,
                Name =briefcaseDto.Name,
                Color =briefcaseDto.Color,
                DashboardId = briefcaseDto.DashboardId,
            };

            return responseDto;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBriefcase(int id, BriefcaseDto briefcaseDto)
        {
            if (id != briefcaseDto.Id)
            {
                return BadRequest();
            }

            var briefcase = await _context.Briefcases.FindAsync(id);

            if (briefcase == null)
            {
                return NotFound();
            }

            var color = await _colorService.GetColorByNameAsync(briefcaseDto.Color);

            if (color == null)
            {
                return BadRequest("Invalid color");
            }

            briefcase.Name = briefcaseDto.Name;
            briefcase.ColorId = color.Id;

            try
            {
                await _context.SaveChangesAsync();

                return Ok(briefcaseDto);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await BriefcaseExist(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDashbo(int id)
        {
            var briefcase = await _context.Briefcases.FindAsync(id);
            if(briefcase == null)
            {
                return NotFound();
            }

            _context.Briefcases.Remove(briefcase);
            await _context.SaveChangesAsync();

            return Ok(id);
        }

        private async Task<bool> BriefcaseExist(int id)
        {
            return await _context.Briefcases.AnyAsync(e => e.Id == id);
        }

        public class BriefcaseDto
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public string Color { get; set; }

            public int DashboardId { get; set; }
        }
    }
}