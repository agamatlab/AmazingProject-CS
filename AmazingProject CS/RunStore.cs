using Product;
using System.Drawing;
using System.Text.Json;
using System.Media;
using UIElements;
using System.Text;
using System.Runtime.InteropServices;

partial class RunStore {
    
    public const int MAXDAYS = 98;

    public static Store MyStore;
    public static List<Report> Reports { get; set; }
    
    
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
        var names = Reports.Select(r => r.Name).ToArray();
        while (true)
        {
            category = UI.GetCategory(names, true);
            if (category == UI.ESCAPE) break;
            Report report = Reports[category];


            Console.Clear();
            Console.WriteLine($"Current Customer Count: {report.CustomerCount.ToString()}");
            Console.WriteLine($"Current Profit: {Math.Round(report.TotalProfit, 2).ToString()}$");

            Console.Write("\nPress any Key to continue...");
            Console.ReadKey();

            int dayChoice = default;

            var days = report.Statistics
                    .Select(s => "Day " + s.OnDay.ToString())
                    .ToArray();

            while (true)
            {
                dayChoice = UI.GetChoice(report.Name, days, true);

                Console.Clear();

                if (dayChoice == UI.ESCAPE) break;
                else
                {
                    report.Statistics[dayChoice].ShowStats();
                    try
                    {
                        string data = Extra.ReadFile(DefaultValues.logPath + $"/day {(DefaultValues.DayCountWeek * (category) + (dayChoice + 1)).ToString()}.txt");
                        Colorful.Console.WriteWithGradient(data, UI.Colors.GetRandom(), UI.Colors.GetRandom(), 6);
                    

                    }
                    catch (IOException)
                    {
                        Console.WriteLine("Log Information can not accesed, as FILE ");
                    }
                    finally
                    {
                        Console.ReadKey();
                        UI.ResetConsole();

                    }

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

    public static void GameLoop()
    {
        while(MyStore.DayCount <= MAXDAYS + 1)
        {
            if (MyStore.DayCount % DefaultValues.DayCountWeek == 1 && Reports.Count != 0)
                CustomMenu();

            if (!MyStore.IsQuarantine() && Extra.RandomChance()) MyStore.StartQuarantine();
            
            if (MyStore.IsQuarantine()) MyStore.Quarantine();
            else
            {
                MyStore.ReStock();
                MyStore.StartSales(CreateCustomerList(GetCustomerCount()));
            }

            SimulateLastDay();
            MyStore.NewDay();
        }

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

    private static void SimulateLastDay()
    {
        // Heftedeki satishlari gostermek meqsedi ile tamamile 'ruchnoy' kodlarla yazilmish functiondir.

        DateTime dateTime = new DateTime(1,1,1,9,0,0); // default time | TimeOnly'de lazimi methodlar olmadigindan,
                                      // Datetime istifade olunub.
        int lineIndex = 0;
        var lines = MyStore.CurrentReport
            .Statistics[Store.CalculateIndex(MyStore.DayCount)]
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

        Colorful.Console.WriteAscii($"DAY {MyStore.DayCount.ToString()}", startColor);
        Colorful.Console.WriteWithGradient(Extra.CreateString(divider, lines[lineIndex++], divider), startColor, endColor, 7);
        int customerCount = lines.Count - 2;

        if (lines.Count > 2)
        {

            var breaks = Split(WORKHOURS * MSPERHOUR, customerCount).ToList();
            for (int i = 0; i < breaks.Count; i++)
            {
                //Thread.Sleep(breaks[i]);
                dateTime = dateTime.AddSeconds(breaks[i]);
                Colorful.Console.WriteWithGradient(Extra.CreateString((" ==> " + dateTime.ToLongTimeString()), divider, lines[lineIndex++], divider), startColor, endColor, 4);
            }

            Colorful.Console.WriteWithGradient($"\nCount: {customerCount.ToString()}", startColor, endColor);
        }


        Console.ReadKey();
        Console.Clear();
        Colorful.Console.ReplaceAllColorsWithDefaults();


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
        File.WriteAllText(DefaultValues.storePath, JsonSerializer.Serialize(MyStore));
        File.WriteAllText(DefaultValues.reportsPath, JsonSerializer.Serialize(Reports));
        File.WriteAllText(DefaultValues.daysPath, MyStore.DayCount.ToString());

        Environment.Exit(0);
    }

    private static int GetCustomerCount() => MyStore.Rating + ((MyStore.Rating > 95) ? Random.Shared.Next((int)MyStore.DayCount, (int)MyStore.DayCount * 3) : 0);


    public static void InitializeGame()
    {
        InitializePath();

        uint dayCount = JsonSerializer.Deserialize<uint>(File.ReadAllText(DefaultValues.daysPath));
        if(dayCount == 1)
            Extra.DeleteFilesInDirectory(DefaultValues.logPath);

        Reports = JsonSerializer.Deserialize<List<Report>>(File.ReadAllText(DefaultValues.reportsPath)) ?? new List<Report>();
        MyStore = JsonSerializer.Deserialize<Store>(File.ReadAllText(DefaultValues.storePath)) ?? new Store(0, 10, "Bazar Store", 200, 0.2, InitializeStends());
        DefaultValues.MinimumWeights = JsonSerializer.Deserialize<Dictionary<string, double>>(File.ReadAllText(DefaultValues.weightsPath));
        MyStore.DayCount = dayCount;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            Console.SetWindowSize(Console.LargestWindowWidth / 10 * 9, Console.LargestWindowHeight / 10 * 9);


    }



    private static void InitializePath()
    {
        if(!Directory.Exists(DefaultValues.logPath)) 
            Directory.CreateDirectory(DefaultValues.logPath);

        if (!File.Exists(DefaultValues.weightsPath))
            Extra.CreateFile(DefaultValues.weightsPath, "{\"Pomegranate\":0.312,\"Cucumber\":0.214,\"Tomato\":0.132,\"Garlic\":0.33,\"Grape\":0.48,\"Union\":0.112}");

        if (!File.Exists(DefaultValues.storePath)) 
            Extra.CreateFile(DefaultValues.storePath, "null");

        if(!File.Exists(DefaultValues.reportsPath)) 
            Extra.CreateFile(DefaultValues.reportsPath, "null");

        if(!File.Exists(DefaultValues.daysPath)) 
            Extra.CreateFile(DefaultValues.daysPath,"1");
    }

    public static void StartMusic()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            SoundPlayer player = new SoundPlayer();
            player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + @"ConsoleMusic.wav";
            player.PlayLooping();
        }
    }

    static Dictionary<string, Stend> InitializeStends()
    {
        // Eger oyunda evvelki save olunmus datalar yoxdursa, ilk defe default melumatlarla magaza stendlerinin yaradilmasi
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
