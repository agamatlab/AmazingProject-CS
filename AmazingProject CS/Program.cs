
using Product;
using System.Drawing;
using System.Text.Json;
using System.Media;
using UIElements;

class Program
{



    static void Main(string[] args)
    {
        RunStore.StartMusic();
        UI.ChangeEncoding(System.Text.Encoding.Unicode);
        RunStore.InitializeGame();
        RunStore.GameLoop();
    }
}