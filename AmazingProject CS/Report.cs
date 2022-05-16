
//using Colorful;
//using Console = Colorful.Console;

class Report
{
    public uint CustomerCount { get; set; }
    public double TotalProfit { get; set; }
    public string Name { get; set; }
    //public List<List<Notification>> Notifications{ get; set; } = new List<List<Notification>>();
    public List<DayStats> Statistics{ get; set; } = new List<DayStats>();

    //public (List<Notification>, DayStats) this[int index]
    //{
    //    get => (Notifications[index], Stcatistics[index]);
    //}

    public static void ShowDetails((List<Notification> Notifications, DayStats Statistics) pair)
    {
        pair.Statistics.ShowStats();
        Console.Clear();
        if (pair.Notifications.Count == 0) 
            Console.WriteLine("No Notification Exists...");
        else 
            pair.Notifications.ForEach(notification => Console.WriteLine(notification));
                Console.ReadKey();
    }

}
