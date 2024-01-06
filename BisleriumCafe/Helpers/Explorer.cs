using BisleriumCafe.Model;
using BisleriumCafe.Providers;
using BisleriumCafe.Enums;
namespace BisleriumCafe.Helpers;

internal static class Explorer
{
    // 10MB
    public const int MAX_ALLOWED_IMPORT_SIZE = 1024 * 1024 * 10;

    public static string GetAppDataDirectoryPath()
    {
        return FileSystem.AppDataDirectory;
    }

    public static string GetDocumentsDirectoryPath()
    {
        return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    }

    public static FileProvider<TSource> GetFileProvider<TSource>(FileExtension extension) where TSource : IModel
    {
        return extension switch
        {
            FileExtension.json => new JsonFileProvider<TSource>(),
            _ => throw new Exception("Only JSON supported!"),
        };
    }

    public static string GetDesktopDirectoryPath()
    {
        return Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
    }

    public static string GetFilePath(string directory, string fileName)
    {
        string FullPath = Path.Combine(directory, fileName);

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (!File.Exists(FullPath))
        {
            // Create an empty list and serialize it to JSON
            var emptyList = new List<object>(); // Change 'object' to the type you want in the list
            string jsonContent = JsonSerializer.Serialize(emptyList);

            // Write the JSON content to the file
            File.WriteAllText(FullPath, jsonContent);
        }

        return FullPath;
    }

    public static string GetFile<TSource>(FileExtension extension)
    {
        return $"{typeof(TSource).Name}.{Enum.GetName(extension)}";
    }


    public static string GetDefaultExportFilePath<TSource>(FileExtension extension)
    {
        return GetFilePath(GetDesktopDirectoryPath(), GetFile<TSource>(extension));
    }

    public static string GetDefaultFilePath<TSource>(FileExtension extension)
    {
        return GetFilePath(GetDocumentsDirectoryPath(), GetFile<TSource>(extension));
    }
}