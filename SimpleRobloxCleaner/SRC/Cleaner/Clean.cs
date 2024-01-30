using System.Diagnostics;
using static SRC.Utils.Files;
using static System.ConsoleColor;
using static SRC.Utils.ConsoleUtil;

namespace SRC.Cleaner
{
    internal class Clean
    {
        internal static async Task CleanAll()
        {
            string logsFolder = Path.Combine(Program.RobloxDirectory, "logs");
            string webviewFolder = Path.Combine(Program.RobloxDirectory, "UniversalApp", "WebView2", "EBWebView");
            var deleteList = new List<(string DirectoryPath, string FileFormats)>
            {
                (logsFolder, "*.log"), // Logs
                (Path.Combine(Program.RobloxDirectory, "Downloads"), "*.*"), // Old downloaded files I'm assuming
                (Path.Combine(webviewFolder, "GraphiteDawnCache"), "*.*"), // Cache
                (Path.Combine(webviewFolder, "GrShaderCache"), "*.*"),
                (Path.Combine(webviewFolder, "ShaderCache"), "*.*")
            };
            if (Program.AppState.DeleteRiskyFiles)
            {
                var riskyFilesList = new List<(string DirectoryPath, string FileFormats)>
                {
                    (Path.Combine(Program.RobloxDirectory, "Versions"), "RobloxPlayerInstaller.exe"),
                };

                deleteList.AddRange(riskyFilesList);
            }

            Stopwatch cleanTime = Stopwatch.StartNew();
            await DeleteFileType(deleteList);
            cleanTime.Stop();
            Print(cleanTime.Elapsed.TotalSeconds < 1 ? $"Finished in {cleanTime.Elapsed.Milliseconds}ms" : $"Finished in {cleanTime.Elapsed.Seconds}s", Yellow);
        }
    }
}
