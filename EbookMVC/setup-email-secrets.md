# Cấu hình Email Secrets

## Sử dụng User Secrets (Development)

### 1. Khởi tạo User Secrets
```bash
dotnet user-secrets init
```

### 2. Thêm email settings vào secrets
```bash
dotnet user-secrets set "EmailSettings:SenderEmail" "your-email@gmail.com"
dotnet user-secrets set "EmailSettings:Username" "your-email@gmail.com"
dotnet user-secrets set "EmailSettings:Password" "your-app-password"
```

### 3. Xem secrets đã lưu
```bash
dotnet user-secrets list
```

## Sử dụng Environment Variables (Production)

### Windows
```cmd
set EmailSettings__SenderEmail=your-email@gmail.com
set EmailSettings__Username=your-email@gmail.com
set EmailSettings__Password=your-app-password
```

### Linux/Mac
```bash
export EmailSettings__SenderEmail=your-email@gmail.com
export EmailSettings__Username=your-email@gmail.com
export EmailSettings__Password=your-app-password
```

## Azure App Service
Trong Configuration → Application settings:
- EmailSettings__SenderEmail
- EmailSettings__Username  
- EmailSettings__Password

## Docker
```dockerfile
ENV EmailSettings__SenderEmail=your-email@gmail.com
ENV EmailSettings__Username=your-email@gmail.com
ENV EmailSettings__Password=your-app-password
```
