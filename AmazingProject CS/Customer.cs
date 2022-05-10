using Product;

class Customer {
    public string WantToBuy { get; set; }
    public double HowMuch{ get; set; }


    public Customer(uint Rating)
    {
        WantToBuy = typeof(DefaultValues.VegetableList).GetRandomEnumValue().ToString();
        HowMuch = DefaultValues.MinimumWeights[WantToBuy ?? "none"]+ Extra.GetRandom((int)Rating / 10, (int)Rating / 10 + 5) 
            + Extra.GetRandom(1,10) / 10;
    }

    public override string ToString() => $"Customer Wants To Buy => {HowMuch} {WantToBuy}";
    public void Buy(Store store)
    {

    }
}
