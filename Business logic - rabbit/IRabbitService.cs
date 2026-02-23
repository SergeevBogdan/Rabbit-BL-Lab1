using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
namespace Business_logic___rabbit
{
    /// <summary>
    /// Определяет операции для добавления кроликов в систему
    /// Инкапсулирует бизнес-правила валидации при создании
    /// </summary>
    public interface IRabbitAdder
    {
        /// <summary>
        /// Добавляет нового кролика с проверкой входных параметров
        /// </summary>
        /// <param name="id">Уникальный идентификатор кролика</param>
        /// <param name="name">Имя кролика</param>
        /// <param name="age">Возраст кролика в годах</param>
        /// <param name="weight">Вес кролика в кг</param>
        /// <param name="breed">Порода кролика</param>
        /// <returns>Сообщение о результате операции</returns>
        string AddRabbit(int id, string name, int age, int weight, string breed);
    }

    /// <summary>
    /// Определяет операции для удаления кроликов из системы
    /// Обрабатывает сценарии отсутствия кролика и ошибки доступа к данным
    /// </summary>
    public interface IRabbitRemover
    {
        /// <summary>
        /// Удаляет кролика по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор кролика для удаления</param>
        /// <returns>Сообщение о результате операции</returns>
        string RemoveRabbit(int id);
    }

    /// <summary>
    /// Определяет операции для чтения информации о кроликах
    /// Предоставляет детализированную информацию по запросу
    /// </summary>
    public interface IRabbitReader
    {
        /// <summary>
        /// Возвращает подробную информацию о кролике
        /// </summary>
        /// <param name="id">Идентификатор кролика</param>
        /// <returns>Форматированная строка с данными кролика или сообщение об ошибке</returns>
        string ReadRabbit(int id);
    }

    /// <summary>
    /// Определяет операции для обновления данных кроликов
    /// Обеспечивает целостность данных при изменении
    /// </summary>
    public interface IRabbitUpdater
    {
        /// <summary>
        /// Обновляет данные существующего кролика
        /// </summary>
        /// <param name="id">Идентификатор кролика</param>
        /// <param name="name">Новое имя кролика</param>
        /// <param name="age">Новый возраст кролика</param>
        /// <param name="weight">Новый вес кролика</param>
        /// <param name="breed">Новая порода кролика</param>
        void ChangeStatRabbit(int id, string name, int age, int weight, string breed);
    }

    /// <summary>
    /// Определяет операции для статистических расчетов возраста
    /// Выполняет агрегацию данных по возрасту кроликов
    /// </summary>
    public interface IRabbitAgeCalculator
    {
        /// <summary>
        /// Вычисляет средний возраст всех кроликов в системе
        /// </summary>
        /// <returns>Средний возраст в годах</returns>
        double GetAverageAge();
    }

    /// <summary>
    /// Определяет операции для статистических расчетов веса
    /// Выполняет агрегацию данных по весу кроликов
    /// </summary>
    public interface IRabbitWeightCalculator
    {
        /// <summary>
        /// Вычисляет средний вес всех кроликов в системе
        /// </summary>
        /// <returns>Средний вес в кг</returns>
        double GetAverageWeight();
    }

    /// <summary>
    /// Определяет операции для сортировки кроликов
    /// Поддерживает множественные критерии сортировки
    /// </summary>
    public interface IRabbitSorter
    {
        /// <summary>
        /// Сортирует кроликов по указанному полю и направлению
        /// </summary>
        /// <param name="sortField">Поле для сортировки (1-Id, 2-Имя, 3-Порода, 4-Возраст, 5-Вес)</param>
        /// <param name="ascending">Направление сортировки (true - по возрастанию, false - по убыванию)</param>
        void SortRabbits(int sortField, bool ascending);
    }

    /// <summary>
    /// Определяет операции для генерации случайных кроликов
    /// Автоматически создает уникальные данные
    /// </summary>
    public interface IRabbitRandomCreator
    {
        /// <summary>
        /// Создает кролика со случайными параметрами
        /// </summary>
        /// <returns>Сообщение о созданном кролике</returns>
        string AddRandomRabbit();
    }

