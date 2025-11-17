using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using AplikasiAlarmWindows.Model;

namespace AplikasiAlarmWindows.Service
{
    /// <summary>
    /// Layanan untuk memonitor waktu sistem dan memicu alarm pada waktu yang tepat
    /// </summary>
    public class LayananTimer
    {
        private DispatcherTimer _timer;
        private ObservableCollection<Alarm> _daftarAlarm;

        /// <summary>
        /// Event yang dipicu ketika alarm cocok dengan waktu sistem
        /// </summary>
        public event EventHandler<Alarm> AlarmTerpicu;

        /// <summary>
        /// Memulai monitoring alarm dengan interval 1 detik
        /// </summary>
        /// <param name="daftarAlarm">Koleksi alarm yang akan dimonitor</param>
        public void Mulai(ObservableCollection<Alarm> daftarAlarm)
        {
            // Validasi parameter
            if (daftarAlarm == null)
            {
                throw new ArgumentNullException(nameof(daftarAlarm));
            }

            // Simpan reference ke daftar alarm
            _daftarAlarm = daftarAlarm;

            // Stop timer lama jika ada
            if (_timer != null && _timer.IsEnabled)
            {
                _timer.Stop();
            }

            // Buat dan konfigurasi timer baru
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1) // Interval 1 detik
            };

            // Setup event handler untuk timer tick
            _timer.Tick += (sender, e) =>
            {
                CekAlarm();
            };

            // Mulai timer
            _timer.Start();
            
            Console.WriteLine("LayananTimer dimulai. Monitoring alarm setiap 1 detik.");
        }

        /// <summary>
        /// Menghentikan monitoring alarm
        /// </summary>
        public void Berhenti()
        {
            if (_timer != null && _timer.IsEnabled)
            {
                _timer.Stop();
                Console.WriteLine("LayananTimer dihentikan.");
            }
        }

        /// <summary>
        /// Mengecek waktu sistem terhadap semua alarm dalam daftar
        /// </summary>
        private void CekAlarm()
        {
            // Validasi daftar alarm
            if (_daftarAlarm == null || _daftarAlarm.Count == 0)
            {
                return;
            }

            // Dapatkan waktu Indonesia (WIB) saat ini
            DateTime waktuSekarang = LayananWaktu.GetWaktuIndonesia();
            int jamSekarang = waktuSekarang.Hour;
            int menitSekarang = waktuSekarang.Minute;

            // Cek setiap alarm dalam daftar
            foreach (var alarm in _daftarAlarm)
            {
                // Cek apakah alarm sudah pernah terpicu di menit yang sama
                bool sudahTerpicuDiMenitIni = alarm.TerakhirTerpicu.HasValue &&
                    alarm.TerakhirTerpicu.Value.Hour == jamSekarang &&
                    alarm.TerakhirTerpicu.Value.Minute == menitSekarang &&
                    alarm.TerakhirTerpicu.Value.Date == waktuSekarang.Date;

                // Bandingkan jam dan menit
                // Hanya trigger jika alarm belum aktif dan belum pernah terpicu di menit ini
                if (alarm.Jam == jamSekarang && 
                    alarm.Menit == menitSekarang && 
                    !alarm.IsAktif &&
                    !alarm.SudahSelesai &&
                    !sudahTerpicuDiMenitIni)
                {
                    // Simpan waktu trigger
                    alarm.TerakhirTerpicu = waktuSekarang;
                    
                    Console.WriteLine($"Alarm terpicu: {alarm.ToString()} pada {waktuSekarang:HH:mm:ss} WIB");
                    
                    // Trigger event AlarmTerpicu
                    AlarmTerpicu?.Invoke(this, alarm);
                }
            }
        }
    }
}
