using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Business_logic___rabbit;

namespace Console_Rabbit
{
    using System;
    using System.Collections.Generic;

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== СИСТЕМА УПРАВЛЕНИЯ КРОЛИКАМИ ===");
            Console.WriteLine("Архитектура: Data Access Layer с EF и Dapper");

            // Тестируем подключения
            TestDatabaseConnections();

            Console.WriteLine("\nНажмите любую клавишу для запуска основного приложения...");
            Console.ReadKey();
            Console.Clear();

            RunMainApplication();
        }

        static void TestDatabaseConnections()
        {
            Console.WriteLine("\n🔧 ТЕСТИРОВАНИЕ ПОДКЛЮЧЕНИЙ К БАЗЕ ДАННЫХ:");

            // Тест Dapper
            Console.WriteLine("\n--- Тестируем Dapper ---");
            try
            {
                var dapperRepo = new DapperRepository<Rabbit>();
                Console.WriteLine("✅ Dapper: Успешно");

                // Тестовые операции
                var testRabbit = new Rabbit { Id = 999, Name = "ТестDapper", Breed = "Тест", Age = 1, Weight = 1 };
                dapperRepo.Add(testRabbit);
                var rabbits = dapperRepo.ReadAll();

                // ИСПРАВЛЕНО: Count() с круглыми скобками
                int rabbitCount = rabbits.Count();
                dapperRepo.Delete(testRabbit);
                Console.WriteLine($"✅ Dapper: CRUD операции работают (протестировано записей: {rabbitCount})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Dapper ошибка: {ex.Message}");
            }

            // Тест Entity Framework
            Console.WriteLine("\n--- Тестируем Entity Framework ---");
            try
            {
                var context = new RabbitDbContext();
                var efRepo = new EntityRepository<Rabbit>(context);
                Console.WriteLine("✅ Entity Framework: Успешно");

                // Тестовые операции
                var testRabbit = new Rabbit { Id = 888, Name = "ТестEF", Breed = "Тест", Age = 1, Weight = 1 };
                efRepo.Add(testRabbit);
                var rabbits = efRepo.ReadAll();

                // ИСПРАВЛЕНО: Count() с круглыми скобками
                int rabbitCount = rabbits.Count();
                efRepo.Delete(testRabbit);
                Console.WriteLine($"✅ Entity Framework: CRUD операции работают (протестировано записей: {rabbitCount})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Entity Framework ошибка: {ex.Message}");
            }
        }

        static void RunMainApplication()
        {
            // Выбор технологии
            bool useEntityFramework = ChooseTechnology();
            var logic = new Logic(useEntityFramework);

            Console.WriteLine($"\n🚀 Запуск приложения с технологией: {logic.GetCurrentTechnology()}");
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
            Console.Clear();

            RunMainMenu(logic);
        }

        static bool ChooseTechnology()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ВЫБОР ТЕХНОЛОГИИ ДОСТУПА К ДАННЫМ ===");
                Console.WriteLine("1 - Entity Framework (ORM)");
                Console.WriteLine("2 - Dapper (Micro-ORM)");
                Console.WriteLine("3 - Автоматический выбор");
                Console.Write("\nВыберите опцию (1-3): ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.WriteLine("🎯 Выбран: Entity Framework");
                        return true;
                    case "2":
                        Console.WriteLine("🎯 Выбран: Dapper");
                        return false;
                    case "3":
                        Console.WriteLine("🎯 Автоматический выбор...");
                        return TryDetectBestTechnology();
                    default:
                        Console.WriteLine("❌ Неверный выбор! Попробуйте снова.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static bool TryDetectBestTechnology()
        {
            try
            {
                Console.WriteLine("Проверяем Entity Framework...");
                var context = new RabbitDbContext();
                var efRepo = new EntityRepository<Rabbit>(context);
                Console.WriteLine("✅ Entity Framework работает стабильно");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Entity Framework не доступен: {ex.Message}");
                Console.WriteLine("🔄 Используем Dapper...");
                return false;
            }
        }

        static void RunMainMenu(Logic logic)
        {
            string[] breeds = { "Беляк", "Русак", "Толай", "Маньжурский", "Оранжевый" };

            while (true)
            {
                Console.Clear();
                ShowMainMenu(logic.GetCurrentTechnology());

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    ShowError("Неверный ввод! Введите число.");
                    continue;
                }

                if (choice == 11) break;

                ProcessMenuChoice(choice, logic, breeds);
            }

            Console.WriteLine("\n👋 До свидания! Приложение завершено.");
        }

        static void ShowMainMenu(string technology)
        {
            Console.WriteLine($"=== ГЛАВНОЕ МЕНЮ ===");
            Console.WriteLine($"Технология доступа: {technology}");
            Console.WriteLine($"Время: {DateTime.Now:HH:mm:ss}");
            Console.WriteLine();
            Console.WriteLine(" 1. Создать кролика");
            Console.WriteLine(" 2.  Удалить кролика");
            Console.WriteLine(" 3.  Прочесть кролика");
            Console.WriteLine(" 4.  Изменить кролика");
            Console.WriteLine(" 5. Средний возраст");
            Console.WriteLine(" 6.  Средний вес");
            Console.WriteLine(" 7. Создать рандомного кролика");
            Console.WriteLine(" 8. Показать всех кроликов");
            Console.WriteLine(" 9. Сортировать кроликов");
            Console.WriteLine("10. Сменить технологию");
            Console.WriteLine("11. Выход");
            Console.WriteLine();
            Console.Write("Выберите опцию: ");
        }

        static void ProcessMenuChoice(int choice, Logic logic, string[] breeds)
        {
            switch (choice)
            {
                case 1: AddRabbitMenu(logic, breeds); break;
                case 2: RemoveRabbitMenu(logic); break;
                case 3: ReadRabbitMenu(logic); break;
                case 4: UpdateRabbitMenu(logic, breeds); break;
                case 5: ShowAverageAge(logic); break;
                case 6: ShowAverageWeight(logic); break;
                case 7: AddRandomRabbitMenu(logic); break;
                case 8: ShowAllRabbitsMenu(logic); break;
                case 9: SortRabbitsMenu(logic); break;
                case 10: ChangeTechnologyMenu(); break;
                default: ShowError("Неверная опция!"); break;
            }
        }

        static void AddRabbitMenu(Logic logic, string[] breeds)
        {
            Console.Clear();
            Console.WriteLine("=== СОЗДАНИЕ КРОЛИКА ===");

            try
            {
                // Ввод ID
                int id = ReadValidNumber("Введите ID кролика (число): ", 1, 9999);

                // Ввод имени
                Console.Write("Введите имя кролика: ");
                string name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name))
                {
                    ShowError("Имя не может быть пустым!");
                    return;
                }

                // Ввод возраста
                int age = ReadValidNumber("Введите возраст кролика: ", 1, 50);

                // Ввод веса
                int weight = ReadValidNumber("Введите вес кролика: ", 1, 100);

                // Выбор породы
                int breedChoice = ReadValidNumber($"Выберите породу (1-{breeds.Length}):\n" +
                    GetBreedsMenu(breeds), 1, breeds.Length);
                string breed = breeds[breedChoice - 1];

                string result = logic.AddRabbit(id, name, age, weight, breed);
                ShowSuccess(result);
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при создании: {ex.Message}");
            }
        }

        static void RemoveRabbitMenu(Logic logic)
        {
            Console.Clear();
            Console.WriteLine("=== УДАЛЕНИЕ КРОЛИКА ===");

            // Показываем всех кроликов для reference
            string allRabbits = logic.ShowAllRabbits();
            if (allRabbits.Contains("пуст"))
            {
                ShowInfo("Список кроликов пуст!");
                return;
            }

            Console.WriteLine("Текущие кролики:");
            Console.WriteLine(allRabbits);
            Console.WriteLine();

            try
            {
                int id = ReadValidNumber("Введите ID кролика для удаления: ", 1, 9999);
                string result = logic.RemoveRabbit(id);
                ShowSuccess(result);
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при удалении: {ex.Message}");
            }
        }

        static void ReadRabbitMenu(Logic logic)
        {
            Console.Clear();
            Console.WriteLine("=== ПРОСМОТР КРОЛИКА ===");

            try
            {
                int id = ReadValidNumber("Введите ID кролика: ", 1, 9999);
                string result = logic.ReadRabbit(id);

                if (result.Contains("не найден"))
                    ShowError(result);
                else
                    ShowSuccess($"Данные кролика:\n{result}");
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при чтении: {ex.Message}");
            }
        }

        static void UpdateRabbitMenu(Logic logic, string[] breeds)
        {
            Console.Clear();
            Console.WriteLine("=== ИЗМЕНЕНИЕ КРОЛИКА ===");

            // Показываем текущих кроликов
            string allRabbits = logic.ShowAllRabbits();
            if (allRabbits.Contains("пуст"))
            {
                ShowInfo("Список кроликов пуст!");
                return;
            }

            Console.WriteLine("Текущие кролики:");
            Console.WriteLine(allRabbits);
            Console.WriteLine();

            try
            {
                int id = ReadValidNumber("Введите ID кролика для изменения: ", 1, 9999);

                // Проверяем существование
                var existing = logic.ReadRabbit(id);
                if (existing.Contains("не найден"))
                {
                    ShowError(existing);
                    return;
                }

                Console.WriteLine($"Текущие данные: {existing}");
                Console.WriteLine();

                // Ввод новых данных
                Console.Write("Введите новое имя: ");
                string name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name))
                {
                    ShowError("Имя не может быть пустым!");
                    return;
                }

                int age = ReadValidNumber("Введите новый возраст: ", 1, 50);
                int weight = ReadValidNumber("Введите новый вес: ", 1, 100);

                int breedChoice = ReadValidNumber($"Выберите новую породу (1-{breeds.Length}):\n" +
                    GetBreedsMenu(breeds), 1, breeds.Length);
                string breed = breeds[breedChoice - 1];

                logic.ChangeStatRabbit(id, name, age, weight, breed);
                ShowSuccess("Данные кролика успешно обновлены!");
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при изменении: {ex.Message}");
            }
        }

        static void ShowAverageAge(Logic logic)
        {
            Console.Clear();
            Console.WriteLine("=== СРЕДНИЙ ВОЗРАСТ ===");

            double averageAge = logic.GetAverageAge();
            ShowInfo($"Средний возраст всех кроликов: {averageAge:F2} лет");
        }

        static void ShowAverageWeight(Logic logic)
        {
            Console.Clear();
            Console.WriteLine("=== СРЕДНИЙ ВЕС ===");

            double averageWeight = logic.GetAverageWeight();
            ShowInfo($"Средний вес всех кроликов: {averageWeight:F2} кг");
        }

        static void AddRandomRabbitMenu(Logic logic)
        {
            Console.Clear();
            Console.WriteLine("=== СОЗДАНИЕ РАНДОМНОГО КРОЛИКА ===");

            try
            {
                string result = logic.AddRandomRabbit();
                ShowSuccess(result);
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при создании: {ex.Message}");
            }
        }

        static void ShowAllRabbitsMenu(Logic logic)
        {
            Console.Clear();
            Console.WriteLine("=== ВСЕ КРОЛИКИ ===");

            string result = logic.ShowAllRabbits();
            if (result.Contains("пуст"))
                ShowInfo(result);
            else
                Console.WriteLine(result);
        }

        static void SortRabbitsMenu(Logic logic)
        {
            Console.Clear();
            Console.WriteLine("=== СОРТИРОВКА КРОЛИКОВ ===");

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

                logic.SortRabbits(field, ascending);
                ShowSuccess("Кролики успешно отсортированы!");
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при сортировке: {ex.Message}");
            }
        }

        static void ChangeTechnologyMenu()
        {
            Console.Clear();
            Console.WriteLine("=== СМЕНА ТЕХНОЛОГИИ ===");
            Console.WriteLine("Для смены технологии требуется перезапуск приложения.");
            Console.WriteLine("Завершите текущую сессию и запустите приложение заново.");
        }

        // Вспомогательные методы
        static int ReadValidNumber(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int result) && result >= min && result <= max)
                    return result;

                ShowError($"Введите число от {min} до {max}!");
            }
        }

        static string GetBreedsMenu(string[] breeds)
        {
            string menu = "";
            for (int i = 0; i < breeds.Length; i++)
            {
                menu += $"{i + 1}. {breeds[i]}\n";
            }
            return menu;
        }

        static void ShowSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✅ {message}");
            Console.ResetColor();
            WaitForContinue();
        }

        static void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"❌ {message}");
            Console.ResetColor();
            WaitForContinue();
        }

        static void ShowInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"ℹ️ {message}");
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
