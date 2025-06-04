using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

var builder = Host.CreateApplicationBuilder(args);

// Đăng ký HttpClient và dịch vụ Download
builder.Services.AddHttpClient<IDownloadService, DownloadService>();

var host = builder.Build();

// Gọi thử dịch vụ sau khi ứng dụng khởi chạy
await host.StartAsync();

var downloadService = host.Services.GetRequiredService<IDownloadService>();

string url = "https://example.com/image.jpg";  //địa chỉ file cần tải.
string folderPath = "D:\\Downloads"; //folderPath: nơi lưu file.
string fileName = "image.jpg";//fileName: tên file lưu.
string savePath = Path.Combine(folderPath, fileName);  //Path.Combine(...): kết hợp đường dẫn + tên file thành đường dẫn đầy đủ, tránh lỗi \ hoặc /.

// Tạo thư mục nếu chưa có
if (!Directory.Exists(folderPath))
{
    Directory.CreateDirectory(folderPath);
}

bool result = await downloadService.DownloadFileAsync(url, savePath);

Console.WriteLine(result
    ? $"✅ Đã tải thành công về: {savePath}"
    : "❌ Tải file thất bại.");

// Dừng app (nếu bạn chạy console) Dừng ứng dụng và giải phóng tài nguyên nếu có
await host.StopAsync();
