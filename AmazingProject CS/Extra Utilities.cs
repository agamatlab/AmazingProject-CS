static class Extra
{
    public static int GetRandom(int max = 1000) => DateTime.Now.Millisecond % max;
}