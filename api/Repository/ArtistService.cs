using api.Data;
using api.Models;
using api.Services;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class ArtistService : IArtistService
{
    private readonly AuctionDbContext _context;

    public ArtistService(AuctionDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Artist>> GetAllArtistsAsync()
    {
        return await _context.Artists.ToListAsync();
    }

    public async Task<Artist> GetArtistByIdAsync(int id)
    {
        return await _context.Artists.FindAsync(id);
    }

    public async Task AddArtistAsync(Artist artist)
    {
        _context.Artists.Add(artist);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateArtistAsync(int id, Artist artist)
    {
        _context.Entry(artist).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteArtistAsync(int id)
    {
        var artist = await _context.Artists.FindAsync(id);
        if (artist != null)
        {
            _context.Artists.Remove(artist);
            await _context.SaveChangesAsync();
        }
    }
}