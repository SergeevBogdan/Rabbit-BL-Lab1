using System;
using System.Collections.Generic;
using System.Linq; // 🔥 ДОЛЖНА БЫТЬ
using System.Text;
using System.Threading.Tasks;

namespace Business_logic___rabbit
{
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
                    var provider = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
                    var context = new RabbitDbContext();
                    _repository = new EntityRepository<Rabbit>(context);
                    _technology = "Entity Framework";
                    Console.WriteLine("✅ Entity Framework готов");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Entity Framework ошибка: {ex.Message}");
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
        }

        public string GetCurrentTechnology() => _technology;

        public string AddRabbit(int id, string name, int age, int weight, string breed)
        {
            try
            {
                var existing = _repository.ReadById(id);
                if (existing != null) return "такой id уже есть";

                var rabbit = new Rabbit { Id = id, Name = name, Age = age, Weight = weight, Breed = breed };

                Console.WriteLine($"🔍 Добавляем кролика: ID={id}, Name={name}, Breed={breed}, Age={age}, Weight={weight}");

                _repository.Add(rabbit);
                return "Кролик успешно добавлен";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Детали ошибки добавления: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"🔍 Внутренняя ошибка: {ex.InnerException.Message}");
                    if (ex.InnerException.InnerException != null)
                    {
                        Console.WriteLine($"🔍 Детали SQL: {ex.InnerException.InnerException.Message}");
                    }
                }
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
            catch (Exception ex) { Console.WriteLine($"Ошибка при изменении: {ex.Message}"); }
        }

        public double GetAverageAge()
        {
            try
            {
                var rabbits = _repository.ReadAll().ToList();
                if (rabbits.Count != 0)
                {
                    return rabbits.Average(r => r.Age);
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при расчете среднего возраста: {ex.Message}");
                return 0;
            }
        }

        public double GetAverageWeight()
        {
            try
            {
                var rabbits = _repository.ReadAll().ToList();
                if (rabbits.Count != 0)
                {
                    return rabbits.Average(r => r.Weight);
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при расчете среднего веса: {ex.Message}");
                return 0;
            }
        }

        public string AddRandomRabbit()
        {
            try
            {
                string[] names = { "Пушок", "Снежинка", "Игнат", "Ибрагим", "Ма-му-ма-ба", "Кастет", "Энцегорловье", "Доминико дэ-ко-ко", "Эй-эй-эй", "La lepre che salta in alto" };
                string[] breeds = { "Беляк", "Русак", "Толай", "Маньжурский", "Оранжевый" };

                string name = names[_rnd.Next(names.Length)];
                int id = _rnd.Next(1, 1000);

                int count = 0;
                while (_repository.ReadById(id) != null && count < 1000)
                {
                    id = _rnd.Next(1, 1000);
                    count++;
                }

                Console.WriteLine($"🔍 Создаем рандомного кролика: ID={id}, Name={name}");

                var randomRabbit = new Rabbit
                {
                    Id = id,
                    Name = name,
                    Breed = breeds[_rnd.Next(breeds.Length)],
                    Age = _rnd.Next(1, 14),
                    Weight = _rnd.Next(1, 15)
                };

                Console.WriteLine($"🔍 Параметры: Breed={randomRabbit.Breed}, Age={randomRabbit.Age}, Weight={randomRabbit.Weight}");

                _repository.Add(randomRabbit);
                return $"Рандомный кролик: {name} создан с id: {id}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Детали ошибки рандомного кролика: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"🔍 Внутренняя ошибка: {ex.InnerException.Message}");
                    if (ex.InnerException.InnerException != null)
                    {
                        Console.WriteLine($"🔍 Детали SQL: {ex.InnerException.InnerException.Message}");
                    }
                }
                return $"Ошибка при создании рандомного кролика: {ex.Message}";
            }
        }

        public void SortRabbits(int sortField, bool ascending)
        {
            try
            {
                var rabbits = _repository.ReadAll().ToList();

                List<Rabbit> sortedRabbits;

                switch (sortField)
                {
                    case 1:
                        sortedRabbits = ascending ?
                            rabbits.OrderBy(r => r.Id).ToList() :
                            rabbits.OrderByDescending(r => r.Id).ToList();
                        break;
                    case 2:
                        sortedRabbits = ascending ?
                            rabbits.OrderBy(r => r.Name).ToList() :
                            rabbits.OrderByDescending(r => r.Name).ToList();
                        break;
                    case 3:
                        sortedRabbits = ascending ?
                            rabbits.OrderBy(r => r.Breed).ToList() :
                            rabbits.OrderByDescending(r => r.Breed).ToList();
                        break;
                    case 4:
                        sortedRabbits = ascending ?
                            rabbits.OrderBy(r => r.Age).ToList() :
                            rabbits.OrderByDescending(r => r.Age).ToList();
                        break;
                    case 5:
                        sortedRabbits = ascending ?
                            rabbits.OrderBy(r => r.Weight).ToList() :
                            rabbits.OrderByDescending(r => r.Weight).ToList();
                        break;
                    default:
                        sortedRabbits = rabbits;
                        break;
                }

                Console.WriteLine("=== ОТСОРТИРОВАННЫЙ СПИСОК ===");
                foreach (var rabbit in sortedRabbits)
                {
                    Console.WriteLine($"ID: {rabbit.Id} | Имя: {rabbit.Name} | Порода: {rabbit.Breed} | Возраст: {rabbit.Age} | Вес: {rabbit.Weight}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сортировке: {ex.Message}");
            }
        }

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
                return $"❌ Ошибка при получении списка кроликов: {ex.Message}";
            }
        }

        public string[] GetBreeds()
        {
            return new string[] { "Беляк", "Русак", "Толай", "Маньжурский", "Оранжевый" };
        }
    }
}