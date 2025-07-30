using api.Data;
using api.Models;
using api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Services.Implementation;

public class EventRepository : IEventService
{
    private readonly AuctionDbContext _context;
    public EventRepository(AuctionDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Event>> GetAllEventsAsync(int? page = null, int? pageSize = null)
    {
        var query = _context.Events
            .AsQueryable();

        // Apply pagination only if both page and pageSize are passed
        if (page.HasValue && pageSize.HasValue)
        {
            query = query.Skip((page.Value - 1) * pageSize.Value)
                .Take(pageSize.Value);
        }

        return await query.ToListAsync();
    }
    public async Task<Event> GetEventByIdAsync(int id)
    {
        return await _context.Events.FindAsync(id);
    }
    public async Task AddEventAsync(Event eventItem)
    {
        _context.Events.Add(eventItem);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateEventAsync(int id, Event eventItem)
    {
        _context.Entry(eventItem).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
    public async Task DeleteEventAsync(int id)
    {
        var eventItem = await _context.Events.FindAsync(id);
        if (eventItem != null)
        {
            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();
        }
    }
    public async Task<IEnumerable<Event>> GetEventsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Events
            .Where(e => e.StartDate >= startDate && e.EndDate <= endDate)
            .ToListAsync();
    }
}