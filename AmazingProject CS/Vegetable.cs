
namespace Product
{
    enum Conditions { New, Normal, Rotten, Toxic };
    enum VegetableList { Vegatable = -1,Cucumber = 1, Tomato, Greenery};

    abstract class Vegetable
    {
        public uint Quantity { get; set; }
        public string? Name { get; set; }
        public uint BuyPrice { get; set; }
        public uint SellPrice { get; set; }
        public byte Condition { get; set; }

        public static VegetableList GetClassName() => VegetableList.Vegatable;
    }
}