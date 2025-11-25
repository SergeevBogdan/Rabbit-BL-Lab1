using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitDAL;
using RabbitModel;

namespace BusinessLogicMVP
{
    public class LogicMVP : ILogicMVP
    {
        private readonly IRepository _repository;
        private readonly IRabbitAdderMVP _adder;
        private readonly IRabbitRemoverMVP _remover;
        private readonly IRabbitReaderMVP _reader;
        private readonly IRabbitUpdaterMVP _updater;
        private readonly IRabbitAgeCalculatorMVP _ageCalculator;
        private readonly IRabbitWeightCalculatorMVP _weightCalculator;
        private readonly IRabbitSorterMVP _sorter;
        private readonly IRabbitRandomCreatorMVP _randomCreator;
        private readonly IRabbitDisplayerMVP _displayer;
        private readonly IRabbitBreedProviderMVP _breedProvider;
        private readonly IRabbitProviderMVP _provider;
        private readonly string _technology;

        public LogicMVP(
            IRepository repository,
            IRabbitAdderMVP adder,
            IRabbitRemoverMVP remover,
            IRabbitReaderMVP reader,
            IRabbitUpdaterMVP updater,
            IRabbitAgeCalculatorMVP ageCalculator,
            IRabbitWeightCalculatorMVP weightCalculator,
            IRabbitSorterMVP sorter,
            IRabbitRandomCreatorMVP randomCreator,
            IRabbitDisplayerMVP displayer,
            IRabbitBreedProviderMVP breedProvider,
            IRabbitProviderMVP provider)
        {
            _repository = repository;
            _adder = adder;
            _remover = remover;
            _reader = reader;
            _updater = updater;
            _ageCalculator = ageCalculator;
            _weightCalculator = weightCalculator;
            _sorter = sorter;
            _randomCreator = randomCreator;
            _displayer = displayer;
            _breedProvider = breedProvider;
            _provider = provider;
            _technology = repository.GetType().Name.Contains("Entity") ? "Entity Framework" : "Dapper";
        }

        public string GetCurrentTechnology() => _technology;

        public string AddRabbit(int id, string name, int age, int weight, string breed)
        {
            return _adder.AddRabbit(id, name, age, weight, breed);
        }

        public string RemoveRabbit(int id)
        {
            return _remover.RemoveRabbit(id);
        }

        public string ReadRabbit(int id)
        {
            return _reader.ReadRabbit(id);
        }

        public void ChangeStatRabbit(int id, string name, int age, int weight, string breed)
        {
            _updater.ChangeStatRabbit(id, name, age, weight, breed);
        }

        public double GetAverageAge()
        {
            return _ageCalculator.GetAverageAge();
        }

        public double GetAverageWeight()
        {
            return _weightCalculator.GetAverageWeight();
        }

        public void SortRabbits(int sortField, bool ascending)
        {
            _sorter.SortRabbits(sortField, ascending);
        }

        public string AddRandomRabbit()
        {
            return _randomCreator.AddRandomRabbit();
        }

        public string ShowAllRabbits()
        {
            return _displayer.ShowAllRabbits();
        }

        public string[] GetBreeds()
        {
            return _breedProvider.GetBreeds();
        }

        public Rabbit GetRabbitById(int id)
        {
            return _provider.GetRabbitById(id);
        }

        public List<Rabbit> GetAllRabbits()
        {
            return _provider.GetAllRabbits();
        }
    }

    public class RabbitProviderMVP : IRabbitProviderMVP
    {
        private readonly IRepository _repository;

        public RabbitProviderMVP(IRepository repository)
        {
            _repository = repository;
        }

        public Rabbit GetRabbitById(int id)
        {
            if (id <= 0) return null;
            return _repository.ReadById(id);
        }

        public List<Rabbit> GetAllRabbits()
        {
            var rabbits = _repository.ReadAll();
            return rabbits?.ToList() ?? new List<Rabbit>();
        }
    }

    public class RabbitAdderMVP : IRabbitAdderMVP
    {
        private readonly IRepository _repository;

        public RabbitAdderMVP(IRepository repository) => _repository = repository;

