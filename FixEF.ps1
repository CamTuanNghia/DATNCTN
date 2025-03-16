
Write-Host "==== Bắt đầu Fix lỗi Entity Framework ===="
Write-Host "1. Bỏ chặn tất cả các file DLL..."
Get-ChildItem -Recurse .\packages\EntityFramework.6.2.0\ | Unblock-File

Write-Host "2. Xóa cache NuGet..."
nuget locals all -clear

Write-Host "3. Reset NuGet Cache..."
if (Test-Path "$HOME\.nuget") {
    Remove-Item -Recurse -Force "$HOME\.nuget"
} else {
    Write-Host "Không tìm thấy thư mục NuGet Cache!"
}

Write-Host "4. Cài đặt lại Entity Framework..."
Uninstall-Package EntityFramework -Force
Install-Package EntityFramework -Version 6.2.0

Write-Host "5. Reset Migrations..."
Remove-Item -Recurse -Force .\Migrations
Write-Host "==== Fix lỗi thành công! ===="
