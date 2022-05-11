using Product;

class Customer {
    public string WantToBuy { get; set; }
    public double HowMuch{ get; set; }


    public Customer(uint Rating)
    {
        WantToBuy = DefaultValues.GetRandomEnumVegetable();
        HowMuch = DefaultValues.MinimumWeights[WantToBuy ?? "none"]+ Extra.GetRandom((int)Rating / 35, (int)Rating / 35 + 3) 
            + Extra.GetRandom(1,10) / 30;
    }

    public override string ToString() => $"Customer Wants To Buy => {HowMuch} {WantToBuy}";
    public void Buy(Store store)
    {

    }
}
