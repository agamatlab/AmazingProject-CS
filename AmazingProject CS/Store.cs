using Product;
using System.Linq;


class Store
{
    public Store(uint clientCount, byte rating, string? name, uint budget)
    {
        ClientCount = clientCount;
        Rating = rating;
        Name = name;
        Budget = budget;

    }

    private byte _rating;

    public byte Rating
    {
        get { return _rating; }
        set { if (value < 1) Environment.Exit(777); 
            else if(_rating + value > 100) _rating = 100; 
            else _rating = value; 
        }
    }


    public event Action? OnNotify;
    public uint ClientCount { get; set; }
    public string? Name { get; set; }
    public double Budget { get; set; }
    public uint CustomerCount { get; set; } = 0;
    //public Dictionary<string, Vegetable>? Vegetables{ get; set; }
    public Dictionary<string, Stend> Stends { get; set; } = new();

    public void NewDay()
    {
                    Console.WriteLine("Budget starts: {0}", Budget);
        OnNotify?.Invoke();
        ReStock();
                    Console.WriteLine("Budget ends: {0}", Budget);


        int initialSize = Stends.Sum(p => p.Value.Stock.Count);

        foreach (var pair in Stends)
            pair.Value.Stock = RemoveUnwantedCondition(pair.Value.Stock, Conditions.Toxic,Conditions.Virus);

        RunStore.Notifications.Add(new Notification(String.Format(Extra.templateRemovedVegetable, 
            (initialSize - Stends.Sum(p => p.Value.Stock.Count)).ToString() )));
        
        RunStore.DayCount++;
    }

    private void AddVegetable(Stack<Vegetable> stock, Vegetable element)
    {
        stock.Push(element);
        OnNotify += element.Decay;
    }

    void ReStock()
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

    Stack<Vegetable> RemoveUnwantedCondition(Stack<Vegetable> vegetables, params Conditions[] conditions)
    {
        foreach (var vegetable in vegetables)
            if (conditions.Contains(vegetable.Condition)) {
                vegetables = new Stack<Vegetable>(vegetables.Where(v => v != vegetable).Reverse());
                OnNotify -= vegetable.Decay;
            }

        return vegetables;
    }

    void ReOrganize(IEnumerable<Vegetable> newStock, IEnumerable<Vegetable> oldStock)
    {
        foreach (var vegetable in newStock)
        {
            if (vegetable.Condition == Conditions.Virus) OnNotify += vegetable.Decay;
        }
        // newStock.Concat(oldStock);
    }

    void NegativeReview() { Rating -= (byte)Extra.GetRandom(1, 5); 
        Console.WriteLine($"NegativeReview() Called => Current Rating: {Rating}"); }

    public void StartSales(List<Customer> customers)
    {
        foreach (var customer in customers)
        {
            var currentStend = Stends[customer.WantToBuy];
            if (currentStend.Stock.Count == 0)
            {
                Console.WriteLine("No {0} LEft", customer.WantToBuy);
                NegativeReview();
                continue;
            }

                               Console.WriteLine(customer);

            int quantity = (int)(customer.HowMuch / currentStend.AvrageWeight);
            bool isContinue = true;

                         Console.WriteLine($"Quantity: {quantity}");

            for (int i = 0; i < quantity && isContinue; i++)
            {
                currentStend.Stock.TryPop(out Vegetable? bought);
                if (bought != null)
                    switch (bought.Condition)
                    {
                        case Conditions.Normal:
                        case Conditions.New:
                            OnNotify -= bought.Decay;
                            Budget += currentStend.SellPrice;
                            break;
                        case Conditions.Toxic:
                        case Conditions.Virus:
                            Console.WriteLine("Veegetable Contain Virus or Toxic...");
                            NegativeReview();
                            isContinue = false;
                            break;
                        case Conditions.Rotten:
                            i--;
                            break;
                    }
                else break;


                // if (bought != null) Console.WriteLine($"{bought} --> In Stock{currentStend.Stock.Count}");
            }

            if (isContinue) { CustomerCount++; Rating++;
                Console.WriteLine("Store Rating IS CURRENTLY: {0}", Rating); }
                            
                            Console.ReadKey();
        }
    }
}

