using System.Windows;
using AplikasiAlarmWindows.ViewModel;

namespace AplikasiAlarmWindows.View
{
    /// <summary>
    /// Interaction logic untuk MainWindow.xaml
    /// Window utama aplikasi alarm
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel;

        /// <summary>
        /// Constructor MainWindow
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            
            // Inisialisasi dan set DataContext dengan MainViewModel
            _viewModel = new MainViewModel();
            DataContext = _viewModel;

            // Subscribe ke event Closing untuk cleanup
            Closing += MainWindow_Closing;
        }

        /// <summary>
        /// Event handler saat window akan ditutup
        /// </summary>
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Cleanup semua resource
            _viewModel?.Cleanup();
        }
    }
}
