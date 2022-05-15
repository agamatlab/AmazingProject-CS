
using Product;
using System.Drawing;
using System.Text.Json;
using System.Media;
using UIElements;

partial class RunStore
{


    private const int MAXDAYS = 98;

    private static uint _days = default;
    public static uint DayCount
    {
        get { return _days; }
        set { _days = value; if (value % DefaultValues.DayCountWeek == 1 && value != 1) CustomMenu(); }
    }

    
    public static Store MyStore;
    public static List<Report> Reports{ get; set; }




    static void Main(string[] args)
    {
        StartMusic();
        UI.ChangeEncoding(System.Text.Encoding.Unicode);
        InitializeGame();

        GameLoopStart();
    }
}