﻿
namespace Product
{
    enum Conditions { Virus = -1, NonDefined, New, Normal, Rotten, Toxic };
    enum VegetableList { Vegatable = -1,Cucumber = 1, Tomato, Greenery, Union, Garlic, Grape, Pomegranate};

    class Vegetable
    {
        public string? Name { get; set; }
        public double BuyPrice { get; set; }
        public double SellPrice { get; set; }
        public Conditions Condition { get; set; }

        void ok()
        {
            Extra ed = new Extra();
            Extra.GetRandom()
        }

        public static VegetableList GetClassName() => VegetableList.Vegatable;
        public void Decay() {
            if (Extra.GetRandom() != 1) Condition++;
            else Condition = Conditions.Toxic; 
        }
        public override string ToString() => $"{Name} --> {BuyPrice.ToString()} ~ {SellPrice.ToString()} --> {Condition.ToString()}";
    }
}