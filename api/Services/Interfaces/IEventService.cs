using api.Models;

namespace api.Services.Interfaces;

public interface IEventService
{
    Task<IEnumerable<Event>> GetAllEventsAsync(int? page = null, int? pageSize = null);
    Task<Event> GetEventByIdAsync(int id);
    Task AddEventAsync(Event eventItem);
    Task UpdateEventAsync(int id, Event eventItem);
    Task DeleteEventAsync(int id);
    Task<IEnumerable<Event>> GetEventsByDateRangeAsync(DateTime startDate, DateTime endDate);
}