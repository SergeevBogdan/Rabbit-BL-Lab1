using System;
using System.Collections.Generic;
using System.Linq;
using Rabbit.Entities;
using Rabbit.DataAccess;



namespace Business_logic___rabbit
{
    /// <summary>
    /// Основной класс бизнес-логики для управления кроликами
    /// Обеспечивает работу с данными через репозиторий
    /// Поддерживает Entity Framework и Dapper
    /// </summary>
    public class Logic
    {
        private IRepository<RabbitModel> _repository;  // ← RabbitModel вместо Rabbit
        private static Random _rnd = new Random();
        private readonly string _technology;

        public Logic(bool useEntityFramework = true)
        {
            if (useEntityFramework)
            {
                try
                {
                    var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
                    var context = new RabbitDbContext();
                    _repository = new EntityRepository<RabbitModel>(context);  // ← RabbitModel
                    _technology = "Entity Framework";
                }
                catch (Exception ex)
                {
                    _repository = new DapperRepository();  // ← RabbitModel
                    _technology = "Dapper (автопереключение)";
                }
            }
            else
            {
                _repository = new DapperRepository();  // ← RabbitModel
                _technology = "Dapper";
            }
        }

        public string GetCurrentTechnology() => _technology;

        public string AddRabbit(int id, string name, int age, int weight, string breed)
        {
            try
            {
                var existing = _repository.ReadById(id);
                if (existing != null) return "Такой id уже есть";

                var rabbit = new RabbitModel { Id = id, Name = name, Age = age, Weight = weight, Breed = breed };  // ← RabbitModel
                _repository.Add(rabbit);
                return "Кролик успешно добавлен";
            }
            catch (Exception ex)
            {
                return $"Ошибка при добавлении: {ex.Message}";
            }
        }

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

        public string AddRandomRabbit()
        {
            try
            {
                string[] names = { "Пушок", "Снежинка", "Игнат", "Ибрагим", "Ма-му-ма-ба", "Кастет" };
                string[] breeds = { "Беляк", "Русак", "Толай", "Маньжурский", "Оранжевый" };

                string name = names[_rnd.Next(names.Length)];
                int id = _rnd.Next(1, 9999);

                int count = 0;
                while (_repository.ReadById(id) != null && count < 9999)
                {
                    id = _rnd.Next(1, 9999);
                    count++;
                }

                var randomRabbit = new RabbitModel  // ← RabbitModel
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

        public void SortRabbits(int sortField, bool ascending)
        {
            try
            {
                var rabbits = _repository.ReadAll().ToList();
                List<RabbitModel> sortedRabbits;  // ← RabbitModel

                switch (sortField)
                {
                    case 1: sortedRabbits = ascending ? rabbits.OrderBy(r => r.Id).ToList() : rabbits.OrderByDescending(r => r.Id).ToList(); break;
                    case 2: sortedRabbits = ascending ? rabbits.OrderBy(r => r.Name).ToList() : rabbits.OrderByDescending(r => r.Name).ToList(); break;
                    case 3: sortedRabbits = ascending ? rabbits.OrderBy(r => r.Breed).ToList() : rabbits.OrderByDescending(r => r.Breed).ToList(); break;
                    case 4: sortedRabbits = ascending ? rabbits.OrderBy(r => r.Age).ToList() : rabbits.OrderByDescending(r => r.Age).ToList(); break;
                    case 5: sortedRabbits = ascending ? rabbits.OrderBy(r => r.Weight).ToList() : rabbits.OrderByDescending(r => r.Weight).ToList(); break;
                    default: sortedRabbits = rabbits; break;
                }

                Console.WriteLine("ОТСОРТИРОВАННЫЙ СПИСОК");
                foreach (var rabbit in sortedRabbits)
                {
                    Console.WriteLine($"ID: {rabbit.Id} | Имя: {rabbit.Name} | Порода: {rabbit.Breed} | Возраст: {rabbit.Age} | Вес: {rabbit.Weight}");
                }
            }
            catch (Exception ex)
            {
            }
        }

        public string ShowAllRabbits()
        {
            try
            {
                var rabbits = _repository.ReadAll();

                if (rabbits == null || !rabbits.Any())
                    return "Список кроликов пуст";

                string result = "СПИСОК ВСЕХ КРОЛИКОВ\n";
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

        public string[] GetBreeds()
        {
            return new string[] { "Беляк", "Русак", "Толай", "Маньжурский", "Оранжевый" };
        }
    }
}