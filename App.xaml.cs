using EBoard.Navigation;
using EBoard.ViewModels;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Media;

namespace EBoard
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private NavigationStore _navigationStore;

        public App()
        {
            _navigationStore = new NavigationStore();
        }


        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow mainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_navigationStore)
            };

            mainWindow.Show();


            base.OnStartup(e);
        }

    }

}
