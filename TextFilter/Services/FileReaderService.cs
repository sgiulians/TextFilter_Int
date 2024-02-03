public class FileReaderService : IFileReaderService
{
    public string ReadAllText(string path) => File.ReadAllText(path);
}

