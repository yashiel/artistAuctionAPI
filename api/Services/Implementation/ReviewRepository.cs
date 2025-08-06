using api.Data;
using api.Models;
using api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Services.Implementation;

public class ReviewRepository : IReviewService
{
    private readonly AuctionDbContext _context;
    public ReviewRepository(AuctionDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Review>> GetAllReviewsAsync(int? page = null, int? pageSize = null)
    {
        var query = _context.Reviews
            .AsQueryable();

        // Apply pagination only if both page and pageSize are passed
        if (page.HasValue && pageSize.HasValue)
        {
            query = query.Skip((page.Value - 1) * pageSize.Value)
                .Take(pageSize.Value);
        }

        return await query.ToListAsync();
    }
    public async Task<Review> GetReviewByIdAsync(int id)
    {
        return await _context.Reviews.FindAsync(id);
    }
    public async Task AddReviewAsync(Review review)
    {
        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateReviewAsync(int id, Review review)
    {
        _context.Entry(review).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
    public async Task DeleteReviewAsync(int id)
    {
        var review = await _context.Reviews.FindAsync(id);
        if (review != null)
        {
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
        }
    }
    public async Task<IEnumerable<Review>> GetReviewsByProductIdAsync(int productId, int? page = null, int? pageSize = null)
    {
        IOrderedQueryable<Review> query = _context.Reviews
            .Where(r => r.ProductId == productId)
            .OrderBy(r => r.Id); // Always order before skip/take

        if (page.HasValue && pageSize.HasValue)
        {
            var skip = (page.Value - 1) * pageSize.Value;
            query = (IOrderedQueryable<Review>)query.Skip(skip).Take(pageSize.Value);
        }

        return await query.ToListAsync();
    }
    public async Task<IEnumerable<Review>> GetReviewsByCustomerEmailAsync(string email, int? page = null, int? pageSize = null)
    {
        IOrderedQueryable<Review> query = _context.Reviews
            .Where(r => r.ReviewerEmail == email)
            .OrderBy(r => r.Id); // Always order before skip/take

        if (page.HasValue && pageSize.HasValue)
        {
            var skip = (page.Value - 1) * pageSize.Value;
            query = (IOrderedQueryable<Review>)query.Skip(skip).Take(pageSize.Value);
        }

        return await query.ToListAsync();
    }
    public async Task<IEnumerable<Review>> GetReviewsByRatingAsync(int rating, int? page = null, int? pageSize = null)
    {
        IOrderedQueryable<Review> query = _context.Reviews
            .Where(r => r.Rating == rating)
            .OrderBy(r => r.Id); // Always order before skip/take

        if (page.HasValue && pageSize.HasValue)
        {
            var skip = (page.Value - 1) * pageSize.Value;
            query = (IOrderedQueryable<Review>)query.Skip(skip).Take(pageSize.Value);
        }

        return await query.ToListAsync();
    }
}