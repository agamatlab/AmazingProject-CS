
using System;

namespace Product
{
    enum Conditions { Virus = -1, NonDefined, New, Normal, Rotten, Toxic };

    class Vegetable
    {
        public Guid ID = Guid.NewGuid();
        public string? Name { get; set; }
        public double Weight { get; set; }
        public Conditions Condition { get; set; }

        public Vegetable(double avrageWeight)
        {
            if(avrageWeight > 5)Weight = avrageWeight + Extra.GetRandom(-5, 5);
            else Weight = avrageWeight + Extra.GetRandom() / 1000.0;
        }

        public void Decay() {
            if (Extra.GetRandom() != 1) Condition++;
            else Condition = Conditions.Toxic; 
        }
        public override string ToString() => $"{Name} --> {Condition.ToString()}";

        public static bool operator ==(Vegetable? v1, Vegetable? v2) { 
            return v1?.ID == v2?.ID; 
        }
        public static bool operator!=(Vegetable? v1, Vegetable? v2)
        {
            return v1?.ID != v2?.ID;
        }
        //public static bool operator ==(Vegetable v, string name) => v.Name == name;
        //public static bool operator !=(Vegetable v, string text) => v.Name != text;

        // public static bool operator ==(Vegetable v, VegetableList vegetableType) => v.Name == vegetableType.ToString();
        // public static bool operator !=(Vegetable v, VegetableList vegetableType) => v.Name != vegetableType.ToString();
    }
}