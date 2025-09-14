using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rabbit_Model;

namespace Business_logic___rabbit
{
    public class Logic
    {
        //Сделать вечером: 1 изменить систему изменения животный по ID, сделать поиск по имени и Id, доработать ЗОТ

        private List<Rabbit> Rabbits = new List<Rabbit>();

        /// <summary>
        /// Создает новый объект Rabbit с заданными параметрами и добавляет его в список.
        /// </summary>
        /// <param name="id">Уникальный идентификатор кролика.</param>
        /// <param name="name">Имя кролика.</param>
        /// <param name="age">Возраст кролика.</param>
        /// <param name="weight">Вес кролика.</param>
        /// <param name="breed">Порода кролика.</param>
        public void Add(int id, string name, int age, int weight, string breed) //TODO ЗАЩИТЯ ДЛЯ ДУРАКОВ, ЕСЛИ ЧТО ТО НЕ ДОБАВИЛ
        {
            //добавить проверку на уникальность id сюда
            Rabbit rabbit = new Rabbit() { Age = age, Breed = breed, Id = id, Name = name, Weight = weight };
            Rabbits.Add(rabbit);
        }

        /// <summary>
        /// Удаляет первого кролика из списка Rabbits по заданному идентификатору Id.
        /// </summary>
        /// <param name="id">Уникальный идентификатор кролика для удаления.</param>
        public string Remove(int id) //УЗНАТЬ КАК РЕШИТЬ ПРОБЛЕМУ С ЗАЩИТОЙ ДЛЯ ДУРАКОВ, + добавить поиск через имя
        {
            if (ID_rabbit(id))
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
            else
            {
                return "Неверный id";
            }
        }

        /// <summary>
        /// Ищет кролика с заданным Id в списке Rabbits и возвращает его данные(чтение).
        /// </summary>
        /// <param name="id">Идентификатор кролика для поиска.</param>
        /// <returns>Строка с информацией о кролике (имя, возраст, вес, порода) или сообщение о ненахождении кролика.</returns>
        public string Read(int id)
        {
            if (ID_rabbit(id))
            {
                Rabbit rabbit = Rabbits.Find(r => r.Id == id);
                if (rabbit == null)
                    return "Кролик с заданным Id не найден";

                string text = "Имя: " + rabbit.Name + "\nВозраст: " + rabbit.Age + "\nВес: " + rabbit.Weight + "\nПорода: " + rabbit.Breed;
                return text;
            }
            else
            {
                return "Неверный id";
            }
        }

        /// <summary>
        /// Изменение его характеристик (имя, возраст, вес, порода).
        /// </summary>
        /// <param name="id">Уникальный идентификатор кролика, который нужно изменить.</param>
        /// <param name="name">Новое имя кролика.</param>
        /// <param name="age">Новый возраст кролика.</param>
        /// <param name="weight">Новый вес кролика.</param>
        /// <param name="breed">Новая порода кролика.</param>
        public void Change(int id, string name, int age, int weight, string breed) //ЗАЩИТА ЕСЛИ ОДНОГО И БОЛЕЕ ПОКАЗАТЕЛЯ НЕТ - UI
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
        public double GetAverageAge()
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
        public double GetAverageWeight()//ЗАЩИТА  ЕСЛИ КРОЛИКОВ НЕТ
        {
            if (Rabbits.Count != 0)
            {
                return Rabbits.Average(r => r.Weight);
            }
            return 0;
        }

        //Список с отдельной породой

        //Список по фильтрации с опред параметром, сделать аналогию для консоли и для FW
        /// <summary>
        /// Сортирует список кроликов по выбранному полю и направлению.
        /// </summary>
        /// <param name="change">
        /// Поле для сортировки:
        /// 1 - Id,
        /// 2 - Name,
        /// 3 - Breed,
        /// 4 - Age,
        /// 5 - Weight.
        /// </param>
        /// <param name="direction">Направление сортировки: true - по возрастанию, false - по убыванию.</param>
        public void Filter(int change, bool direction)
        {
            switch (change)
            {
                case 1: // Id
                    Rabbits = direction ? Rabbits.OrderBy(r => r.Id).ToList()
                                 : Rabbits.OrderByDescending(r => r.Id).ToList();
                    break;
                case 2: // Name
                    Rabbits = direction ? Rabbits.OrderBy(r => r.Name).ToList()
                                 : Rabbits.OrderByDescending(r => r.Name).ToList();
                    break;
                case 3: // Breed
                    Rabbits = direction ? Rabbits.OrderBy(r => r.Breed).ToList()
                                 : Rabbits.OrderByDescending(r => r.Breed).ToList();
                    break;
                case 4: // Age
                    Rabbits = direction ? Rabbits.OrderBy(r => r.Age).ToList()
                                 : Rabbits.OrderByDescending(r => r.Age).ToList();
                    break;
                case 5: // Weight
                    Rabbits = direction ? Rabbits.OrderBy(r => r.Weight).ToList()
                                 : Rabbits.OrderByDescending(r => r.Weight).ToList();
                    break;
            }
        }

        //Добавить кролика по рандому
        /// <summary>
        /// СОздает рандомного кролика и добавляет его в список.
        /// </summary>
        private Random rnd = new Random();

        public string Random_rabbit_add()
        {
            string[] names = { "Пушок", "Снежинка", "Игнат", "Ибрагим", "Ма-му-ма-ба", "Кастет", "Энцегорловье","Доминико дэ-ко-ко", "","" };
            string[] breeds = { "Беляк", "Русак", "Толай", "Маньжурский", "Оранжевый" };

            string name = names[rnd.Next(names.Length)];  // случайное имя из списка
            Rabbit randomRabbit = new Rabbit
            {
                Id = rnd.Next(1, 1000),    // id от 1 до 999                  
                Name = name,
                Breed = breeds[rnd.Next(breeds.Length)],    // случайная порода из списка
                Age = rnd.Next(1, 14),  // от 1 до 14                    
                Weight = rnd.Next(1, 15)   //  от 1 до 15             
            };

            Rabbits.Add(randomRabbit);
            return "Рандомный кролик: " + name+ " был создан";
        }



        // условие на ID
        private bool ID_rabbit(int id)
        {
            if (Rabbits.Any(r => r.Id == id))
            {
                return false;
            }
            return true;
        }

    }
}
