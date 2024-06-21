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
        public async Task<ActionResult<IEnumerable<DiaryDto>>> GetDiary([FromQuery] int userId)
        {
            var diaries = await _context.Diary
                .Where(d => d.UserId == userId)
                .Select(d => new DiaryDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    TimeStart = d.TimeStart,
                    DueDate = d.DueDate,
                }).ToListAsync();

            return diaries;
        }

        [HttpPost]
        public async Task<ActionResult<DiaryDto>> PostDiary([FromQuery] int userId, DiaryDto diary)
        {
            var diaryDb = new Diary
            {
                Id = diary.Id,
                Name = diary.Name,
                Description = diary.Description,
                TimeStart = diary.TimeStart,
                DueDate = diary.DueDate,
                UserId = userId,
            };

            _context.Diary.Add(diaryDb);
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

        public class DiaryDto
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }

            public TimeOnly TimeStart { get; set; }

            public DateOnly DueDate { get; set; }
        }
    }
}