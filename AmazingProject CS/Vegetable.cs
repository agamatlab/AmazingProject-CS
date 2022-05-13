
using System;

namespace Product
{
    // Virus = -101, 
    enum Conditions { NonDefined, New, Normal, Rotten, Toxic };

    class Vegetable
    {
        public Guid ID = Guid.NewGuid();
        public string? Name { get; set; }
        public double Weight { get; set; }
        public Conditions Condition { get; set; }

        public Vegetable(double avrageWeight)
            => Weight = avrageWeight + Random.Shared.NextDouble();

        public void Decay() {
            if (Condition == Conditions.Toxic) return;
            else if (Random.Shared.Next(1,100) == 1) Condition = Conditions.Toxic; 
            else Condition++;
        }
        public override string ToString() => $"{Name} --> {Condition.ToString()}";

        public static bool operator ==(Vegetable? v1, Vegetable? v2) 
            => v1?.ID == v2?.ID; 
        public static bool operator!=(Vegetable? v1, Vegetable? v2)
            => v1?.ID != v2?.ID;

    }
}