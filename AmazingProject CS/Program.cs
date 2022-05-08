
using Product;

class RunStore
{
    public static Store Store = new Store(0, 0, "MyStore", 5000);

    static void Main(string[] args)
    {
        // Union, Garlic, Grape, Pomegranate
        Store.Stend.Add(VegetableList.Tomato, (new Stack<Vegetable>(), new Vegetable() { Name = "Kend Pomidoru", Condition = Conditions.NonDefined, BuyPrice = .84, SellPrice = 1.99 }));
        Store.Stend.Add(VegetableList.Cucumber, (new Stack<Vegetable>(), new Vegetable() { Name = "Kend Xiyari", Condition = Conditions.NonDefined, BuyPrice = 2.16, SellPrice = 3.39 }));
        Store.Stend.Add(VegetableList.Greenery, (new Stack<Vegetable>(), new Vegetable() { Name = "Petrushka", Condition = Conditions.NonDefined, BuyPrice = .08, SellPrice = .20 }));
        Store.Stend.Add(VegetableList.Union, (new Stack<Vegetable>(), new Vegetable() { Name = "Soghan", Condition = Conditions.NonDefined, BuyPrice = .55, SellPrice = 1.39 }));
        Store.Stend.Add(VegetableList.Garlic, (new Stack<Vegetable>(), new Vegetable() { Name = "Kend Pomidoru", Condition = Conditions.NonDefined, BuyPrice = .96, SellPrice = 2.19 }));
        Store.Stend.Add(VegetableList.Grape, (new Stack<Vegetable>(), new Vegetable() { Name = "Uzum", Condition = Conditions.NonDefined, BuyPrice = 2.24, SellPrice = 4.49 }));
        Store.Stend.Add(VegetableList.Pomegranate, (new Stack<Vegetable>(), new Vegetable() { Name = "Nar", Condition = Conditions.NonDefined, BuyPrice = 4.50, SellPrice = 8.99 }));


        Store.NewDay();

        foreach (var pair in Store.Stend)
            foreach (var item in pair.Value.Stock)
                Console.WriteLine(item);
        
        Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
        Store.NewDay();


        foreach (var pair in Store.Stend)
            foreach (var item in pair.Value.Stock)
                Console.WriteLine(item);
    }

}