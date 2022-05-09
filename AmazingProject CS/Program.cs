
using Product;
using System.Text.Json;

class RunStore
{

    public static uint DayCount { get; set; } = default;
    public static List<Notification> Notifications{ get; set; } = new List<Notification>();
    public static Store Store = new Store(0, 0, "MyStore", 5000);

    static void Main(string[] args)
    {
        Store.Stend.Add(new Vegetable(.132) { Name = DefaultValues.VegetableList.Tomato.ToString(), Condition = Conditions.NonDefined, BuyPrice = .84, SellPrice = 1.99 }, new Stack<Vegetable>());
        Store.Stend.Add(new Vegetable(.22) { Name =  DefaultValues.VegetableList.Cucumber.ToString(), Condition = Conditions.NonDefined, BuyPrice = 2.16, SellPrice = 3.39 }, new Stack<Vegetable>());
        Store.Stend.Add(new Vegetable(.34) { Name =  DefaultValues.VegetableList.Union.ToString(), Condition = Conditions.NonDefined, BuyPrice = .55, SellPrice = 1.39 }, new Stack<Vegetable>());
        Store.Stend.Add(new Vegetable(.11) { Name =  DefaultValues.VegetableList.Garlic.ToString(), Condition = Conditions.NonDefined, BuyPrice = .96, SellPrice = 2.19 }, new Stack<Vegetable>());
        Store.Stend.Add(new Vegetable(.48) { Name =  DefaultValues.VegetableList.Grape.ToString(), Condition = Conditions.NonDefined, BuyPrice = 2.24, SellPrice = 4.49 }, new Stack<Vegetable>());
        Store.Stend.Add(new Vegetable(.312) { Name = DefaultValues.VegetableList.Pomegranate.ToString(), Condition = Conditions.NonDefined, BuyPrice = 4.50, SellPrice = 8.99 }, new Stack<Vegetable>());


        Store.NewDay();

        foreach (var pair in Store.Stend)
            foreach (var item in pair.Value)
                Console.WriteLine(item);

        Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
        Store.NewDay();


        foreach (var pair in Store.Stend)
            foreach (var item in pair.Value)
                Console.WriteLine(item);
    }

}