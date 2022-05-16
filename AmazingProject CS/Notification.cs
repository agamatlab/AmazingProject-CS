public class Notification
{
    public Notification(string message, uint onDay)
    {
        Message = message;
        Time = DateTime.Now;
        OnDay = onDay;
    }

    public Notification(string message, DateTime time, uint onDay)
    {
        Message = message;
        Time = time;
        OnDay = onDay;
    }

    public Notification() { }

    public string? Message { get; set; }
    public DateTime Time { get; set; }
    public uint OnDay { get; set; } = default;

    public override string ToString() => $"On Day: {OnDay} At Time: {Time}\nMessage: {Message}";


}