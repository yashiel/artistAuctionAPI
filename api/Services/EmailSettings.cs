namespace api.Services;

public class EmailSettings
{
    public string SmtpServer { get; set; } = string.Empty;
    public int SmtpPort { get; set; } = 587;
    public string SmtpUser { get; set; } = string.Empty;
    public string SmtpPassword { get; set; } = string.Empty;
    public string FromEmail { get; set; } = string.Empty;

    public string ToEmail { get; set; } = string.Empty;
    public string ToName { get; set; } = "Customer";
    public string Subject { get; set; } = "Auction Notification";
    public string FromName { get; set; } = "Auction Service";
    //public bool UseSsl { get; set; } = true;
}