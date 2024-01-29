using static System.ConsoleColor;

namespace SRC.Utils
{
    internal class ConsoleUtil
    {
        static object obj = new object();
        internal static void Print(string msg, ConsoleColor color = White)
        {
            lock (obj)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(msg);
            }
        }

        internal static string? UserInput(string msg = "", ConsoleColor color = White, bool typeInSameLine = false)
        {
            lock (obj)
            {
                Console.ForegroundColor = color;
                if (!typeInSameLine)
                    Console.WriteLine(msg);
                else
                    Console.Write(msg);
                return Console.ReadLine();
            }
        }
    }
}
