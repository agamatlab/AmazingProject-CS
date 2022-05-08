using Product;

class Store
{
    public Store(uint clientCount, byte rating, string? name, uint budget, Dictionary<string, Vegetable>? vegetables)
    {
        ClientCount = clientCount;
        Rating = rating;
        Name = name;
        Budget = budget;
        Vegetables = vegetables;
    }

    public uint ClientCount { get; set; }
    public byte Rating { get; set; }
    public string? Name { get; set; }
    public uint Budget { get; set; }
    public Dictionary<string, Vegetable>? Vegetables{ get; set; }
    public Dictionary<VegetableList, Stack<Vegetable>> Stend { get; set; } = new();

    public void ReStock()
    {

    }
}

