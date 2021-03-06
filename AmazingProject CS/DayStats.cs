using ConsoleTables;
using System.Drawing;

class DayStats
{

    public uint OnDay { get; set; } = default;
    public string BeforeStock { get; set; }
    public string AfterStock { get; set; }
    public string BuyerMessages { get; set; }


    public DayStats() { }
    public DayStats(uint dayCount) => OnDay = dayCount;

    public void ShowStats()
    {

        Colorful.Console.WriteWithGradient(BeforeStock, Color.Yellow, Color.Fuchsia, 14);
        Console.WriteLine("\n\n");
        Colorful.Console.WriteWithGradient(AfterStock, Color.Yellow, Color.Fuchsia, 14);

        Console.ReadKey();
        Console.Clear();
        Colorful.Console.ReplaceAllColorsWithDefaults();

        Colorful.Console.WriteWithGradient(BuyerMessages, Color.Lime, Color.Cyan, 8);
        Console.ReadKey();
        Console.Clear();

        Colorful.Console.ReplaceAllColorsWithDefaults();
    }

}

