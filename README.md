# Elearn - Hệ thống quản lý khóa học lập trình

![Elearn Logo](https://img.shields.io/badge/Elearn-Admin%20Panel-blue)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-6.0-brightgreen)
![AdminLTE](https://img.shields.io/badge/AdminLTE-3.2-orange)

Elearn là một hệ thống quản lý khóa học lập trình được phát triển bằng ASP.NET Core MVC. Hệ thống cung cấp giao diện quản trị cho phép quản lý danh mục, khóa học và theo dõi hoạt động người dùng.

## Tính năng chính

### Quản lý khóa học
- Xem danh sách khóa học
- Thêm, sửa, xóa khóa học
- Xem chi tiết khóa học

### Quản lý danh mục
- Xem danh sách danh mục
- Thêm, sửa, xóa danh mục
- Xem chi tiết danh mục và các khóa học thuộc danh mục

### Dashboard
- Hiển thị thông tin tổng quan về hệ thống
- Biểu đồ thống kê khóa học theo danh mục
- Biểu đồ thống kê hoạt động người dùng
- Nhật ký hoạt động gần đây

### Hệ thống thông báo
- Hiển thị thông báo thành công/lỗi/cảnh báo
- Tự động ẩn thông báo sau 5 giây

### Nhật ký hoạt động
- Ghi lại tất cả các hoạt động CRUD
- Xem lịch sử hoạt động

## Công nghệ sử dụng

- **ASP.NET Core MVC 6.0**: Framework phát triển web
- **C#**: Ngôn ngữ lập trình
- **AdminLTE 3.2**: Template giao diện quản trị
- **Bootstrap 5**: Framework CSS
- **Chart.js**: Thư viện vẽ biểu đồ
- **Font Awesome**: Thư viện icon

## Yêu cầu hệ thống

- .NET 6.0 SDK hoặc cao hơn
- Visual Studio 2022 hoặc Visual Studio Code
- Trình duyệt web hiện đại (Chrome, Firefox, Edge, Safari)

## Hướng dẫn cài đặt

### Cài đặt .NET SDK

1. Tải và cài đặt [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) hoặc cao hơn.
2. Kiểm tra cài đặt bằng cách mở Command Prompt hoặc Terminal và chạy lệnh:
   ```
   dotnet --version
   ```

### Tải mã nguồn

1. Clone repository từ GitHub:
   ```
   git clone <repository-url>
   ```
   hoặc tải xuống dưới dạng file ZIP và giải nén.

2. Di chuyển vào thư mục dự án:
   ```
   cd Elearn
   ```

### Khôi phục các gói NuGet

```
dotnet restore
```

## Hướng dẫn khởi động

### Sử dụng Visual Studio

1. Mở file solution `EbookMVC.sln` bằng Visual Studio.
2. Nhấn F5 hoặc click vào nút "Start" để chạy ứng dụng.
3. Trình duyệt sẽ tự động mở với địa chỉ mặc định (thường là https://localhost:5001 hoặc http://localhost:5000).

### Sử dụng Command Line

1. Di chuyển vào thư mục dự án (thư mục chứa file .csproj):
   ```
   cd EbookMVC
   ```

2. Chạy ứng dụng:
   ```
   dotnet run
   ```

3. Mở trình duyệt và truy cập địa chỉ hiển thị trong terminal (thường là http://localhost:5127).

## Cấu trúc dự án

```
EbookMVC/
├── Controllers/         # Chứa các controller xử lý logic
├── Models/              # Chứa các model dữ liệu
├── Repository/          # Chứa các repository xử lý dữ liệu
├── Views/               # Chứa các view hiển thị giao diện
│   ├── Home/            # View cho trang chủ và dashboard
│   ├── Product/         # View cho quản lý khóa học
│   ├── Category/        # View cho quản lý danh mục
│   └── Shared/          # View dùng chung
├── wwwroot/             # Chứa các file tĩnh (CSS, JS, images)
└── Program.cs           # Cấu hình ứng dụng
```

## Hướng dẫn sử dụng

### Dashboard

Trang Dashboard hiển thị thông tin tổng quan về hệ thống, bao gồm:
- Số lượng khóa học
- Số lượng danh mục
- Doanh thu (giả định)
- Số lượng người dùng
- Biểu đồ thống kê khóa học theo danh mục
- Biểu đồ thống kê hoạt động
- Nhật ký hoạt động gần đây

### Quản lý khóa học

- **Xem danh sách**: Truy cập menu "Quản lý khóa học"
- **Thêm mới**: Click vào nút "Thêm khóa học mới"
- **Xem chi tiết**: Click vào icon "Xem chi tiết" bên cạnh khóa học
- **Chỉnh sửa**: Click vào icon "Chỉnh sửa" bên cạnh khóa học
- **Xóa**: Click vào icon "Xóa" bên cạnh khóa học

### Quản lý danh mục

- **Xem danh sách**: Truy cập menu "Quản lý danh mục"
- **Thêm mới**: Click vào nút "Thêm danh mục mới"
- **Xem chi tiết**: Click vào icon "Xem chi tiết" bên cạnh danh mục
- **Chỉnh sửa**: Click vào icon "Chỉnh sửa" bên cạnh danh mục
- **Xóa**: Click vào icon "Xóa" bên cạnh danh mục

### Nhật ký hoạt động

- Truy cập từ Dashboard, click vào "Xem tất cả" ở phần Hoạt động gần đây

## Phát triển

### Thêm chức năng mới

1. Tạo model mới trong thư mục `Models/`
2. Tạo repository interface và implementation trong thư mục `Repository/`
3. Đăng ký repository trong `Program.cs`
4. Tạo controller mới trong thư mục `Controllers/`
5. Tạo các view trong thư mục `Views/`

## Liên hệ

Nếu bạn có bất kỳ câu hỏi hoặc góp ý nào, vui lòng liên hệ:
- Email: [tienphat3968@gmail.com](mailto:tienphat3968@gmail.com)
- GitHub: [iLuminase](https://github.com/iLuminase)

## Giấy phép

Dự án này được phân phối dưới giấy phép MIT. Xem file `LICENSE` để biết thêm chi tiết.
