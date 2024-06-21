using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using tracker.Data;
using tracker.Models;

public class StatusService
{
    private readonly ApplicationDbContext _context;

    public StatusService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Status> GetStatusByNameAsync(string statusName)
    {
        return await _context.Status.FirstOrDefaultAsync(c => c.Name == statusName);
    }
}