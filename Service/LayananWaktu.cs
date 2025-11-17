using System;

namespace AplikasiAlarmWindows.Service
{
    /// <summary>
    /// Layanan untuk mengelola waktu Indonesia (WIB - Waktu Indonesia Barat)
    /// </summary>
    public class LayananWaktu
    {
        private static readonly TimeZoneInfo _timezoneWIB;

        /// <summary>
        /// Static constructor untuk inisialisasi timezone WIB
        /// </summary>
        static LayananWaktu()
        {
            try
            {
                // Coba gunakan timezone ID untuk Windows
                _timezoneWIB = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            }
            catch
            {
                try
                {
                    // Fallback untuk Linux/Mac
                    _timezoneWIB = TimeZoneInfo.FindSystemTimeZoneById("Asia/Jakarta");
                }
                catch
                {
                    // Jika gagal, buat custom timezone WIB (UTC+7)
                    _timezoneWIB = TimeZoneInfo.CreateCustomTimeZone(
                        "WIB",
                        TimeSpan.FromHours(7),
                        "Waktu Indonesia Barat",
                        "WIB"
                    );
                }
            }
        }

        /// <summary>
        /// Mendapatkan waktu Indonesia (WIB) saat ini
        /// </summary>
        /// <returns>DateTime dalam timezone WIB</returns>
        public static DateTime GetWaktuIndonesia()
        {
            // Konversi UTC ke WIB
            DateTime utcNow = DateTime.UtcNow;
            return TimeZoneInfo.ConvertTimeFromUtc(utcNow, _timezoneWIB);
        }
    }
}
