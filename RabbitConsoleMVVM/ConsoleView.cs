using RabbitViewModels;
using System;
using System.Linq;

namespace RabbitConsoleMVVM
{
    public class ConsoleView
    {
        private MainViewModel _viewModel;

        public ConsoleView(MainViewModel viewModel)
        {
            _viewModel = viewModel;

            // Подписываемся на события View
            if (_viewModel.MainView != null)
            {
                _viewModel.MainView.Requested += OnMainViewRequested;
                _viewModel.MainView.Closed += OnMainViewClosed;
            }

            if (_viewModel.DetailsView != null)
            {
                _viewModel.DetailsView.Requested += OnDetailsViewRequested;
                _viewModel.DetailsView.Closed += OnDetailsViewClosed;
            }

            if (_viewModel.StatsView != null)
            {
                _viewModel.StatsView.Requested += OnStatsViewRequested;
                _viewModel.StatsView.Closed += OnStatsViewClosed;
            }
        }

        // Обработчики событий View
        private void OnMainViewRequested()
        {
            Console.WriteLine("Главное консольное окно открыто");
            _viewModel.LoadRabbits();
        }

        private void OnMainViewClosed()
        {
            Console.WriteLine("Главное консольное окно закрыто");
        }

        private void OnDetailsViewRequested()
        {
            Console.WriteLine("Запрошены детали кролика");
        }

        private void OnDetailsViewClosed()
        {
            Console.WriteLine("Детали кролика закрыты");
        }

        private void OnStatsViewRequested()
        {
            Console.WriteLine("Запрошена статистика");
        }

        private void OnStatsViewClosed()
        {
            Console.WriteLine("Статистика закрыта");
        }

