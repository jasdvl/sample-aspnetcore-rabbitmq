namespace PubLib.Backoffice.WebApp.Services;

public class NotificationReceivedEventArgs : EventArgs
{
    public string Message { get; set; }

    public string QueueName { get; set; }

    public NotificationReceivedEventArgs(string message, string queueName)
    {
        Message = message;
        QueueName = queueName;
    }
}

public class MessageService
{
    public event EventHandler<NotificationReceivedEventArgs> MessageReceived = delegate { };

    public void AddMessage(string message, string queueName)
    {
        var args = new NotificationReceivedEventArgs(message, queueName);
        MessageReceived?.Invoke(this, args);
    }
}
