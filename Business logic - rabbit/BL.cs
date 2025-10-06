using System;
using System.Collections.Generic;
using System.Linq;

namespace Business_logic___rabbit
{
    /// <summary>
    /// Основной класс бизнес-логики для управления кроликами
    /// Обеспечивает работу с данными через репозиторий
    /// Поддерживает Entity Framework и Dapper
    /// </summary>
    public class Logic
    {
        private IRepository<Rabbit> _repository;
        private static Random _rnd = new Random();
        private readonly string _technology;

        /// <summary>
        /// Инициализирует новый экземпляр Logic с выбранной технологией доступа к данным
        /// </summary>
        /// <param name="useEntityFramework">true - использовать Entity Framework, false - использовать Dapper</param>
        public Logic(bool useEntityFramework = true)
        {
            if (useEntityFramework)
            {
                try
                {
                    var context = new RabbitDbContext();
                    _repository = new EntityRepository<Rabbit>(context);
                    _technology = "Entity Framework";
                }
                catch (Exception ex)
                {
                    _repository = new DapperRepository<Rabbit>();
                    _technology = "Dapper (автопереключение)";
                }
            }
            else
            {
                _repository = new DapperRepository<Rabbit>();
                _technology = "Dapper";
            }
        }

        /// <summary>
        /// Возвращает название текущей используемой технологии доступа к данным
        /// </summary>
        /// <returns>Название технологии (Entity Framework или Dapper)</returns>
        public string GetCurrentTechnology() => _technology;

        /// <summary>
        /// Добавляет нового кролика в базу данных
        /// </summary>
        /// <param name="id">Уникальный идентификатор кролика</param>
        /// <param name="name">Имя кролика</param>
        /// <param name="age">Возраст кролика в годах</param>
        /// <param name="weight">Вес кролика в кг</param>
        /// <param name="breed">Порода кролика</param>
        /// <returns>Результат операции: сообщение об успехе или ошибке</returns>
        public string AddRabbit(int id, string name, int age, int weight, string breed)
        {
            try
            {
                var existing = _repository.ReadById(id);
                if (existing != null) return "такой id уже есть";

                var rabbit = new Rabbit { Id = id, Name = name, Age = age, Weight = weight, Breed = breed };
                _repository.Add(rabbit);
                return "Кролик успешно добавлен";
            }
            catch (Exception ex)
            {
                return $"Ошибка при добавлении: {ex.Message}";
            }
        }

        /// <summary>
        /// Удаляет кролика из базы данных по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор кролика для удаления</param>
        /// <returns>Результат операции: сообщение об успехе или ошибке</returns>
        public string RemoveRabbit(int id)
        {
            try
            {
                var rabbit = _repository.ReadById(id);
                if (rabbit != null)
                {
                    _repository.Delete(rabbit);
                    return "Кролик удален";
                }
                return "Кролик не найден";
            }
            catch (Exception ex) { return $"Ошибка при удалении: {ex.Message}"; }
        }

        /// <summary>
        /// Получает информацию о кролике по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор кролика</param>
        /// <returns>Информация о кролике или сообщение об ошибке</returns>
        public string ReadRabbit(int id)
        {
            try
            {
                var rabbit = _repository.ReadById(id);
                if (rabbit == null) return "Кролик с заданным Id не найден";

                return $"Имя: {rabbit.Name}\nВозраст: {rabbit.Age}\nВес: {rabbit.Weight}\nПорода: {rabbit.Breed}";
            }
            catch (Exception ex) { return $"Ошибка при чтении: {ex.Message}"; }
        }

