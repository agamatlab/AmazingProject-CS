

namespace UIElements
{
    using System.Text;


    static class UI
    {
        public static readonly int ESCAPE = -1;
        public static void ChangeEncoding(Encoding input, Encoding output)
        {
            Console.OutputEncoding = output;
            Console.InputEncoding = input;
        }

        public static void ChangeEncoding(Encoding encoding)
        {
            Console.OutputEncoding = encoding;
            Console.InputEncoding = encoding;
        }

        public static void SetColor(ConsoleColor foreground = ConsoleColor.White, ConsoleColor background = ConsoleColor.Black)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
        }

        public static int ManageChoice(ref ushort choice, ushort answerCount, bool IsEscape)
        {
            // Uİ'da dəyərlərin keyboardla idarəsini təmin edir.

            ConsoleKey kb = Console.ReadKey().Key;

            switch (kb)
            {
                case ConsoleKey.W:
                case ConsoleKey.A:
                case ConsoleKey.UpArrow:
                case ConsoleKey.LeftArrow:
                    if (choice == 0) choice = answerCount;
                    choice--;
                    return 1;

                case ConsoleKey.S:
                case ConsoleKey.D:
                case ConsoleKey.DownArrow:
                case ConsoleKey.RightArrow:
                    choice++;
                    choice %= answerCount;
                    return 1;

                case ConsoleKey.Enter:
                    return 0;

                case ConsoleKey.Escape:
                    if (IsEscape) return ESCAPE;
                    return 1;

                default:
                    return 1;
            }
        }

        public static int GetChoice(string question, string[] answers, bool IsEscape = false)
        {
            // KEYBOARD - CONTROLLED Menu ilə MOD Seçimi

            ushort answerCount = Convert.ToUInt16(answers.Length);
            sbyte @break = 1;
            ushort choice = 0;

            while (@break > 0)
            {
                Console.Clear();
                Console.WriteLine(question);
                for (ushort i = 0; i < answerCount; i++)
                {
                    char prefix = ' ';
                    if (i == choice) { SetColor(ConsoleColor.DarkGreen, ConsoleColor.Gray); prefix = '◙'; }
                    Console.WriteLine($" {prefix} << {answers[i]} >>");
                    Console.ResetColor();
                }
                if (IsEscape) Console.WriteLine("Press ESC To Exit...");
                @break = Convert.ToSByte(ManageChoice(ref choice, answerCount, IsEscape));
                if (@break == ESCAPE) return ESCAPE;
            }

            return choice;
        }

        public static (int IndexCategory,int ChoiceIndex) ChoiceMenuWithCategory(string[] answers, string[] categories, bool IsEscape = false)
        {

            ushort answerCount = Convert.ToUInt16(answers.Length);
            sbyte @break = 1;
            ushort choice = 0;
            ushort category = 0;

            while (@break > 0)
            {
                Console.Clear();
                Console.WriteLine($"< {categories[category]} >\n");
                for (ushort i = 0; i < answerCount; i++)
                {
                    char prefix = ' ';
                    if (i == choice) { SetColor(ConsoleColor.DarkGreen, ConsoleColor.Gray); prefix = '◙'; }
                    Console.WriteLine($" {prefix} << {answers[i]} >>");
                    Console.ResetColor();
                }
                if (IsEscape) Console.WriteLine("Press ESC To Exit...");
                @break = Convert.ToSByte(ManageChoiceCategory(ref choice, answerCount, ref category, categories.Length, IsEscape));
                if (@break == ESCAPE) return (ESCAPE, ESCAPE);
            }

            return (category, choice);

        }

        public static int ManageChoiceCategory(ref ushort choice, int answerCount, ref ushort category, int categoryCount, bool IsEscape)
        {
            // Uİ'da dəyərlərin keyboardla idarəsini təmin edir.

            ConsoleKey kb = Console.ReadKey().Key;

            switch (kb)
            {
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    if (category == 0) category = (ushort)categoryCount;
                    category--;
                    return 1;

                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    category++;
                    category %= (ushort)categoryCount;
                    return 1;

                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    if (choice == 0) choice = (ushort)answerCount;
                    choice--;
                    return 1;

                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    choice++;
                    choice %= (ushort)answerCount;
                    return 1;

                case ConsoleKey.Enter:
                    return 0;

                case ConsoleKey.Escape:
                    if (IsEscape) return ESCAPE;
                    return 1;
                default:
                    return 1;
            }
        }


    }
}