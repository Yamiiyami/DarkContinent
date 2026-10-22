# DarkContinent

## Tổng quan
DarkContinent là backend **ASP.NET Core Web API** cho một trò chơi nuôi thú cưng ảo. Dự án cung cấp API cho:

- Xác thực người dùng bằng JWT + refresh token.
- Quản lý vai trò và quyền (role/claim-based permission).
- Quản lý danh mục gameplay: `pet_types`, `foods`, `activities`.
- Vận hành vòng đời thú cưng (tạo thú cưng, bắt đầu/dừng game, cho ăn, chơi hoạt động).

> Nội dung README này được tổng hợp từ mã nguồn trong repository và **không dùng** các file trong thư mục `Doc/Docs`.

## Phạm vi API hiện có
Các controller chính:

- `AuthController` (`/api/auth`): `login`, `register`, `refresh`.
- `RoleController` (`/api/role`): CRUD role (phân trang danh sách).
- `RoleClaimController` (`/api/roleclaim`): quản lý permission claim cho role.
- `FoodController` (`/api/food`): CRUD food.
- `ActivityController` (`/api/activity`): CRUD activity.
- `PetTypeController` (`/api/pettype`): CRUD loại thú cưng.
- `PetController` (`/api/pet`): tạo thú cưng, lấy thú cưng còn sống, start/stop game, feed, play activity.

Lưu ý:

- Mặc định các endpoint yêu cầu `[Authorize]`; riêng các endpoint trong `AuthController` có `[AllowAnonymous]`.
- Cơ chế quyền chi tiết dùng custom policy `[HasPermission("...")]` với claim type `permission`.
- Service có logic `Sleep/WakeUp` cho thú cưng, nhưng hiện **chưa có endpoint controller tương ứng**.

## Công nghệ và thư viện chính
- .NET `net10.0` (`Microsoft.NET.Sdk.Web`).
- ASP.NET Core Web API.
- Entity Framework Core + SQL Server.
- ASP.NET Core Identity (custom `User : IdentityUser`).
- JWT access token + refresh token (ký bằng khóa đối xứng).
- OpenAPI + Scalar (`Scalar.AspNetCore`) để xem tài liệu API khi chạy Development.

## Cấu trúc mã nguồn đáng chú ý
- `Program.cs`: đăng ký DI, authentication/authorization, CORS, OpenAPI/Scalar, exception handler.
- `Controllers/`: định nghĩa endpoint API.
- `Services/`: nghiệp vụ chính (token, role/claim, food/activity/pettype, gameplay pet).
- `Data/`:
  - `AuthDbContext`: Identity + `RefreshTokens`.
  - `DarkContinentContext`: mapping bảng gameplay, soft delete query filter, audit timestamps.
- `Models/`: entity/domain models.
- `DTOs/`: request/response DTOs.
- `Authorization/`: custom permission policy provider + handler.
- `Middlewares/`: có `ApiKeyMiddleware`, `RequestLoggingMiddleware`, `GlobalExceptionHandler` (trong đó chỉ global exception handler đang được bật trong pipeline).

## Yêu cầu môi trường
- .NET SDK 10.0 (theo `TargetFramework net10.0`).
- SQL Server.
- Kết nối DB qua `ConnectionStrings:DefaultConnection`.

## Cấu hình
Thiết lập trong `appsettings.json` (hoặc `appsettings.Development.json`/User Secrets/biến môi trường):

- `ConnectionStrings:DefaultConnection`
- `Jwt:Key`
- `Jwt:Issuer`
- `Jwt:Audience`
- `Jwt:AccessTokenMinutes`
- `Jwt:RefreshTokenDays`
- `ApiKey`

Khuyến nghị: không dùng secret cứng trong mã nguồn khi triển khai thực tế; chuyển qua secret manager hoặc biến môi trường.

## Cài đặt và chạy
```bash
dotnet restore
dotnet build
dotnet run
```

Theo `launchSettings.json`, local profile dùng:

- HTTP: `http://localhost:5016`
- HTTPS: `https://localhost:7154`

## Sử dụng API
### 1) Lấy token
Gọi:

- `POST /api/auth/register`
- `POST /api/auth/login`
- `POST /api/auth/refresh`

### 2) Gọi endpoint bảo vệ
- Gửi header `Authorization` kèm JWT access token.
- Với endpoint có `[HasPermission]`, user cần có claim `permission` phù hợp (qua role claim hoặc user claim).

### 3) Tài liệu OpenAPI/Scalar
Khi chạy ở môi trường `Development`, truy cập giao diện Scalar (được map bởi `MapScalarApiReference`) để thử API.

## Database / Migration
Repository có migration cho `AuthDbContext` trong `Migrations/Auth`.

Ví dụ cập nhật DB Identity:
```bash
dotnet ef database update --context AuthDbContext
```

Mã nguồn hiện tại không thể hiện bước tự động migrate khi app khởi động.

## Build/Test
- Build: `dotnet build`
- Test: repository hiện không có project test riêng (`*Tests.csproj`), nên chưa có test tự động để chạy bằng `dotnet test`.

## Ghi chú quan trọng
- JSON response/request được cấu hình `snake_case` trong `Program.cs`.
- `GlobalExceptionHandler` trả lỗi chuẩn JSON với `message` và map mã lỗi theo loại exception.
- Có middleware kiểm tra API key (`x-client-request`) trong mã nguồn, nhưng hiện chưa được đăng ký vào pipeline HTTP.
