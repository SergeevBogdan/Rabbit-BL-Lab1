using RabbitViewModels;
using System;
using System.Linq;

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
                Console.WriteLine("Режим ввода ID: РУЧНОЙ");
                Console.WriteLine();
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
                    default: Console.WriteLine("Неверная опция!"); WaitForContinue(); break;
                }
            }
        }

        private void ShowAllRabbits()
        {
            _viewModel.LoadRabbits();
            Console.WriteLine("\n=== СПИСОК ВСЕХ КРОЛИКОВ ===");

            if (_viewModel.Rabbits.Count == 0)
            {
                Console.WriteLine("Список кроликов пуст");
            }
            else
            {
                Console.WriteLine("ID    Имя             Порода         Возраст    Вес   ");

                foreach (var rabbit in _viewModel.Rabbits)
                {
                    Console.WriteLine($" {rabbit.Id,3}  {rabbit.Name,-15}  {rabbit.Breed,-12}  {rabbit.Age,6}  {rabbit.Weight,5} ");
                }

                Console.WriteLine($"Всего кроликов: {_viewModel.Rabbits.Count}");
            }

            Console.WriteLine($"\n{_viewModel.StatusMessage}");
            WaitForContinue();
        }

        private void ShowAddRabbitMenu()
        {
            Console.Clear();
            Console.WriteLine("=== ДОБАВЛЕНИЕ НОВОГО КРОЛИКА ===");


            _viewModel.LoadRabbits();
            if (_viewModel.Rabbits.Count > 0)
            {
                Console.WriteLine("\n=== ТЕКУЩИЕ КРОЛИКИ ===");
                foreach (var rabbit in _viewModel.Rabbits)
                {
                    Console.WriteLine($"ID: {rabbit.Id,3} | {rabbit.Name,-15} | {rabbit.Breed,-12} | Возраст: {rabbit.Age,2} | Вес: {rabbit.Weight,2} кг");
                }
            }


            int id;
            while (true)
            {
                Console.Write("\nВведите ID нового кролика: ");
                if (!int.TryParse(Console.ReadLine(), out id))
                {
                    Console.WriteLine("Ошибка! Введите число.");
                    continue;
                }

                if (id <= 0)
                {
                    Console.WriteLine("ID должен быть положительным числом!");
                    continue;
                }


                if (_viewModel.Rabbits.Any(r => r.Id == id))
                {
                    Console.WriteLine($"ID {id} уже занят кроликом: {_viewModel.Rabbits.First(r => r.Id == id).Name}");
                    Console.WriteLine("Выберите другой ID из списка свободных:");

                    int maxId = _viewModel.Rabbits.Count > 0 ? _viewModel.Rabbits.Max(r => r.Id) + 10 : 10;
                    var freeIds = Enumerable.Range(1, maxId)
                        .Where(i => !_viewModel.Rabbits.Any(r => r.Id == i))
                        .Take(20);

                    Console.WriteLine(string.Join(", ", freeIds));
                    continue;
                }

                break;
            }

            string name;
            while (true)
            {
                Console.Write("Введите имя кролика: ");
                name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Имя не может быть пустым!");
                    continue;
                }
                break;
            }

            int age;
            while (true)
            {
                Console.Write("Введите возраст (1-50): ");
                if (!int.TryParse(Console.ReadLine(), out age))
                {
                    Console.WriteLine("Ошибка! Введите число.");
                    continue;
                }
                if (age < 1 || age > 50)
                {
                    Console.WriteLine("Возраст должен быть от 1 до 50 лет!");
                    continue;
                }
                break;
            }

            int weight;
            while (true)
            {
                Console.Write("Введите вес (1-100 кг): ");
                if (!int.TryParse(Console.ReadLine(), out weight))
                {
                    Console.WriteLine("Ошибка! Введите число.");
                    continue;
                }
                if (weight < 1 || weight > 100)
                {
                    Console.WriteLine("Вес должен быть от 1 до 100 кг!");
                    continue;
                }
                break;
            }

            string breed;
            if (_viewModel.Breeds != null && _viewModel.Breeds.Length > 0)
            {
                Console.WriteLine("\nДоступные породы:");
                for (int i = 0; i < _viewModel.Breeds.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {_viewModel.Breeds[i]}");
                }

                int breedChoice;
                while (true)
                {
                    Console.Write($"Выберите породу (1-{_viewModel.Breeds.Length}): ");
                    if (!int.TryParse(Console.ReadLine(), out breedChoice) ||
                        breedChoice < 1 || breedChoice > _viewModel.Breeds.Length)
                    {
                        Console.WriteLine($"Введите число от 1 до {_viewModel.Breeds.Length}!");
                        continue;
                    }
                    break;
                }
                breed = _viewModel.Breeds[breedChoice - 1];
            }
            else
            {
                Console.Write("Введите породу кролика: ");
                breed = Console.ReadLine();
            }
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
            Console.Clear();
            Console.WriteLine("=== УДАЛЕНИЕ КРОЛИКА ===");

            _viewModel.LoadRabbits();
            if (_viewModel.Rabbits.Count == 0)
            {
                Console.WriteLine("Список кроликов пуст!");
                WaitForContinue();
                return;
            }

            Console.WriteLine("\nТекущие кролики:");
            foreach (var rabbit in _viewModel.Rabbits)
            {
                Console.WriteLine($"ID: {rabbit.Id,3} | {rabbit.Name}");
            }

            Console.Write("\nВведите ID кролика для удаления: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Ошибка! Введите число.");
                WaitForContinue();
                return;
            }

            var rabbitToDelete = _viewModel.Rabbits.FirstOrDefault(r => r.Id == id);
            if (rabbitToDelete == null)
            {
                Console.WriteLine($"Кролик с ID {id} не найден!");
                WaitForContinue();
                return;
            }

            _viewModel.SelectedRabbit = rabbitToDelete;
            _viewModel.RemoveRabbit();
            Console.WriteLine($"\n{_viewModel.StatusMessage}");
            WaitForContinue();
        }

        private void ShowUpdateRabbitMenu()
        {
            Console.Clear();
            Console.WriteLine("=== ОБНОВЛЕНИЕ ДАННЫХ КРОЛИКА ===");

            _viewModel.LoadRabbits();
            if (_viewModel.Rabbits.Count == 0)
            {
                Console.WriteLine("Список кроликов пуст!");
                WaitForContinue();
                return;
            }

            Console.WriteLine("\nТекущие кролики:");
            foreach (var rabbits in _viewModel.Rabbits)
            {
                Console.WriteLine($"ID: {rabbits.Id,3} | {rabbits.Name} | {rabbits.Breed} | {rabbits.Age} лет | {rabbits.Weight} кг");
            }

            Console.Write("\nВведите ID кролика для обновления: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Ошибка! Введите число.");
                WaitForContinue();
                return;
            }

            var rabbit = _viewModel.Rabbits.FirstOrDefault(r => r.Id == id);
            if (rabbit == null)
            {
                Console.WriteLine($"Кролик с ID {id} не найден!");
                WaitForContinue();
                return;
            }

            Console.WriteLine($"\nОбновление кролика: {rabbit.Name} (ID: {rabbit.Id})");

            string name;
            Console.Write("Новое имя (оставьте пустым для сохранения текущего): ");
            name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                name = rabbit.Name;
            }

            int age;
            while (true)
            {
                Console.Write("Новый возраст (1-50, 0 для сохранения текущего): ");
                if (!int.TryParse(Console.ReadLine(), out age))
                {
                    Console.WriteLine("Ошибка! Введите число.");
                    continue;
                }
                if (age == 0)
                {
                    age = rabbit.Age;
                }
                if (age < 1 || age > 50)
                {
                    Console.WriteLine("Возраст должен быть от 1 до 50 лет!");
                    continue;
                }
                break;
            }

            int weight;
            while (true)
            {
                Console.Write("Новый вес (1-100, 0 для сохранения текущего): ");
                if (!int.TryParse(Console.ReadLine(), out weight))
                {
                    Console.WriteLine("Ошибка! Введите число.");
                    continue;
                }
                if (weight == 0)
                {
                    weight = rabbit.Weight;
                }
                if (weight < 1 || weight > 100)
                {
                    Console.WriteLine("Вес должен быть от 1 до 100 кг!");
                    continue;
                }
                break;
            }

            string breed;
            if (_viewModel.Breeds != null && _viewModel.Breeds.Length > 0)
            {
                Console.WriteLine("\nДоступные породы:");
                for (int i = 0; i < _viewModel.Breeds.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {_viewModel.Breeds[i]}");
                }
                Console.Write($"Выберите породу (1-{_viewModel.Breeds.Length}, 0 для сохранения текущей): ");

                if (int.TryParse(Console.ReadLine(), out int breedChoice))
                {
                    if (breedChoice == 0)
                    {
                        breed = rabbit.Breed;
                    }
                    else if (breedChoice >= 1 && breedChoice <= _viewModel.Breeds.Length)
                    {
                        breed = _viewModel.Breeds[breedChoice - 1];
                    }
                    else
                    {
                        breed = rabbit.Breed;
                    }
                }
                else
                {
                    breed = rabbit.Breed;
                }
            }
            else
            {
                Console.Write("Новая порода (оставьте пустым для сохранения текущей): ");
                breed = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(breed))
                {
                    breed = rabbit.Breed;
                }
            }

            rabbit.Name = name;
            rabbit.Age = age;
            rabbit.Weight = weight;
            rabbit.Breed = breed;
            _viewModel.SelectedRabbit = rabbit;

            _viewModel.UpdateRabbit();
            Console.WriteLine($"\n{_viewModel.StatusMessage}");
            WaitForContinue();
        }

        private void AddRandomRabbit()
        {
            Console.Clear();
            Console.WriteLine("=== ДОБАВЛЕНИЕ СЛУЧАЙНОГО КРОЛИКА ===");

            _viewModel.AddRandomRabbit();
            Console.WriteLine($"\n{_viewModel.StatusMessage}");
            WaitForContinue();
        }

        private void ShowStatistics()
        {
            Console.Clear();
            Console.WriteLine("=== СТАТИСТИКА ===");

            _viewModel.ShowStatistics();
            Console.WriteLine($"\n{_viewModel.StatusMessage}");
            WaitForContinue();
        }

        private void WaitForContinue()
        {
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }
}