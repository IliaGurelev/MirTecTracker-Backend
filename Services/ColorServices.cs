using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using tracker.Data;
using tracker.Models;

public class ColorService
{
    private readonly ApplicationDbContext _context;

    public ColorService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Color> GetColorByNameAsync(string colorName)
    {
        return await _context.Colors.FirstOrDefaultAsync(c => c.Name == colorName);
    }
}