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
        private IViewManagerService _viewManager;
        private MainViewModel _mainViewModel;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            bool useEF = e.Args.Length == 0 || e.Args[0].ToLower() != "dapper";
            var model = ModelFactory.CreateModel(useEF);

            // Создаем ViewModel
            _mainViewModel = new MainViewModel(model);

            // Создаем View
            var mainView = new MainView();
            var detailsView = new RabbitDetailsView();
            var statsView = new StatisticsView();

            // Инициализируем View в ViewModel
            _mainViewModel.InitializeViews(mainView, detailsView, statsView);

            // Создаем ViewManagerService
            _viewManager = new ViewManagerService();

            // Регистрируем View используя typeof()
            _viewManager.RegisterView(typeof(MainView), _mainViewModel);
            _viewManager.RegisterView(typeof(RabbitDetailsView), _mainViewModel);
            _viewManager.RegisterView(typeof(StatisticsView), _mainViewModel);

            // Подписываемся на события
            _viewManager.ViewRequested += OnViewRequested;
            _viewManager.ViewClosed += OnViewClosed;

            // Запускаем главное окно
            _viewManager.ShowView(typeof(MainView));
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
            else if (viewType == typeof(RabbitDetailsView) && viewModel is MainViewModel vm)
            {
                // Показываем детали в MessageBox
                ShowDetailsInMessageBox(vm);

                // Уведомляем View
                vm.DetailsView?.OnRequested();
            }
            else if (viewType == typeof(StatisticsView) && viewModel is MainViewModel vms)
            {
                // Показываем статистику в MessageBox
                ShowStatisticsInMessageBox(vms);

                // Уведомляем View
                vms.StatsView?.OnRequested();
            }
        }

        private void ShowDetailsInMessageBox(MainViewModel vm)
        {
            if (vm.SelectedRabbit != null)
            {
                MessageBox.Show(
                    $"Детали кролика:\n" +
                    $"ID: {vm.SelectedRabbit.Id}\n" +
                    $"Имя: {vm.SelectedRabbit.Name}\n" +
                    $"Порода: {vm.SelectedRabbit.Breed}\n" +
                    $"Возраст: {vm.SelectedRabbit.Age}\n" +
                    $"Вес: {vm.SelectedRabbit.Weight}\n" +
                    $"Создан: {vm.SelectedRabbit.CreatedDate}",
                    "Детали кролика",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Выберите кролика для просмотра деталей",
                    "Информация",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        private void ShowStatisticsInMessageBox(MainViewModel vm)
        {
            vm.ShowStatistics();

            MessageBox.Show(
                $"Статистика кроликов:\n" +
                $"Всего кроликов: {vm.Rabbits.Count}\n" +
                $"{vm.StatusMessage}",
                "Статистика",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void OnViewClosed(Type viewType)
        {
            if (viewType == typeof(MainView))
            {
                _mainViewModel.MainView?.OnClosed();
                Current.Shutdown();
            }
            else if (viewType == typeof(RabbitDetailsView))
            {
                _mainViewModel.DetailsView?.OnClosed();
            }
            else if (viewType == typeof(StatisticsView))
            {
                _mainViewModel.StatsView?.OnClosed();
            }
        }
    }
}