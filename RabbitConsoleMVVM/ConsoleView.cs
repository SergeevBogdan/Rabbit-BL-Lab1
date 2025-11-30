using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitViewModels;
using RabbitSharedMVP;

namespace RabbitConsoleMVVM
{
    public class ConsoleView
    {
        private MainViewModel _viewModel;

        public void Initialize(MainViewModel viewModel)
        {
            _viewModel = viewModel;
            ShowMainMenu();
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
                Console.WriteLine("7. Выход");
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
                    case "7": return;
                    default: Console.WriteLine("Неверная опция!"); break;
                }

                WaitForContinue();
            }
        }

        private void ShowAllRabbits()
        {
            _viewModel.LoadRabbits();
            Console.WriteLine("\n=== СПИСОК КРОЛИКОВ ===");
            foreach (var rabbit in _viewModel.Rabbits)
            {
                Console.WriteLine($"ID: {rabbit.Id} | Имя: {rabbit.Name} | Порода: {rabbit.Breed} | Возраст: {rabbit.Age} | Вес: {rabbit.Weight}");
            }
            Console.WriteLine(_viewModel.StatusMessage);
        }

        private void ShowAddRabbitMenu()
        {
            Console.WriteLine("\n--- ДОБАВЛЕНИЕ КРОЛИКА ---");

            Console.Write("ID: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("Имя: ");
            string name = Console.ReadLine();

            Console.Write("Возраст: ");
            int age = int.Parse(Console.ReadLine());

            Console.Write("Вес: ");
            int weight = int.Parse(Console.ReadLine());

            Console.WriteLine("Породы:");
            for (int i = 0; i < _viewModel.Breeds.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {_viewModel.Breeds[i]}");
            }
            Console.Write("Выберите породу: ");
            int breedChoice = int.Parse(Console.ReadLine());
            string breed = _viewModel.Breeds[breedChoice - 1];

            var rabbitDto = new RabbitSharedMVP.RabbitDTO
            {
                Id = id,
                Name = name,
                Age = age,
                Weight = weight,
                Breed = breed
            };

            _viewModel.AddRabbit();
            Console.WriteLine(_viewModel.StatusMessage);
        }

        private void ShowRemoveRabbitMenu()
        {
            Console.WriteLine("\n--- УДАЛЕНИЕ КРОЛИКА ---");
            ShowAllRabbits();

            Console.Write("Введите ID кролика для удаления: ");
            int id = int.Parse(Console.ReadLine());

            _viewModel.SelectedRabbit = _viewModel.Rabbits.FirstOrDefault(r => r.Id == id);
            if (_viewModel.SelectedRabbit != null)
            {
                _viewModel.RemoveRabbit();
                Console.WriteLine(_viewModel.StatusMessage);
            }
            else
            {
                Console.WriteLine("Кролик не найден!");
            }
        }

        private void ShowUpdateRabbitMenu()
        {
            Console.WriteLine("\n--- ОБНОВЛЕНИЕ КРОЛИКА ---");
            ShowAllRabbits();

            Console.Write("Введите ID кролика для обновления: ");
            int id = int.Parse(Console.ReadLine());

            _viewModel.SelectedRabbit = _viewModel.Rabbits.FirstOrDefault(r => r.Id == id);
            if (_viewModel.SelectedRabbit != null)
            {
                Console.Write("Новое имя: ");
                string name = Console.ReadLine();

                Console.Write("Новый возраст: ");
                int age = int.Parse(Console.ReadLine());

                Console.Write("Новый вес: ");
                int weight = int.Parse(Console.ReadLine());

                Console.WriteLine("Породы:");
                for (int i = 0; i < _viewModel.Breeds.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {_viewModel.Breeds[i]}");
                }
                Console.Write("Выберите породу: ");
                int breedChoice = int.Parse(Console.ReadLine());
                string breed = _viewModel.Breeds[breedChoice - 1];

                _viewModel.SelectedRabbit.Name = name;
                _viewModel.SelectedRabbit.Age = age;
                _viewModel.SelectedRabbit.Weight = weight;
                _viewModel.SelectedRabbit.Breed = breed;

                _viewModel.UpdateRabbit();
                Console.WriteLine(_viewModel.StatusMessage);
            }
            else
            {
                Console.WriteLine("Кролик не найден!");
            }
        }

        private void AddRandomRabbit()
        {
            _viewModel.AddRandomRabbit();
            Console.WriteLine(_viewModel.StatusMessage);
        }

        private void ShowStatistics()
        {
            _viewModel.ShowStatistics();
            Console.WriteLine(_viewModel.StatusMessage);
        }

        private void WaitForContinue()
        {
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }
}
