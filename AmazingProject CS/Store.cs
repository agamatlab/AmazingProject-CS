using ConsoleTables;
using Product;
using System.Drawing;
using System.Linq;
using UIElements;
class Store
{
    private double _prof;

    public double ProfitMargin
    {
        get { return _prof; }
        set { 
            if(value <= 1 && value >= 0)_prof = value; 
            else throw new ArgumentOutOfRangeException("Profit must be between 1 and 0");
        }
    }

    public Store(uint clientCount, byte rating, string? name, uint budget, double profit, Dictionary<string, Stend> stends)
    {
        Stends = stends;
        ClientCount = clientCount;
        Rating = rating;
        Name = name;
        Budget = budget;
        ProfitMargin = profit;

        ReStock();
        InitNewReport();
    }

    private byte _rating;

    public byte Rating
    {
        get { return _rating; }
        set { if (value < 1) Environment.Exit(777); 
            else if(value > 100) _rating = 100; 
            else _rating = value; 
        }
    }

    private double _budget;

    public double Budget
    {
        get { return _budget; }
        set { 
            if ( value < 0) 
                throw new ArgumentOutOfRangeException("Value can't be less than 0"); 
            _budget = value; 
        }
    }



    public event Action? OnNotify;
    public double Profit { get; set; }
    public uint ClientCount { get; set; }
    public string? Name { get; set; }
    public uint CustomerCount { get; set; } = 0;
    public Dictionary<string, Stend> Stends { get; set; } = new();
    private Report _currentReport = new Report();

    int CalculateIndex() => ((RunStore.DayCount % DefaultValues.DayCountWeek == 0)
        ? DefaultValues.DayCountWeek 
        : (int)RunStore.DayCount % DefaultValues.DayCountWeek)
        - 1;     

    public void NewDay(bool isQuarantine = false)
    {
        if (RunStore.DayCount % DefaultValues.DayCountWeek  == 0 && RunStore.DayCount != 0) {
            _currentReport.CustomerCount = CustomerCount;
            _currentReport.TotalProfit = Profit;
            _currentReport.Name = $"Day: {(RunStore.DayCount - 6).ToString()} --> {RunStore.DayCount.ToString()} Reports";
            RunStore.Reports.Add(_currentReport);
            _currentReport = new Report();
            InitNewReport();
        }
            
        RunStore.DayCount++;
        OnNotify?.Invoke();

        if (!isQuarantine){
            //Console.ForegroundColor = ConsoleColor.White;
            //Console.WriteLine("Day {0}", RunStore.DayCount);
                        //Console.WriteLine("Budget starts: {0}", Budget);
            ReStock();
                        //Console.WriteLine("Budget ends: {0}", Budget);

            ReOrganize();
        }
    }

    void InitNewReport()
    {
        for (int i = 0; i < DefaultValues.DayCountWeek; i++)
        {
            _currentReport.Notifications.Add(new List<Notification>());
            _currentReport.Statistics.Add(new DayStats() { OnDay = (uint)(i + 1) });
        }
    }

    private void AddVegetable(Stack<Vegetable> stock, Vegetable element)
    {
        stock.Push(element);
        OnNotify += element.Decay;
    }

    void ReStock1()
    {
        uint sum = (uint)Stends.Sum(p => p.Value.Stock.Count);
        double moneySpent = default;
        Random rand = new Random();
        
        foreach (var pair in Stends)
        {
            Stack<Vegetable> newVegetables = new Stack<Vegetable>();
            Stend stend = pair.Value;

            var useableMoney = Budget * ((sum != 0)? stend.Stock.Count / (double)sum : 1.0 / Stends.Count);
            int count = (int)(useableMoney / stend.BuyPrice);
            moneySpent += count * stend.BuyPrice;

            for (int i = 0; i < count; i++)
            {
                Vegetable vegetable = pair.Value.CreateNewSample();
                OnNotify += vegetable.Decay;
                newVegetables.Push(vegetable);
            }


            pair.Value.Stock = new Stack<Vegetable>(newVegetables.Concat(pair.Value.Stock));
        }
        Budget -= moneySpent;
    
    }

