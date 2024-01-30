using static System.ConsoleColor;
using static SRC.Utils.ConsoleUtil;

namespace SRC.Utils
{
    internal class Application
    {
        private static string SrcDataFolder = string.Empty;
        private static string SettingsFile = string.Empty;

        internal static async Task LoadSettings()
        {
            SrcDataFolder = Path.Combine(Program.AppDirectory, "SrcData");
            SettingsFile = Path.Combine(SrcDataFolder, "Settings.yaml");

            try
            {
                Directory.CreateDirectory(SrcDataFolder);

                if (!File.Exists(SettingsFile))
                    await CreateSettingsFile();

                Program.AppState.VerboseLogging = Convert.ToBoolean(await Files.GetOptionValue(SettingsFile, "VerboseLogging"));
                Program.AppState.DeleteRiskyFiles = Convert.ToBoolean(await Files.GetOptionValue(SettingsFile, "DeleteRiskyFiles"));

            } catch (Exception ex)
            {
                Print($"Could not load settings. Error: {ex.Message}", Red);
            }
        }

        internal static async Task CreateSettingsFile()
        {
            File.Create(SettingsFile).Close();
            await Files.SetOptionValue(SettingsFile, "VerboseLogging", true);
            await Files.SetOptionValue(SettingsFile, "DeleteRiskyFiles", false);
        }
    }
}
