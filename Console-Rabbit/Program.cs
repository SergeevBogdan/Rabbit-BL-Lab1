using System;
using System.Collections.Generic;
using Business_logic___rabbit;

namespace Console_Rabbit
{
    class Program
    {
        private static IRabbitAdder _adder;
        private static IRabbitRemover _remover;
        private static IRabbitReader _reader;
        private static IRabbitUpdater _updater;
        private static IRabbitAgeCalculator _ageCalculator;
        private static IRabbitWeightCalculator _weightCalculator;
        private static IRabbitSorter _sorter;
        private static IRabbitRandomCreator _randomCreator;
        private static IRabbitDisplayer _displayer;
        private static IRabbitBreedProvider _breedProvider;

        static void Main(string[] args)
        {
            Console.WriteLine("СИСТЕМА УПРАВЛЕНИЯ КРОЛИКАМИ (SOLID + DI)");

            bool useEF = ChooseTechnology();
            InitializeServices(useEF);

            Console.WriteLine($"Используется: {(useEF ? "Entity Framework" : "Dapper")} с DI контейнером");
            RunMainMenu();
        }

        static void InitializeServices(bool useEntityFramework)
        {
            _adder = LogicFactory.CreateRabbitAdder(useEntityFramework);
            _remover = LogicFactory.CreateRabbitRemover(useEntityFramework);
            _reader = LogicFactory.CreateRabbitReader(useEntityFramework);
            _updater = LogicFactory.CreateRabbitUpdater(useEntityFramework);
            _ageCalculator = LogicFactory.CreateRabbitAgeCalculator(useEntityFramework);
            _weightCalculator = LogicFactory.CreateRabbitWeightCalculator(useEntityFramework);
            _sorter = LogicFactory.CreateRabbitSorter(useEntityFramework);
            _randomCreator = LogicFactory.CreateRabbitRandomCreator(useEntityFramework);
            _displayer = LogicFactory.CreateRabbitDisplayer(useEntityFramework);
            _breedProvider = LogicFactory.CreateRabbitBreedProvider(useEntityFramework);
        }

        static bool ChooseTechnology()
        {
            Console.WriteLine("Выберите технологию:");
            Console.WriteLine("1 - Entity Framework");
            Console.WriteLine("2 - Dapper");

            while (true)
            {
                var choice = Console.ReadLine();
                if (choice == "1") return true;
                else if (choice == "2") return false;
                Console.Write("Введите 1 или 2: ");
            }
        }

        static void RunMainMenu()
        {
            while (true)
            {
                Console.Clear();
                ShowMainMenu();

                if (!int.TryParse(Console.ReadLine(), out int choice)) continue;
                if (choice == 10) break;

                ProcessMenuChoice(choice);
            }
        }

        static void ShowMainMenu()
        {
            Console.WriteLine("ГЛАВНОЕ МЕНЮ (SOLID + DI)");
            Console.WriteLine("1. Создать кролика");
            Console.WriteLine("2. Удалить кролика");
            Console.WriteLine("3. Прочесть кролика");
            Console.WriteLine("4. Изменить кролика");
            Console.WriteLine("5. Средний возраст");
            Console.WriteLine("6. Средний вес");
            Console.WriteLine("7. Создать рандомного кролика");
            Console.WriteLine("8. Показать всех кроликов");
            Console.WriteLine("9. Сортировать кроликов");
            Console.WriteLine("10. Выход");
            Console.Write("Выберите опцию: ");
        }

        static void ProcessMenuChoice(int choice)
        {
            try
            {
                switch (choice)
                {
                    case 1: AddRabbitMenu(); break;
                    case 2: RemoveRabbitMenu(); break;
                    case 3: ReadRabbitMenu(); break;
                    case 4: UpdateRabbitMenu(); break;
                    case 5: ShowAverageAge(); break;
                    case 6: ShowAverageWeight(); break;
                    case 7: AddRandomRabbitMenu(); break;
                    case 8: ShowAllRabbitsMenu(); break;
                    case 9: SortRabbitsMenu(); break;
                    default: Console.WriteLine("Неверная опция!"); break;
                }
            }
            catch (Exception ex)
            {
                ShowError($"Системная ошибка: {ex.Message}");
            }
        }

