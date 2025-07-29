using api.Models;
using Microsoft.EntityFrameworkCore;
using api.Data;

namespace api.Repositories
{
    public class ArtistRepository
    {
        private readonly AuctionDbContext _context;
        public ArtistRepository(AuctionDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
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
}

