# Script untuk build dan package aplikasi Alarm Windows
# Author: vickymosafan

param(
    [string]$Version = "1.0.0"
)

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Alarm Windows - Build & Package" -ForegroundColor Cyan
Write-Host "  Version: $Version" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Konfigurasi
$AppName = "AlarmWindows"
$OutputDir = "release"
$PublishDir = "bin\Release\net10.0-windows\win-x64\publish"

# Bersihkan folder output lama
Write-Host "[1/5] Membersihkan folder output lama..." -ForegroundColor Yellow
if (Test-Path $OutputDir) {
    Remove-Item -Recurse -Force $OutputDir
}
New-Item -ItemType Directory -Path $OutputDir | Out-Null

# Build dan Publish aplikasi
Write-Host "[2/5] Building aplikasi..." -ForegroundColor Yellow
dotnet clean --configuration Release | Out-Null

Write-Host "[3/5] Publishing aplikasi (self-contained)..." -ForegroundColor Yellow
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true

if ($LASTEXITCODE -ne 0) {
    Write-Host "Error: Build gagal!" -ForegroundColor Red
    exit 1
}

# Copy files ke folder release
Write-Host "[4/5] Menyalin files ke folder release..." -ForegroundColor Yellow
$ReleaseFolder = "$OutputDir\$AppName-v$Version"
New-Item -ItemType Directory -Path $ReleaseFolder | Out-Null

# Copy executable dan dependencies
Copy-Item "$PublishDir\*" -Destination $ReleaseFolder -Recurse -Force

# Buat README untuk user
$ReadmeContent = @"
# Alarm Windows v$Version
Aplikasi alarm sederhana untuk Windows dengan timezone Indonesia (WIB)

## Cara Menggunakan:
1. Double-click file 'AplikasiAlarmWindows.exe'
2. Pilih jam dan menit untuk alarm
3. Pilih suara alarm (opsional)
4. Klik tombol '+ TAMBAH' untuk menambahkan alarm
5. Alarm akan berbunyi otomatis pada waktu yang ditentukan
6. Klik 'STOP ALARM' untuk menghentikan alarm yang berbunyi

## Fitur:
- Timezone Indonesia (WIB) otomatis
- Multiple alarms
- Custom sound alarm (file .wav)
- Auto-delete alarm setelah berbunyi
- Notifikasi popup saat alarm aktif

## Menambahkan Suara Alarm Sendiri:
1. Siapkan file audio format .wav
2. Copy file .wav ke folder 'Resource'
3. Restart aplikasi
4. Suara baru akan muncul di dropdown

## System Requirements:
- Windows 10/11 (64-bit)
- Tidak perlu install .NET Runtime (sudah included)

## Troubleshooting:
- Jika aplikasi tidak bisa dibuka, pastikan Windows Defender tidak memblokir
- Klik kanan > Properties > Unblock (jika ada)

---
Made with ❤️ by vickymosafan © 2025
"@

Set-Content -Path "$ReleaseFolder\README.txt" -Value $ReadmeContent -Encoding UTF8

# Buat ZIP file
Write-Host "[5/5] Membuat file ZIP..." -ForegroundColor Yellow
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

# Tampilkan isi folder
Write-Host "Isi folder release:" -ForegroundColor Cyan
Get-ChildItem $ReleaseFolder | Format-Table Name, Length, LastWriteTime -AutoSize

Write-Host ""
Write-Host "Siap untuk didistribusikan! 🎉" -ForegroundColor Green
Write-Host "Upload file ZIP ke Google Drive, OneDrive, atau GitHub Releases" -ForegroundColor Yellow
Write-Host ""