        static void AddRabbitMenu()
        {
            Console.Clear();
            Console.WriteLine("СОЗДАНИЕ КРОЛИКА");

            try
            {
                string allRabbits = _displayer.ShowAllRabbits();
                if (!allRabbits.Contains("пуст"))
                {
                    Console.WriteLine("Текущие кролики:");
                    Console.WriteLine(allRabbits);
                    Console.WriteLine();
                }

                int id = ReadValidNumber("Введите ID кролика (1-9999): ", 1, 9999);

                string existingRabbit = _reader.ReadRabbit(id);
                if (!existingRabbit.Contains("не найден"))
                {
                    ShowError("Кролик с ID " + id + " уже существует!");
                    return;
                }

                Console.Write("Введите имя кролика: ");
                string name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name))
                {
                    ShowError("Имя не может быть пустым!");
                    return;
                }

                int age = ReadValidNumber("Введите возраст кролика (1-50): ", 1, 50);
                int weight = ReadValidNumber("Введите вес кролика (1-100): ", 1, 100);

                string[] breeds = _breedProvider.GetBreeds();
                Console.WriteLine("Доступные породы:");
                for (int i = 0; i < breeds.Length; i++)
                {
                    Console.WriteLine((i + 1) + ". " + breeds[i]);
                }
                int breedChoice = ReadValidNumber("Выберите породу (1-5): ", 1, breeds.Length);
                string breed = breeds[breedChoice - 1];

                Console.WriteLine("Подтвердите данные:");
                Console.WriteLine("ID: " + id);
                Console.WriteLine("Имя: " + name);
                Console.WriteLine("Возраст: " + age + " лет");
                Console.WriteLine("Вес: " + weight + " кг");
                Console.WriteLine("Порода: " + breed);
                Console.Write("Добавить кролика? (y/n): ");

                string confirm = Console.ReadLine();
                if (confirm?.ToLower() != "y")
                {
                    ShowInfo("Создание отменено");
                    return;
                }

                string result = _adder.AddRabbit(id, name, age, weight, breed);

