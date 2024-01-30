using static System.ConsoleColor;
using static SRC.Utils.ConsoleUtil;

namespace SRC
{
    internal class Program
    {
        internal static string RobloxDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Roblox");
        internal static string AppDirectory = AppDomain.CurrentDomain.BaseDirectory;

        static async Task Main(string[] args)
        {
            Print("Welcome to SimpleRobloxCleaner (SRC)! This tool removes obsolete files from the Roblox directory to free up storage space.", Cyan);
            Print($"Roblox Path: {RobloxDirectory}", DarkYellow);

            if (!Directory.Exists(RobloxDirectory))
            {
                Print($"Could not find the Roblox directory at {RobloxDirectory}.", Red);
                UserInput("Press Enter to close.", Red);
                Environment.Exit(0);
            }
            await Utils.Application.LoadSettings();

            if (AppState.DeleteRiskyFiles)
                Print("Delete risky files is enabled.", DarkRed);
            UserInput("Press enter to clean.");
            await Cleaner.Clean.CleanAll();
            UserInput("Done, press enter to close.", Green);
        }

        internal static class AppState
        {
            internal static bool VerboseLogging = true;
            internal static bool DeleteRiskyFiles = false;
        }
    }
}
