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

1. Pastikan .NET 6.0 SDK terinstall
2. Tambahkan file `alarm-sound.wav` ke folder `Resource/` (lihat Resource/README.md)
3. Restore NuGet packages:
   ```
   dotnet restore
   ```
4. Build proyek:
   ```
   dotnet build
   ```
5. Run aplikasi:
   ```
   dotnet run
   ```

## Cara Build Release

Untuk membuat executable yang dapat didistribusikan:

```
dotnet publish -c Release -r win-x64 --self-contained false
```

Executable akan berada di folder `bin/Release/net6.0-windows/win-x64/publish/`

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

## Lisensi

Proyek ini dibuat untuk keperluan pembelajaran dan development.

## Catatan

Seluruh kode ditulis dalam bahasa Indonesia untuk memudahkan pemeliharaan dan kolaborasi tim lokal.
