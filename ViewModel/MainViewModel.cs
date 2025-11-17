using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using AplikasiAlarmWindows.Command;
using AplikasiAlarmWindows.Model;
using AplikasiAlarmWindows.Service;

namespace AplikasiAlarmWindows.ViewModel
{
    /// <summary>
    /// ViewModel utama untuk aplikasi alarm yang mengelola state dan business logic
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region Private Fields

        private readonly LayananTimer _layananTimer;
        private readonly LayananPenyimpanan _layananPenyimpanan;
        private readonly LayananNotifikasi _layananNotifikasi;
        private readonly PemutarAudio _pemutarAudio;

        private int _jamInput;
        private int _menitInput;
        private string _pesanValidasi;
        private string _fileSuaraTerpilih;
        private string _waktuSekarang;
        private bool _adaAlarmAktif;
        private System.Windows.Threading.DispatcherTimer _timerWaktu;

        #endregion

        #region Public Properties

        /// <summary>
        /// Koleksi alarm yang tersimpan
        /// </summary>
        public ObservableCollection<Alarm> DaftarAlarm { get; set; }

        /// <summary>
        /// Daftar file suara yang tersedia
        /// </summary>
        public ObservableCollection<string> DaftarFileSuara { get; set; }

        /// <summary>
        /// Daftar pilihan jam (0-23)
        /// </summary>
        public ObservableCollection<int> DaftarJam { get; set; }

        /// <summary>
        /// Daftar pilihan menit (0-59)
        /// </summary>
        public ObservableCollection<int> DaftarMenit { get; set; }