        public void ShowMainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== НОВАЯ MVVM АРХИТЕКТУРА (CONSOLE) ===");
                Console.WriteLine("1. Показать всех кроликов");
                Console.WriteLine("2. Добавить кролика");
                Console.WriteLine("3. Удалить кролика");
                Console.WriteLine("4. Обновить кролика");
                Console.WriteLine("5. Добавить случайного кролика");
                Console.WriteLine("6. Показать статистику");
                Console.WriteLine("7. Детали кролика");
                Console.WriteLine("8. Выход");
                Console.Write("Выберите опцию: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ShowAllRabbits(); break;
                    case "2": ShowAddRabbitMenu(); break;
                    case "3": ShowRemoveRabbitMenu(); break;
                    case "4": ShowUpdateRabbitMenu(); break;
                    case "5": AddRandomRabbit(); break;
                    case "6": ShowStatistics(); break;
                    case "7": ShowDetailsView(); break;
                    case "8":
                        // Уведомляем View о закрытии
                        _viewModel.MainView?.OnClosed();
                        return;
                    default: Console.WriteLine("Неверная опция!"); WaitForContinue(); break;
                }
            }
        }

        private void ShowAllRabbits()
        {
            _viewModel.LoadRabbits();
            Console.WriteLine("\n=== СПИСОК КРОЛИКОВ ===");

            if (_viewModel.Rabbits.Count == 0)
            {
                Console.WriteLine("Список пуст");
            }
            else
            {
                foreach (var rabbit in _viewModel.Rabbits)
                {
                    Console.WriteLine($"ID: {rabbit.Id} | Имя: {rabbit.Name} | Порода: {rabbit.Breed} | Возраст: {rabbit.Age} | Вес: {rabbit.Weight}");
                }
            }

            Console.WriteLine($"\n{_viewModel.StatusMessage}");
            WaitForContinue();
        }

        private void ShowAddRabbitMenu()
        {
            Console.Clear();
            Console.WriteLine("=== ДОБАВЛЕНИЕ КРОЛИКА ===");

            Console.Write("ID: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("Имя: ");
            string name = Console.ReadLine();

            Console.Write("Возраст: ");
            int age = int.Parse(Console.ReadLine());

            Console.Write("Вес: ");
            int weight = int.Parse(Console.ReadLine());

            Console.Write("Порода: ");
            string breed = Console.ReadLine();

            _viewModel.NewRabbit.Id = id;
            _viewModel.NewRabbit.Name = name;
            _viewModel.NewRabbit.Age = age;
            _viewModel.NewRabbit.Weight = weight;
            _viewModel.NewRabbit.Breed = breed;

            _viewModel.AddRabbit();
            Console.WriteLine($"\n{_viewModel.StatusMessage}");
            WaitForContinue();
        }

        private void ShowRemoveRabbitMenu()
        {
            Console.Write("Введите ID кролика для удаления: ");
            int id = int.Parse(Console.ReadLine());

            var rabbit = _viewModel.Rabbits.FirstOrDefault(r => r.Id == id);
            if (rabbit != null)
            {
                _viewModel.SelectedRabbit = rabbit;
                _viewModel.RemoveRabbit();
            }
            else
            {
                Console.WriteLine("Кролик не найден!");
            }

            Console.WriteLine($"\n{_viewModel.StatusMessage}");
            WaitForContinue();
        }

        private void ShowUpdateRabbitMenu()
        {
            Console.Write("Введите ID кролика для обновления: ");
            int id = int.Parse(Console.ReadLine());

            var rabbit = _viewModel.Rabbits.FirstOrDefault(r => r.Id == id);
            if (rabbit != null)
            {
                _viewModel.SelectedRabbit = rabbit;

                Console.Write("Новое имя: ");
                rabbit.Name = Console.ReadLine();

                Console.Write("Новый возраст: ");
                rabbit.Age = int.Parse(Console.ReadLine());

                Console.Write("Новый вес: ");
                rabbit.Weight = int.Parse(Console.ReadLine());

                Console.Write("Новая порода: ");
                rabbit.Breed = Console.ReadLine();

                _viewModel.UpdateRabbit();
            }
            else
            {
                Console.WriteLine("Кролик не найден!");
            }

            Console.WriteLine($"\n{_viewModel.StatusMessage}");
            WaitForContinue();
        }

        private void AddRandomRabbit()
        {
            _viewModel.AddRandomRabbit();
            Console.WriteLine($"\n{_viewModel.StatusMessage}");
            WaitForContinue();
        }

        private void ShowStatistics()
        {
            // Уведомляем View статистики
            _viewModel.StatsView?.OnRequested();

            Console.WriteLine("\n=== СТАТИСТИКА ===");
            _viewModel.ShowStatistics();
            Console.WriteLine($"\n{_viewModel.StatusMessage}");
            WaitForContinue();

            _viewModel.StatsView?.OnClosed();
        }

        private void ShowDetailsView()
        {
            Console.Write("Введите ID кролика для просмотра деталей: ");
            int id = int.Parse(Console.ReadLine());

            var rabbit = _viewModel.Rabbits.FirstOrDefault(r => r.Id == id);
            if (rabbit != null)
            {
                _viewModel.SelectedRabbit = rabbit;

                // Уведомляем View деталей
                _viewModel.DetailsView?.OnRequested();

                Console.WriteLine("\n=== ДЕТАЛИ КРОЛИКА ===");
                Console.WriteLine($"ID: {rabbit.Id}");
                Console.WriteLine($"Имя: {rabbit.Name}");
                Console.WriteLine($"Порода: {rabbit.Breed}");
                Console.WriteLine($"Возраст: {rabbit.Age}");
                Console.WriteLine($"Вес: {rabbit.Weight}");
                Console.WriteLine($"Создан: {rabbit.CreatedDate}");

                WaitForContinue();

                _viewModel.DetailsView?.OnClosed();
            }
            else
            {
                Console.WriteLine("Кролик не найден!");
                WaitForContinue();
            }
        }

        private void WaitForContinue()
        {
            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }
    }
}