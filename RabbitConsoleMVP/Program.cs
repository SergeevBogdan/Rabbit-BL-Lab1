using System;
using BusinessLogicMVP;
using RabbitPresenter;
using RabbitSharedMVP;

namespace RabbitConsoleMVP
{

    public class ConsoleView : IView
    {

        public event Action<RabbitDTO> AddRabbitRequested;
        public event Action<int> RemoveRabbitRequested;
        public event Action<int> ReadRabbitRequested;
        public event Action<RabbitDTO> UpdateRabbitRequested;
        public event Action ShowAverageAgeRequested;
        public event Action ShowAverageWeightRequested;
        public event Action AddRandomRabbitRequested;
        public event Action ShowAllRabbitsRequested;
        public event Action<SortOperationDTO> SortRabbitsRequested;

        private string[] _breeds;

        public void Initialize(string[] breeds)
        {
            _breeds = breeds;
            ShowMainMenu();
        }

        public void ShowMainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== MVP CONSOLE - СИСТЕМА УПРАВЛЕНИЯ КРОЛИКАМИ ===");
                Console.WriteLine("1. Показать всех кроликов");
                Console.WriteLine("2. Добавить кролика");
                Console.WriteLine("3. Удалить кролика");
                Console.WriteLine("4. Просмотреть кролика");
                Console.WriteLine("5. Обновить кролика");
                Console.WriteLine("6. Добавить случайного кролика");
                Console.WriteLine("7. Средний возраст");
                Console.WriteLine("8. Средний вес");
                Console.WriteLine("9. Сортировать кроликов");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите опцию: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ShowAllRabbitsRequested?.Invoke(); break;
                    case "2": ShowAddRabbitMenu(); break;
                    case "3": ShowRemoveRabbitMenu(); break;
                    case "4": ShowReadRabbitMenu(); break;
                    case "5": ShowUpdateRabbitMenu(); break;
                    case "6": AddRandomRabbitRequested?.Invoke(); break;
                    case "7": ShowAverageAgeRequested?.Invoke(); break;
                    case "8": ShowAverageWeightRequested?.Invoke(); break;
                    case "9": ShowSortMenu(); break;
                    case "0": return;
                    default: DisplayMessage("Неверная опция!"); break;
                }

                WaitForContinue();
            }
        }

        private void ShowAddRabbitMenu()
        {
            Console.WriteLine("\n--- ДОБАВЛЕНИЕ КРОЛИКА ---");

            int id = ReadValidNumber("ID: ", 1, 9999);
            Console.Write("Имя: ");
            string name = Console.ReadLine();
            int age = ReadValidNumber("Возраст: ", 1, 50);
            int weight = ReadValidNumber("Вес: ", 1, 100);

            Console.WriteLine("Породы:");
            for (int i = 0; i < _breeds.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {_breeds[i]}");
            }
            int breedChoice = ReadValidNumber("Выберите породу: ", 1, _breeds.Length);
            string breed = _breeds[breedChoice - 1];

            var rabbitDto = new RabbitDTO
            {
                Id = id,
                Name = name,
                Age = age,
                Weight = weight,
                Breed = breed
            };
            AddRabbitRequested?.Invoke(rabbitDto); 
        }

        private void ShowRemoveRabbitMenu()
        {
            Console.WriteLine("\n--- УДАЛЕНИЕ КРОЛИКА ---");
            int id = ReadValidNumber("ID кролика для удаления: ", 1, 9999);
            RemoveRabbitRequested?.Invoke(id);
        }

        private void ShowReadRabbitMenu()
        {
            Console.WriteLine("\n--- ПРОСМОТР КРОЛИКА ---");
            int id = ReadValidNumber("ID кролика: ", 1, 9999);
            ReadRabbitRequested?.Invoke(id);
        }

        private void ShowUpdateRabbitMenu()
        {
            Console.WriteLine("\n--- ОБНОВЛЕНИЕ КРОЛИКА ---");
            int id = ReadValidNumber("ID кролика: ", 1, 9999);
            Console.Write("Новое имя: ");
            string name = Console.ReadLine();
            int age = ReadValidNumber("Новый возраст: ", 1, 50);
            int weight = ReadValidNumber("Новый вес: ", 1, 100);

            Console.WriteLine("Породы:");
            for (int i = 0; i < _breeds.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {_breeds[i]}");
            }
            int breedChoice = ReadValidNumber("Выберите породу: ", 1, _breeds.Length);
            string breed = _breeds[breedChoice - 1];

            var rabbitDto = new RabbitDTO
            {
                Id = id,
                Name = name,
                Age = age,
                Weight = weight,
                Breed = breed
            };
            UpdateRabbitRequested?.Invoke(rabbitDto); 
        }

        private void ShowSortMenu()
        {
            Console.WriteLine("\n--- СОРТИРОВКА КРОЛИКОВ ---");
            Console.WriteLine("Поля: 1-ID, 2-Имя, 3-Порода, 4-Возраст, 5-Вес");
            int field = ReadValidNumber("Поле: ", 1, 5);
            Console.WriteLine("Направление: 1-По возрастанию, 2-По убыванию");
            int direction = ReadValidNumber("Направление: ", 1, 2);
            var sortDto = new SortOperationDTO(field, direction == 1);
            SortRabbitsRequested?.Invoke(sortDto); 
        }

        private int ReadValidNumber(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int result) && result >= min && result <= max)
                    return result;
                Console.WriteLine($"Введите число от {min} до {max}!");
            }
        }


        public void DisplayMessage(string message)
        {
            Console.WriteLine($"\n {message}");
        }

        public void DisplayAllRabbits(string rabbits)
        {
            Console.WriteLine("\n=== СПИСОК КРОЛИКОВ ===");
            Console.WriteLine(rabbits);
        }

        public void DisplayRabbitDetails(string details)
        {
            Console.WriteLine("\n=== ДАННЫЕ КРОЛИКА ===");
            Console.WriteLine(details);
        }

        public void DisplayStatistics(string stats)
        {
            Console.WriteLine($"\n {stats}");
        }

        public string[] GetBreeds() => _breeds;

        private void WaitForContinue()
        {
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {

            bool useEF = args.Length == 0 || args[0].ToLower() != "dapper";

            Console.WriteLine($"=== MVP CONSOLE ({GetTechName(useEF)}) ===");
            Console.WriteLine("СИСТЕМА УПРАВЛЕНИЯ КРОЛИКАМИ");

            try
            {

                var model = ModelFactory.CreateModel(useEF);
                var view = new ConsoleView();
                var presenter = new Presenter(view, model);
                var breeds = presenter.GetBreeds();
                view.Initialize(breeds);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка запуска: {ex.Message}");
                Console.ReadKey();
            }
        }

        static string GetTechName(bool useEF) => useEF ? "Entity Framework" : "Dapper";
    }
}