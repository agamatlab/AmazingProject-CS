using System.Text;
using System.Text.Json;

static class DefaultValues
{
    public static string reportsPath = AppDomain.CurrentDomain.BaseDirectory + "reports.json";
    public static string weightsPath = AppDomain.CurrentDomain.BaseDirectory + "minimumWeights.txt";
    public static string storePath = AppDomain.CurrentDomain.BaseDirectory + "store.json";
    public static string daysPath = AppDomain.CurrentDomain.BaseDirectory + "day.txt";
    public static string logPath = AppDomain.CurrentDomain.BaseDirectory + "days";

    public static string GetRandomEnumVegetable()
    {
        Array values = Enum.GetValues(typeof(VegetableList));
        return ((VegetableList)values
            .GetValue(Random.Shared.Next(values.Length)))
            .ToString();
    }
    public enum VegetableList { Cucumber = 1, Tomato, Union, Garlic, Grape, Pomegranate };
    public static Dictionary<string, double> MinimumWeights = new Dictionary<string, double>();
    public const int DayCountWeek = 7;

}

static class Extra
{
    public static string ReadFile(string path)
    {
        using FileStream fs = new FileStream(path, FileMode.Open);

        byte[] buffer = new byte[fs.Length];
        fs.Read(buffer, 0, buffer.Length);
        return Encoding.Default.GetString(buffer);
    }

    public static void CreateFile(string path, string value)
    {
        using FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);

        byte[] bytes = Encoding.Default.GetBytes(value);
        fs.Write(bytes, 0, bytes.Length);
    }

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

static class ExtensionMethods
{
    public static T? GetRandom<T>(this T[] arr) => (arr.Length == 0) ? default(T) : arr[Random.Shared.Next(arr.Length)];
}