using Product;
using System.Drawing;
using System.Text.Json;
using System.Media;
using UIElements;
using System.Text;

partial class RunStore {

    public static void CustomMenu()
    {
        string[] answers = new string[] {"Show Statistics of Each Week", "Flush Data" ,"Save & Exit", "Continue" };
        int mainChoice;
        while (true)
        {
            mainChoice = UI.GetChoice("Do You Want To:", answers);

            if (mainChoice == 0) ShowDataWeek();
            else if (mainChoice == 1) FlushFiles();
            else if (mainChoice == 2) ExitProgram();
            else if (mainChoice == 3) break;

            Console.Clear();

        }
    }

    private static void ShowDataWeek()
    {
        int category;

        while (true)
        {
            category = UI.GetCategory(Reports.Select(r => r.Name).ToArray(), true);
            if (category == UI.ESCAPE) break;
            Report report = Reports[category];


            Console.Clear();
            Console.WriteLine($"Current Customer Count: {report.CustomerCount.ToString()}");
            Console.WriteLine($"Current Profit: {Math.Round(report.TotalProfit, 2).ToString()}$");

            Console.Write("\nPress any Key to continue...");
            Console.ReadKey();

            int dayChoice = default;
            while (true)
            {
                dayChoice = UI.GetChoice(report.Name, report.Statistics
                    .Select(s => "Day " + s.OnDay.ToString()).ToArray(), true);

                Console.Clear();

                if (dayChoice == UI.ESCAPE) break;
                else
                {
                    report.Statistics[dayChoice].ShowStats();
                    var data = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + $"days/day {(DefaultValues.DayCountWeek * (category) + (dayChoice + 1)).ToString()}.txt");
                    Colorful.Console.WriteWithGradient(data, UI.Colors[Random.Shared.Next(UI.Colors.Length)], UI.Colors[Random.Shared.Next(UI.Colors.Length)], 6);
                    Console.ReadKey();
                    Console.Clear();
                    Colorful.Console.ReplaceAllColorsWithDefaults();
                }
            }


        }
    }

    private static void FlushFiles()
    {
        File.WriteAllText(DefaultValues.storePath, "null");
        File.WriteAllText(DefaultValues.reportsPath, "null");
        File.WriteAllText(DefaultValues.daysPath, "1");
        //Extra.DeleteFilesInDirectory(DefaultValues.logPath);

        Environment.Exit(0);
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
                    SimulateWeek();
                //CustomMenu();
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
            var lines = Reports
                .Last()
                .Statistics[dayIndex++]
                .BuyerMessages
                .Split("\r\n")
                .Distinct()
                .ToList();

            string divider = lines.First();
            lines = lines.Skip(1).ToList();
            
            Color startColor = UI.Colors[Random.Shared.Next(UI.Colors.Length)];
            Color endColor;
            do { endColor = UI.Colors[Random.Shared.Next(UI.Colors.Length)]; } 
            while (endColor == startColor);

            Colorful.Console.WriteAscii($"DAY {(MyStore.DayCount - DefaultValues.DayCountWeek + dayIndex - 1).ToString()}", startColor);
            Colorful.Console.WriteWithGradient(Extra.CreateString(divider, lines[lineIndex++], divider), startColor, endColor, 7);


            var breaks = Split(WORKHOURS * MSPERHOUR, lines.Count - 2).ToList();
            for (int i = 0; i < breaks.Count; i++)
            {
                //Thread.Sleep(breaks[i]);
                dateTime = dateTime.AddSeconds(breaks[i]);
                Colorful.Console.WriteWithGradient(Extra.CreateString((" ==> " + dateTime.ToLongTimeString()), divider, lines[lineIndex++], divider), startColor, endColor, 4);
            }

            Console.ReadKey();
            Console.Clear();
            Colorful.Console.ReplaceAllColorsWithDefaults();

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
        Reports = JsonSerializer.Deserialize<List<Report>>(File.ReadAllText(DefaultValues.reportsPath)) ?? new List<Report>();
        MyStore = JsonSerializer.Deserialize<Store>(File.ReadAllText(DefaultValues.storePath)) ?? new Store(0, 10, "MyStore", 200, 0.2, InitMyStore());

        try{ MyStore.DayCount = JsonSerializer.Deserialize<uint>(File.ReadAllText(DefaultValues.daysPath)); }
        catch {}

        try { Console.SetWindowSize(Console.WindowWidth / 10 * 12, Console.WindowHeight / 10 * 11); }
        catch { }

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
