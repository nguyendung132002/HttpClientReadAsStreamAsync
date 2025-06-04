using System.Net.Http;

public class DownloadService : IDownloadService
{
    private readonly HttpClient _httpClient;

    public DownloadService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> DownloadFileAsync(string url, string savePath)
    {
        try
        {
            using var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

            if (!response.IsSuccessStatusCode)
                return false;

            await using var stream = await response.Content.ReadAsStreamAsync();
            await using var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None);

            await stream.CopyToAsync(fileStream);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
