using RabbitViewModels;
using BusinessLogicMVP;
using ViewManager;
using RabbitViewModels;
using System;

namespace RabbitConsoleMVVM
{
    public class Program
    {
        public static void Main(string[] args)
        {
            bool useEF = args.Length == 0 || args[0].ToLower() != "dapper";

            Console.WriteLine($"=== НОВАЯ MVVM АРХИТЕКТУРА (CONSOLE) ===");
            Console.WriteLine($"Технология: {(useEF ? "Entity Framework" : "Dapper")}");

            try
            {
                // Создаем модель и ViewModel
                var model = ModelFactory.CreateModel(useEF);
                var mainViewModel = new MainViewModel(model);

                // Создаем консольные View
                var mainView = new ConsoleMainView();
                var detailsView = new ConsoleDetailsView();
                var statsView = new ConsoleStatsView();

                // Инициализируем View в ViewModel
                mainViewModel.InitializeViews(mainView, detailsView, statsView);

                // Создаем ViewManager
                var viewManager = new ViewManager.ViewManager();
                viewManager.RegisterView<ConsoleMainView, MainViewModel>(mainViewModel);
                viewManager.RegisterView<ConsoleDetailsView, MainViewModel>(mainViewModel);
                viewManager.RegisterView<ConsoleStatsView, MainViewModel>(mainViewModel);

                // Подписываемся на события
                viewManager.ViewRequested += OnViewRequested;

                // Запускаем главное окно
                viewManager.ShowView<ConsoleMainView>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка запуска: {ex.Message}");
                Console.ReadKey();
            }
        }

        static void OnViewRequested(Type viewType, BaseViewModel viewModel)
        {
            if (viewType == typeof(ConsoleMainView) && viewModel is MainViewModel mainViewModel)
            {
                var consoleView = new ConsoleView(mainViewModel);
                consoleView.ShowMainMenu();
            }
        }
    }

    // Консольные реализации View
    public class ConsoleMainView : IMainView
    {
        public Type ViewType => typeof(ConsoleMainView);
        public string Title => "Консольное главное окно";

        public event Action Requested;
        public event Action Closed;

        public void OnRequested() => Requested?.Invoke();
        public void OnClosed() => Closed?.Invoke();
    }

    public class ConsoleDetailsView : IRabbitDetailsView
    {
        public Type ViewType => typeof(ConsoleDetailsView);
        public string Title => "Консольные детали кролика";

        public int RabbitId { get; set; }
        public string RabbitName { get; set; }

        public event Action Requested;
        public event Action Closed;

        public void OnRequested() => Requested?.Invoke();
        public void OnClosed() => Closed?.Invoke();
    }

    public class ConsoleStatsView : IStatisticsView
    {
        public Type ViewType => typeof(ConsoleStatsView);
        public string Title => "Консольная статистика";

        public double AverageAge { get; set; }
        public double AverageWeight { get; set; }
        public int TotalRabbits { get; set; }

        public event Action Requested;
        public event Action Closed;

        public void OnRequested() => Requested?.Invoke();
        public void OnClosed() => Closed?.Invoke();
    }
}
