using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Business_logic___rabbit;

namespace Console_Rabbit
{
    internal class Program
    { 
        static void Main(string[] args)
        {
            Console.WriteLine(Environment.CurrentDirectory);

            int weight, age, id, breedChoice;
            string name;
            Logic logic = new Logic();

            Logic.SaveRabbitsToFile();

            Logic.LoadRabbitsFromFile();
            //TODO Сделать оформление
            string text = " Выберите опцию: " +
                "\n 1. Создать кролика" +
                "\n 2. Удалить кролика " +
                "\n 3. Прочесть кролика" +
                "\n 4. Изменить кролика" +
                "\n 5. Вычесть средний возраст кролика"+
                "\n 6. Вычесть средний вес кролика"+
                "\n 7. Создать  рандомного кролика" +
                "\n 8. Показать весь список кроликов" +
                "\n 9. Изменить/фильтр весь список кроликов по определенному индексу" +
                "\n 10. Выход";
            int position = 0;
            while (position != 10)
            {
                Console.WriteLine(text);
                string inputPos = Console.ReadLine();

                if (string.IsNullOrEmpty(inputPos) || !int.TryParse(inputPos, out position))
                {
                    Console.Clear();
                    continue;
                }
                switch (position)
                {
                    case 1:
                        string[] breeds = { "Беляк", "Русак", "Толай", "Маньжурский", "Оранжевый" };

                        while (true)
                        {
                            Console.WriteLine("Введите id кролика (до 3 цифр):");
                            string inputId = Console.ReadLine();

                            if (string.IsNullOrEmpty(inputId) || inputId.Length > 3 || !int.TryParse(inputId, out id))
                            {
                                Console.WriteLine("Ошибка: Введите число не больше 3 цифр.");
                                continue;
                            }
                            break;
                        }

                        Console.WriteLine("Введите имя кролика:");
                        name = Console.ReadLine();

                        while (true)
                        {
                            Console.WriteLine("Введите возраст кролика:");
                            string inputAge = Console.ReadLine();

                            if (string.IsNullOrEmpty(inputAge) || !int.TryParse(inputAge, out age))
                            {
                                Console.WriteLine("Ошибка: Введите корректное число для возраста.");
                                continue;
                            }
                            break;
                        }

                        while (true)
                        {
                            Console.WriteLine("Введите вес кролика:");
                            string inputWeight = Console.ReadLine();

                            if (string.IsNullOrEmpty(inputWeight) || !int.TryParse(inputWeight, out weight))
                            {
                                Console.WriteLine("Ошибка: Введите корректное число для веса.");
                                continue;
                            }
                            break;
                        }

                        while (true)
                        {
                            Console.WriteLine("Выберите породу кролика, введя цифру:");
                            for (int i = 0; i < breeds.Length; i++)
                            {
                                Console.WriteLine($"{i + 1}. {breeds[i]}");
                            }
                            string inputBreed = Console.ReadLine();

                            if (string.IsNullOrEmpty(inputBreed) || !int.TryParse(inputBreed, out breedChoice) || breedChoice < 1 || breedChoice > breeds.Length)
                            {
                                Console.WriteLine("Ошибка: Выберите цифру из списка.");
                                continue;
                            }
                            break;
                        }

                        string breed = breeds[breedChoice - 1];

                        string result1 = Logic.AddRabbit(id, name, age, weight, breed);
                        Console.WriteLine(result1);
                        Console.WriteLine("Нажмите любую клавишу...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 2:
                        int idToRemove;
                        while (true)
                        {
                            Console.WriteLine("Введите id кролика для удаления:");
                            string input = Console.ReadLine();

                            if (string.IsNullOrEmpty(input) || !int.TryParse(input, out idToRemove))
                            {
                                Console.WriteLine("Ошибка: введите корректное число.");
                                continue;
                            }
                            break;

                        }

                        string result2 = Logic.ReadRabbit(idToRemove);
                        Console.WriteLine(result2);
                        Console.WriteLine("Нажмите любую клавишу...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 3:
                        int idToRead;
                        while (true)
                        {
                            Console.WriteLine("Введите id кролика для чтения:");
                            string input = Console.ReadLine();

                            if (string.IsNullOrEmpty(input) || !int.TryParse(input, out idToRead))
                            {
                                Console.WriteLine("Ошибка: Введите корректное число");
                                continue;
                            }
                            break;
                        }

                        string output = Logic.ReadRabbit(idToRead);
                        Console.WriteLine(output);
                        Console.WriteLine("Нажмите любую клавишу...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 4:
 
                        while (true)
                        {
                            Console.WriteLine("Введите id кролика для изменения (не изменяется):");
                            string inputId = Console.ReadLine();
                            if (string.IsNullOrEmpty(inputId) || !int.TryParse(inputId, out id) || id.ToString().Length > 3)
                            {
                                Console.WriteLine("Ошибка: введите число до 3 цифр");
                                continue;
                            }
                            break;
                        }

                        while (true)
                        {
                            Console.WriteLine("Введите имя кролика:");
                            name = Console.ReadLine();
                            if (string.IsNullOrEmpty(name))
                            {
                                Console.WriteLine("Имя не может быть пустым");
                                continue;
                            }
                            break;
                        }

                        while (true)
                        {
                            Console.WriteLine("Введите возраст кролика:");
                            string inputAge = Console.ReadLine();
                            if (string.IsNullOrEmpty(inputAge) || !int.TryParse(inputAge, out age))
                            {
                                Console.WriteLine("Введите корректное число для возраста");
                                continue;
                            }
                            break;
                        }

                        while (true)
                        {
                            Console.WriteLine("Введите вес кролика:");
                            string inputWeight = Console.ReadLine();
                            if (string.IsNullOrEmpty(inputWeight) || !int.TryParse(inputWeight, out weight))
                            {
                                Console.WriteLine("Введите корректное число для веса");
                                continue;
                            }
                            break;
                        }

                        string[] breeds1 = { "Беляк", "Русак", "Толай", "Маньжурский", "Оранжевый" };
                        while (true)
                        {
                            Console.WriteLine("Выберите породу кролика, введя цифру:");
                            for (int i = 0; i < breeds1.Length; i++)
                            {
                                Console.WriteLine($"{i + 1}. {breeds1[i]}");
                            }
                            string inputBreed = Console.ReadLine();
                            if (string.IsNullOrEmpty(inputBreed) || !int.TryParse(inputBreed, out breedChoice) || breedChoice < 1 || breedChoice > breeds1.Length)
                            {
                                Console.WriteLine("Выберите цифру из списка");
                                continue;
                            }
                            break;
                        }

                        string breed1 = breeds1[breedChoice - 1];

                        Logic.ChangeStatRabbit(id, name, age, weight, breed1);
                        Console.WriteLine("Кролик изменён");
                        Console.WriteLine("Нажмите любую клавишу...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 5:
                        double ages = Logic.GetAverageAge();
                        Console.WriteLine("Средний возраст кроликов: "+ ages);
                        Console.WriteLine("Нажмите любую клавишу...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 6:
                        double weights = Logic.GetAverageWeight();
                        Console.WriteLine("Средний вес кроликов: " + weights);
                        Console.WriteLine("Нажмите любую клавишу...");
                        Console.ReadKey();
                        Console.Clear();
                        break; 
                    case 7:
                        string RandomRabit = Logic.AddRandomRabbit();
                        Console.WriteLine(RandomRabit);
                        Console.WriteLine("Нажмите любую клавишу...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 8:
                        string ShowAll = Logic.ShowAllRabbits();
                        Console.WriteLine(ShowAll);
                        Console.WriteLine("Нажмите любую клавишу...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 9:
                        int change;
                        while (true)
                        {
                            Console.WriteLine("Выберите индекс сортировки:\n1 - Id\n2 - Name\n3 - Breed\n4 - Age\n5 - Weight");
                            string inputChange = Console.ReadLine();
                            if (!int.TryParse(inputChange, out change) || change < 1 || change > 5)
                            {
                                Console.WriteLine("Введите число от 1 до 5");
                                continue;
                            }
                            break;
                        }

                        bool direction;
                        while (true)
                        {
                            Console.WriteLine("Выберите направление сортировки:\n1 - по возрастанию\n0 - по убыванию");
                            string inputDir = Console.ReadLine();
                            if (inputDir == "1")
                            {
                                direction = true;
                                break;
                            }
                            else if (inputDir == "0")
                            {
                                direction = false;
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Введите 1 или 0");
                            }
                        }
                        Logic.SortRabbits(change, direction);
                        Console.WriteLine("Нажмите любую клавишу...");
                        Console.ReadKey();
                        Console.Clear();
                        break;

                }

            }
            Logic.SaveRabbitsToFile();
        }
    }
}
