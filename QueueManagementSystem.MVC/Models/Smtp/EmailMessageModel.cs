namespace QueueManagementSystem.MVC.Models.Smtp
{
    public class EmailMessageModel
    {
        public string? Sender { get; set; }
        public string? SenderName { get; set; }
        public string[]? Recipients { get; set; }
        public string? Body { get; set; }
        public string? Subject { get; set; }
        public List<EmailAttachmentModel> Attachments { get; set; } = new List<EmailAttachmentModel>();

    }
}
