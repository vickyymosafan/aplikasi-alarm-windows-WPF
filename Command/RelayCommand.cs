using System;
using System.Windows.Input;

namespace AplikasiAlarmWindows.Command
{
    /// <summary>
    /// Implementasi ICommand untuk MVVM pattern yang mengenkapsulasi command logic
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        /// <summary>
        /// Event yang dipicu ketika kondisi CanExecute berubah
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Constructor untuk command tanpa kondisi CanExecute
        /// </summary>
        /// <param name="execute">Action yang akan dieksekusi ketika command dipanggil</param>
        /// <exception cref="ArgumentNullException">Jika execute null</exception>
        public RelayCommand(Action<object> execute) : this(execute, null)
        {
        }

        /// <summary>
        /// Constructor untuk command dengan kondisi CanExecute
        /// </summary>
        /// <param name="execute">Action yang akan dieksekusi ketika command dipanggil</param>
        /// <param name="canExecute">Fungsi yang menentukan apakah command dapat dieksekusi</param>
        /// <exception cref="ArgumentNullException">Jika execute null</exception>
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Menentukan apakah command dapat dieksekusi dalam kondisi saat ini
        /// </summary>
        /// <param name="parameter">Parameter untuk command</param>
        /// <returns>True jika command dapat dieksekusi, false jika tidak</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        /// <summary>
        /// Eksekusi command dengan parameter yang diberikan
        /// </summary>
        /// <param name="parameter">Parameter untuk command</param>
        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
