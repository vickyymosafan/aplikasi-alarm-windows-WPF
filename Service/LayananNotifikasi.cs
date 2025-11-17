using System;
using AplikasiAlarmWindows.Model;
using AplikasiAlarmWindows.View;

namespace AplikasiAlarmWindows.Service
{
    /// <summary>
    /// Layanan untuk menampilkan dan mengelola popup notifikasi alarm
    /// </summary>
    public class LayananNotifikasi
    {
        private NotifikasiWindow _windowAktif;

        /// <summary>
        /// Menampilkan popup notifikasi untuk alarm yang aktif
        /// </summary>
        /// <param name="alarm">Alarm yang akan ditampilkan di notifikasi</param>
        public void TampilkanNotifikasi(Alarm alarm)
        {
            try
            {
                // Validasi parameter
                if (alarm == null)
                {
                    Console.WriteLine("Error: Alarm null, tidak dapat menampilkan notifikasi");
                    return;
                }

                // Tutup window lama jika ada untuk prevent multiple windows
                TutupNotifikasi();

                // Buat instance NotifikasiWindow baru
                _windowAktif = new NotifikasiWindow
                {
                    // Set DataContext dengan alarm untuk data binding
                    DataContext = alarm
                };

                // Tampilkan window
                // WindowStartupLocation sudah di-set di XAML (CenterScreen)
                _windowAktif.Show();

                Console.WriteLine($"Notifikasi ditampilkan untuk alarm: {alarm.ToString()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saat menampilkan notifikasi: {ex.Message}");
            }
        }

        /// <summary>
        /// Menutup popup notifikasi yang sedang aktif
        /// </summary>
        public void TutupNotifikasi()
        {
            try
            {
                if (_windowAktif != null)
                {
                    // Close window jika masih terbuka
                    _windowAktif.Close();
                    _windowAktif = null;

                    Console.WriteLine("Notifikasi ditutup");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saat menutup notifikasi: {ex.Message}");
                
                // Set null untuk cleanup meskipun ada error
                _windowAktif = null;
            }
        }
    }
}
