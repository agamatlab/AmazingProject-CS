
using Product;

class RunStore
{
    public static Store store = new Store(0, 0, "MyStore", 5000, null);

    static void Main(string[] args)
    {
        store.Stend.Add(VegetableList.Tomato, new Stack<Vegetable>());
        store.Stend.Add(VegetableList.Cucumber, new Stack<Vegetable>());
        store.Stend.Add(VegetableList.Cucumber, new Stack<Vegetable>());


        Console.WriteLine("da");
    }

}