        /// <summary>
        /// File suara yang dipilih untuk alarm baru
        /// </summary>
        public string FileSuaraTerpilih
        {
            get => _fileSuaraTerpilih;
            set
            {
                _fileSuaraTerpilih = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Input jam untuk alarm baru (0-23)
        /// </summary>
        public int JamInput
        {
            get => _jamInput;
            set
            {
                _jamInput = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Input menit untuk alarm baru (0-59)
        /// </summary>
        public int MenitInput
        {
            get => _menitInput;
            set
            {
                _menitInput = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Pesan validasi untuk input yang tidak valid
        /// </summary>
        public string PesanValidasi
        {
            get => _pesanValidasi;
            set
            {
                _pesanValidasi = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Waktu Indonesia (WIB) saat ini dalam format string
        /// </summary>
        public string WaktuSekarang
        {
            get => _waktuSekarang;
            set
            {
                _waktuSekarang = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Indikator apakah ada alarm yang sedang aktif
        /// </summary>
        public bool AdaAlarmAktif
        {
            get => _adaAlarmAktif;
            set
            {
                _adaAlarmAktif = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Command untuk menambahkan alarm baru
        /// </summary>
        public ICommand TambahAlarmCommand { get; }

        /// <summary>
        /// Command untuk menghentikan alarm yang aktif
        /// </summary>
        public ICommand StopAlarmCommand { get; }

        /// <summary>
        /// Command untuk menghapus alarm dari daftar
        /// </summary>
        public ICommand HapusAlarmCommand { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor MainViewModel - inisialisasi services dan load data
        /// </summary>
        public MainViewModel()
        {
            // Inisialisasi koleksi alarm
            DaftarAlarm = new ObservableCollection<Alarm>();
            DaftarFileSuara = new ObservableCollection<string>();
            
            // Inisialisasi daftar jam (0-23)
            DaftarJam = new ObservableCollection<int>();
            for (int i = 0; i <= 23; i++)
            {
                DaftarJam.Add(i);
            }
            
            // Inisialisasi daftar menit (0-59)
            DaftarMenit = new ObservableCollection<int>();
            for (int i = 0; i <= 59; i++)
            {
                DaftarMenit.Add(i);
            }

            // Inisialisasi services
            _layananTimer = new LayananTimer();
            _layananPenyimpanan = new LayananPenyimpanan();
            _layananNotifikasi = new LayananNotifikasi();
            _pemutarAudio = new PemutarAudio();

            // Muat daftar file suara yang tersedia
            MuatDaftarFileSuara();

            // Muat alarm dari penyimpanan
            var alarmTersimpan = _layananPenyimpanan.MuatAlarm();
            foreach (var alarm in alarmTersimpan)
            {
                DaftarAlarm.Add(alarm);
            }

            // Mulai layanan timer untuk monitoring alarm
            _layananTimer.Mulai(DaftarAlarm);

            // Subscribe ke event AlarmTerpicu
            _layananTimer.AlarmTerpicu += OnAlarmTerpicu;

            // Inisialisasi commands
            TambahAlarmCommand = new RelayCommand(ExecuteTambahAlarm);
            StopAlarmCommand = new RelayCommand(ExecuteStopAlarm);
            HapusAlarmCommand = new RelayCommand(ExecuteHapusAlarm);

            // Inisialisasi timer untuk update waktu realtime
            InisialisasiTimerWaktu();

            Console.WriteLine($"MainViewModel diinisialisasi. {DaftarAlarm.Count} alarm dimuat.");
        }

        /// <summary>
        /// Inisialisasi timer untuk update waktu Indonesia realtime
        /// </summary>
        private void InisialisasiTimerWaktu()
        {
            // Update waktu pertama kali
            UpdateWaktu();

            // Buat timer yang update setiap detik
            _timerWaktu = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            _timerWaktu.Tick += (sender, e) => UpdateWaktu();
            _timerWaktu.Start();
        }

        /// <summary>
        /// Update display waktu Indonesia
        /// </summary>
        private void UpdateWaktu()
        {
            DateTime waktuIndonesia = LayananWaktu.GetWaktuIndonesia();
            WaktuSekarang = $"{waktuIndonesia:dddd, dd MMMM yyyy HH:mm:ss} WIB";
        }

        #endregion

        #region Command Implementations

        /// <summary>
        /// Eksekusi command untuk menambahkan alarm baru
        /// </summary>
        private void ExecuteTambahAlarm(object parameter)
        {
            // Tentukan file suara yang akan digunakan
            string fileSuara = string.IsNullOrEmpty(FileSuaraTerpilih) ? "Bangkit.wav" : FileSuaraTerpilih;

            // Buat alarm baru dengan file suara yang dipilih
            var alarmBaru = new Alarm(JamInput, MenitInput, fileSuara);

            // Validasi jam
            if (!alarmBaru.ValidasiJam())
            {
                PesanValidasi = "Jam harus antara 0 dan 23";
                return;
            }

            // Validasi menit
            if (!alarmBaru.ValidasiMenit())
            {
                PesanValidasi = "Menit harus antara 0 dan 59";
                return;
            }

            // Input valid, tambahkan alarm
            DaftarAlarm.Add(alarmBaru);

            // Simpan ke file
            _layananPenyimpanan.SimpanAlarm(DaftarAlarm);

            // Clear pesan validasi dan reset input
            PesanValidasi = string.Empty;
            JamInput = 0;
            MenitInput = 0;

            Console.WriteLine($"Alarm baru ditambahkan: {alarmBaru.ToString()} dengan suara {fileSuara}");
        }

        /// <summary>
        /// Eksekusi command untuk menghentikan alarm yang aktif
        /// </summary>
        private void ExecuteStopAlarm(object parameter)
        {
            // Hentikan audio
            _pemutarAudio.Berhenti();

            // Tutup notifikasi
            _layananNotifikasi.TutupNotifikasi();

            // Cari alarm yang aktif
            var alarmAktif = DaftarAlarm.FirstOrDefault(a => a.IsAktif);
            if (alarmAktif != null)
            {
                Console.WriteLine($"Alarm dihentikan dan dihapus: {alarmAktif.ToString()}");
                
                // Tandai alarm sudah selesai
                alarmAktif.IsAktif = false;
                alarmAktif.SudahSelesai = true;
                
                // Auto-delete alarm setelah stop
                DaftarAlarm.Remove(alarmAktif);
                
                // Simpan perubahan ke file
                _layananPenyimpanan.SimpanAlarm(DaftarAlarm);
                
                // Update flag ada alarm aktif
                AdaAlarmAktif = false;
            }
        }

        /// <summary>
        /// Eksekusi command untuk menghapus alarm dari daftar
        /// </summary>
        private void ExecuteHapusAlarm(object parameter)
        {
            if (parameter is Alarm alarm)
            {
                // Jika alarm sedang aktif, hentikan dulu
                if (alarm.IsAktif)
                {
                    _pemutarAudio.Berhenti();
                    _layananNotifikasi.TutupNotifikasi();
                }

                // Hapus alarm dari daftar
                DaftarAlarm.Remove(alarm);

                // Simpan perubahan ke file
                _layananPenyimpanan.SimpanAlarm(DaftarAlarm);

                Console.WriteLine($"Alarm dihapus: {alarm.ToString()}");
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handler untuk event AlarmTerpicu dari LayananTimer
        /// </summary>
        private void OnAlarmTerpicu(object sender, Alarm alarm)
        {
            // Set status alarm menjadi aktif
            alarm.IsAktif = true;
            AdaAlarmAktif = true;

            // Tampilkan notifikasi
            _layananNotifikasi.TampilkanNotifikasi(alarm);

            // Muat dan putar suara alarm menggunakan file suara yang dipilih untuk alarm ini
            string namaFile = string.IsNullOrEmpty(alarm.NamaFileSuara) ? "Bangkit.wav" : alarm.NamaFileSuara;
            string pathSuara = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resource", namaFile);
            _pemutarAudio.MuatSuara(pathSuara);
            _pemutarAudio.Putar();

            Console.WriteLine($"Alarm aktif: {alarm.ToString()} dengan suara {namaFile}");
        }

        /// <summary>
        /// Memuat daftar file suara WAV yang tersedia di folder Resource
        /// </summary>
        private void MuatDaftarFileSuara()
        {
            try
            {
                string folderResource = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resource");
                
                if (Directory.Exists(folderResource))
                {
                    var wavFiles = Directory.GetFiles(folderResource, "*.wav");
                    
                    foreach (var filePath in wavFiles)
                    {
                        string namaFile = Path.GetFileName(filePath);
                        DaftarFileSuara.Add(namaFile);
                    }

                    // Set default selection ke file pertama jika ada
                    if (DaftarFileSuara.Count > 0)
                    {
                        FileSuaraTerpilih = DaftarFileSuara[0];
                    }

                    Console.WriteLine($"{DaftarFileSuara.Count} file suara ditemukan di folder Resource");
                }
                else
                {
                    Console.WriteLine("Folder Resource tidak ditemukan");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saat memuat daftar file suara: {ex.Message}");
            }
        }

        #endregion

        #region Cleanup

        /// <summary>
        /// Cleanup semua resource saat aplikasi ditutup
        /// </summary>
        public void Cleanup()
        {
            Console.WriteLine("Melakukan cleanup resource...");

            // Stop timer waktu
            if (_timerWaktu != null && _timerWaktu.IsEnabled)
            {
                _timerWaktu.Stop();
                _timerWaktu = null;
            }

            // Stop layanan timer alarm
            _layananTimer?.Berhenti();

            // Stop audio jika sedang bermain
            _pemutarAudio?.Berhenti();

            // Tutup notifikasi jika ada
            _layananNotifikasi?.TutupNotifikasi();

            // Unsubscribe dari event
            if (_layananTimer != null)
            {
                _layananTimer.AlarmTerpicu -= OnAlarmTerpicu;
            }

            Console.WriteLine("Cleanup selesai.");
        }

        #endregion
    }
}
