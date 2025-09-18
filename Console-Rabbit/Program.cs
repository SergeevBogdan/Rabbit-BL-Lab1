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
            Logic logic = new Logic();
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

                if (string.IsNullOrEmpty(inputPos) || !int.TryParse(inputPos, out position))//2
                {
                    Console.Clear();
                    continue;
                }
                switch (position)
                {
                    case 1:
                        string[] breeds = { "Беляк", "Русак", "Толай", "Маньжурский", "Оранжевый" };

                        int id;
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
                        string name = Console.ReadLine();

                        int age;
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

                        int weight;
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

                        int breedChoice;
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

                        string result1 = Logic.Add(id, name, age, weight, breed);
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

                        string result2 = Logic.Remove(idToRemove);
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

                        string output = Logic.Read(idToRead);
                        Console.WriteLine(output);
                        Console.WriteLine("Нажмите любую клавишу...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 4:
                        int id1;
                        while (true)
                        {
                            Console.WriteLine("Введите id кролика для изменения (не изменяется):");
                            string inputId = Console.ReadLine();
                            if (string.IsNullOrEmpty(inputId) || !int.TryParse(inputId, out id1) || id1.ToString().Length > 3)
                            {
                                Console.WriteLine("Ошибка: введите число до 3 цифр");
                                continue;
                            }
                            break;
                        }

                        string name1;
                        while (true)
                        {
                            Console.WriteLine("Введите имя кролика:");
                            name1 = Console.ReadLine();
                            if (string.IsNullOrEmpty(name1))
                            {
                                Console.WriteLine("Имя не может быть пустым");
                                continue;
                            }
                            break;
                        }

                        int age1;
                        while (true)
                        {
                            Console.WriteLine("Введите возраст кролика:");
                            string inputAge = Console.ReadLine();
                            if (string.IsNullOrEmpty(inputAge) || !int.TryParse(inputAge, out age1))
                            {
                                Console.WriteLine("Введите корректное число для возраста");
                                continue;
                            }
                            break;
                        }

                        int weight1;
                        while (true)
                        {
                            Console.WriteLine("Введите вес кролика:");
                            string inputWeight = Console.ReadLine();
                            if (string.IsNullOrEmpty(inputWeight) || !int.TryParse(inputWeight, out weight1))
                            {
                                Console.WriteLine("Введите корректное число для веса");
                                continue;
                            }
                            break;
                        }

                        string[] breeds1 = { "Беляк", "Русак", "Толай", "Маньжурский", "Оранжевый" };
                        int breedChoice1;
                        while (true)
                        {
                            Console.WriteLine("Выберите породу кролика, введя цифру:");
                            for (int i = 0; i < breeds1.Length; i++)
                            {
                                Console.WriteLine($"{i + 1}. {breeds1[i]}");
                            }
                            string inputBreed = Console.ReadLine();
                            if (string.IsNullOrEmpty(inputBreed) || !int.TryParse(inputBreed, out breedChoice1) || breedChoice1 < 1 || breedChoice1 > breeds1.Length)
                            {
                                Console.WriteLine("Выберите цифру из списка");
                                continue;
                            }
                            break;
                        }

                        string breed1 = breeds1[breedChoice1 - 1];

                        Logic.Change(id1, name1, age1, weight1, breed1);
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
                        string RandomRabit = Logic.Random_rabbit_add();
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
                        Logic.Filter(change, direction);
                        Console.WriteLine("Нажмите любую клавишу...");
                        Console.ReadKey();
                        Console.Clear();
                        break;

                }

            }
        }
    }
}
