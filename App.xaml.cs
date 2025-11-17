using System;
using System.IO;
using System.Windows;

namespace AplikasiAlarmWindows
{
    /// <summary>
    /// Interaction logic untuk App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Global exception handler untuk unhandled exceptions
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            DispatcherUnhandledException += OnDispatcherUnhandledException;
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogException(e.ExceptionObject as Exception);
            MessageBox.Show(
                $"Terjadi error fatal:\n\n{(e.ExceptionObject as Exception)?.Message}\n\nAplikasi akan ditutup.",
                "Error Fatal",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        private void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            LogException(e.Exception);
            MessageBox.Show(
                $"Terjadi error:\n\n{e.Exception.Message}\n\nStack Trace:\n{e.Exception.StackTrace}",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            e.Handled = true;
        }

        private void LogException(Exception ex)
        {
            if (ex == null) return;

            try
            {
                string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error.log");
                string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {ex.GetType().Name}: {ex.Message}\n{ex.StackTrace}\n\n";
                File.AppendAllText(logPath, logMessage);
            }
            catch
            {
                // Ignore logging errors
            }
        }
    }
}
