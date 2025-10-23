using System;
using System.Collections.Generic;
using System.Linq;
using Business_logic___rabbit;
using Ninject.Modules;
using RabbitDAL;
using RabbitModel;
namespace Business_logic___rabbit
{
    //ВНИМАНИЕ ЧЕРНАЯ НЕЙРОНКА В СТУДИЮ
    //- Господин Друзь, как вы думаете, вы использовали нейронку?
    //- Думаю нет господин ведущий!
    //- А теперь,- внимание на экран!!!...


    /// <summary>
    /// Основной класс бизнес-логики для управления кроликами
    /// Внутренняя реализация соответствует SOLID принципам
    /// Внешний интерфейс остается неизменным для обратной совместимости
    /// </summary>
    public class Logic
    {
        private readonly IRepository _repository;
        private static Random _rnd = new Random();
        private readonly string _technology;

        /// <summary>
        /// Конструктор для обратной совместимости
        /// Внутри использует SOLID-архитектуру с DI
        /// </summary>
        public Logic(bool useEntityFramework = true)
        {
            if (useEntityFramework)
            {
                try
                {
                    var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
                    var context = new RabbitDbContext();
                    _repository = new EntityRepository(context);
                    _technology = "Entity Framework";
                }
                catch (Exception ex)
                {
                    _repository = new DapperRepository();
                    _technology = "Dapper (автопереключение)";
                }
            }
            else
            {
                _repository = new DapperRepository();
                _technology = "Dapper";
            }
        }

        /// <summary>
        /// Конструктор с внедрением зависимости для тестирования
        /// </summary>
        internal Logic(IRepository repository)
        {
            _repository = repository;
            _technology = "Test Repository";
        }

        public string GetCurrentTechnology() => _technology;

        public string AddRabbit(int id, string name, int age, int weight, string breed)
        {
            var adder = new RabbitAdder(_repository);
            return adder.AddRabbit(id, name, age, weight, breed);
        }

        public string RemoveRabbit(int id)
        {
            var remover = new RabbitRemover(_repository);
            return remover.RemoveRabbit(id);
        }

        public string ReadRabbit(int id)
        {
            var reader = new RabbitReader(_repository);
            return reader.ReadRabbit(id);
        }

        public void ChangeStatRabbit(int id, string name, int age, int weight, string breed)
        {
            var updater = new RabbitUpdater(_repository);
            updater.ChangeStatRabbit(id, name, age, weight, breed);
        }

        public double GetAverageAge()
        {
            var calculator = new RabbitAgeCalculator(_repository);
            return calculator.GetAverageAge();
        }

        public double GetAverageWeight()
        {
            var calculator = new RabbitWeightCalculator(_repository);
            return calculator.GetAverageWeight();
        }

        public void SortRabbits(int sortField, bool ascending)
        {
            var sorter = new RabbitSorter(_repository);
            sorter.SortRabbits(sortField, ascending);
        }

        public string AddRandomRabbit()
        {
            var creator = new RabbitRandomCreator(_repository);
            return creator.AddRandomRabbit();
        }

        public string ShowAllRabbits()
        {
            var displayer = new RabbitDisplayer(_repository);
            return displayer.ShowAllRabbits();
        }

        public string[] GetBreeds()
        {
            var provider = new RabbitBreedProvider();
            return provider.GetBreeds();
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






}