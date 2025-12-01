using RabbitViewModels;
using BusinessLogicMVP;
using System.Windows;

namespace RabbitWPFApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            bool useEF = e.Args.Length == 0 || e.Args[0].ToLower() != "dapper";
            var model = ModelFactory.CreateModel(useEF);
            var mainViewModel = new MainViewModel(model);

            var mainWindow = new MainWindow(mainViewModel);
            mainWindow.Show();
        }
    }
}
