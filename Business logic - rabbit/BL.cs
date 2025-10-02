using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Business_logic___rabbit
{
    //Я еблан 
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Logic
    {
        private IRepository<Rabbit> _repository;
        private static Random _rnd = new Random();
        private readonly string _technology;

        public Logic(bool useEntityFramework = true)
        {
            if (useEntityFramework)
            {
                try
                {
                    Console.WriteLine("🔄 Инициализация Entity Framework...");
                    var context = new RabbitDbContext();
                    _repository = new EntityRepository<Rabbit>(context);
                    _technology = "Entity Framework";
                    Console.WriteLine("✅ Entity Framework готов");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ EF не работает: {ex.Message}");
                    Console.WriteLine("🔄 Переключаемся на Dapper...");
                    _repository = new DapperRepository<Rabbit>();
                    _technology = "Dapper";
                }
            }
            else
            {
                _repository = new DapperRepository<Rabbit>();
                _technology = "Dapper";
                Console.WriteLine("✅ Dapper готов");
            }

            Console.WriteLine($"📊 Используется: {_technology}");
        }

        public string GetCurrentTechnology()
        {
            return _technology;
        }

        public string AddRabbit(int id, string name, int age, int weight, string breed)
        {
            var existing = _repository.ReadById(id);
            if (existing != null)
                return "такой id уже есть";

            var rabbit = new Rabbit { Id = id, Name = name, Age = age, Weight = weight, Breed = breed };
            _repository.Add(rabbit);
            return "Кролик успешно добавлен";
        }

        public string RemoveRabbit(int id)
        {
            var rabbit = _repository.ReadById(id);
            if (rabbit != null)
            {
                _repository.Delete(rabbit);
                return "Кролик удален";
            }
            return "Кролик не найден";
        }

        public string ReadRabbit(int id)
        {
            var rabbit = _repository.ReadById(id);
            if (rabbit == null)
                return "Кролик с заданным Id не найден";

            return $"Имя: {rabbit.Name}\nВозраст: {rabbit.Age}\nВес: {rabbit.Weight}\nПорода: {rabbit.Breed}";
        }

        public void ChangeStatRabbit(int id, string name, int age, int weight, string breed)
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

        public double GetAverageAge()
        {
            var rabbits = _repository.ReadAll().ToList();
            // ИСПРАВЛЕНО: Count() с круглыми скобками
            if (rabbits.Count() != 0)
            {
                return rabbits.Average(r => r.Age);
            }
            return 0;
        }

        public double GetAverageWeight()
        {
            var rabbits = _repository.ReadAll().ToList();
            // ИСПРАВЛЕНО: Count() с круглыми скобками
            if (rabbits.Count() != 0)
            {
                return rabbits.Average(r => r.Weight);
            }
            return 0;
        }

        public string AddRandomRabbit()
        {
            string[] names = { "Пушок", "Снежинка", "Игнат", "Ибрагим", "Ма-му-ма-ба", "Кастет", "Энцегорловье", "Доминико дэ-ко-ко", "Эй-эй-эй", "La lepre che salta in alto" };
            string[] breeds = { "Беляк", "Русак", "Толай", "Маньжурский", "Оранжевый" };

            string name = names[_rnd.Next(names.Length)];
            int id = _rnd.Next(1, 1000);

            // Проверяем уникальность ID
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

        public void SortRabbits(int sortField, bool ascending)
        {
            var rabbits = _repository.ReadAll().ToList();

            switch (sortField)
            {
                case 1:
                    rabbits = ascending ? rabbits.OrderBy(r => r.Id).ToList() : rabbits.OrderByDescending(r => r.Id).ToList();
                    break;
                case 2:
                    rabbits = ascending ? rabbits.OrderBy(r => r.Name).ToList() : rabbits.OrderByDescending(r => r.Name).ToList();
                    break;
                case 3:
                    rabbits = ascending ? rabbits.OrderBy(r => r.Breed).ToList() : rabbits.OrderByDescending(r => r.Breed).ToList();
                    break;
                case 4:
                    rabbits = ascending ? rabbits.OrderBy(r => r.Age).ToList() : rabbits.OrderByDescending(r => r.Age).ToList();
                    break;
                case 5:
                    rabbits = ascending ? rabbits.OrderBy(r => r.Weight).ToList() : rabbits.OrderByDescending(r => r.Weight).ToList();
                    break;
                default:
                    break;
            }

            // Перезаписываем данные
            var allRabbits = _repository.ReadAll().ToList();
            foreach (var rabbit in allRabbits)
                _repository.Delete(rabbit);

            foreach (var rabbit in rabbits)
                _repository.Add(rabbit);
        }

        public string ShowAllRabbits()
        {
            try
            {
                var rabbits = _repository.ReadAll();

                // Проверяем, есть ли кролики
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
                return $"❌ Ошибка при получении списка кроликов: {ex.Message}";
            }
        }
    }
}
