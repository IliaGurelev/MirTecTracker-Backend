using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tracker.Data;
using tracker.Models;

namespace tracker.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiaryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DiaryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Diary>>> GetDiary()
        {
            return await _context.Diary
                .Select(d => new Diary
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    TimeStart = d.TimeStart,
                    DueDate = d.DueDate,
                }).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Diary>> PostDiary(Diary diary)
        {
            _context.Diary.Add(diary);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDiary", new { id = diary.Id }, diary);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiary(int id)
        {
            var diary = await _context.Diary.FindAsync(id);
            if (diary == null)
            {
                return NotFound();
            }

            _context.Diary.Remove(diary);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}