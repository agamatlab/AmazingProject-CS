using System.Text;
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
    public static string CreateString(params string[] texts)
    {
        StringBuilder sb = new StringBuilder();
        foreach (string text in texts)
            sb.AppendLine(text);
        return sb.ToString();
    }
    public static void DeleteFilesInDirectory(string path)
    {
        DirectoryInfo di = new DirectoryInfo(path);
        FileInfo[] files = di.GetFiles();
        foreach (FileInfo file in files)
            file.Delete();
    }

    public static void ResetTxt(string path)
    {

        if (File.Exists(path))
            File.WriteAllText(path, "");
    }

    public static bool RandomChance(int min = 1, int max = 100)
        => Random.Shared.Next(min, max) == min;
}