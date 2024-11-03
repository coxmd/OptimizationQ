namespace QueueManagementSystem.MVC.Models.Smtp
{
    public class EmailAttachmentModel
    {
        public string? FileName { get; set; }
        public byte[]? Content { get; set; }
        public string? ContentType { get; set; }

    }
}
