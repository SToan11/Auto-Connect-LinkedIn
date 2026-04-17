# AutoConnectLinkedIn

Ứng dụng WPF trên Windows dùng Selenium để tự động đăng nhập LinkedIn và gửi lời mời kết nối theo luồng hiện tại của dự án.

## Tổng Quan

Sau khi nhập `email` và `password`, ứng dụng sẽ:

1. Mở Chrome và truy cập LinkedIn.
2. Đăng nhập bằng tài khoản đã nhập trên giao diện.
3. Vào trang `My Network`.
4. Quét các nút `Connect` hiển thị trên trang.
5. Chọn ngẫu nhiên khoảng 10 đến 20 người để gửi lời mời kết nối.
6. Ghi log kết quả ra các file trong thư mục `logs`.

## Công Nghệ

- `WPF`
- `.NET Framework 4.7.2`
- `Selenium WebDriver`
- `ChromeDriver`
- `Node.js` cho script chuyển dữ liệu `TXT` sang `JSON`

## Yêu Cầu

- Windows
- Visual Studio có hỗ trợ `.NET Framework 4.7.2`
- Google Chrome đã cài đặt
- NuGet packages được restore đầy đủ
- Node.js nếu muốn chạy `convert.js`

## Cách Chạy

1. Mở `AutoConnectLinkedIn.sln` trong Visual Studio.
2. Restore NuGet packages nếu cần.
3. Build solution.
4. Chạy ứng dụng.
5. Nhập `Email` và `Password`.
6. Bấm `Start`.

## Luồng Hoạt Động

Phần xử lý chính nằm trong `MainWindow.xaml.cs`:

- Kiểm tra `Email` và `Password`.
- Mở LinkedIn bằng `ChromeDriver`.
- Đăng nhập.
- Phát hiện lỗi đăng nhập qua phần tử `#error-for-password`.
- Nếu đăng nhập thành công, mở `My Network`.
- Tìm các nút `Connect`.
- Ghi lại từng profile đã connect bằng `LogService`.
- Kết thúc bằng cách đóng browser và thoát ứng dụng.

## File Sinh Ra Khi Chạy

`LogService` ghi file theo thư mục base của ứng dụng, nên khi chạy từ Visual Studio các file này thường nằm trong `bin\\Debug\\logs\\`:

- `login_log.txt`
- `connect_log.txt`
- `connected_people.txt`

Ngoài ra dự án còn có:

- `logs/connected_people.json`

File này có thể được tạo từ `logs/connected_people.txt` bằng script `convert.js`.

## Script Chuyển JSON

Repo có `convert.js` ở thư mục gốc và một bản tương tự trong `logs/`.
Script này đọc `connected_people.txt`, tách từng dòng theo định dạng:

```text
name | profileUrl | connectedAt
```

và xuất ra `connected_people.json`.

Lưu ý: script đang dùng đường dẫn tương đối, nên hãy chạy nó trong đúng thư mục đang chứa `connected_people.txt`.

Ví dụ:

```bash
cd logs
node convert.js
```

## Cấu Trúc Dự Án

- `App.xaml`, `App.xaml.cs`: entry point của ứng dụng WPF.
- `MainWindow.xaml`: giao diện đăng nhập.
- `MainWindow.xaml.cs`: toàn bộ logic Selenium và tự động connect.
- `LogService.cs`: ghi log đăng nhập, connect, và danh sách người đã connect.
- `convert.js`: chuyển danh sách kết nối từ `TXT` sang `JSON`.
- `autologin.txt`: file resource hiện có trong project, nhưng chưa được code chính sử dụng trực tiếp.
- `packages.config`, `AutoConnectLinkedIn.csproj`: khai báo dependencies và cấu hình build.

## Ghi Chú

- Đây là công cụ tự động hóa trình duyệt cho LinkedIn, nên cần dùng cẩn thận và tuân thủ chính sách của nền tảng.
- Dự án hiện đang có cả file source lẫn file output lịch sử trong `logs/`, `bin/`, và `obj/`. `.gitignore` đã được bổ sung để tránh các file sinh ra mới bị đưa vào Git.