        public string AddRabbit(int id, string name, int age, int weight, string breed)
        {
            if (id <= 0) return "ID должен быть положительным числом";
            if (string.IsNullOrWhiteSpace(name)) return "Имя не может быть пустым";
            if (age <= 0) return "Возраст должен быть положительным числом";
            if (weight <= 0) return "Вес должен быть положительным числом";
            if (string.IsNullOrWhiteSpace(breed)) return "Порода не может быть пустой";

            var existing = _repository.ReadById(id);
            if (existing != null) return "такой id уже есть";

            var rabbit = new Rabbit { Id = id, Name = name, Age = age, Weight = weight, Breed = breed };
            _repository.Add(rabbit);
            return "Кролик успешно добавлен";
        }
    }

    public class RabbitRemoverMVP : IRabbitRemoverMVP
    {
        private readonly IRepository _repository;

        public RabbitRemoverMVP(IRepository repository) => _repository = repository;

        public string RemoveRabbit(int id)
        {
            if (id <= 0) return "ID должен быть положительным числом";

            var rabbit = _repository.ReadById(id);
            if (rabbit != null)
            {
                try
                {
                    _repository.Delete(rabbit);
                    return "Кролик удален";
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("удален") || ex.Message.Contains("concurrency"))
                        return "Кролик уже был удален";
                    else if (ex.Message.Contains("баз") || ex.Message.Contains("database"))
                        return "Ошибка доступа к базе данных";
                    else
                        return $"Ошибка при удалении: {ex.Message}";
                }
            }
            return "Кролик не найден";
        }
    }

    public class RabbitReaderMVP : IRabbitReaderMVP
    {
        private readonly IRepository _repository;

        public RabbitReaderMVP(IRepository repository) => _repository = repository;

        public string ReadRabbit(int id)
        {
            if (id <= 0) return "ID должен быть положительным числом";

            var rabbit = _repository.ReadById(id);
            if (rabbit == null) return "Кролик с заданным Id не найден";

            return $"Имя: {rabbit.Name}\nВозраст: {rabbit.Age}\nВес: {rabbit.Weight}\nПорода: {rabbit.Breed}";
        }
    }

    public class RabbitUpdaterMVP : IRabbitUpdaterMVP
    {
        private readonly IRepository _repository;

        public RabbitUpdaterMVP(IRepository repository) => _repository = repository;

        public void ChangeStatRabbit(int id, string name, int age, int weight, string breed)
        {
            if (id <= 0) return;
            if (string.IsNullOrWhiteSpace(name)) return;
            if (age <= 0) return;
            if (weight <= 0) return;
            if (string.IsNullOrWhiteSpace(breed)) return;

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
    }

    public class RabbitAgeCalculatorMVP : IRabbitAgeCalculatorMVP
    {
        private readonly IRepository _repository;

        public RabbitAgeCalculatorMVP(IRepository repository) => _repository = repository;

        public double GetAverageAge()
        {
            var rabbits = _repository.ReadAll().ToList();
            return rabbits.Count != 0 ? rabbits.Average(r => r.Age) : 0;
        }
    }

    public class RabbitWeightCalculatorMVP : IRabbitWeightCalculatorMVP
    {
        private readonly IRepository _repository;

        public RabbitWeightCalculatorMVP(IRepository repository) => _repository = repository;

        public double GetAverageWeight()
        {
            var rabbits = _repository.ReadAll().ToList();
            return rabbits.Count != 0 ? rabbits.Average(r => r.Weight) : 0;
        }
    }

    public class RabbitSorterMVP : IRabbitSorterMVP
    {
        private readonly IRepository _repository;

        public RabbitSorterMVP(IRepository repository) => _repository = repository;

        public void SortRabbits(int sortField, bool ascending)
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
        }
    }

    public class RabbitRandomCreatorMVP : IRabbitRandomCreatorMVP
    {
        private readonly IRepository _repository;
        private static Random _rnd = new Random();

        public RabbitRandomCreatorMVP(IRepository repository) => _repository = repository;

        public string AddRandomRabbit()
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
    }

    public class RabbitDisplayerMVP : IRabbitDisplayerMVP
    {
        private readonly IRepository _repository;

        public RabbitDisplayerMVP(IRepository repository) => _repository = repository;

        public string ShowAllRabbits()
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
    }

    public class RabbitBreedProviderMVP : IRabbitBreedProviderMVP
    {
        public string[] GetBreeds()
        {
            return new string[] { "Беляк", "Русак", "Толай", "Маньжурский", "Оранжевый" };
        }
    }
}
