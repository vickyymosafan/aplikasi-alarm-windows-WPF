using System;
using System.Collections.ObjectModel;
using System.IO;
using AplikasiAlarmWindows.Common;
using AplikasiAlarmWindows.Model;
using Newtonsoft.Json;

namespace AplikasiAlarmWindows.Service
{
    /// <summary>
    /// Layanan untuk menyimpan dan memuat data alarm dari file JSON
    /// </summary>
    public class LayananPenyimpanan
    {
        private readonly string _namaFile = Constants.AlarmDataFileName;

        /// <summary>
        /// Path lengkap ke file penyimpanan alarm
        /// </summary>
        private string PathLengkap => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _namaFile);

        /// <summary>
        /// Menyimpan daftar alarm ke file JSON
        /// </summary>
        /// <param name="daftarAlarm">Koleksi alarm yang akan disimpan</param>
        public void SimpanAlarm(ObservableCollection<Alarm> daftarAlarm)
        {
            try
            {
                // Serialize daftar alarm ke format JSON dengan indentasi
                string jsonData = JsonConvert.SerializeObject(daftarAlarm, Formatting.Indented);
                
                // Tulis ke file
                File.WriteAllText(PathLengkap, jsonData);
                
                Console.WriteLine($"Alarm berhasil disimpan ke {PathLengkap}");
            }
            catch (IOException ex)
            {
                // Error saat operasi file I/O
                Console.WriteLine($"Error saat menyimpan alarm: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                // Error permission denied
                Console.WriteLine($"Tidak memiliki akses untuk menyimpan file: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Error lainnya
                Console.WriteLine($"Error tidak terduga saat menyimpan alarm: {ex.Message}");
            }
        }

        /// <summary>
        /// Memuat daftar alarm dari file JSON
        /// </summary>
        /// <returns>Koleksi alarm yang dimuat dari file, atau koleksi kosong jika file tidak ada atau error</returns>
        public ObservableCollection<Alarm> MuatAlarm()
        {
            try
            {
                // Cek apakah file ada
                if (!File.Exists(PathLengkap))
                {
                    Console.WriteLine($"File {_namaFile} tidak ditemukan. Membuat daftar alarm kosong.");
                    return new ObservableCollection<Alarm>();
                }

                // Baca file JSON
                string jsonData = File.ReadAllText(PathLengkap);
                
                // Deserialize JSON ke ObservableCollection<Alarm>
                var daftarAlarm = JsonConvert.DeserializeObject<ObservableCollection<Alarm>>(jsonData);
                
                // Jika deserialisasi menghasilkan null, return koleksi kosong
                if (daftarAlarm == null)
                {
                    Console.WriteLine("Data alarm kosong atau tidak valid. Membuat daftar alarm kosong.");
                    return new ObservableCollection<Alarm>();
                }
                
                Console.WriteLine($"Berhasil memuat {daftarAlarm.Count} alarm dari {PathLengkap}");
                return daftarAlarm;
            }
            catch (IOException ex)
            {
                // Error saat operasi file I/O
                Console.WriteLine($"Error saat memuat alarm: {ex.Message}");
                return new ObservableCollection<Alarm>();
            }
            catch (JsonException ex)
            {
                // Error saat parsing JSON (file corrupt atau format tidak valid)
                Console.WriteLine($"Error parsing JSON: {ex.Message}. File mungkin corrupt.");
                return new ObservableCollection<Alarm>();
            }
            catch (Exception ex)
            {
                // Error lainnya
                Console.WriteLine($"Error tidak terduga saat memuat alarm: {ex.Message}");
                return new ObservableCollection<Alarm>();
            }
        }
    }
}
