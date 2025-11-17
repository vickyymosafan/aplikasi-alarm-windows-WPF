# Aplikasi Alarm Windows

Aplikasi desktop alarm sederhana yang dibangun dengan C# dan WPF menggunakan pola MVVM.

## Deskripsi

Aplikasi Alarm Windows memungkinkan pengguna untuk:
- Menambahkan alarm dengan input jam dan menit
- Menyimpan alarm secara persisten dalam file JSON
- Menerima notifikasi visual dan audio ketika alarm aktif
- Menghentikan alarm yang sedang berbunyi

## Teknologi

- **Framework**: .NET 6.0
- **UI**: WPF (Windows Presentation Foundation)
- **Pattern**: MVVM (Model-View-ViewModel)
- **Dependencies**: Newtonsoft.Json
- **Audio**: System.Media.SoundPlayer

## Struktur Proyek

```
AplikasiAlarmWindows/
├── Model/              # Data models
├── ViewModel/          # ViewModels untuk MVVM
├── View/               # XAML views
├── Service/            # Business logic services
├── Resource/           # Resources (styles, audio)
├── Command/            # ICommand implementations
├── App.xaml            # Application entry point
└── App.xaml.cs
```

## Persyaratan Sistem

- Windows 10 atau lebih baru
- .NET 6.0 SDK atau runtime
- Visual Studio 2022 (untuk development)

## Cara Build

### Prerequisites
1. Pastikan .NET 6.0 SDK terinstall
2. Tambahkan file `alarm-sound.wav` ke folder `Resource/` (lihat Resource/README.md)

### Build Debug
1. Restore NuGet packages:
   ```
   dotnet restore
   ```

2. Build proyek (Debug mode):
   ```
   dotnet build
   ```

3. Run aplikasi:
   ```
   dotnet run
   ```
   
   Atau jalankan executable langsung:
   ```
   .\bin\Debug\net6.0-windows\AplikasiAlarmWindows.exe
   ```

### Build Release

Untuk membuat executable yang dapat didistribusikan:

1. Build Release mode:
   ```
   dotnet build -c Release
   ```
   
   Executable: `bin\Release\net6.0-windows\AplikasiAlarmWindows.exe`

2. Publish untuk distribusi (framework-dependent):
   ```
   dotnet publish -c Release -r win-x64 --self-contained false
   ```
   
   Output: `bin\Release\net6.0-windows\win-x64\publish\`

3. Publish self-contained (tidak perlu .NET runtime):
   ```
   dotnet publish -c Release -r win-x64 --self-contained true
   ```
   
   Output: `bin\Release\net6.0-windows\win-x64\publish\`

### Build Configuration

- **Output Type**: Windows Application (WinExe) - no console window
- **Platform**: x64
- **Target Framework**: .NET 6.0 Windows
- **Dependencies**: Newtonsoft.Json (automatically restored)

### Deployment

Untuk distribusi aplikasi:

1. Copy seluruh folder `publish\` ke komputer target
2. Pastikan file `alarm-sound.wav` ada di folder `Resource\`
3. Jalankan `AplikasiAlarmWindows.exe`

**Framework-dependent deployment:**
- Ukuran lebih kecil
- Memerlukan .NET 6.0 Runtime di komputer target

**Self-contained deployment:**
- Ukuran lebih besar (~70MB)
- Tidak memerlukan .NET Runtime di komputer target
- Recommended untuk distribusi ke end users

## Status Development

Proyek ini sedang dalam tahap development mengikuti spec yang ada di `.kiro/specs/aplikasi-alarm-windows/`

### Task Progress

- [x] Task 1: Setup proyek dan struktur folder
- [ ] Task 2: Implementasi Model layer
- [ ] Task 3: Implementasi Command infrastructure
- [ ] Task 4-7: Implementasi Service layer
- [ ] Task 8-9: Implementasi ViewModel
- [ ] Task 10: Implementasi styling
- [ ] Task 11-12: Implementasi Views
- [ ] Task 13: Setup App.xaml
- [ ] Task 14: Resource embedding
- [ ] Task 15: Testing (optional)
- [ ] Task 16: Build configuration

## Author

**vickymosafan**

Made with by vickymosafan © 2025

## Lisensi

Proyek ini dibuat untuk keperluan pembelajaran dan development.

## Catatan

Seluruh kode ditulis dalam bahasa Indonesia untuk memudahkan pemeliharaan dan kolaborasi tim lokal.
