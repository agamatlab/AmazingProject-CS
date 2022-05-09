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
    public Dictionary<VegetableList, (Stack<Vegetable> Stock, Vegetable Item)> Stend { get; set; } = new();

    public void NewDay()
    {
        OnNotify?.Invoke();
        ReStock();
    }

    private void AddVegetable(Stack<Vegetable> stock, Vegetable element)
    {
        stock.Push(element);
        OnNotify += element.Decay;
    }

    void ReStock()
    {
        uint sum = (uint)Stend.Average(p => p.Value.Stock.Count);
        double moneySpent = default;
        Random rand = new Random();
        
        foreach (var pair in Stend)
        {
            Stack<Vegetable> newVegetables = new Stack<Vegetable>();
            Vegetable @ref = pair.Value.Item;

            var useableMoney = Budget * ((sum != 0)? pair.Value.Stock.Count / (double)sum : 1.0 / Stend.Count);
            int count = (int)(useableMoney / @ref.BuyPrice);
            moneySpent += count * @ref.BuyPrice;

            for (int i = 0; i < count; i++)
            {

                Vegetable vegetable = new Vegetable { Name = @ref.Name, BuyPrice = @ref.BuyPrice, 
                    Condition = (rand.Next(0,100) == 1) ? Conditions.Virus : Conditions.New, SellPrice = @ref.SellPrice };
                OnNotify += vegetable.Decay;
                newVegetables.Push(vegetable);
            }
        }
        Budget -= moneySpent;
    }

    IEnumerable<Vegetable> RemoveUnwantedCondition(IEnumerable<Vegetable> vegetables, Conditions condition)
    {
        foreach (var vegetable in vegetables)
        {
            if (vegetable.Condition == condition) { 
                vegetables = vegetables.Where(v => v != vegetable).Reverse(); 
                OnNotify -= vegetable.Decay;
            }
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
}