                if (result.Contains("успешно"))
                {
                    ShowSuccess(result);
                    Console.WriteLine("Обновленный список кроликов:");
                    Console.WriteLine(_displayer.ShowAllRabbits());
                }
                else
                {
                    ShowError(result);
                }
            }
            catch (Exception ex)
            {
                ShowError("Ошибка при создании кролика: " + ex.Message);
            }
        }

        static void RemoveRabbitMenu()
        {
            Console.Clear();
            Console.WriteLine("УДАЛЕНИЕ КРОЛИКА");

            string allRabbits = _displayer.ShowAllRabbits();
            Console.WriteLine(allRabbits);
            Console.WriteLine();

            try
            {
                int id = ReadValidNumber("Введите ID кролика для удаления: ", 1, 9999);
                string result = _remover.RemoveRabbit(id);
                ShowSuccess(result);
                Console.WriteLine("Обновленный список:");
                Console.WriteLine(_displayer.ShowAllRabbits());
            }
            catch (Exception ex)
            {
                ShowError("Ошибка при удалении: " + ex.Message);
            }
        }

        static void ReadRabbitMenu()
        {
            Console.Clear();
            Console.WriteLine("ПРОСМОТР КРОЛИКА");

            try
            {
                int id = ReadValidNumber("Введите ID кролика: ", 1, 9999);
                string result = _reader.ReadRabbit(id);

                if (result.Contains("не найден"))
                    ShowError(result);
                else
                    ShowSuccess("Данные кролика:\n" + result);
            }
            catch (Exception ex)
            {
                ShowError("Ошибка при чтении: " + ex.Message);
            }
        }

        static void UpdateRabbitMenu()
        {
            Console.Clear();
            Console.WriteLine("ИЗМЕНЕНИЕ ДАННЫХ КРОЛИКА");

            string allRabbits = _displayer.ShowAllRabbits();
            if (allRabbits.Contains("пуст"))
            {
                ShowInfo("Список кроликов пуст! Сначала создайте кроликов.");
                return;
            }

            Console.WriteLine("Текущие кролики:");
            Console.WriteLine(allRabbits);
            Console.WriteLine();

            try
            {
                int id = ReadValidNumber("Введите ID кролика для изменения: ", 1, 9999);
                string currentData = _reader.ReadRabbit(id);
                if (currentData.Contains("не найден"))
                {
                    ShowError("Кролик с ID " + id + " не найден!");
                    return;
                }

                Console.WriteLine("Текущие данные кролика:");
                Console.WriteLine(currentData);
                Console.WriteLine("Введите новые данные:");

                Console.Write("Введите новое имя кролика: ");
                string name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name))
                {
                    ShowError("Имя не может быть пустым!");
                    return;
                }

                int age = ReadValidNumber("Введите новый возраст кролика (1-50): ", 1, 50);
                int weight = ReadValidNumber("Введите новый вес кролика (1-100): ", 1, 100);

                string[] breeds = _breedProvider.GetBreeds();
                Console.WriteLine("Доступные породы:");
                for (int i = 0; i < breeds.Length; i++)
                {
                    Console.WriteLine((i + 1) + ". " + breeds[i]);
                }
                int breedChoice = ReadValidNumber("Выберите новую породу (1-5): ", 1, breeds.Length);
                string breed = breeds[breedChoice - 1];

                Console.WriteLine("Подтвердите изменения:");
                Console.WriteLine("ID: " + id + " (неизменяем)");
                Console.WriteLine("Новое имя: " + name);
                Console.WriteLine("Новый возраст: " + age + " лет");
                Console.WriteLine("Новый вес: " + weight + " кг");
                Console.WriteLine("Новая порода: " + breed);
                Console.Write("Сохранить изменения? (y/n): ");

                string confirm = Console.ReadLine();
                if (confirm?.ToLower() != "y")
                {
                    ShowInfo("Изменения отменены");
                    return;
                }

                _updater.ChangeStatRabbit(id, name, age, weight, breed);
                ShowSuccess("Данные кролика успешно обновлены!");

                Console.WriteLine("Обновленные данные:");
                string updatedData = _reader.ReadRabbit(id);
                Console.WriteLine(updatedData);
            }
            catch (Exception ex)
            {
                ShowError("Ошибка при изменении данных кролика: " + ex.Message);
            }
        }

        static void ShowAverageAge()
        {
            Console.Clear();
            Console.WriteLine("СРЕДНИЙ ВОЗРАСТ");
            double averageAge = _ageCalculator.GetAverageAge();
            ShowInfo("Средний возраст всех кроликов: " + averageAge + " лет");
        }

        static void ShowAverageWeight()
        {
            Console.Clear();
            Console.WriteLine("СРЕДНИЙ ВЕС");
            double averageWeight = _weightCalculator.GetAverageWeight();
            ShowInfo("Средний вес всех кроликов: " + averageWeight + " кг");
        }

        static void AddRandomRabbitMenu()
        {
            Console.Clear();
            Console.WriteLine("СОЗДАНИЕ РАНДОМНОГО КРОЛИКА");

            try
            {
                string result = _randomCreator.AddRandomRabbit();
                ShowSuccess(result);
                Console.WriteLine("Обновленный список:");
                Console.WriteLine(_displayer.ShowAllRabbits());
            }
            catch (Exception ex)
            {
                ShowError("Ошибка при создании рандомного кролика: " + ex.Message);
            }
        }

        static void ShowAllRabbitsMenu()
        {
            Console.Clear();
            Console.WriteLine("ВСЕ КРОЛИКИ");

            try
            {
                string result = _displayer.ShowAllRabbits();
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
            WaitForContinue();
        }

        static void SortRabbitsMenu()
        {
            Console.Clear();
            Console.WriteLine("СОРТИРОВКА КРОЛИКОВ");

            try
            {
                Console.WriteLine("Выберите поле для сортировки:");
                Console.WriteLine("1 - ID");
                Console.WriteLine("2 - Имя");
                Console.WriteLine("3 - Порода");
                Console.WriteLine("4 - Возраст");
                Console.WriteLine("5 - Вес");

                int field = ReadValidNumber("Поле: ", 1, 5);

                Console.WriteLine("Направление сортировки:");
                Console.WriteLine("1 - По возрастанию");
                Console.WriteLine("2 - По убыванию");

                int directionChoice = ReadValidNumber("Направление: ", 1, 2);
                bool ascending = directionChoice == 1;

                _sorter.SortRabbits(field, ascending);
                WaitForContinue();
            }
            catch (Exception ex)
            {
                ShowError("Ошибка при сортировке: " + ex.Message);
            }
        }

        static int ReadValidNumber(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int result) && result >= min && result <= max)
                    return result;
                ShowError("Введите число от " + min + " до " + max + "!");
            }
        }

        static void ShowSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
            WaitForContinue();
        }

        static void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
            WaitForContinue();
        }

        static void ShowInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
            WaitForContinue();
        }

        static void WaitForContinue()
        {
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }
}
