using Product;
using System.Drawing;
using System.Text.Json;
using System.Media;
using UIElements;

partial class RunStore {

    private static void CustomMenu()
    {
        string[] answers = new string[] { "Continue", "Show Statistics of Week", "Exit" };
        (int category, int choice) answer;

        while (true)
        {
            answer = UI.ChoiceMenuWithCategory(answers, Reports.Select(r => r.Name).ToArray());
            if (answer.choice == 1)
            {
                Report report = Reports[answer.category];

                Console.Clear();
                Console.WriteLine($"Current Customer Count: {report.CustomerCount.ToString()}");
                Console.WriteLine($"Current Profit: {Math.Round(report.TotalProfit, 2).ToString()}$");

                Console.WriteLine("\nPress any Key to continue...");
                Console.ReadKey();

                int dayChoice = default;
                while (true)
                {
                    dayChoice = UI.GetChoice("Choose the Day:", report.Statistics
                        .Select(s => "Day " + s.OnDay.ToString()).ToArray(), true);

                    Console.Clear();

                    if (dayChoice == -1) break;
                    else Report.ShowDetails(report[dayChoice]);
                }
            }
            else if (answer.choice == 0) break;
            else if (answer.choice == 2) ExitProgram();

            Console.Clear();

        }
    }

    private static void GameLoopStart()
    {
        while(MyStore.DayCount <= MAXDAYS + 1)
        {
            if (Extra.RandomChance()) MyStore.Quarantine();
            else
            {
                MyStore.ReStock();
                List<Customer> list = CreateCustomerList(GetCustomerCount());

                MyStore.StartSales(list);
                MyStore.NewDay();
                if (MyStore.DayCount % DefaultValues.DayCountWeek == 1 && Reports.Count != 0)
                    CustomMenu();
                    //SimulateWeek();
            }
        }

        CustomMenu();
    }

    static IEnumerable<int> Split(int number, int part)
    {

        if (number < part)
            Console.WriteLine("-1 ");

        else if (number % part == 0)
            for (int i = 0; i < part; i++)
                yield return (number / part);

        else
        {
            int zp = part - (number % part);
            int pp = number / part;

            for (int i = 0; i < part; i++)
                if (i >= zp)
                    yield return pp + 1;

                else
                    yield return pp;
        }
    }

    const int WORKHOURS = 8;
    const int MSPERHOUR = 3600;
    const int MSPERMIN = MSPERHOUR / 60;
    const int MSPERSEC = MSPERHOUR / 3600;

    static void SurroundText(string text, string surrounding)
    {
        Console.WriteLine(surrounding);
        Console.WriteLine(text);
        Console.WriteLine(surrounding);
    }

    private static void SimulateWeek()
    {
        int dayIndex = 0;
        while (dayIndex < DefaultValues.DayCountWeek)
        {
            DateTime dateTime = new DateTime(1,1,1,9,0,0);
            int lineIndex = 0;
            var lines = Reports.Last().Statistics[dayIndex++].BuyerMessages.Split("\r\n").ToList().Distinct().ToList();
            string divider = lines.First();
            lines = lines.Skip(1).ToList();

            Console.WriteLine(dateTime.ToLongTimeString());
            SurroundText(lines[lineIndex++], divider);

            var breaks = Split(WORKHOURS * MSPERHOUR, lines.Count - 2).ToList();
            for (int i = 0; i < breaks.Count; i++)
            {
                Thread.Sleep(breaks[i]);
                dateTime = dateTime.AddSeconds(breaks[i]);
                Console.WriteLine('\n' + dateTime.ToLongTimeString());
                SurroundText(lines[lineIndex++], divider);
            }

            Console.Clear();
            //Console.ReadKey();

        }

        Console.ReadKey();

    }

    public static List<Customer> CreateCustomerList(int count)
    {
        List<Customer> list = new List<Customer>();
        
        for (int i = 0; i < count; i++)
            list = list.Append(new Customer(MyStore.Rating)).ToList();

        return list;
    }

    private static void ExitProgram()
    {
        File.WriteAllText("store.json", JsonSerializer.Serialize(MyStore));
        File.WriteAllText("reports.json", JsonSerializer.Serialize(Reports));
        File.WriteAllText("day.txt", MyStore.DayCount.ToString());

        Environment.Exit(0);
    }

    private static int GetCustomerCount() => MyStore.Rating + ((MyStore.Rating > 95) ? Random.Shared.Next(0, (int)MyStore.DayCount * 3) : 0);


    private static void InitializeGame()
    {
        Reports = JsonSerializer.Deserialize<List<Report>>(File.ReadAllText("reports.json")) ?? new List<Report>();
        MyStore = JsonSerializer.Deserialize<Store>(File.ReadAllText("store.json")) ?? new Store(0, 10, "MyMyStore", 200, 0.2, InitMyStore());

        try{ MyStore.DayCount = JsonSerializer.Deserialize<uint>(File.ReadAllText("day.txt")); }
        catch {}

        Console.SetWindowSize(Console.WindowWidth / 10 * 12, Console.WindowHeight / 10 * 11);


    }

    static void StartMusic()
    {
        SoundPlayer player = new SoundPlayer();
        player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + @"ConsoleMusic.wav";
        player.PlayLooping();
    }

    static Dictionary<string, Stend> InitMyStore()
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

}
