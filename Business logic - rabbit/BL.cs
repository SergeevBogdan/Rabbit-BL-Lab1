using System;
using System.Collections.Generic;
using System.Linq;
using Business_logic___rabbit;
using Ninject.Modules;
using RabbitDAL;
using RabbitModel;
namespace Business_logic___rabbit
{

    /// <summary>
    /// Основной класс бизнес-логики для управления кроликами
    /// Полностью соответствует SOLID принципам и DI
    /// </summary>
    public class Logic
    {
        private readonly IRepository _repository;
        private readonly IRabbitAdder _adder;
        private readonly IRabbitRemover _remover;
        private readonly IRabbitReader _reader;
        private readonly IRabbitUpdater _updater;
        private readonly IRabbitAgeCalculator _ageCalculator;
        private readonly IRabbitWeightCalculator _weightCalculator;
        private readonly IRabbitSorter _sorter;
        private readonly IRabbitRandomCreator _randomCreator;
        private readonly IRabbitDisplayer _displayer;
        private readonly IRabbitBreedProvider _breedProvider;
        private readonly string _technology;

        /// <summary>
        /// Основной конструктор для DI - получает ВСЕ зависимости через Ninject
        /// </summary>
        public Logic(
            IRepository repository,
            IRabbitAdder adder,
            IRabbitRemover remover,
            IRabbitReader reader,
            IRabbitUpdater updater,
            IRabbitAgeCalculator ageCalculator,
            IRabbitWeightCalculator weightCalculator,
            IRabbitSorter sorter,
            IRabbitRandomCreator randomCreator,
            IRabbitDisplayer displayer,
            IRabbitBreedProvider breedProvider)
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

            _technology = repository is EntityRepository ? "Entity Framework" :
                         repository is DapperRepository ? "Dapper" : "Test Repository";
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
    }

    /// <summary>
    /// Сервис для добавления кроликов в базу данных
    /// Отвечает за валидацию и создание новых записей
    /// </summary>
    public class RabbitAdder : IRabbitAdder
    {
        private readonly IRepository _repository;

        public RabbitAdder(IRepository repository)
        {
            _repository = repository;
        }

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

    /// <summary>
    /// Сервис для удаления кроликов из базы данных
    /// Обрабатывает проверки и исключения при удалении
    /// </summary>
    public class RabbitRemover : IRabbitRemover
    {
        private readonly IRepository _repository;

        public RabbitRemover(IRepository repository)
        {
            _repository = repository;
        }

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
                catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException)
                {
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

    /// <summary>
    /// Сервис для чтения информации о кроликах
    /// Предоставляет детальную информацию по идентификатору
    /// </summary>
    public class RabbitReader : IRabbitReader
    {
        private readonly IRepository _repository;

        public RabbitReader(IRepository repository)
        {
            _repository = repository;
        }

        public string ReadRabbit(int id)
        {
            if (id <= 0) return "ID должен быть положительным числом";

            var rabbit = _repository.ReadById(id);
            if (rabbit == null) return "Кролик с заданным Id не найден";

            return $"Имя: {rabbit.Name}\nВозраст: {rabbit.Age}\nВес: {rabbit.Weight}\nПорода: {rabbit.Breed}";
        }
    }

    /// <summary>
    /// Сервис для обновления данных кроликов
    /// Обеспечивает корректное изменение существующих записей
    /// </summary>
    public class RabbitUpdater : IRabbitUpdater
    {
        private readonly IRepository _repository;

        public RabbitUpdater(IRepository repository)
        {
            _repository = repository;
        }

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

    /// <summary>
    /// Сервис для вычисления среднего возраста кроликов
    /// Выполняет статистические расчеты
    /// </summary>
    public class RabbitAgeCalculator : IRabbitAgeCalculator
    {
        private readonly IRepository _repository;

        public RabbitAgeCalculator(IRepository repository)
        {
            _repository = repository;
        }

        public double GetAverageAge()
        {
            var rabbits = _repository.ReadAll().ToList();
            return rabbits.Count != 0 ? rabbits.Average(r => r.Age) : 0;
        }
    }

<<<<<<< HEAD
=======

>>>>>>> хуйня
    /// <summary>
    /// Сервис для вычисления среднего веса кроликов
    /// Выполняет статистические расчеты
    /// </summary>
    public class RabbitWeightCalculator : IRabbitWeightCalculator
    {
        private readonly IRepository _repository;

        public RabbitWeightCalculator(IRepository repository)
        {
            _repository = repository;
        }

        public double GetAverageWeight()
        {
            var rabbits = _repository.ReadAll().ToList();
            return rabbits.Count != 0 ? rabbits.Average(r => r.Weight) : 0;
        }
    }

    /// <summary>
    /// Сервис для сортировки кроликов по различным полям
    /// Поддерживает сортировку по возрастанию и убыванию
    /// </summary>
    public class RabbitSorter : IRabbitSorter
    {
        private readonly IRepository _repository;

        public RabbitSorter(IRepository repository)
        {
            _repository = repository;
        }

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

            Console.WriteLine("=== ОТСОРТИРОВАННЫЙ СПИСОК ===");
            foreach (var rabbit in sortedRabbits)
            {
                Console.WriteLine($"ID: {rabbit.Id} | Имя: {rabbit.Name} | Порода: {rabbit.Breed} | Возраст: {rabbit.Age} | Вес: {rabbit.Weight}");
            }
        }
    }

    /// <summary>
    /// Сервис для создания кроликов со случайными параметрами
    /// Генерирует уникальные данные и обеспечивает их корректность
    /// </summary>
    public class RabbitRandomCreator : IRabbitRandomCreator
    {
        private readonly IRepository _repository;
        private static Random _rnd = new Random();

        public RabbitRandomCreator(IRepository repository)
        {
            _repository = repository;
        }

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

    /// <summary>
    /// Сервис для отображения всех кроликов
    /// Форматирует данные для удобного представления
    /// </summary>
    public class RabbitDisplayer : IRabbitDisplayer
    {
        private readonly IRepository _repository;

        public RabbitDisplayer(IRepository repository)
        {
            _repository = repository;
        }

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

    /// <summary>
    /// Сервис для предоставления списка доступных пород кроликов
    /// Содержит фиксированный набор пород
    /// </summary>
    public class RabbitBreedProvider : IRabbitBreedProvider
    {
        public string[] GetBreeds()
        {
            return new string[] { "Беляк", "Русак", "Толай", "Маньжурский", "Оранжевый" };
        }
    }
<<<<<<< HEAD


=======
>>>>>>> хуйня
}