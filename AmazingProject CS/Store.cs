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

    public Store(uint clientCount, byte rating, string? name, uint budget, double profit)
    {
        ClientCount = clientCount;
        Rating = rating;
        Name = name;
        Budget = budget;
        ProfitMargin = profit;

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
    //public Dictionary<string, Vegetable>? Vegetables{ get; set; }
    public Dictionary<string, Stend> Stends { get; set; } = new();

    public void NewDay()
    {
        Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Day {0}", RunStore.DayCount);
                    Console.WriteLine("Budget starts: {0}", Budget);
        OnNotify?.Invoke();
        ReStock();
                    Console.WriteLine("Budget ends: {0}", Budget);

        ReOrganize();
        RunStore.DayCount++;
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

    bool StandardizeStends(IEnumerable<KeyValuePair<string, Stend>> collection, double amount)
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
        if(!StandardizeStends(belowNeeded, maxCount)) return;

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

        RunStore.Notifications.Add(new Notification(String.Format(Extra.templateRemovedVegetable,
            (initialSize - Stends.Sum(p => p.Value.Stock.Count)).ToString())));
    }

    void NegativeReview() { Rating -= (byte)Extra.GetRandom(1, 5); 
        // Console.WriteLine($"NegativeReview() Called => Current Rating: {Rating}"); 
    }

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
        Colorful.Console.WriteWithGradient(GetStatusTable().ToString(), Color.Yellow, Color.Fuchsia, 14);

        var table = new ConsoleTable("Customer NO.", "Wants To Buy", "How Much (kq)", "Quanity", "Rating", "Message");

        foreach (var customer in customers)
        {
            CustomerCount++;

            // Console.WriteLine($"Customer: {customerNO}/{customers.Count}");
            var currentStend = Stends[customer.WantToBuy];
            int quantity = (int)(customer.HowMuch / currentStend.AvrageWeight);
            
            if (currentStend.Stock.Count == 0)
            {

                RunStore.Notifications
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
                            Profit = currentStend.SellPrice * ProfitMargin;
                            break;
                        case Conditions.Toxic:
                            //case Conditions.Virus:
                            table.AddRow(CustomerCount.ToString(), customer.WantToBuy, Math.Round(customer.HowMuch, 3).ToString(),
                                quantity.ToString(), Rating.ToString(), "Customer Faced Virused or Toxic Vegetable.");

                            RunStore.Notifications.Add(new Notification()
                            { Message = $"Customer Faced Toxic or Virus {customer.WantToBuy}. Decreasing Rating..." });

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

        Console.WriteLine("\n\n");
        Colorful.Console.WriteLine(table.ToString(), Color.White);
        
        Console.WriteLine("\n\n");
        Colorful.Console.WriteWithGradient(GetStatusTable().ToString(), Color.Yellow, Color.Fuchsia, 14);

        Console.ReadKey();


    }
}

