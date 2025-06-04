public interface IDownloadService
{
    Task<bool> DownloadFileAsync(string url, string savePath);
}
