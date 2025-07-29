using api.Models;

namespace api.Services;

public interface IReviewService
{
    Task<IEnumerable<Review>> GetAllReviewsAsync();
    Task<Review> GetReviewByIdAsync(int id);
    Task AddReviewAsync(Review review);
    Task UpdateReviewAsync(int id, Review review);
    Task DeleteReviewAsync(int id);
    Task<IEnumerable<Review>> GetReviewsByProductIdAsync(int productId);
    Task<IEnumerable<Review>> GetReviewsByCustomerEmailAsync(string email);
    Task<IEnumerable<Review>> GetReviewsByRatingAsync(int rating);
}