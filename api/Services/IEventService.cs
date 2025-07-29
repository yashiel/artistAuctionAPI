using api.Models;

namespace api.Services;

public interface IEventService
{
    Task<IEnumerable<Event>> GetAllEventsAsync();
    Task<Event> GetEventByIdAsync(int id);
    Task AddEventAsync(Event eventItem);
    Task UpdateEventAsync(int id, Event eventItem);
    Task DeleteEventAsync(int id);
    Task<IEnumerable<Event>> GetEventsByDateRangeAsync(DateTime startDate, DateTime endDate);
}