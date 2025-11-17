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
        /// <summary>
        /// Constructor MainWindow
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            
            // Inisialisasi dan set DataContext dengan MainViewModel
            DataContext = new MainViewModel();
        }
    }
}
