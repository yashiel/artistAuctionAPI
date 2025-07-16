namespace api.Models;

public class EventArtist
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public Event? Event { get; set; }
    public int ArtistId { get; set; }
    public Artist? Artist { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}