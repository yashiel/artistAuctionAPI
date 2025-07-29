using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Models;
using api.Services;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly ILogger<ReviewsController> _logger;

        public ReviewsController(IReviewService reviewService, ILogger<ReviewsController> logger)
        {
            _reviewService = reviewService ?? throw new ArgumentNullException(nameof(reviewService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/Reviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {
            try
            {
                _logger.LogInformation("Fetching all reviews from the database.");
                var reviews = await _reviewService.GetAllReviewsAsync();
                if (reviews == null || !reviews.Any())
                {
                    _logger.LogWarning("No reviews found in the database.");
                    return NotFound("No reviews found.");
                }
                _logger.LogInformation($"Found {reviews.Count()} reviews.");
                return Ok(reviews);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An error occurred while fetching reviews.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching reviews.");
            }
        }

        // GET: api/Reviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReview(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching review with ID {id} from the database.");
                var review = await _reviewService.GetReviewByIdAsync(id);
                if (review == null)
                {
                    _logger.LogWarning($"Review with ID {id} not found.");
                    return NotFound($"Review with ID {id} not found.");
                }
                return Ok(review);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An error occurred while fetching the review.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the review.");
            }
        }

        // PUT: api/Reviews/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReview(int id, Review review)
        {
            if (id != review.Id)
            {
                _logger.LogWarning($"Review ID {id} does not match the provided review ID {review.Id}.");
                return BadRequest("Review ID mismatch.");
            }
            try
            {
                _logger.LogInformation($"Updating review with ID {id}.");
                await _reviewService.UpdateReviewAsync(id, review);
                _logger.LogInformation($"Review with ID {id} updated successfully.");
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _reviewService.GetReviewByIdAsync(id) == null)
                {
                    _logger.LogWarning($"Review with ID {id} not found for update.");
                    return NotFound($"Review with ID {id} not found.");
                }
                else
                {
                    _logger.LogError("An error occurred while updating the review.");
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the review.");
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"An error occurred while updating review with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the review.");
            }
        }

        // POST: api/Reviews
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Review>> PostReview(Review review)
        {
            try
            {
                if (review == null)
                {
                    _logger.LogWarning("Received null review object.");
                    return BadRequest("Review cannot be null.");
                }
                _logger.LogInformation("Creating a new review.");
                    await _reviewService.AddReviewAsync(review);
                _logger.LogInformation($"Review with ID {review.Id} created successfully.");
                return CreatedAtAction(nameof(GetReview), new { id = review.Id }, review);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An error occurred while creating the review.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the review.");
            }
        }

        // DELETE: api/Reviews/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting review with ID {id}.");
                var review = await _reviewService.GetReviewByIdAsync(id);
                if (review == null)
                {
                    _logger.LogWarning($"Review with ID {id} not found for deletion.");
                    return NotFound($"Review with ID {id} not found.");
                }
                await _reviewService.DeleteReviewAsync(id);
                _logger.LogInformation($"Review with ID {id} deleted successfully.");
                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"An error occurred while deleting review with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the review.");
            }
        }

        // GET: api/Reviews/Product/{productId}
        [HttpGet("Product/{productId}")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsByProductId(int productId)
        {
            try
            {
                _logger.LogInformation($"Fetching reviews for product with ID {productId}.");
                var reviews = await _reviewService.GetReviewsByProductIdAsync(productId);
                if (reviews == null || !reviews.Any())
                {
                    _logger.LogWarning($"No reviews found for product with ID {productId}.");
                    return NotFound($"No reviews found for product with ID {productId}.");
                }
                return Ok(reviews);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An error occurred while fetching reviews by product ID.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching reviews by product ID.");
            }
        }

        // GET: api/Reviews/ByReviewerEmail/{email}
        [HttpGet("ByReviewerEmail/{email}")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsByCustomerEmail(string email)
        {
            try
            {
                _logger.LogInformation($"Fetching reviews for customer with email {email}.");
                var reviews = await _reviewService.GetReviewsByCustomerEmailAsync(email);
                if (reviews == null || !reviews.Any())
                {
                    _logger.LogWarning($"No reviews found for customer with email {email}.");
                    return NotFound($"No reviews found for customer with email {email}.");
                }
                return Ok(reviews);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An error occurred while fetching reviews by customer email.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching reviews by customer email.");
            }
        }

        // GET: api/Reviews/ByRating/{rating}
        [HttpGet("ByRating/{rating}")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsByRating(int rating)
        {
            try
            {
                _logger.LogInformation($"Fetching reviews with rating {rating}.");
                var reviews = await _reviewService.GetReviewsByRatingAsync(rating);
                if (reviews == null || !reviews.Any())
                {
                    _logger.LogWarning($"No reviews found with rating {rating}.");
                    return NotFound($"No reviews found with rating {rating}.");
                }
                return Ok(reviews);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An error occurred while fetching reviews by rating.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching reviews by rating.");
            }
        }
    }
}
