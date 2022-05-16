﻿using ConsoleTables;
using Microsoft.SqlServer.Server;
using Product;
using Serilog;
using System.Drawing;
using System.Linq;
using UIElements;
class Store
{
    public int MyProperty { get; set; }

    private double _prof;

    public double ProfitMargin
    {
        get { return _prof; }
        set { 
            if(value <= 1 && value >= 0)_prof = value; 
            else throw new ArgumentOutOfRangeException("Profit must be between 1 and 0");
        }
    }


    public Store() { }

    void ConfigureLog()
    {

        Log.Logger = new LoggerConfiguration()
            .WriteTo.File(_currentPath)
            .CreateLogger();

    }

    public Store(uint clientCount, byte rating, string? name, uint budget, double profit, Dictionary<string, Stend> stends)
    {
        Stends = stends;
        ClientCount = clientCount;
        Rating = rating;
        Name = name;
        Budget = budget;
        ProfitMargin = profit;

        InitNewReport();
        Extra.DeleteFilesInDirectory(AppDomain.CurrentDomain.BaseDirectory + "days");
        NewDay();
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
    public Report CurrentReport { get; set; } = new Report();

    private uint _dayCount = 0;

    public uint DayCount
    {
        get { return _dayCount; }
        set { _dayCount = value; }
    }

    private string _currentPath;

    public static int CalculateIndex(uint dayCount) => ((dayCount % DefaultValues.DayCountWeek == 0)
        ? DefaultValues.DayCountWeek 
        : (int)dayCount % DefaultValues.DayCountWeek)
        - 1;     

    public void NewDay()
    {
        if (DayCount % DefaultValues.DayCountWeek  == 0 && DayCount != 0) {
            CurrentReport.CustomerCount = CustomerCount;
            CurrentReport.TotalProfit = Profit;
            CurrentReport.Name = $"Day: {(DayCount - 6).ToString()} --> {DayCount.ToString()} Reports";
            RunStore.Reports.Add(CurrentReport);
            CurrentReport = new Report();
            InitNewReport();
        }
          
        if(DayCount < RunStore.MAXDAYS)
            OnNotify?.Invoke();
        DayCount++;

        _currentPath = AppDomain.CurrentDomain.BaseDirectory
                + @$"days\day {DayCount.ToString()}.txt";

        Extra.ResetTxt(_currentPath);
        ConfigureLog();

    }



    public void ReStock()
    {
        AddStock();
        ReOrganize();
    }

    void InitNewReport()
    {
        for (int i = 1; i <= DefaultValues.DayCountWeek; i++)
        {
            CurrentReport.Notifications.Add(new List<Notification>());
            CurrentReport.Statistics.Add(new DayStats((uint)(DayCount + i)));
        }
    }

    private void AddVegetable(Stack<Vegetable> stock, Vegetable element)
    {
        stock.Push(element);
        OnNotify += element.Decay;
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

    void AddStock()
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

        //CurrentReport.Notifications[CalculateIndex(DayCount)]
        //    .Add(new Notification());
        
        Log.Information($"Found and Removed => {(initialSize - Stends.Sum(p => p.Value.Stock.Count)).ToString()} " +
            $"Toxic / Virus Vegetables\n\t~ On Day: {DayCount.ToString()}");
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
        int currentIndex = CalculateIndex(DayCount); 
        CurrentReport.Statistics[currentIndex].BeforeStock = GetStatusTable().ToStringAlternative();

        var table = new ConsoleTable("Customer NO.", "Wants To Buy", "How Much (kq)", "Quanity", "Rating", "Message");
        foreach (var customer in customers)
        {
            CustomerCount++;

            var currentStend = Stends[customer.WantToBuy];
            int quantity = (int)(customer.HowMuch / currentStend.AvrageWeight);

            if (currentStend.Stock.Count == 0)
            {

                //CurrentReport.Notifications[currentIndex]
                //    .Add(new Notification($"{customer.WantToBuy} Does Not Exist. Decreasing Rating...", DayCount));
                Log.Warning($"{customer.WantToBuy} Does Not Exist. Decreasing Rating...\n\t~ On Day: {DayCount}");

                NegativeReview();

                table.AddRow(CustomerCount.ToString(), customer.WantToBuy, Math.Round(customer.HowMuch, 3).ToString(),
                    quantity.ToString(), Rating.ToString(), $"NO {customer.WantToBuy} in STOCK.");

                continue;
            }

            bool isContinue = true;


            for (int i = 0; i < quantity && isContinue; i++)
            {


                if (currentStend.Stock.TryPop(out Vegetable? bought))
                {

                    switch (bought.Condition)
                    {
                        case Conditions.Normal:
                        case Conditions.New:
                            OnNotify -= bought.Decay;
                            Budget += currentStend.SellPrice * (1 - ProfitMargin);
                            Profit += currentStend.SellPrice * ProfitMargin;
                            break;
                        case Conditions.Toxic:
                            table.AddRow(CustomerCount.ToString(), customer.WantToBuy, Math.Round(customer.HowMuch, 3).ToString(),
                                quantity.ToString(), Rating.ToString(), "Customer Faced Virused or Toxic Vegetable.");

                            //CurrentReport.Notifications[currentIndex]
                                //.Add(new Notification($"Customer Faced Toxic or Virus {customer.WantToBuy}. Decreasing Rating...", DayCount));
                            Log.Warning($"Customer Faced Toxic or Virus {customer.WantToBuy}. Decreasing Rating...\n\t~ On Day: {DayCount}");


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

            if (isContinue)
            {
                Rating++;

                table.AddRow(CustomerCount.ToString(), customer.WantToBuy, Math.Round(customer.HowMuch, 3).ToString(),
                    quantity.ToString(), Rating.ToString(), "Customer SUCCESSFULLY Shopped.");
            }

        }

        CurrentReport.Statistics[currentIndex].BuyerMessages = table.ToStringAlternative();
        CurrentReport.Statistics[currentIndex].AfterStock = GetStatusTable().ToStringAlternative();

    }


    public void Quarantine()
    {
        for (int i = 14; i > 0; i--)
        {
            int currentIndex = CalculateIndex(DayCount);
            //CurrentReport.Notifications[currentIndex].Add(new Notification("Quarantine Started... NO WORK for 14 DAYS", DayCount));
            Log.Information($"Quarantine Started... NO WORK for {i.ToString()} more DAYS\n\t~ On Day: {DayCount}");

            CurrentReport.Statistics[currentIndex].BeforeStock = GetStatusTable().ToStringAlternative();
            CurrentReport.Statistics[currentIndex].BuyerMessages = new ConsoleTable("Message", "Quarantine Started... NO WORK for 14 DAYS").ToStringAlternative();
            CurrentReport.Statistics[currentIndex].AfterStock = GetStatusTable().ToStringAlternative();
        
            NewDay();
        }
    }
}

