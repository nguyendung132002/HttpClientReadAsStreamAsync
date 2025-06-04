using System.Net.Http;

public class DownloadService : IDownloadService


{// Tạo một biến HttpClient dùng để gửi HTTP request.
// Nó được inject thông qua DI container trong constructor.
    private readonly HttpClient _httpClient;
    //Constructor nhận HttpClient từ bên ngoài (qua DI).Gán vào biến _httpClient để dùng cho các phương thức trong class.

    public DownloadService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

// Phương thức này bất đồng bộ (async) và trả về Task.

// Nhận vào:

// fileUrl: đường dẫn URL đến file muốn tải.

// targetFilePath: đường dẫn trên máy bạn muốn lưu file về.
    public async Task<bool> DownloadFileAsync(string url, string savePath)
    {
        try
        {//Gửi request đi và chỉ đọc headers trước, chưa đọc body ngay.

//HttpCompletionOption.ResponseHeadersRead giúp tiết kiệm bộ nhớ khi tải file lớn.
            using var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

            if (!response.IsSuccessStatusCode)
                return false;
//Lấy nội dung response dưới dạng stream (luồng dữ liệu).


            await using var stream = await response.Content.ReadAsStreamAsync();
//             Tạo file mới để ghi vào.

// FileMode.Create: tạo mới (ghi đè nếu đã tồn tại).

// FileAccess.Write: chỉ ghi.

// FileShare.None: không cho tiến trình khác truy cập trong khi đang ghi.
            await using var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None);
//Đọc dữ liệu từ httpStream (từ mạng) và ghi vào fileStream (đĩa).

// Dùng bất đồng bộ để không chặn luồng.
            await stream.CopyToAsync(fileStream);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
