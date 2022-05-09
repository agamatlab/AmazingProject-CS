using Product;

class Customer {
    public string WantToBuy { get; set; }
    public double HowMuch{ get; set; }


    public Customer(uint Rating)
    {
        WantToBuy = typeof(DefaultValues.VegetableList).GetRandomEnumValue().ToString();
        HowMuch = DefaultValues.MinimumWeights[WantToBuy ?? "none"]+ Extra.GetRandom(Rating / 10, Rating / 10 + 5) 
            + Extra.GetRandom(1,10) / 10;
    }

    public void Buy(Store store)
    {

    }
}
