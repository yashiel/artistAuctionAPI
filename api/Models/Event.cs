using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace api.Models;

public class Event
{
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Title { get; set; }

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Location { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
    [JsonIgnore]
    public ICollection<EventArtist>? EventArtists { get; set; }

}