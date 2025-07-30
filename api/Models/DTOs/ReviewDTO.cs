namespace api.Models.DTOs;

public class ReviewDTO
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string? ReviewerName { get; set; }
    public string? ReviewerEmail { get; set; }
    public string? Comment { get; set; } = string.Empty;
    public int Rating { get; set; }
}
public class CreateReviewDTO
{
    public int ProductId { get; set; }
    public string? ReviewerName { get; set; }
    public string? ReviewerEmail { get; set; }
    public string? Comment { get; set; } = string.Empty;
    public int Rating { get; set; }
}