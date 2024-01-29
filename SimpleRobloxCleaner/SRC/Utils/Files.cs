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
                                        Print($"Deleted: {file}", Yellow);
                                    }
                                    catch (Exception ex)
                                    {
                                        Print($"Error deleting file {file}: {ex.Message}", Red);
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
    }
}
