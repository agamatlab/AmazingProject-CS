using System.Text.Json;

static class DefaultValues
{
    public enum VegetableList { Cucumber = 1, Tomato, Union, Garlic, Grape, Pomegranate };
    public static Dictionary<string, double> MinimumWeights = JsonSerializer.Deserialize<Dictionary<string, double>>(File.ReadAllText("MinimumWeights.txt"));

}

static class Extra
{
    public static Enum GetRandomEnumValue(this Type t)
    {
        return Enum.GetValues(t)          // get values from Type provided
            .OfType<Enum>()               // casts to Enum
            .OrderBy(e => Guid.NewGuid()) // mess with order of results
            .FirstOrDefault();            // take first item in result
    }

    public static string templateRemovedVegetable = "Found and Removed => {0} Toxic / Virus Vegetables";
    public static int GetRandom(int min = 0, int max = 1000) 
        => min + (DateTime.Now.Millisecond % (max - min));
}