using ConsoleTables;
using System.Drawing;

class DayStats
{
    public string BeforeStock { get; set; }
    public string AfterStock { get; set; }
    public string BuyerMessages { get; set; }

    public void ShowStats()
    {
        Colorful.Console.WriteWithGradient(BeforeStock, Color.Yellow, Color.Fuchsia, 14);
        Console.WriteLine("\n\n");
        Colorful.Console.WriteLine(BuyerMessages, Color.White);
        Console.WriteLine("\n\n");
        Colorful.Console.WriteWithGradient(AfterStock, Color.Yellow, Color.Fuchsia, 14);

        Console.ReadKey();
        Colorful.Console.ReplaceAllColorsWithDefaults();
    }

}

