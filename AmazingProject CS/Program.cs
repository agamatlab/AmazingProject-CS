
using Product;
using System.Drawing;
using System.Text.Json;
using System.Media;
using UIElements;

partial class RunStore
{


    public const int MAXDAYS = 98;
    
    public static Store MyStore;
    public static List<Report> Reports{ get; set; }




    static void Main(string[] args)
    {
        //StartMusic();
        UI.ChangeEncoding(System.Text.Encoding.Unicode);
        InitializeGame();
        GameLoop();
    }
}