using static System.ConsoleColor;

namespace SRC.Utils
{
    internal class ConsoleUtil
    {
        static object obj = new object();
        internal static void Print(string msg, ConsoleColor color = White, bool requiresVerboseLogging = false)
        {
            lock (obj)
            {
                if (!requiresVerboseLogging)
                {
                    Console.ForegroundColor = color;
                    Console.WriteLine(msg);
                } else
                {
                    if (Program.AppState.VerboseLogging)
                    {
                        Console.ForegroundColor = color;
                        Console.WriteLine(msg);
                    }
                }
            }
        }

        internal static string? UserInput(string msg = "", ConsoleColor color = White)
        {
            lock (obj)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(msg);
                return Console.ReadLine();
            }
        }
    }
}
