using Product;

class Stend
{
    public Stend() { }
    public Stend(Vegetable product) => Product = product;
    public Stend(double buyPrice, double sellPrice, double avrageWeight, Vegetable product)
    {
        BuyPrice = buyPrice;
        SellPrice = sellPrice;
        AvrageWeight = avrageWeight;
        Product = product;
    }

    public double BuyPrice { get; set; }
    public double SellPrice { get; set; }
    public double AvrageWeight { get; set; }
    public Vegetable Product { get; init; }

    public Vegetable CreateNewSample()
        => new Vegetable(AvrageWeight)
            { Condition = Conditions.New, Name = Product.Name };

    public Stack<Vegetable> Stock { get; set; } = new Stack<Vegetable>();

}

