using api.Models;

namespace api.Services;

public interface IArtistService
{
    Task<IEnumerable<Artist>> GetAllArtistsAsync();
    Task<Artist> GetArtistByIdAsync(int id);
    Task AddArtistAsync(Artist artist);
    Task UpdateArtistAsync(int id, Artist artist);
    Task DeleteArtistAsync(int id);
}