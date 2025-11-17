using System;
using AplikasiAlarmWindows.Common;

namespace AplikasiAlarmWindows.Model
{
    /// <summary>
    /// Model data untuk alarm yang menyimpan informasi waktu dan status alarm
    /// </summary>
    public class Alarm
    {
        /// <summary>
        /// ID unik untuk setiap alarm
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Jam alarm (0-23)
        /// </summary>
        public int Jam { get; set; }

        /// <summary>
        /// Menit alarm (0-59)
        /// </summary>
        public int Menit { get; set; }

        /// <summary>
        /// Status apakah alarm sedang aktif/berbunyi
        /// </summary>
        public bool IsAktif { get; set; }

        /// <summary>
        /// Nama file suara alarm (WAV)
        /// </summary>
        public string NamaFileSuara { get; set; }

        /// <summary>
        /// Waktu terakhir alarm terpicu (untuk mencegah repeat trigger)
        /// </summary>
        public DateTime? TerakhirTerpicu { get; set; }

        /// <summary>
        /// Flag untuk menandai alarm sudah selesai dan siap dihapus
        /// </summary>
        public bool SudahSelesai { get; set; }

        /// <summary>
        /// Constructor default untuk deserialisasi JSON
        /// </summary>
        public Alarm()
        {
            Id = Guid.NewGuid();
            IsAktif = false;
            NamaFileSuara = Constants.DefaultSoundFileName;
            TerakhirTerpicu = null;
            SudahSelesai = false;
        }

        /// <summary>
        /// Constructor dengan parameter untuk membuat alarm baru
        /// </summary>
        /// <param name="jam">Jam alarm (0-23)</param>
        /// <param name="menit">Menit alarm (0-59)</param>
        /// <param name="namaFileSuara">Nama file suara alarm (optional)</param>
        public Alarm(int jam, int menit, string namaFileSuara = null) : this()
        {
            Jam = jam;
            Menit = menit;
            NamaFileSuara = namaFileSuara ?? Constants.DefaultSoundFileName;
        }

        /// <summary>
        /// Validasi apakah nilai jam valid (0-23)
        /// </summary>
        /// <returns>True jika jam valid, false jika tidak</returns>
        public bool ValidasiJam()
        {
            return Jam >= Constants.MinHour && Jam <= Constants.MaxHour;
        }

        /// <summary>
        /// Validasi apakah nilai menit valid (0-59)
        /// </summary>
        /// <returns>True jika menit valid, false jika tidak</returns>
        public bool ValidasiMenit()
        {
            return Menit >= Constants.MinMinute && Menit <= Constants.MaxMinute;
        }

        /// <summary>
        /// Konversi alarm ke format string "HH:mm"
        /// </summary>
        /// <returns>String representasi waktu alarm</returns>
        public override string ToString()
        {
            return $"{Jam:D2}:{Menit:D2}";
        }
    }
}
