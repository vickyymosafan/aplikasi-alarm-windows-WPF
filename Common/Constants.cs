namespace AplikasiAlarmWindows.Common
{
    /// <summary>
    /// Kelas untuk menyimpan konstanta yang digunakan di seluruh aplikasi
    /// </summary>
    public static class Constants
    {
        #region File and Folder Names

        /// <summary>
        /// Nama folder resource
        /// </summary>
        public const string ResourceFolderName = "Resource";

        /// <summary>
        /// Nama file suara default
        /// </summary>
        public const string DefaultSoundFileName = "Bangkit.wav";

        /// <summary>
        /// Ekstensi file suara
        /// </summary>
        public const string SoundFileExtension = "*.wav";

        /// <summary>
        /// Nama file penyimpanan data alarm
        /// </summary>
        public const string AlarmDataFileName = "alarm-data.json";

        #endregion

        #region Validation Ranges

        /// <summary>
        /// Jam minimum (0)
        /// </summary>
        public const int MinHour = 0;

        /// <summary>
        /// Jam maksimum (23)
        /// </summary>
        public const int MaxHour = 23;

        /// <summary>
        /// Menit minimum (0)
        /// </summary>
        public const int MinMinute = 0;

        /// <summary>
        /// Menit maksimum (59)
        /// </summary>
        public const int MaxMinute = 59;

        #endregion

        #region Validation Messages

        /// <summary>
        /// Pesan error untuk jam tidak valid
        /// </summary>
        public const string InvalidHourMessage = "Jam harus antara 0 dan 23";

        /// <summary>
        /// Pesan error untuk menit tidak valid
        /// </summary>
        public const string InvalidMinuteMessage = "Menit harus antara 0 dan 59";

        #endregion
    }
}
