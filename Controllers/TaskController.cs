using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tracker.Data;
using tracker.Models;

namespace tracker.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly StatusService _statusService;

        public TaskController(ApplicationDbContext context, StatusService statusService)
        {
            _context = context;
            _statusService = statusService;
        }

        [HttpGet("{dashboardId}")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasks(int dashboardId)
        {
            var tasks = await _context.Tasks
                .Where(t => t.DashboardId == dashboardId)
                .Include(t => t.Briefcase)
                .ThenInclude(b => b.Color)
                .Include(t => t.Status)
                .Include(t => t.Workers)
                .ToListAsync();

            var taskDtos = tasks.Select(task => new TaskDto
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                Status = task.Status.Name,
                CreatedAt = task.CreatedAt,
                DueDate = task.DueDate,
                DashboardId = task.DashboardId,
                Briefcase = new BriefcaseDto
                {
                    Id = task.Briefcase.Id,
                    Name = task.Briefcase.Name,
                    Color = task.Briefcase.Color.Name
                },
                Workers = task.Workers.Select(w => new WorkerDto
                {
                    Id = w.Id,
                    Name = w.Name,
                    Avatar = w.Avatar
                }).ToList()
            }).ToList();

            return Ok(taskDtos);
        }

        [HttpGet("briefcase/{briefcaseId}")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasksByBriefcase(int briefcaseId)
        {
            var tasks = await _context.Tasks
                .Where(t => t.BriefcaseId == briefcaseId)
                .Include(t => t.Briefcase)
                .ThenInclude(b => b.Color)
                .Include(t => t.Status)
                .Include(t => t.Workers)
                .ToListAsync();

            var taskDtos = tasks.Select(task => new TaskDto
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                Status = task.Status.Name,
                CreatedAt = task.CreatedAt,
                DueDate = task.DueDate,
                DashboardId = task.DashboardId,
                Briefcase = new BriefcaseDto
                {
                    Id = task.Briefcase.Id,
                    Name = task.Briefcase.Name,
                    Color = task.Briefcase.Color.Name
                },
                Workers = task.Workers.Select(w => new WorkerDto
                {
                    Id = w.Id,
                    Name = w.Name,
                    Avatar = w.Avatar
                }).ToList()
            }).ToList();

            return Ok(taskDtos);
        }

        [HttpPost]
        public async Task<ActionResult<TaskDto>> PostTask(TaskDto taskDto)
        {
            var status = await _statusService.GetStatusByNameAsync(taskDto.Status);
            if (status == null)
            {
                return BadRequest("Invalid color");
            }

            var task = new Models.Task
            {
                Name = taskDto.Name,
                Description = taskDto.Description,
                StatusId = status.Id,
                CreatedAt = taskDto.CreatedAt,
                DueDate = taskDto.DueDate,
                BriefcaseId = taskDto.Briefcase.Id,
                DashboardId = taskDto.DashboardId,
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            taskDto.Id = task.Id;

            return taskDto;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, TaskDto taskDto)
        {
            if (id != taskDto.Id)
            {
                return BadRequest();
            }

            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            var status = await _statusService.GetStatusByNameAsync(taskDto.Status);
            if (status == null)
            {
                return BadRequest("Invalid color");
            }

            task.Name = taskDto.Name;
            task.Description = taskDto.Description;
            task.StatusId = status.Id;
            task.DueDate = taskDto.DueDate;
            task.BriefcaseId = taskDto.Briefcase.Id;


            try
            {
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetTasks", new { id = task.Id }, taskDto);
            }
            catch
            {
                return NoContent();
            }

        }

        [HttpPost("addWorker")]
        public async Task<IActionResult> AddWorkerToTask(AddWorkerToTaskDto dto)
        {
            var task = await _context.Tasks
                .Include(t => t.Workers)
                .FirstOrDefaultAsync(t => t.Id == dto.TaskId);

            /*if (task == null)
            {
                return NotFound("Task not found");
            }*/

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == dto.UserId);

            /*if (user == null)
            {
                return NotFound("User not found");
            }*/

            if (!task.Workers.Contains(user))
            {
                task.Workers.Add(user);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("removeWorker")]
        public async Task<IActionResult> RemoveWorkerFromTask(AddWorkerToTaskDto dto)
        {
            var task = await _context.Tasks
                .Include(t => t.Workers)
                .FirstOrDefaultAsync(t => t.Id == dto.TaskId);

            if (task == null)
            {
                return NotFound("Task not found");
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == dto.UserId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            if (task.Workers.Contains(user))
            {
                task.Workers.Remove(user);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        public class TaskDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Status { get; set; }
            public DateOnly CreatedAt { get; set; }
            public DateOnly DueDate { get; set; }
            public BriefcaseDto Briefcase { get; set; }
            public List<WorkerDto> Workers { get; set; }
            public int DashboardId {  get; set; }
        }

        public class BriefcaseDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Color { get; set; }
        }

        public class WorkerDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Avatar { get; set; }
        }
        public class AddWorkerToTaskDto
        {
            public int TaskId { get; set; }
            public int UserId { get; set; }
        }

    }
}