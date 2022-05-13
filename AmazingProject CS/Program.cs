
using Product;
using System.Drawing;
using System.Text.Json;
using System.Media;
using UIElements;

class RunStore
{

    static void StartMusic()
    {
        SoundPlayer player = new SoundPlayer();
        player.SoundLocation = @"C:\Users\user\source\repos\AmazingProject CS\AmazingProject CS\bin\Debug\net6.0\ConsoleMusic.wav";
        player.PlayLooping();
    }

    static Dictionary<string, Stend> InitStore()
    {
        Dictionary<string, Stend> Stends = new Dictionary<string, Stend>(); 
        Stends.Add(DefaultValues.VegetableList.Tomato.ToString(),
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


        Stends.Add(DefaultValues.VegetableList.Cucumber.ToString(),
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


        Stends.Add(DefaultValues.VegetableList.Union.ToString(),
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

        Stends.Add(DefaultValues.VegetableList.Garlic.ToString(),
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

        Stends.Add(DefaultValues.VegetableList.Grape.ToString(),
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

        Stends.Add(DefaultValues.VegetableList.Pomegranate.ToString(),
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

        return Stends;
    }

    public static uint DayCount { get; set; } = 1;
    public static Store Store = new Store(0, 10, "MyStore", 100, 0.2, InitStore());
    public static List<Report> Reports{ get; set; } = new List<Report>();

    static void Main(string[] args)
    {
        UI.ChangeEncoding(System.Text.Encoding.Unicode);

        Console.SetWindowSize(Console.WindowWidth / 10 * 11, Console.WindowHeight/ 10 * 11);
        // StartMusic();
        InitStore();


        for (int k = 0; k <= 105; k++)
        {


            if (Random.Shared.Next(0, 100) == 0) { Console.WriteLine("Quarantine Called...");  Store.Quarantine(); }
            else
            {
                if(DayCount % 7 == 1 && DayCount != 1)
                    CustomMenu();


                List<Customer> list = new List<Customer>();
                int loops = Store.Rating;

                for (int i = 0; i < loops; i++)
                    list = list.Append(new Customer(Store.Rating)).ToList();

                Store.StartSales(list);
                Store.NewDay();
            }


            //Console.ReadKey();
             //Console.Clear();

        }


        // foreach (var item in Notifications)
        //     Console.WriteLine(item);
    }

    private static void CustomMenu()
    {
        string[] answers = new string[] { "Continue" , "Show Statistics of Week", "Exit" };

        while (true)
        {
            //int choice = UI.GetChoice("Do you Want To:", answers);
            (int category, int choice) answer = UI.ChoiceMenuWithCategory(answers, Reports.Select(r => r.Name).ToArray());
            if (answer.choice == 1) {

                while (true)
                {
                    var report = Reports[answer.category];
                    Console.Clear(); 
                    Console.WriteLine($"Current Customer Count: {report.CustomerCount.ToString()}");
                    Console.WriteLine($"Current Profit: {Math.Round(report.TotalProfit,2).ToString()}$");
                    
                    Console.WriteLine("\nPress any Key to continue...");
                    Console.ReadKey();

                    int dayChoice = UI.GetChoice("Choose the Day:", report.Statistics
                        .Select(s => "Day " + s.OnDay.ToString()).ToArray(), true);

                    Console.Clear();

                    if (dayChoice == -1) break;
                    else Report.ShowDetails(report[dayChoice]);
                }
            }
            else if (answer.choice == 0) break;
            else if (answer.choice == 2)
            {
                var notifations = Reports[answer.category].Notifications; 
                foreach (var notf in notifations)
                    Console.WriteLine(notf);
                Console.ReadKey();
            }
            else if (answer.choice == 3) ExitProgram();

            Console.Clear();

        }
    }

    private static void ExitProgram()
    {
        Environment.Exit(0);
    }
}