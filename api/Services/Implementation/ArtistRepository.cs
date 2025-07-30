using api.Data;
using api.Models;
using api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Services.Implementation;

public class ArtistRepository : IArtistService
{
    private readonly AuctionDbContext _context;

    public ArtistRepository(AuctionDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Artist>> GetAllArtistsAsync(int? page = null, int? pageSize = null)
    {
        var query = _context.Artists
            .AsQueryable();

        // Apply pagination only if both page and pageSize are passed
        if (page.HasValue && pageSize.HasValue)
        {
            query = query.Skip((page.Value - 1) * pageSize.Value)
                .Take(pageSize.Value);
        }

        return await query.ToListAsync();
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