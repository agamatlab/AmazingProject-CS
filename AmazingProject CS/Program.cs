
using Product;
using System.Drawing;
using System.Text.Json;
using System.Media;
//using Colorful;
//using Console = Colorful.Console;

class RunStore
{
    static void StartMusic()
    {
        SoundPlayer player = new SoundPlayer();
        player.SoundLocation = @"C:\Users\user\source\repos\AmazingProject CS\AmazingProject CS\bin\Debug\net6.0\ConsoleMusic.wav";
        player.PlayLooping();
    }
    static void InitStore()
    {
        Store.Stends.Add(DefaultValues.VegetableList.Tomato.ToString(),
        new Stend()
        {
            BuyPrice = .42,
            SellPrice = 1.86,
            AvrageWeight = .132,
            Product = new Vegetable(.132)
            {
                Condition = Conditions.NonDefined,
                Name = DefaultValues.VegetableList.Tomato.ToString(),
            }
        }
        );


        Store.Stends.Add(DefaultValues.VegetableList.Cucumber.ToString(),
        new Stend()
        {
            BuyPrice = 1.16,
            SellPrice = 2.39,
            AvrageWeight = .214,
            Product = new Vegetable(.214)
            {
                Condition = Conditions.NonDefined,
                Name = DefaultValues.VegetableList.Cucumber.ToString(),
            }
        }
        );


        Store.Stends.Add(DefaultValues.VegetableList.Union.ToString(),
        new Stend()
        {
            BuyPrice = .35,
            SellPrice = 1.49,
            AvrageWeight = .112,
            Product = new Vegetable(.112)
            {
                Condition = Conditions.NonDefined,
                Name = DefaultValues.VegetableList.Union.ToString(),
            }
        }
        );

        Store.Stends.Add(DefaultValues.VegetableList.Garlic.ToString(),
        new Stend()
        {
            BuyPrice = .86,
            SellPrice = 2.59,
            AvrageWeight = .33,
            Product = new Vegetable(.33)
            {
                Condition = Conditions.NonDefined,
                Name = DefaultValues.VegetableList.Garlic.ToString(),
            }
        }
        );

        Store.Stends.Add(DefaultValues.VegetableList.Grape.ToString(),
        new Stend()
        {
            BuyPrice = 1.12,
            SellPrice = 2.99,
            AvrageWeight = .48,
            Product = new Vegetable(.48)
            {
                Condition = Conditions.NonDefined,
                Name = DefaultValues.VegetableList.Grape.ToString(),
            }
        }
        );

        Store.Stends.Add(DefaultValues.VegetableList.Pomegranate.ToString(),
        new Stend()
        {
            BuyPrice = 1.5,
            SellPrice = 4.39,
            AvrageWeight = .312,
            Product = new Vegetable(.312)
            {
                Condition = Conditions.NonDefined,
                Name = DefaultValues.VegetableList.Pomegranate.ToString(),
            }
        }
        );
    }

    public static uint DayCount { get; set; } = default;
    public static List<Notification> Notifications{ get; set; } = new List<Notification>();
    public static Store Store = new Store(0, 10, "MyStore", 100, 0.2);

    static void Main(string[] args)
    {
        Console.SetWindowSize(Console.WindowWidth / 10 * 11, Console.WindowHeight/ 10 * 11);
        // StartMusic();
        InitStore();

        for (int k = 0; k < 10; k++)
        {
            Store.NewDay();

            List<Customer> list = new List<Customer>();
            int loops = Store.Rating;

            for (int i = 0; i < loops; i++)
                list = list.Append(new Customer(Store.Rating)).ToList();

            Store.StartSales(list);

            Console.Clear();
        }

        foreach (var item in Notifications)
            Console.WriteLine(item);
    }   

}