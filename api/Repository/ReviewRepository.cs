using api.Data;
using api.Models;
using api.Services;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class ReviewRepository : IReviewService
{
    private readonly AuctionDbContext _context;
    public ReviewRepository(AuctionDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Review>> GetAllReviewsAsync()
    {
        return await _context.Reviews.ToListAsync();
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
    public async Task<IEnumerable<Review>> GetReviewsByProductIdAsync(int productId)
    {
        return await _context.Reviews
            .Where(r => r.ProductId == productId)
            .ToListAsync();
    }
    public async Task<IEnumerable<Review>> GetReviewsByCustomerEmailAsync(string email)
    {
        return await _context.Reviews
                .Where(r => r.ReviewerEmail == email)
            .ToListAsync();
    }
    public async Task<IEnumerable<Review>> GetReviewsByRatingAsync(int rating)
    {
        return await _context.Reviews
            .Where(r => r.Rating == rating)
            .ToListAsync();
    }
}