using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AplikasiAlarmWindows.ViewModel
{
    /// <summary>
    /// Base class untuk semua ViewModel yang menyediakan implementasi INotifyPropertyChanged
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Event yang dipicu ketika nilai property berubah
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Memicu event PropertyChanged untuk memberitahu UI bahwa property telah berubah
        /// </summary>
        /// <param name="propertyName">Nama property yang berubah (auto-detect jika tidak diisi)</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
