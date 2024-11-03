using static QueueManagementSystem.MVC.Components.ServicePointDialog;

namespace QueueManagementSystem.MVC.Models
{
    public class ServicePointDialogResult
    {
        public int ServicePointId { get; set; }
        public string Status { get; set; }
        public DialogAction Action { get; set; }
    }

    public enum DialogAction
    {
        SignIn,
        SignOut,
        Update
    }
}
