using static System.ConsoleColor;
using static SRC.Utils.ConsoleUtil;

namespace SRC.Utils
{
    internal class Files
    {
        internal static async Task DeleteFileType(List<(string DirectoryPath, string FileFormats)> directories, bool deleteInSubfolders = true)
        {
            try
            {
                await Task.Run(() =>
                {
                    Parallel.ForEach(directories, directory =>
                    {
                        if (!Directory.Exists(directory.DirectoryPath))
                        {
                            Print($"Directory does not exist: {directory.DirectoryPath}", Red);
                            return;
                        }

                        if (string.IsNullOrWhiteSpace(directory.FileFormats))
                        {
                            Print($"Invalid file formats for directory {directory.DirectoryPath}.", Red);
                            return;
                        }

                        try
                        {
                            string[] formats = directory.FileFormats.Split('|');

                            SearchOption searchOption = deleteInSubfolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

                            Parallel.ForEach(formats, format =>
                            {
                                var filesToDelete = Directory.EnumerateFiles(directory.DirectoryPath, format, searchOption);

                                Parallel.ForEach(filesToDelete, file =>
                                {
                                    try
                                    {
                                        File.Delete(file);
                                        Print($"Deleted: {file}", Yellow, true);
                                    }
                                    catch (Exception ex)
                                    {
                                        Print($"Error deleting file {file}: {ex.Message}", Red, true);
                                    }
                                });
                            });
                        }
                        catch (Exception ex)
                        {
                            Print($"An error occurred while processing files in directory {directory.DirectoryPath}: {ex.Message}", Red);
                        }
                    });
                });
            }
            catch (Exception ex)
            {
                Print($"An unexpected error occurred: {ex.Message}", Red);
            }
        }

        public static async Task<string> GetOptionValue(string file, string key)
        {
            if (!File.Exists(file))
                return "N/A";

            using (var sr = new StreamReader(file))
            {
                string line;
                while ((line = await sr.ReadLineAsync()) != null)
                {
                    if (line.StartsWith(key))
                    {
                        string[] parts = line.Split(':');
                        return parts.Length > 1 ? parts[1] : "N/A";
                    }
                }
            }

            return "N/A";
        }

        public static async Task<bool> SetOptionValue<T>(string file, string key, T value)
        {
            try
            {
                if (!File.Exists(file))
                    return false;

                string[] lines = File.ReadAllLines(file);
                bool keyFound = false;

                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith(key))
                    {
                        lines[i] = $"{key}:{value}";
                        keyFound = true;
                        break;
                    }
                }

                if (!keyFound)
                {
                    Array.Resize(ref lines, lines.Length + 1);
                    lines[lines.Length - 1] = $"{key}:{value}";
                }

                using (var sw = new StreamWriter(file))
                {
                    foreach (var line in lines)
                    {
                        await sw.WriteLineAsync(line);
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task DeleteSubfolders(string directoryPath)
        {
            try
            {
                await Task.Run(() =>
                {
                    if (!Directory.Exists(directoryPath))
                    {
                        Print($"Directory does not exist: {directoryPath}", Red);
                        return;
                    }

                    try
                    {
                        var subfolders = Directory.EnumerateDirectories(directoryPath);

                        Parallel.ForEach(subfolders, subfolder =>
                        {
                            try
                            {
                                Directory.Delete(subfolder, true);
                                Print($"Deleted sub-folder: {subfolder}", DarkYellow, true);
                            }
                            catch (Exception ex)
                            {
                                Print($"Error deleting sub-folder {subfolder}: {ex.Message}", Red, true);
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        Print($"An error occurred while processing sub-folders in directory {directoryPath}: {ex.Message}", Red);
                    }
                });
            }
            catch (Exception ex)
            {
                Print($"An unexpected error occurred: {ex.Message}", Red);
            }
        }
    }
}
