
using Product;
using System.Text.Json;

class RunStore
{

    public static uint DayCount { get; set; } = default;
    public static List<Notification> Notifications{ get; set; } = new List<Notification>();
    public static Store Store = new Store(0, 100, "MyStore", 1000);

    static void Main(string[] args)
    {
        {

            Store.Stends.Add(DefaultValues.VegetableList.Tomato.ToString(), 
            new Stend()
                {
                    BuyPrice = .84,
                    SellPrice = 1.99,
                    AvrageWeight = .132,
                    Product = new Vegetable(.132)
                    {
                        Condition = Conditions.NonDefined,
                        Name = DefaultValues.VegetableList.Tomato.ToString(),
                    }
                }
            );

        
            Store.Stends.Add(DefaultValues.VegetableList.Cucumber.ToString(), 
            new Stend()
                {
                    BuyPrice = 2.16,
                    SellPrice = 3.39,
                    AvrageWeight = .214,
                    Product = new Vegetable(.214)
                    {
                        Condition = Conditions.NonDefined,
                        Name = DefaultValues.VegetableList.Cucumber.ToString(),
                    }
                }
            );

        
            Store.Stends.Add(DefaultValues.VegetableList.Union.ToString(), 
            new Stend()
                {
                    BuyPrice = .55,
                    SellPrice = 1.36,
                    AvrageWeight = .112,
                    Product = new Vegetable(.112)
                    {
                        Condition = Conditions.NonDefined,
                        Name = DefaultValues.VegetableList.Union.ToString(),
                    }
                }
            );

            Store.Stends.Add(DefaultValues.VegetableList.Garlic.ToString(), 
            new Stend()
                {
                    BuyPrice = .96,
                    SellPrice = 2.19,
                    AvrageWeight = .33,
                    Product = new Vegetable(.33)
                    {
                        Condition = Conditions.NonDefined,
                        Name = DefaultValues.VegetableList.Garlic.ToString(),
                    }
                }
            );

            Store.Stends.Add(DefaultValues.VegetableList.Grape.ToString(), 
            new Stend()
                {
                    BuyPrice = 2.24,
                    SellPrice = 4.49,
                    AvrageWeight = .48,
                    Product = new Vegetable(.48)
                    {
                        Condition = Conditions.NonDefined,
                        Name = DefaultValues.VegetableList.Grape.ToString(),
                    }
                }
            );

            Store.Stends.Add(DefaultValues.VegetableList.Pomegranate.ToString(), 
            new Stend()
                {
                    BuyPrice = 4.5,
                    SellPrice = 8.99,
                    AvrageWeight = .312,
                    Product = new Vegetable(.312)
                    {
                        Condition = Conditions.NonDefined,
                        Name = DefaultValues.VegetableList.Pomegranate.ToString(),
                    }
                }
            );

        }


        Store.NewDay();

        foreach (var pair in Store.Stends)
            foreach (var item in pair.Value.Stock)
                Console.WriteLine(item);

        Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
        Console.ReadKey();

        List<Customer> list = new List<Customer>();
        for (int i = 0; i < 100; i++)
            list = list.Append(new Customer(Store.Rating)).ToList();

        Store.StartSales(list);
    }

}