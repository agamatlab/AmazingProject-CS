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

    public event Action? OnNotify;
    public uint ClientCount { get; set; }
    public byte Rating { get; set; }
    public string? Name { get; set; }
    public double Budget { get; set; }
    //public Dictionary<string, Vegetable>? Vegetables{ get; set; }
    public Dictionary<Vegetable, Stack<Vegetable>> Stend { get; set; } = new();

    public void NewDay()
    {
        OnNotify?.Invoke();
        ReStock();

        int initialSize = Stend.Sum(p => p.Value.Count);

        foreach (var pair in Stend)
            Stend[pair.Key] = RemoveUnwantedCondition(pair.Value, Conditions.Toxic,Conditions.Virus);

        RunStore.Notifications.Add(new Notification(String.Format(Extra.templateRemovedVegetable, (initialSize - Stend.Sum(p => p.Value.Count)).ToString() )));
    }

    private void AddVegetable(Stack<Vegetable> stock, Vegetable element)
    {
        stock.Push(element);
        OnNotify += element.Decay;
    }

    void ReStock()
    {
        uint sum = (uint)Stend.Average(p => p.Value.Count);
        double moneySpent = default;
        Random rand = new Random();
        
        foreach (var pair in Stend)
        {
            Stack<Vegetable> newVegetables = new Stack<Vegetable>();
            Vegetable @ref = pair.Key;

            var useableMoney = Budget * ((sum != 0)? pair.Value.Count / (double)sum : 1.0 / Stend.Count);
            int count = (int)(useableMoney / @ref.BuyPrice);
            moneySpent += count * @ref.BuyPrice;

            for (int i = 0; i < count; i++)
            {

                Vegetable vegetable = new Vegetable(DefaultValues.MinimumWeights[pair.Key.Name ?? "none"]) { Name = @ref.Name, BuyPrice = @ref.BuyPrice, 
                    Condition = (rand.Next(0,100) == 1) ? Conditions.Virus : Conditions.New, SellPrice = @ref.SellPrice };
                OnNotify += vegetable.Decay;
                newVegetables.Push(vegetable);
            }

            Stend[pair.Key] = new Stack<Vegetable>(newVegetables.Concat(pair.Value));
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


    void StartSales(List<Customer> customers)
    {
        foreach (var customer in customers)
        {
            //var currentStock = Stend[customer.WantToBuy];
        }
    }
}

