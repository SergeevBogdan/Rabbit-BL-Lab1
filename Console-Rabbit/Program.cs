using System;
using System.Collections.Generic;
using Business_logic_rabbit;

namespace Console_Rabbit
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("СИСТЕМА УПРАВЛЕНИЯ КРОЛИКАМИ");

            bool useEF = ChooseTechnology();
            var logic = new Logic(useEF);

            Console.WriteLine("Используется: " + logic.GetCurrentTechnology());

            RunMainMenu(logic);
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

        static void RunMainMenu(Logic logic)
        {
            while (true)
            {
                Console.Clear();
                ShowMainMenu(logic.GetCurrentTechnology());

                if (!int.TryParse(Console.ReadLine(), out int choice)) continue;

                if (choice == 10) break;

                ProcessMenuChoice(choice, logic);
            }
        }

        static void ShowMainMenu(string technology)
        {
            Console.WriteLine("ГЛАВНОЕ МЕНЮ (" + technology + ")");
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

        static void ProcessMenuChoice(int choice, Logic logic)
        {
            switch (choice)
            {
                case 1: AddRabbitMenu(logic); break;
                case 2: RemoveRabbitMenu(logic); break;
                case 3: ReadRabbitMenu(logic); break;
                case 4: UpdateRabbitMenu(logic); break;
                case 5: ShowAverageAge(logic); break;
                case 6: ShowAverageWeight(logic); break;
                case 7: AddRandomRabbitMenu(logic); break;
                case 8: ShowAllRabbitsMenu(logic); break;
                case 9: SortRabbitsMenu(logic); break;
                default: Console.WriteLine("Неверная опция!"); break;
            }
            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }

        static void AddRabbitMenu(Logic logic)
        {
            Console.Clear();
            Console.WriteLine("СОЗДАНИЕ КРОЛИКА");

            try
            {
                string allRabbits = logic.ShowAllRabbits();
                if (!allRabbits.Contains("пуст"))
                {
                    Console.WriteLine("Текущие кролики:");
                    Console.WriteLine(allRabbits);
                    Console.WriteLine();
                }

                int id = ReadValidNumber("Введите ID кролика (1-9999): ", 1, 9999);

                string existingRabbit = logic.ReadRabbit(id);
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

                string[] breeds = logic.GetBreeds();
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

                string result = logic.AddRabbit(id, name, age, weight, breed);

                if (result.Contains("успешно"))
                {
                    ShowSuccess(result);
                    Console.WriteLine("Обновленный список кроликов:");
                    Console.WriteLine(logic.ShowAllRabbits());
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

        static void RemoveRabbitMenu(Logic logic)
        {
            Console.Clear();
            Console.WriteLine("УДАЛЕНИЕ КРОЛИКА");

            string allRabbits = logic.ShowAllRabbits();
            Console.WriteLine(allRabbits);
            Console.WriteLine();

            try
            {
                int id = ReadValidNumber("Введите ID кролика для удаления: ", 1, 9999);
                string result = logic.RemoveRabbit(id);
                ShowSuccess(result);

                Console.WriteLine("Обновленный список:");
                Console.WriteLine(logic.ShowAllRabbits());
            }
            catch (Exception ex)
            {
                ShowError("Ошибка при удалении: " + ex.Message);
            }
        }

        static void ReadRabbitMenu(Logic logic)
        {
            Console.Clear();
            Console.WriteLine("ПРОСМОТР КРОЛИКА");

            try
            {
                int id = ReadValidNumber("Введите ID кролика: ", 1, 9999);
                string result = logic.ReadRabbit(id);

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

        static void UpdateRabbitMenu(Logic logic)
        {
            Console.Clear();
            Console.WriteLine("ИЗМЕНЕНИЕ ДАННЫХ КРОЛИКА");
            string allRabbits = logic.ShowAllRabbits();
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
                string currentData = logic.ReadRabbit(id);
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
                string[] breeds = logic.GetBreeds();

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

                logic.ChangeStatRabbit(id, name, age, weight, breed);
                ShowSuccess("Данные кролика успешно обновлены!");

                Console.WriteLine("Обновленные данные:");
                string updatedData = logic.ReadRabbit(id);
                Console.WriteLine(updatedData);

                Console.WriteLine("Обновленный список всех кроликов:");
                Console.WriteLine(logic.ShowAllRabbits());
            }
            catch (Exception ex)
            {
                ShowError("Ошибка при изменении данных кролика: " + ex.Message);
            }
        }

        static void ShowAverageAge(Logic logic)
        {
            Console.Clear();
            Console.WriteLine("СРЕДНИЙ ВОЗРАСТ");

            double averageAge = logic.GetAverageAge();
            ShowInfo("Средний возраст всех кроликов: " + averageAge + " лет");
        }

        static void ShowAverageWeight(Logic logic)
        {
            Console.Clear();
            Console.WriteLine("СРЕДНИЙ ВЕС");

            double averageWeight = logic.GetAverageWeight();
            ShowInfo("Средний вес всех кроликов: " + averageWeight + " кг");
        }

        static void AddRandomRabbitMenu(Logic logic)
        {
            Console.Clear();
            Console.WriteLine("СОЗДАНИЕ РАНДОМНОГО КРОЛИКА");

            try
            {
                string result = logic.AddRandomRabbit();
                ShowSuccess(result);

                Console.WriteLine("Обновленный список:");
                Console.WriteLine(logic.ShowAllRabbits());
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        static void ShowAllRabbitsMenu(Logic logic)
        {
            Console.Clear();
            Console.WriteLine("ВСЕ КРОЛИКИ");

            try
            {
                string result = logic.ShowAllRabbits();
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            WaitForContinue();
        }

        static void SortRabbitsMenu(Logic logic)
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

                logic.SortRabbits(field, ascending);
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
            Console.WriteLine(message);
            WaitForContinue();
        }

        static void ShowError(string message)
        {
            Console.WriteLine(message);
            WaitForContinue();
        }

        static void ShowInfo(string message)
        {
            Console.WriteLine(message);
            WaitForContinue();
        }

        static void WaitForContinue()
        {
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }
}