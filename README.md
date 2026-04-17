# AutoConnectLinkedIn

## Tiếng Việt

Ứng dụng WPF trên Windows dùng Selenium để tự động đăng nhập LinkedIn và gửi lời mời kết nối theo luồng hiện tại của dự án.

### Tổng Quan

Sau khi nhập `email` và `password`, ứng dụng sẽ:

1. Mở Chrome và truy cập LinkedIn.
2. Đăng nhập bằng tài khoản đã nhập trên giao diện.
3. Vào trang `My Network`.
4. Quét các nút `Connect` hiển thị trên trang.
5. Chọn ngẫu nhiên khoảng 10 đến 20 người để gửi lời mời kết nối.
6. Ghi log kết quả ra các file trong thư mục `logs`.

### Công Nghệ

- `WPF`
- `.NET Framework 4.7.2`
- `Selenium WebDriver`
- `ChromeDriver`
- `Node.js` cho script chuyển dữ liệu `TXT` sang `JSON`

### Yêu Cầu

- Windows
- Visual Studio có hỗ trợ `.NET Framework 4.7.2`
- Google Chrome đã cài đặt
- NuGet packages được restore đầy đủ
- Node.js nếu muốn chạy `convert.js`

### Cách Chạy

1. Mở `AutoConnectLinkedIn.sln` trong Visual Studio.
2. Restore NuGet packages nếu cần.
3. Build solution.
4. Chạy ứng dụng.
5. Nhập `Email` và `Password`.
6. Bấm `Start`.

### Luồng Hoạt Động

Phần xử lý chính nằm trong `MainWindow.xaml.cs`:

- Kiểm tra `Email` và `Password`.
- Mở LinkedIn bằng `ChromeDriver`.
- Đăng nhập.
- Phát hiện lỗi đăng nhập qua phần tử `#error-for-password`.
- Nếu đăng nhập thành công, mở `My Network`.
- Tìm các nút `Connect`.
- Ghi lại từng profile đã connect bằng `LogService`.
- Kết thúc bằng cách đóng browser và thoát ứng dụng.

### File Sinh Ra Khi Chạy

`LogService` ghi file theo thư mục base của ứng dụng, nên khi chạy từ Visual Studio các file này thường nằm trong `bin\\Debug\\logs\\`:

- `login_log.txt`
- `connect_log.txt`
- `connected_people.txt`

Ngoài ra dự án còn có:

- `logs/connected_people.json`

File này có thể được tạo từ `logs/connected_people.txt` bằng script `convert.js`.

### Script Chuyển JSON

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

### Cấu Trúc Dự Án

- `App.xaml`, `App.xaml.cs`: entry point của ứng dụng WPF.
- `MainWindow.xaml`: giao diện đăng nhập.
- `MainWindow.xaml.cs`: toàn bộ logic Selenium và tự động connect.
- `LogService.cs`: ghi log đăng nhập, connect, và danh sách người đã connect.
- `convert.js`: chuyển danh sách kết nối từ `TXT` sang `JSON`.
- `autologin.txt`: file resource hiện có trong project, nhưng chưa được code chính sử dụng trực tiếp.
- `packages.config`, `AutoConnectLinkedIn.csproj`: khai báo dependencies và cấu hình build.

### Ghi Chú

- Đây là công cụ tự động hóa trình duyệt cho LinkedIn, nên cần dùng cẩn thận và tuân thủ chính sách của nền tảng.
- Dự án hiện đang có cả file source lẫn file output lịch sử trong `logs/`, `bin/`, và `obj/`. `.gitignore` đã được bổ sung để tránh các file sinh ra mới bị đưa vào Git.

## English

This is a Windows WPF application that uses Selenium to automate LinkedIn sign-in and send connection requests based on the current project flow.

### Overview

After entering `email` and `password`, the app will:

1. Open Chrome and navigate to LinkedIn.
2. Sign in with the credentials entered in the UI.
3. Go to `My Network`.
4. Scan the page for visible `Connect` buttons.
5. Randomly choose about 10 to 20 people to connect with.
6. Write the results to files inside the `logs` folder.

### Tech Stack

- `WPF`
- `.NET Framework 4.7.2`
- `Selenium WebDriver`
- `ChromeDriver`
- `Node.js` for converting `TXT` data to `JSON`

### Requirements

- Windows
- Visual Studio with `.NET Framework 4.7.2` support
- Google Chrome installed
- NuGet packages restored
- Node.js if you want to run `convert.js`

### How To Run

1. Open `AutoConnectLinkedIn.sln` in Visual Studio.
2. Restore NuGet packages if needed.
3. Build the solution.
4. Run the application.
5. Enter `Email` and `Password`.
6. Click `Start`.

### Workflow

The main logic lives in `MainWindow.xaml.cs`:

- Validate `Email` and `Password`.
- Open LinkedIn with `ChromeDriver`.
- Sign in.
- Detect login errors through the `#error-for-password` element.
- If login succeeds, open `My Network`.
- Find `Connect` buttons.
- Log each connected profile through `LogService`.
- Close the browser and exit the app.

### Generated Files

`LogService` writes files relative to the application's base directory, so when run from Visual Studio these files are usually located in `bin\\Debug\\logs\\`:

- `login_log.txt`
- `connect_log.txt`
- `connected_people.txt`

The repository also includes:

- `logs/connected_people.json`

This file can be generated from `logs/connected_people.txt` using `convert.js`.

### JSON Conversion Script

The repo contains `convert.js` in the root folder and a similar copy in `logs/`.
The script reads `connected_people.txt`, splits each line using this format:

```text
name | profileUrl | connectedAt
```

and writes `connected_people.json`.

Note: the script uses a relative path, so run it from the directory that contains `connected_people.txt`.

Example:

```bash
cd logs
node convert.js
```

### Project Structure

- `App.xaml`, `App.xaml.cs`: WPF application entry point.
- `MainWindow.xaml`: login UI.
- `MainWindow.xaml.cs`: Selenium automation and connect logic.
- `LogService.cs`: logs sign-in results, connect counts, and connected people.
- `convert.js`: converts connection logs from `TXT` to `JSON`.
- `autologin.txt`: existing resource file in the project, but it is not used directly by the main code.
- `packages.config`, `AutoConnectLinkedIn.csproj`: dependency and build configuration.

### Notes

- This is a browser automation tool for LinkedIn, so it should be used carefully and in line with the platform's policies.
- The repository currently contains both source files and historical runtime output in `logs/`, `bin/`, and `obj/`. `.gitignore` was added to prevent new generated files from being committed.
