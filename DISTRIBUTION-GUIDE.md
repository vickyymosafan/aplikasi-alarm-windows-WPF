# Panduan Distribusi Aplikasi Alarm Windows

## 📦 Cara Build & Package

### Opsi 1: Self-Contained (Recommended)
**Ukuran: ~150 MB | Tidak perlu install .NET**

```powershell
.\build-release.ps1 -Version "1.0.0"
```

**Hasil:**
- `release\AlarmWindows-v1.0.0\` - Folder aplikasi
- `release\AlarmWindows-v1.0.0-win-x64.zip` - File untuk distribusi

**Kelebihan:**
- ✅ User tidak perlu install .NET Runtime
- ✅ Langsung bisa dijalankan
- ✅ Cocok untuk user awam

**Kekurangan:**
- ❌ Ukuran file besar (~150 MB)

---

### Opsi 2: Portable (Framework-Dependent)
**Ukuran: ~5-10 MB | Perlu .NET 10 Runtime**

```powershell
.\build-release-portable.ps1 -Version "1.0.0"
```

**Hasil:**
- `release\AlarmWindows-Portable-v1.0.0\` - Folder aplikasi
- `release\AlarmWindows-Portable-v1.0.0-win-x64.zip` - File untuk distribusi

**Kelebihan:**
- ✅ Ukuran file kecil
- ✅ Startup lebih cepat

**Kekurangan:**
- ❌ User harus install .NET 10 Runtime dulu

---

## 🌐 Cara Distribusi

### 1. Google Drive / OneDrive
1. Upload file `.zip` hasil build
2. Set sharing ke "Anyone with the link"
3. Copy link dan bagikan

**Contoh:**
```
Download Alarm Windows v1.0.0:
https://drive.google.com/file/d/xxxxx/view?usp=sharing

Cara Install:
1. Download file ZIP
2. Extract ke folder
3. Jalankan AplikasiAlarmWindows.exe
```

---

### 2. GitHub Releases (Recommended)
1. Push code ke GitHub repository
2. Buat tag baru:
   ```bash
   git tag -a v1.0.0 -m "Release version 1.0.0"
   git push origin v1.0.0
   ```
3. Buka GitHub > Releases > Create new release
4. Upload file `.zip`
5. Tulis release notes

**Contoh Release Notes:**
```markdown
## Alarm Windows v1.0.0

### Features
- ⏰ Multiple alarms dengan timezone Indonesia (WIB)
- 🔊 Custom sound alarm (file .wav)
- 🗑️ Auto-delete alarm setelah berbunyi
- 📢 Notifikasi popup saat alarm aktif

### Download
- [AlarmWindows-v1.0.0-win-x64.zip](link) - Self-contained (150 MB)
- [AlarmWindows-Portable-v1.0.0-win-x64.zip](link) - Portable (10 MB, perlu .NET 10)

### System Requirements
- Windows 10/11 (64-bit)
- Tidak perlu install .NET Runtime (versi self-contained)

### Installation
1. Download file ZIP
2. Extract ke folder
3. Jalankan AplikasiAlarmWindows.exe
```

---

### 3. Website Sendiri
Upload ke hosting dan buat landing page sederhana.

**Contoh struktur:**
```
website/
├── index.html
├── downloads/
│   ├── AlarmWindows-v1.0.0-win-x64.zip
│   └── AlarmWindows-Portable-v1.0.0-win-x64.zip
└── screenshots/
    ├── screenshot1.png
    └── screenshot2.png
```

---

## 🛡️ Mengatasi Windows Defender

User mungkin mendapat warning dari Windows Defender karena aplikasi tidak memiliki digital signature.

**Solusi untuk User:**
1. Klik "More info"
2. Klik "Run anyway"

**Solusi untuk Developer (Anda):**
1. Beli Code Signing Certificate (~$100-300/tahun)
2. Sign aplikasi dengan certificate
3. Windows akan trust aplikasi Anda

**Alternatif Gratis:**
- Publish ke Microsoft Store (one-time fee $19)
- Build reputation dengan banyak download

---

## 📝 Checklist Sebelum Distribusi

- [ ] Test di komputer lain (bukan development machine)
- [ ] Test di Windows 10 dan Windows 11
- [ ] Pastikan folder Resource ter-copy dengan benar
- [ ] Pastikan file .wav bisa diputar
- [ ] Test semua fitur (tambah alarm, delete, stop)
- [ ] Buat README.txt yang jelas
- [ ] Screenshot aplikasi untuk promosi
- [ ] Tulis release notes yang informatif

---

## 🚀 Update Aplikasi

Ketika ada versi baru:

1. Update version number di script:
   ```powershell
   .\build-release.ps1 -Version "1.1.0"
   ```

2. Upload ke tempat yang sama

3. Informasikan user tentang update:
   - Email blast
   - Post di social media
   - Update di website/GitHub

---

## 📊 Tracking Downloads

### GitHub Releases
- Otomatis tracking download count
- Lihat di halaman Releases

### Google Drive
- Tidak ada tracking otomatis
- Gunakan URL shortener dengan analytics (bit.ly, tinyurl)

### Website Sendiri
- Gunakan Google Analytics
- Track download button clicks

---

## 💡 Tips Marketing

1. **Buat Video Demo**
   - Upload ke YouTube
   - Tunjukkan cara pakai

2. **Screenshot Menarik**
   - Tampilkan UI yang clean
   - Highlight fitur utama

3. **Social Media**
   - Post di Facebook, Twitter, Instagram
   - Gunakan hashtag: #AlarmWindows #ProductivityApp

4. **Forum & Community**
   - Post di Kaskus, Reddit
   - Share di grup WhatsApp/Telegram

5. **Blog Post**
   - Tulis artikel tentang aplikasi
   - SEO optimization

---

## 📞 Support User

Siapkan channel untuk user bertanya:
- Email: your-email@example.com
- GitHub Issues (jika open source)
- WhatsApp/Telegram group
- FAQ di README

---

Made with ❤️ by vickymosafan © 2025