    bool StandardizeStends(IEnumerable<KeyValuePair<string, Stend>> collection, uint amount)
    {
        while (Stends.Min(p => p.Value.Stock.Count) != amount)
            foreach (var pair in collection)
                if (pair.Value.Stock.Count < amount)
                    try
                    {
                        Budget -= pair.Value.BuyPrice;
                        AddVegetable(pair.Value.Stock, pair.Value.CreateNewSample());

                    }
                    catch (ArgumentOutOfRangeException) { return false; }
                else collection = collection.Where(p => p.Key != pair.Key);

        return true;
    }

    void ReStock()
    {
        //double minPrice = Stends.Min(p => p.Value.BuyPrice);

        uint avrage = (uint)Stends.Average(p => p.Value.Stock.Count);
        var belowNeeded = Stends.Where(p => p.Value.Stock.Count < avrage);
        if(!StandardizeStends(belowNeeded, avrage)) return;

        var maxCount = Stends.Max(p => p.Value.Stock.Count);
        belowNeeded = Stends.Where(p => p.Value.Stock.Count < maxCount);
        if(!StandardizeStends(belowNeeded, (uint)maxCount)) return;

        while (true)
            foreach (var pair in Stends)
                try
                {
                    Budget -= pair.Value.BuyPrice;
                    AddVegetable(pair.Value.Stock ,pair.Value.CreateNewSample());
                }
                catch (ArgumentOutOfRangeException) { return; }

    }

    public static Stack<T> SortAscendingStack<T,TResult>(Stack<T> input, Func<T,TResult> selector)
        where TResult : IComparable<TResult>
    {

        Stack<T> tmpStack = new Stack<T>();
        while (input.Count > 0)
        {
            T tmp = input.Pop();
                
            while (tmpStack.Count > 0 
                && selector(tmpStack.Peek())
                   .CompareTo(selector(tmp)) == 1)
                       input.Push(tmpStack.Pop());

            tmpStack.Push(tmp);
        }
        return tmpStack;
    }

    Stack<Vegetable> RemoveUnwantedCondition(Stack<Vegetable> vegetables, params Conditions[] conditions)
    {
        foreach (var vegetable in vegetables)
            if (conditions.Contains(vegetable.Condition)) {
                vegetables = new Stack<Vegetable>(vegetables.Where(v => v != vegetable).Reverse());
                OnNotify -= vegetable.Decay;
            }

        return vegetables;
    }

    void ReOrganize()
    {
        int initialSize = Stends.Sum(p => p.Value.Stock.Count);

        foreach (var pair in Stends)
        {
            pair.Value.Stock = SortAscendingStack(pair.Value.Stock, v => (int)v.Condition);
            pair.Value.Stock = RemoveUnwantedCondition(pair.Value.Stock, Conditions.Toxic); // Conditions.Virus
        }

        _currentReport.Notifications[CalculateIndex()]
            .Add(new Notification(String.Format(Extra.templateRemovedVegetable,
            (initialSize - Stends.Sum(p => p.Value.Stock.Count)).ToString())));
    }

    void NegativeReview() => Rating -= (byte)Random.Shared.Next(1,5); 

    ConsoleTable GetStatusTable()
    {
        var table = new ConsoleTable("Product Name", "Total Quantity", 
             Conditions.New.ToString(), Conditions.Normal.ToString(),
                Conditions.Rotten.ToString(), Conditions.Toxic.ToString());

        foreach (var pair in Stends)
            table.AddRow(pair.Value.Product.Name, pair.Value.Stock.Count,
                pair.Value.Stock.Count(v => v.Condition == Conditions.New),
                pair.Value.Stock.Count(v => v.Condition == Conditions.Normal),
                pair.Value.Stock.Count(v => v.Condition == Conditions.Rotten),
                pair.Value.Stock.Count(v => v.Condition == Conditions.Toxic)
                );

        return table;
    }

