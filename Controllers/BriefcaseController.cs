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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BriefcaseDto>>> GetBriefcase()
        {
            return await _context.Briefcases
                .Select(d => new BriefcaseDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Color = d.Color.Name,
                }).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Briefcase>> PostBriefcase(BriefcaseDto briefcaseDto)
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
            };

            _context.Briefcases.Add(briefcase);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBriefcase", new { id = briefcase.Id }, briefcaseDto);
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

                return CreatedAtAction("GetBriefcase", new { id = briefcase.Id }, briefcaseDto);
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
        public async Task<IActionResult> DeleteBriefcase(int id)
        {
            var briefcase = await _context.Briefcases.FindAsync(id);
            if(briefcase == null)
            {
                return NotFound();
            }

            _context.Briefcases.Remove(briefcase);
            await _context.SaveChangesAsync();

            return NoContent();
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
        }
    }
}