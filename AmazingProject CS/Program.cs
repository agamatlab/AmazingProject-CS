
using Product;
using System.Drawing;
using System.Text.Json;

class RunStore
{
    static void Design()
    {
        
    }

    public static uint DayCount { get; set; } = default;
    public static List<Notification> Notifications{ get; set; } = new List<Notification>();
    public static Store Store = new Store(0, 10, "MyStore", 100);

    static void Main(string[] args)
    {

        {

            Store.Stends.Add(DefaultValues.VegetableList.Tomato.ToString(), 
            new Stend()
                {
                    BuyPrice = .42,
                    SellPrice = 1.36,
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
                    BuyPrice = 1.16,
                    SellPrice = 1.99,
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
                    BuyPrice = .35,
                    SellPrice = 1.06,
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
                    BuyPrice = .86,
                    SellPrice = 1.89,
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
                    BuyPrice = 1.12,
                    SellPrice = 2.29,
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
                    BuyPrice = 1.5,
                    SellPrice = 3.39,
                    AvrageWeight = .312,
                    Product = new Vegetable(.312)
                    {
                        Condition = Conditions.NonDefined,
                        Name = DefaultValues.VegetableList.Pomegranate.ToString(),
                    }
                }
            );

        }

        for (int k = 0; k < 10; k++)
        {
             Store.NewDay();

            List<Customer> list = new List<Customer>();
            int loops = Store.Rating;

            for (int i = 0; i < loops; i++)
                list = list.Append(new Customer(Store.Rating)).ToList();

            Store.StartSales(list);

            Console.Clear();
        }
    }

}