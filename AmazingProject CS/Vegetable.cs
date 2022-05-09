
using System;

namespace Product
{
    enum Conditions { Virus = -1, NonDefined, New, Normal, Rotten, Toxic };

    class Vegetable
    {
        public Guid ID = Guid.NewGuid();
        public string? Name { get; set; }
        public double BuyPrice { get; set; }
        public double SellPrice { get; set; }
        public double Weight { get; set; }
        public Conditions Condition { get; set; }

        public Vegetable(double minWeight)
            => Weight = minWeight + Extra.GetRandom(10);

        public void Decay() {
            if (Extra.GetRandom() != 1) Condition++;
            else Condition = Conditions.Toxic; 
        }
        public override string ToString() => $"{Name} --> {BuyPrice.ToString()} ~ {SellPrice.ToString()} --> {Condition.ToString()}";

        public static bool operator==(Vegetable v1, Vegetable v2) => v1.ID == v2.ID;
        public static bool operator!=(Vegetable v1, Vegetable v2) => v1.ID != v2.ID;

        public static bool operator ==(Vegetable v, string name) => v.Name == name;
        public static bool operator !=(Vegetable v, string text) => v.Name != text;

        // public static bool operator ==(Vegetable v, VegetableList vegetableType) => v.Name == vegetableType.ToString();
        // public static bool operator !=(Vegetable v, VegetableList vegetableType) => v.Name != vegetableType.ToString();
    }
}