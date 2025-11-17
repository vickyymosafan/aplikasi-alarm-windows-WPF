using System;
using System.IO;
using System.Media;

namespace AplikasiAlarmWindows.Service
{
    /// <summary>
    /// Layanan untuk memutar suara alarm menggunakan System.Media.SoundPlayer
    /// </summary>
    public class PemutarAudio : IDisposable
    {
        private SoundPlayer _player;
        private bool _disposed = false;

        /// <summary>
        /// Memuat file suara WAV dari path yang diberikan
        /// </summary>
        /// <param name="pathFile">Path lengkap ke file WAV</param>
        public void MuatSuara(string pathFile)
        {
            try
            {
                // Validasi parameter
                if (string.IsNullOrWhiteSpace(pathFile))
                {
                    Console.WriteLine("Error: Path file suara tidak valid (null atau kosong)");
                    return;
                }

                // Cek apakah file ada
                if (!File.Exists(pathFile))
                {
                    Console.WriteLine($"Error: File suara tidak ditemukan di {pathFile}");
                    return;
                }

                // Dispose player lama jika ada
                if (_player != null)
                {
                    _player.Dispose();
                    _player = null;
                }

                // Buat SoundPlayer baru dan load file
                _player = new SoundPlayer(pathFile);
                _player.Load(); // Load synchronous untuk memastikan file valid
                
                Console.WriteLine($"File suara berhasil dimuat: {pathFile}");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Error: File suara tidak ditemukan - {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Error: File suara tidak valid atau corrupt - {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error tidak terduga saat memuat suara: {ex.Message}");
            }
        }

        /// <summary>
        /// Memutar suara alarm secara loop (berulang)
        /// </summary>
        public void Putar()
        {
            if (_player != null)
            {
                try
                {
                    _player.PlayLooping();
                    Console.WriteLine("Suara alarm diputar (looping)");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saat memutar suara: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Warning: Tidak ada file suara yang dimuat. Melewati pemutaran audio.");
            }
        }

        /// <summary>
        /// Menghentikan pemutaran suara alarm
        /// </summary>
        public void Berhenti()
        {
            if (_player != null)
            {
                try
                {
                    _player.Stop();
                    Console.WriteLine("Suara alarm dihentikan");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saat menghentikan suara: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Membersihkan resources yang digunakan oleh PemutarAudio
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected implementation of Dispose pattern
        /// </summary>
        /// <param name="disposing">True jika dipanggil dari Dispose(), false jika dari finalizer</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources
                    if (_player != null)
                    {
                        _player.Stop();
                        _player.Dispose();
                        _player = null;
                    }
                }

                _disposed = true;
            }
        }
    }
}