        /// <summary>
        /// Изменяет данные существующего кролика
        /// </summary>
        /// <param name="id">Идентификатор кролика</param>
        /// <param name="name">Новое имя кролика</param>
        /// <param name="age">Новый возраст кролика</param>
        /// <param name="weight">Новый вес кролика</param>
        /// <param name="breed">Новая порода кролика</param>
        public void ChangeStatRabbit(int id, string name, int age, int weight, string breed)
        {
            try
            {
                var rabbit = _repository.ReadById(id);
                if (rabbit != null)
                {
                    rabbit.Name = name;
                    rabbit.Age = age;
                    rabbit.Weight = weight;
                    rabbit.Breed = breed;
                    _repository.Update(rabbit);
                }
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// Вычисляет средний возраст всех кроликов в базе данных
        /// </summary>
        /// <returns>Средний возраст кроликов или 0 если кроликов нет</returns>
        public double GetAverageAge()
        {
            try
            {
                var rabbits = _repository.ReadAll().ToList();
                return rabbits.Count != 0 ? rabbits.Average(r => r.Age) : 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// Вычисляет средний вес всех кроликов в базе данных
        /// </summary>
        /// <returns>Средний вес кроликов или 0 если кроликов нет</returns>
        public double GetAverageWeight()
        {
            try
            {
                var rabbits = _repository.ReadAll().ToList();
                return rabbits.Count != 0 ? rabbits.Average(r => r.Weight) : 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// Создает кролика со случайными параметрами
        /// </summary>
        /// <returns>Сообщение о результате создания кролика</returns>
        public string AddRandomRabbit()
        {
            try
            {
                string[] names = { "Пушок", "Снежинка", "Игнат", "Ибрагим", "Ма-му-ма-ба", "Кастет" };
                string[] breeds = { "Беляк", "Русак", "Толай", "Маньжурский", "Оранжевый" };

                string name = names[_rnd.Next(names.Length)];
                int id = _rnd.Next(1, 1000);

                int count = 0;
                while (_repository.ReadById(id) != null && count < 1000)
                {
                    id = _rnd.Next(1, 1000);
                    count++;
                }

                var randomRabbit = new Rabbit
                {
                    Id = id,
                    Name = name,
                    Breed = breeds[_rnd.Next(breeds.Length)],
                    Age = _rnd.Next(1, 14),
                    Weight = _rnd.Next(1, 15)
                };

                _repository.Add(randomRabbit);
                return $"Рандомный кролик: {name} создан с id: {id}";
            }
            catch (Exception ex)
            {
                return $"Ошибка при создании рандомного кролика: {ex.Message}";
            }
        }

        /// <summary>
        /// Сортирует и отображает список кроликов по выбранному полю
        /// </summary>
        /// <param name="sortField">Поле для сортировки: 1-ID, 2-Имя, 3-Порода, 4-Возраст, 5-Вес</param>
        /// <param name="ascending">Направление сортировки: true-по возрастанию, false-по убыванию</param>
        public void SortRabbits(int sortField, bool ascending)
        {
            try
            {
                var rabbits = _repository.ReadAll().ToList();
                List<Rabbit> sortedRabbits;

                switch (sortField)
                {
                    case 1: sortedRabbits = ascending ? rabbits.OrderBy(r => r.Id).ToList() : rabbits.OrderByDescending(r => r.Id).ToList(); break;
                    case 2: sortedRabbits = ascending ? rabbits.OrderBy(r => r.Name).ToList() : rabbits.OrderByDescending(r => r.Name).ToList(); break;
                    case 3: sortedRabbits = ascending ? rabbits.OrderBy(r => r.Breed).ToList() : rabbits.OrderByDescending(r => r.Breed).ToList(); break;
                    case 4: sortedRabbits = ascending ? rabbits.OrderBy(r => r.Age).ToList() : rabbits.OrderByDescending(r => r.Age).ToList(); break;
                    case 5: sortedRabbits = ascending ? rabbits.OrderBy(r => r.Weight).ToList() : rabbits.OrderByDescending(r => r.Weight).ToList(); break;
                    default: sortedRabbits = rabbits; break;
                }

                Console.WriteLine("=== ОТСОРТИРОВАННЫЙ СПИСОК ===");
                foreach (var rabbit in sortedRabbits)
                {
                    Console.WriteLine($"ID: {rabbit.Id} | Имя: {rabbit.Name} | Порода: {rabbit.Breed} | Возраст: {rabbit.Age} | Вес: {rabbit.Weight}");
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Возвращает строковое представление всех кроликов в базе данных
        /// </summary>
        /// <returns>Форматированная строка со списком кроликов или сообщение об ошибке</returns>
        public string ShowAllRabbits()
        {
            try
            {
                var rabbits = _repository.ReadAll();

                if (rabbits == null || !rabbits.Any())
                    return "Список кроликов пуст";

                string result = "=== СПИСОК ВСЕХ КРОЛИКОВ ===\n";
                foreach (var rabbit in rabbits)
                {
                    result += $"ID: {rabbit.Id} | Имя: {rabbit.Name} | Порода: {rabbit.Breed} | Возраст: {rabbit.Age} | Вес: {rabbit.Weight}\n";
                }
                return result;
            }
            catch (Exception ex)
            {
                return $"Ошибка при получении списка кроликов: {ex.Message}";
            }
        }

        /// <summary>
        /// Возвращает массив доступных пород кроликов
        /// </summary>
        /// <returns>Массив строк с названиями пород</returns>
        public string[] GetBreeds()
        {
            return new string[] { "Беляк", "Русак", "Толай", "Маньжурский", "Оранжевый" };
        }
    }
}