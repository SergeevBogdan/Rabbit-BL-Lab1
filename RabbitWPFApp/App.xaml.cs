using RabbitViewModels;
using BusinessLogicMVP;
using ViewManager;
using RabbitViewModels;
using System;
using System.Windows;
namespace RabbitWPFApp
{
    public partial class App : Application
    {
        private ViewManager.ViewManager _viewManager;
        private MainViewModel _mainViewModel;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            bool useEF = e.Args.Length == 0 || e.Args[0].ToLower() != "dapper";
            var model = ModelFactory.CreateModel(useEF);

            // Создаем ViewModel
            _mainViewModel = new MainViewModel(model);

            // Создаем WPF View
            var mainView = new MainView();
            var detailsView = new RabbitDetailsView();
            var statsView = new StatisticsView();

            // Инициализируем View в ViewModel
            _mainViewModel.InitializeViews(mainView, detailsView, statsView);

            // Создаем ViewManager
            _viewManager = new ViewManager.ViewManager();
            _viewManager.RegisterView<MainView, MainViewModel>(_mainViewModel);
            _viewManager.RegisterView<RabbitDetailsView, MainViewModel>(_mainViewModel);
            _viewManager.RegisterView<StatisticsView, MainViewModel>(_mainViewModel);

            // Подписываемся на события
            _viewManager.ViewRequested += OnViewRequested;
            _viewManager.ViewClosed += OnViewClosed;

            // Запускаем главное окно
            _viewManager.ShowView<MainView>();
        }

        private void OnViewRequested(Type viewType, BaseViewModel viewModel)
        {
            if (viewType == typeof(MainView) && viewModel is MainViewModel mainViewModel)
            {
                // Создаем и показываем главное окно
                var mainWindow = new MainWindow(mainViewModel);
                mainWindow.Show();

                // Уведомляем View
                mainViewModel.MainView?.OnRequested();
            }
            else if (viewType == typeof(RabbitDetailsView) && viewModel is MainViewModel)
            {
                // Можно создать окно деталей
                MessageBox.Show("Открыты детали кролика", "Детали",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (viewType == typeof(StatisticsView) && viewModel is MainViewModel)
            {
                // Можно создать окно статистики
                MessageBox.Show("Открыта статистика", "Статистика",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void OnViewClosed(Type viewType)
        {
            // Обработка закрытия View
            if (viewType == typeof(MainView))
            {
                _mainViewModel.MainView?.OnClosed();
                Current.Shutdown();
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Startup logic is handled in OnStartup
        }
    }
}
