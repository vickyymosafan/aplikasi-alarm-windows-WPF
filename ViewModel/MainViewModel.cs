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

            Console.WriteLine($"MainViewModel diinisialisasi. {DaftarAlarm.Count} alarm dimuat.");
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

            // Cari dan update alarm yang aktif
            var alarmAktif = DaftarAlarm.FirstOrDefault(a => a.IsAktif);
            if (alarmAktif != null)
            {
                alarmAktif.IsAktif = false;
                Console.WriteLine($"Alarm dihentikan: {alarmAktif.ToString()}");
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
    }
}
