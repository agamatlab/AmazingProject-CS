using Product;

class Customer {
    public string WantToBuy { get; set; }
    public double HowMuch{ get; set; }


    public Customer(uint Rating)
    {
        WantToBuy = DefaultValues.GetRandomEnumVegetable();
        HowMuch = DefaultValues.MinimumWeights[WantToBuy ?? "none"]+ Random.Shared.Next((int)Rating / 35, (int)Rating / 35 + 3) 
            + Random.Shared.NextDouble();
    }

    public override string ToString() => $"Customer Wants To Buy => {HowMuch} {WantToBuy}";
    public void Buy(Store store)
    {

    }
}
