using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Business_logic___rabbit
{

    public class Logic
    {
        private class Rabbit
        {
            public int Id;
            public string Name;
            public string Breed;
            public int Age;
            public int Weight;
        }
        /*
        private static string folder = "Data";
        private static string filePath = "rabbits.txt";
        //private static string filePath = Path.Combine(folder, fileName);

        public static void SaveRabbitsToFile()
        {
            if (!File.Exists(filePath))
            {
            File.Create(filePath).Dispose();  // Создать и сразу закрыть файл
            }
            else
            {
                var lines = Rabbits.Select(r => $"{r.Id},{r.Name},{r.Breed},{r.Age},{r.Weight}");
                File.WriteAllLines(filePath, lines);
            }
        }

        public static void LoadRabbitsFromFile()
        {
            if (!File.Exists(filePath))
            {
            File.Create(filePath).Dispose();  // Создать и сразу закрыть файл
            }

            Rabbits.Clear();
            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length == 5)
                {
                    var rabbit = new Rabbit
                    {
                        Id = int.Parse(parts[0]),
                        Name = parts[1],
                        Breed = parts[2],
                        Age = int.Parse(parts[3]),
                        Weight = int.Parse(parts[4])
                    };
                    Rabbits.Add(rabbit);
                }
            }
        }

        */

        // Метод для поиска пути к папке проекта, поднимаясь от текущей директории вверх
        private static string dataFolderName = "Data";
        private static string projectFolderName = "Rabbit-BL-Lab1";
        private static string basePath = FindProjectPath(Directory.GetCurrentDirectory(), projectFolderName);
        private static string filePath = Path.Combine(basePath ?? "", dataFolderName, "rabbits.txt");
        public static void SaveRabbitsToFile()
        {
            string directoryPath = Path.Combine(basePath, dataFolderName);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            if (Rabbits.Count == 0) return;  // Если нет данных, ничего не пишем

            var lines = Rabbits.Select(r => $"{r.Id},{r.Name},{r.Breed},{r.Age},{r.Weight}").ToArray();
            File.WriteAllLines(filePath, lines);
            Console.WriteLine($"Файл сохранен по пути {filePath}");
        }
        public static void LoadRabbitsFromFile()
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Dispose();  // Создать и сразу закрыть файл
            }

            Rabbits.Clear();
            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length == 5)
                {
                    var rabbit = new Rabbit
                    {
                        Id = int.Parse(parts[0]),
                        Name = parts[1],
                        Breed = parts[2],
                        Age = int.Parse(parts[3]),
                        Weight = int.Parse(parts[4])
                    };
                    Rabbits.Add(rabbit);
                }
            }
        }// Метод для поиска пути к папке проекта, поднимаясь от текущей директории вверх
        private static string FindProjectPath(string currentDir, string projectFolderName)
        {
            DirectoryInfo dir = new DirectoryInfo(currentDir);

            while (dir != null)
            {
                if (dir.Name.Equals(projectFolderName, StringComparison.OrdinalIgnoreCase))
                {
                    return dir.FullName;
                }
                dir = dir.Parent;  // поднимаемся на папку выше
            }
            return null;
        }


        private static List<Rabbit> Rabbits = new List<Rabbit>();


        /// <summary>
        /// Создает новый объект Rabbit с заданными параметрами и добавляет его в список.
        /// </summary>
        /// <param name="id">Уникальный идентификатор кролика.</param>
        /// <param name="name">Имя кролика.</param>
        /// <param name="age">Возраст кролика.</param>
        /// <param name="weight">Вес кролика.</param>
        /// <param name="breed">Порода кролика.</param>
        /// <returns> Возращявет string значение, чтобы уведомнить об завершении операции </returns> 
        public static string AddRabbit(int id, string name, int age, int weight, string breed)
        {
            // Проверяем, есть ли уже кролик с таким id
            if (Rabbits.Exists(r => r.Id == id))
            {
                return "такой id уже есть"; // возвращаем ошибку, если id не уникален
            }


            Rabbit rabbit = new Rabbit() { Age = age, Breed = breed, Id = id, Name = name, Weight = weight };
            Rabbits.Add(rabbit);

            return "Кролик успешно добавлен"; // успешное добавление
        }


        /// <summary>
        /// Удаляет первого кролика из списка Rabbits по заданному идентификатору Id.
        /// </summary>
        /// <param name="id">Уникальный идентификатор кролика для удаления.</param>
        /// <returns> Возращявет string значение, чтобы уведомнить об завершении операции </returns> 
        public static string RemoveRabbit(int id) //УЗНАТЬ КАК РЕШИТЬ ПРОБЛЕМУ С ЗАЩИТОЙ ДЛЯ ДУРАКОВ, + добавить поиск через имя
        {

                Rabbit rabbitToRemove = Rabbits.Find(r => r.Id == id);
                if (rabbitToRemove != null) // защита на случай отсутствия элемента
                {
                    Rabbits.Remove(rabbitToRemove);
                    return "Заяц удален";
                }
                else
                {
                    return "Список пуст";
                }
        }

        /// <summary>
        /// Ищет кролика с заданным Id в списке Rabbits и возвращает его данные(чтение).
        /// </summary>
        /// <param name="id">Идентификатор кролика для поиска.</param>
        /// <returns>Строка с информацией о кролике (имя, возраст, вес, порода) или сообщение о ненахождении кролика.</returns>
        /// <returns> Возращявет string значение, чтобы дать описание кролика </returns> 
        public static string ReadRabbit(int id)
        {
                Rabbit rabbit = Rabbits.Find(r => r.Id == id);
                if (rabbit == null)
                    return "Кролик с заданным Id не найден";

                string text = "Имя: " + rabbit.Name + "\nВозраст: " + rabbit.Age + "\nВес: " + rabbit.Weight + "\nПорода: " + rabbit.Breed;
                return text;
        }

        /// <summary>
        /// Изменение его характеристик (имя, возраст, вес, порода).
        /// </summary>
        /// <param name="id">Уникальный идентификатор кролика, который нужно изменить.</param>
        /// <param name="name">Новое имя кролика.</param>
        /// <param name="age">Новый возраст кролика.</param>
        /// <param name="weight">Новый вес кролика.</param>
        /// <param name="breed">Новая порода кролика.</param>
        public static void ChangeStatRabbit(int id, string name, int age, int weight, string breed) //ЗАЩИТА ЕСЛИ ОДНОГО И БОЛЕЕ ПОКАЗАТЕЛЯ НЕТ - UI
        {
            int index = Rabbits.FindIndex(r => r.Id == id);
            if (index >= 0)
            {
                Rabbits[index].Age = age;
                Rabbits[index].Weight = weight;
                Rabbits[index].Name = name;
                Rabbits[index].Breed = breed;
            }
        }


        /// <summary>
        /// Рассчитывает средний возраст кроликов в списке.
        /// </summary>
        /// <returns>Средний возраст кроликов как число с плавающей точкой.</returns>
        public static double GetAverageAge()
        {
            if (Rabbits.Count != 0)
            {
                return Rabbits.Average(r => r.Age);
            }
            return 0;
        }

        /// <summary>
        /// Рассчитывает средний вес кроликов в списке.
        /// </summary>
        /// <returns>Средний вес кроликов как число с плавающей точкой.</returns>
        public static double GetAverageWeight()//ЗАЩИТА  ЕСЛИ КРОЛИКОВ НЕТ
        {
            if (Rabbits.Count != 0)
            {
                return Rabbits.Average(r => r.Weight);
            }
            return 0;
        }


        //Список по фильтрации с опред параметром, сделать аналогию для консоли и для FW
        /// <summary>
        /// Сортирует список кроликов по выбранному полю и направлению.
        /// </summary>
        /// <param name="sortField">
        /// Поле для сортировки:
        /// 1 - Id,
        /// 2 - Name,
        /// 3 - Breed,
        /// 4 - Age,
        /// 5 - Weight.
        /// </param>
        /// <param name="ascending">Направление сортировки: true - по возрастанию, false - по убыванию.</param>
        public static void SortRabbits(int sortField, bool ascending)
        { 
            switch (sortField)
            {
                case 1: // Id
                    Rabbits = ascending ? Rabbits.OrderBy(r => r.Id).ToList()
                                 : Rabbits.OrderByDescending(r => r.Id).ToList();
                    break;
                case 2: // Name
                    Rabbits = ascending ? Rabbits.OrderBy(r => r.Name).ToList()
                                 : Rabbits.OrderByDescending(r => r.Name).ToList();
                    break;
                case 3: // Breed
                    Rabbits = ascending ? Rabbits.OrderBy(r => r.Breed).ToList()
                                 : Rabbits.OrderByDescending(r => r.Breed).ToList();
                    break;
                case 4: // Age
                    Rabbits = ascending ? Rabbits.OrderBy(r => r.Age).ToList()
                                 : Rabbits.OrderByDescending(r => r.Age).ToList();
                    break;
                case 5: // Weight
                    Rabbits = ascending ? Rabbits.OrderBy(r => r.Weight).ToList()
                                 : Rabbits.OrderByDescending(r => r.Weight).ToList();
                    break;
            }
        }


        private static Random _rnd = new Random();
        //Добавить кролика по рандому
        /// <summary>
        /// СОздает рандомного кролика и добавляет его в список.
        /// </summary>
        public static string AddRandomRabbit()
        {
            string[] names = { "Пушок", "Снежинка", "Игнат", "Ибрагим", "Ма-му-ма-ба", "Кастет", "Энцегорловье","Доминико дэ-ко-ко", "Эй-эй-эй", "La lepre che salta in alto" };
            string[] breeds = { "Беляк", "Русак", "Толай", "Маньжурский", "Оранжевый" };

            string name = names[_rnd.Next(names.Length)];  // случайное имя из списка
            int id  = _rnd.Next(1, 1000); // id от 1 до 999
            while (true)
            {
                if (!Rabbits.Exists(r => r.Id == id))
                {
                    break;
                }
                id = _rnd.Next(1, 1000); // id от 1 до 999
            }
            Rabbit randomRabbit = new Rabbit
            {
                Id = id,                  
                Name = name,
                Breed = breeds[_rnd.Next(breeds.Length)],    // случайная порода из списка
                Age = _rnd.Next(1, 14),  // от 1 до 14                    
                Weight = _rnd.Next(1, 15)   //  от 1 до 15             
            };

            Rabbits.Add(randomRabbit);
            return "Рандомный кролик: " + name+ " был создан с id: " + id;
        }

        // ШЛАК!
        /// <summary>
        /// Поиск id кролика из списка.
        /// </summary>
        /// <param name="index"> Индекс списка по его порядковому номеру (сугубо для WinFowrms). Если такого показателя нет то возвращает 0 </param>
        public int GetRabbitIdByIndex(int index)
        {
            if (index >= 0 && index < Rabbits.Count)
            {
                return Rabbits[index].Id; // возвращаем id кролика по индексу
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Показывает всех кроликов из списка. Если до этого был измене по фильтру, то показывает с фильтром.
        /// </summary>
        /// <returns> Возвращает string значение, для вывода всех кроликов</returns>
        public static string ShowAllRabbits()
        {
            string result = "";
            foreach (Rabbit rabbit in Rabbits)
            {
                result += "Кролик: ID " + rabbit.Id + 
                    " Имя: " + rabbit.Name +
                    " Вес: " + rabbit.Weight +
                    " Возраст: " + rabbit.Age +
                    " Порода: " + rabbit.Breed +
                    " \n";
            }
            return result;
        }

    }
}
