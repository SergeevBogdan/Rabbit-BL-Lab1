using System;
using System.Collections.Generic;
using System.Linq;
using Business_logic___rabbit;
using Ninject.Modules;
using RabbitDAL;
using RabbitDAL.RabbitDAL;
using RabbitModel;
namespace Business_logic___rabbit
{
    //ВНИМАНИЕ ЧЕРНАЯ НЕЙРОНКА В СТУДИЮ
    //- Господин Друзь, как вы думаете, вы использовали нейронку?
    //- Думаю нет господин ведущий!
    //- А теперь,- внимание на экран!!!...


     
    public class RabbitAdder : IRabbitAdder
    {
        private readonly IReadRepository _readRepository;
        private readonly IWriteRepository _writeRepository;

        public RabbitAdder(IReadRepository readRepository, IWriteRepository writeRepository)
        {
            _readRepository = readRepository;
            _writeRepository = writeRepository;
        }

        public string AddRabbit(int id, string name, int age, int weight, string breed)
        {
            if (id <= 0) return "ID должен быть положительным числом";
            if (string.IsNullOrWhiteSpace(name)) return "Имя не может быть пустым";
            if (age <= 0) return "Возраст должен быть положительным числом";
            if (weight <= 0) return "Вес должен быть положительным числом";
            if (string.IsNullOrWhiteSpace(breed)) return "Порода не может быть пустой";

            var existing = _readRepository.ReadById(id);
            if (existing != null) return "такой id уже есть";

            var rabbit = new Rabbit { Id = id, Name = name, Age = age, Weight = weight, Breed = breed };
            _writeRepository.Add(rabbit);
            return "Кролик успешно добавлен";
        }
    }
    public class RabbitRemover : IRabbitRemover
    {
        private readonly IReadRepository _readRepository;
        private readonly IWriteRepository _writeRepository;

        public RabbitRemover(IReadRepository readRepository, IWriteRepository writeRepository)
        {
            _readRepository = readRepository;
            _writeRepository = writeRepository;
        }

        public string RemoveRabbit(int id)
        {
            if (id <= 0) return "ID должен быть положительным числом";

            var rabbit = _readRepository.ReadById(id);
            if (rabbit != null)
            {
                try
                {
                    _writeRepository.Delete(rabbit);
                    return "Кролик удален";
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException)
                {
                    // Если объект уже удален в другом месте
                    return "Кролик уже был удален";
                }
                catch (System.Data.Entity.Core.EntityCommandExecutionException)
                {
                    return "Ошибка доступа к базе данных";
                }
            }
            return "Кролик не найден";
        }
    }

    public class RabbitReader : IRabbitReader
    {
        private readonly IReadRepository _readRepository;

        public RabbitReader(IReadRepository readRepository)
        {
            _readRepository = readRepository;
        }

        public string ReadRabbit(int id)
        {
            if (id <= 0) return "ID должен быть положительным числом";

            var rabbit = _readRepository.ReadById(id);
            if (rabbit == null) return "Кролик с заданным Id не найден";

            return $"Имя: {rabbit.Name}\nВозраст: {rabbit.Age}\nВес: {rabbit.Weight}\nПорода: {rabbit.Breed}";
        }
    }

    public class RabbitUpdater : IRabbitUpdater
    {
        private readonly IReadRepository _readRepository;
        private readonly IWriteRepository _writeRepository;

        public RabbitUpdater(IReadRepository readRepository, IWriteRepository writeRepository)
        {
            _readRepository = readRepository;
            _writeRepository = writeRepository;
        }

        public void ChangeStatRabbit(int id, string name, int age, int weight, string breed)
        {
            if (id <= 0) return;
            if (string.IsNullOrWhiteSpace(name)) return;
            if (age <= 0) return;
            if (weight <= 0) return;
            if (string.IsNullOrWhiteSpace(breed)) return;

            var rabbit = _readRepository.ReadById(id);
            if (rabbit != null)
            {
                rabbit.Name = name;
                rabbit.Age = age;
                rabbit.Weight = weight;
                rabbit.Breed = breed;
                _writeRepository.Update(rabbit);
            }
        }
    }

    public class RabbitAgeCalculator : IRabbitAgeCalculator
    {
        private readonly IReadRepository _readRepository;

        public RabbitAgeCalculator(IReadRepository readRepository)
        {
            _readRepository = readRepository;
        }

        public double GetAverageAge()
        {
            var rabbits = _readRepository.ReadAll().ToList();
            return rabbits.Count != 0 ? rabbits.Average(r => r.Age) : 0;
        }
    }

    public class RabbitWeightCalculator : IRabbitWeightCalculator
    {
        private readonly IReadRepository _readRepository;

        public RabbitWeightCalculator(IReadRepository readRepository)
        {
            _readRepository = readRepository;
        }

        public double GetAverageWeight()
        {
            var rabbits = _readRepository.ReadAll().ToList();
            return rabbits.Count != 0 ? rabbits.Average(r => r.Weight) : 0;
        }
    }

    public class RabbitSorter : IRabbitSorter
    {
        private readonly IReadRepository _readRepository;

        public RabbitSorter(IReadRepository readRepository)
        {
            _readRepository = readRepository;
        }

        public void SortRabbits(int sortField, bool ascending)
        {
            var rabbits = _readRepository.ReadAll().ToList();
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
    }

    public class RabbitRandomCreator : IRabbitRandomCreator
    {
        private readonly IReadRepository _readRepository;
        private readonly IWriteRepository _writeRepository;
        private static Random _rnd = new Random();

        public RabbitRandomCreator(IReadRepository readRepository, IWriteRepository writeRepository)
        {
            _readRepository = readRepository;
            _writeRepository = writeRepository;
        }

        public string AddRandomRabbit()
        {
            string[] names = { "Пушок", "Снежинка", "Игнат", "Ибрагим", "Ма-му-ма-ба", "Кастет" };
            string[] breeds = { "Беляк", "Русак", "Толай", "Маньжурский", "Оранжевый" };

            string name = names[_rnd.Next(names.Length)];
            int id = _rnd.Next(1, 1000);

            int count = 0;
            while (_readRepository.ReadById(id) != null && count < 1000)
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

            _writeRepository.Add(randomRabbit);
            return $"Рандомный кролик: {name} создан с id: {id}";
        }
    }


    public class RabbitDisplayer : IRabbitDisplayer
    {
        private readonly IReadRepository _readRepository;

        public RabbitDisplayer(IReadRepository readRepository)
        {
            _readRepository = readRepository;
        }

        public string ShowAllRabbits()
        {
            var rabbits = _readRepository.ReadAll();

            if (rabbits == null || !rabbits.Any())
                return "Список кроликов пуст";

            string result = "=== СПИСОК ВСЕХ КРОЛИКОВ ===\n";
            foreach (var rabbit in rabbits)
            {
                result += $"ID: {rabbit.Id} | Имя: {rabbit.Name} | Порода: {rabbit.Breed} | Возраст: {rabbit.Age} | Вес: {rabbit.Weight}\n";
            }
            return result;
        }
    }


    public class RabbitBreedProvider : IRabbitBreedProvider
    {
        public string[] GetBreeds()
        {
            return new string[] { "Беляк", "Русак", "Толай", "Маньжурский", "Оранжевый" };
        }
    }






}