    public void StartSales(List<Customer> customers)
    {
        int currentIndex = CalculateIndex(); 
        _currentReport.Statistics[currentIndex].BeforeStock = GetStatusTable().ToString();

        var table = new ConsoleTable("Customer NO.", "Wants To Buy", "How Much (kq)", "Quanity", "Rating", "Message");

        foreach (var customer in customers)
        {
            CustomerCount++;

            // Console.WriteLine($"Customer: {customerNO}/{customers.Count}");
            var currentStend = Stends[customer.WantToBuy];
            int quantity = (int)(customer.HowMuch / currentStend.AvrageWeight);
            
            if (currentStend.Stock.Count == 0)
            {

                _currentReport.Notifications[currentIndex]
                    .Add(new Notification($"{customer.WantToBuy} Does Not Exist. Decreasing Rating..."));
                
                // Console.WriteLine("No {0} LEft", customer.WantToBuy);
                NegativeReview();

                table.AddRow(CustomerCount.ToString() ,customer.WantToBuy, Math.Round(customer.HowMuch, 3).ToString(), 
                    quantity.ToString(), Rating.ToString() ,$"NO {customer.WantToBuy} in STOCK.");

                continue;
            }

                               // Console.WriteLine(customer);

            bool isContinue = true;

                         // Console.WriteLine($"Quantity: {quantity}");

            for (int i = 0; i < quantity && isContinue; i++)
            {


                if (currentStend.Stock.TryPop(out Vegetable? bought))
                {
                    // Console.WriteLine($"{bought} --> In Stock {currentStend.Stock.Count}");

                    switch (bought.Condition)
                    {
                        case Conditions.Normal:
                        case Conditions.New:
                            OnNotify -= bought.Decay;
                            Budget += currentStend.SellPrice * (1 - ProfitMargin);
                            Profit += currentStend.SellPrice * ProfitMargin;
                            break;
                        case Conditions.Toxic:
                            //case Conditions.Virus:
                            table.AddRow(CustomerCount.ToString(), customer.WantToBuy, Math.Round(customer.HowMuch, 3).ToString(),
                                quantity.ToString(), Rating.ToString(), "Customer Faced Virused or Toxic Vegetable.");

                            _currentReport.Notifications[currentIndex]
                                .Add(new Notification($"Customer Faced Toxic or Virus {customer.WantToBuy}. Decreasing Rating..."));

                            // Console.WriteLine("Veegetable Contain Virus or Toxic...");
                            NegativeReview();
                            isContinue = false;
                            break;
                        case Conditions.Rotten:
                            i--;
                            break;
                    }
                }
                else
                {
                    table.AddRow(CustomerCount.ToString(), customer.WantToBuy, Math.Round(customer.HowMuch, 3).ToString(),
                      quantity.ToString(), Rating.ToString(), "Customer wasn't able buy Enough Quantity => Stock Depleted.");
                    isContinue = false;
                    break;
                }
            }

            if (isContinue) {  
                Rating++;

                table.AddRow(CustomerCount.ToString(), customer.WantToBuy, Math.Round(customer.HowMuch, 3).ToString(),
                    quantity.ToString(), Rating.ToString(), "Customer SUCCESSFULLY Shopped.");
            }

                                            //Console.ReadKey()
        }

        // Console.ReadKey();
        _currentReport.Statistics[currentIndex].BuyerMessages = table.ToString();
        _currentReport.Statistics[currentIndex].AfterStock = GetStatusTable().ToString();
        //Console.ReadKey();


    }


    public void Quarantine()
    {
        for (int i = 0; i < 14; i++)
        {
            int currentIndex = CalculateIndex();
            _currentReport.Notifications[currentIndex].Add(new Notification("Quarantine Started... NO WORK for 14 DAYS"));

            _currentReport.Statistics[currentIndex].BeforeStock = GetStatusTable().ToString();
            _currentReport.Statistics[currentIndex].BuyerMessages = new ConsoleTable("Message", "Quarantine Started... NO WORK for 14 DAYS").ToString();
            _currentReport.Statistics[currentIndex].AfterStock = GetStatusTable().ToString();
        
            NewDay(true);
        }
    }
}

