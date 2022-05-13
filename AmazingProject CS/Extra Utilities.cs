using System.Text.Json;

static class DefaultValues
{
    public static string GetRandomEnumVegetable() 
    {
        Array values = Enum.GetValues(typeof(VegetableList));
        return ((VegetableList)values
            .GetValue(Random.Shared.Next(values.Length)))
            .ToString();
    }
    public enum VegetableList { Cucumber = 1, Tomato, Union, Garlic, Grape, Pomegranate };
    public static Dictionary<string, double> MinimumWeights = JsonSerializer.Deserialize<Dictionary<string, double>>(File.ReadAllText("MinimumWeights.txt"));
    public const int DayCountWeek = 7;

}

static class Extra
{

    public static string templateRemovedVegetable = "Found and Removed => {0} Toxic / Virus Vegetables";
    public static int GetRandom(int min = 0, int max = 1000) 
        => min + (DateTime.Now.Millisecond % (max - min));
}