    /// <summary>
    /// Определяет операции для отображения кроликов
    /// Форматирует данные для удобного представления
    /// </summary>
    public interface IRabbitDisplayer
    {
        /// <summary>
        /// Возвращает форматированный список всех кроликов
        /// </summary>
        /// <returns>Строка с отформатированным списком</returns>
        string ShowAllRabbits();
    }

    /// <summary>
    /// Определяет операции для работы с породами кроликов
    /// Предоставляет справочную информацию о доступных породах
    /// </summary>
    public interface IRabbitBreedProvider
    {
        /// <summary>
        /// Возвращает массив доступных пород кроликов
        /// </summary>
        /// <returns>Массив строк с названиями пород</returns>
        string[] GetBreeds();
    }
    /// <summary>
    /// Определяет контракт основного класса бизнес-логики для управления кроликами
    /// Предоставляет абстракцию над всеми операциями системы
    /// Обеспечивает возможность подмены реализации бизнес-логики
    /// </summary>

    public interface ILogic
    {
        /// <summary>
        /// Возвращает название используемой технологии доступа к данным
        /// </summary>
        /// <returns>Название технологии ("Entity Framework" или "Dapper")</returns>
        string GetCurrentTechnology();

        /// <summary>
        /// Добавляет нового кролика в систему с проверкой бизнес-правил
        /// </summary>
        /// <param name="id">Уникальный идентификатор кролика</param>
        /// <param name="name">Имя кролика</param>
        /// <param name="age">Возраст кролика в годах</param>
        /// <param name="weight">Вес кролика в кг</param>
        /// <param name="breed">Порода кролика</param>
        /// <returns>Сообщение о результате операции</returns>
        string AddRabbit(int id, string name, int age, int weight, string breed);

        /// <summary>
        /// Удаляет кролика из системы по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор кролика для удаления</param>
        /// <returns>Сообщение о результате операции</returns>
        string RemoveRabbit(int id);

        /// <summary>
        /// Возвращает подробную информацию о кролике
        /// </summary>
        /// <param name="id">Идентификатор кролика</param>
        /// <returns>Форматированная строка с данными кролика или сообщение об ошибке</returns>
        string ReadRabbit(int id);

        /// <summary>
        /// Обновляет данные существующего кролика
        /// </summary>
        /// <param name="id">Идентификатор кролика</param>
        /// <param name="name">Новое имя кролика</param>
        /// <param name="age">Новый возраст кролика</param>
        /// <param name="weight">Новый вес кролика</param>
        /// <param name="breed">Новая порода кролика</param>
        void ChangeStatRabbit(int id, string name, int age, int weight, string breed);

        /// <summary>
        /// Вычисляет средний возраст всех кроликов в системе
        /// </summary>
        /// <returns>Средний возраст в годах</returns>
        double GetAverageAge();

        /// <summary>
        /// Вычисляет средний вес всех кроликов в системе
        /// </summary>
        /// <returns>Средний вес в кг</returns>
        double GetAverageWeight();

        /// <summary>
        /// Сортирует кроликов по указанному полю и направлению
        /// </summary>
        /// <param name="sortField">Поле для сортировки (1-Id, 2-Имя, 3-Порода, 4-Возраст, 5-Вес)</param>
        /// <param name="ascending">Направление сортировки (true - по возрастанию, false - по убыванию)</param>
        void SortRabbits(int sortField, bool ascending);

        /// <summary>
        /// Создает кролика со случайными параметрами
        /// </summary>
        /// <returns>Сообщение о созданном кролике</returns>
        string AddRandomRabbit();

        /// <summary>
        /// Возвращает форматированный список всех кроликов
        /// </summary>
        /// <returns>Строка с отформатированным списком</returns>
        string ShowAllRabbits();

        /// <summary>
        /// Возвращает массив доступных пород кроликов
        /// </summary>
        /// <returns>Массив строк с названиями пород</returns>
        string[] GetBreeds();
    }

}
