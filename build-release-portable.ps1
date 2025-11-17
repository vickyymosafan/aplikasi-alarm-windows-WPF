# Script untuk build versi portable (tanpa installer)
# Author: vickymosafan

param(
    [string]$Version = "1.0.0"
)

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Alarm Windows - Portable Build" -ForegroundColor Cyan
Write-Host "  Version: $Version" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Konfigurasi
$AppName = "AlarmWindows-Portable"
$OutputDir = "release"

# Bersihkan folder output lama
Write-Host "[1/4] Membersihkan folder output lama..." -ForegroundColor Yellow
if (Test-Path $OutputDir) {
    Remove-Item -Recurse -Force $OutputDir
}
New-Item -ItemType Directory -Path $OutputDir | Out-Null

# Build dan Publish aplikasi (framework-dependent, lebih kecil)
Write-Host "[2/4] Publishing aplikasi (portable)..." -ForegroundColor Yellow
dotnet clean --configuration Release | Out-Null
dotnet publish -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true

if ($LASTEXITCODE -ne 0) {
    Write-Host "Error: Build gagal!" -ForegroundColor Red
    exit 1
}

# Copy files ke folder release
Write-Host "[3/4] Menyalin files ke folder release..." -ForegroundColor Yellow
$ReleaseFolder = "$OutputDir\$AppName-v$Version"
New-Item -ItemType Directory -Path $ReleaseFolder | Out-Null

# Copy executable dan dependencies
Copy-Item "bin\Release\net10.0-windows\win-x64\publish\*" -Destination $ReleaseFolder -Recurse -Force

# Buat README untuk versi portable
$ReadmeContent = @"
# Alarm Windows v$Version (Portable)
Aplikasi alarm sederhana untuk Windows dengan timezone Indonesia (WIB)

## PENTING - System Requirements:
⚠️ Versi portable ini memerlukan .NET 10 Runtime terinstall di komputer Anda.

Download .NET 10 Runtime:
https://dotnet.microsoft.com/download/dotnet/10.0

Pilih: ".NET Desktop Runtime" untuk Windows x64

## Cara Menggunakan:
1. Pastikan .NET 10 Runtime sudah terinstall
2. Double-click file 'AplikasiAlarmWindows.exe'
3. Pilih jam dan menit untuk alarm
4. Pilih suara alarm (opsional)
5. Klik tombol '+ TAMBAH' untuk menambahkan alarm

## Keuntungan Versi Portable:
✓ Ukuran file lebih kecil (~5-10 MB)
✓ Startup lebih cepat
✓ Update .NET otomatis dari Windows Update

## Kekurangan:
✗ Perlu install .NET Runtime terlebih dahulu
✗ Tidak bisa jalan di komputer tanpa .NET

---
Jika Anda ingin versi yang tidak perlu install .NET,
gunakan versi "AlarmWindows-v$Version-win-x64.zip" (self-contained)

Made with ❤️ by vickymosafan © 2025
"@

Set-Content -Path "$ReleaseFolder\README.txt" -Value $ReadmeContent -Encoding UTF8

# Buat ZIP file
Write-Host "[4/4] Membuat file ZIP..." -ForegroundColor Yellow
$ZipFileName = "$OutputDir\$AppName-v$Version-win-x64.zip"
Compress-Archive -Path $ReleaseFolder -DestinationPath $ZipFileName -Force

# Tampilkan informasi
Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "  Build Selesai!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "Output files:" -ForegroundColor Cyan
Write-Host "  - Folder: $ReleaseFolder" -ForegroundColor White
Write-Host "  - ZIP: $ZipFileName" -ForegroundColor White
Write-Host ""

# Tampilkan ukuran file
$ZipSize = (Get-Item $ZipFileName).Length / 1MB
Write-Host "Ukuran ZIP: $([math]::Round($ZipSize, 2)) MB" -ForegroundColor Cyan
Write-Host ""
Write-Host "⚠️  CATATAN: Versi ini memerlukan .NET 10 Runtime!" -ForegroundColor Yellow
Write-Host ""
