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
        public List<Rabbit> Rabbits = new List<Rabbit>();

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
            Rabbit rabbit = new Rabbit() { Age = age, Breed = breed, Id = id, Name = name, Weight = weight };
            Rabbits.Add(rabbit);
        }

        /// <summary>
        /// Удаляет первого кролика из списка Rabbits по заданному идентификатору Id.
        /// </summary>
        /// <param name="id">Уникальный идентификатор кролика для удаления.</param>
        public string Remove(int id) //УЗНАТЬ КАК РЕШИТЬ ПРОБЛЕМУ С ЗАЩИТОЙ ДЛЯ ДУРАКОВ
        {
            Rabbit rabbitToRemove = Rabbits.Find(r => r.Id == id);
            if (rabbitToRemove != null) // защита на случай отсутствия элемента
            {
                Rabbits.Remove(rabbitToRemove);
                return "";
            }
            else
            {
                return "все не ок";
            }
        }

        /// <summary>
        /// Ищет кролика с заданным Id в списке Rabbits и возвращает его данные(чтение).
        /// </summary>
        /// <param name="id">Идентификатор кролика для поиска.</param>
        /// <returns>Строка с информацией о кролике (имя, возраст, вес, порода) или сообщение о ненахождении кролика.</returns>
        public string Read(int id)
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
        public void Change(int id, string name, int age, int weight, string breed) //ЗАЩИТА ЕСЛИ ОДНОГО И БОЛЕЕ ПОКАЗАТЕЛЯ НЕТ
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
        public void Filter(int change)
        {

        }




        //Поиск кролика по id/ ШЛАК!
        public int Find(int id)
        {
            int index = Rabbits.FindIndex(r => r.Id == id);
            return index;
        }

    }
}
