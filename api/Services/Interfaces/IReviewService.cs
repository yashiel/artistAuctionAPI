using api.Models;

namespace api.Services.Interfaces;

public interface IReviewService
{
    Task<IEnumerable<Review>> GetAllReviewsAsync(int? page = null, int? pageSize = null);
    Task<Review> GetReviewByIdAsync(int id);
    Task AddReviewAsync(Review review);
    Task UpdateReviewAsync(int id, Review review);
    Task DeleteReviewAsync(int id);
    Task<IEnumerable<Review>> GetReviewsByProductIdAsync(int productId, int? page = null, int? pageSize = null);
    Task<IEnumerable<Review>> GetReviewsByCustomerEmailAsync(string email, int? page = null, int? pageSize = null);
    Task<IEnumerable<Review>> GetReviewsByRatingAsync(int rating, int? page = null, int? pageSize = null);
}