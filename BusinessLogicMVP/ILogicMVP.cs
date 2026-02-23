using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitModel;

namespace BusinessLogicMVP
{
    /// <summary>
    /// Основной интерфейс бизнес-логики для системы управления кроликами.
    /// Определяет контракты для всех операций CRUD и бизнес-операций с кроликами.
    /// Реализует принцип разделения интерфейсов через специализированные интерфейсы.
    /// </summary>
    public interface ILogicMVP
    {
        /// <summary>
        /// Возвращает название используемой технологии доступа к данным
        /// </summary>
        /// <returns>Название технологии (Entity Framework или Dapper)</returns>
        string GetCurrentTechnology();

        /// <summary>
        /// Добавляет нового кролика в систему с валидацией данных
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
        /// Получает информацию о кролике по идентификатору
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
        /// <returns>Средний возраст кроликов</returns>
        double GetAverageAge();

        /// <summary>
        /// Вычисляет средний вес всех кроликов в системе
        /// </summary>
        /// <returns>Средний вес кроликов</returns>
        double GetAverageWeight();

        /// <summary>
        /// Сортирует кроликов по указанному полю и направлению
        /// </summary>
        /// <param name="sortField">Поле для сортировки (1-ID, 2-Имя, 3-Порода, 4-Возраст, 5-Вес)</param>
        /// <param name="ascending">Направление сортировки (true - по возрастанию, false - по убыванию)</param>
        void SortRabbits(int sortField, bool ascending);

        /// <summary>
        /// Добавляет в систему кролика со случайными данными
        /// </summary>
        /// <returns>Сообщение о результате операции с информацией о созданном кролике</returns>
        string AddRandomRabbit();

        /// <summary>
        /// Возвращает форматированную строку со списком всех кроликов
        /// </summary>
        /// <returns>Строка с отформатированным списком кроликов</returns>
        string ShowAllRabbits();

        /// <summary>
        /// Возвращает массив доступных пород кроликов
        /// </summary>
        /// <returns>Массив строк с названиями пород</returns>
        string[] GetBreeds();

        /// <summary>
        /// Получает объект кролика по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор кролика</param>
        /// <returns>Объект Rabbit или null если не найден</returns>
        Rabbit GetRabbitById(int id);

        /// <summary>
        /// Получает список всех кроликов в системе
        /// </summary>
        /// <returns>Список объектов Rabbit</returns>
        List<Rabbit> GetAllRabbits();
    }

    /// <summary>
    /// Интерфейс для операции добавления кроликов с валидацией бизнес-правил
    /// </summary>
    public interface IRabbitAdderMVP
    {
        /// <summary>
        /// Добавляет нового кролика с проверкой корректности данных
        /// </summary>
        /// <param name="id">Уникальный идентификатор кролика</param>
        /// <param name="name">Имя кролика (не может быть пустым)</param>
        /// <param name="age">Возраст кролика (должен быть положительным)</param>
        /// <param name="weight">Вес кролика (должен быть положительным)</param>
        /// <param name="breed">Порода кролика (не может быть пустой)</param>
        /// <returns>Сообщение о результате операции</returns>
        string AddRabbit(int id, string name, int age, int weight, string breed);
    }

    /// <summary>
    /// Интерфейс для операции удаления кроликов
    /// </summary>
    public interface IRabbitRemoverMVP
    {
        /// <summary>
        /// Удаляет кролика по идентификатору с обработкой ошибок
        /// </summary>
        /// <param name="id">Идентификатор кролика для удаления</param>
        /// <returns>Сообщение о результате операции</returns>
        string RemoveRabbit(int id);
    }

    /// <summary>
    /// Интерфейс для операции чтения данных кролика
    /// </summary>
    public interface IRabbitReaderMVP
    {
        /// <summary>
        /// Получает форматированную информацию о кролике
        /// </summary>
        /// <param name="id">Идентификатор кролика</param>
        /// <returns>Форматированная строка с данными кролика</returns>
        string ReadRabbit(int id);
    }

    /// <summary>
    /// Интерфейс для операции обновления данных кролика
    /// </summary>
    public interface IRabbitUpdaterMVP
    {
        /// <summary>
        /// Обновляет данные кролика с валидацией новых значений
        /// </summary>
        /// <param name="id">Идентификатор кролика</param>
        /// <param name="name">Новое имя кролика</param>
        /// <param name="age">Новый возраст кролика</param>
        /// <param name="weight">Новый вес кролика</param>
        /// <param name="breed">Новая порода кролика</param>
        void ChangeStatRabbit(int id, string name, int age, int weight, string breed);
    }

    /// <summary>
    /// Интерфейс для расчета статистики по возрасту кроликов
    /// </summary>
    public interface IRabbitAgeCalculatorMVP
    {
        /// <summary>
        /// Вычисляет средний возраст всех кроликов
        /// </summary>
        /// <returns>Средний возраст кроликов</returns>
        double GetAverageAge();
    }

    /// <summary>
    /// Интерфейс для расчета статистики по весу кроликов
    /// </summary>
    public interface IRabbitWeightCalculatorMVP
    {
        /// <summary>
        /// Вычисляет средний вес всех кроликов
        /// </summary>
        /// <returns>Средний вес кроликов</returns>
        double GetAverageWeight();
    }

    /// <summary>
    /// Интерфейс для операции сортировки кроликов
    /// </summary>
    public interface IRabbitSorterMVP
    {
        /// <summary>
        /// Сортирует кроликов по указанному критерию
        /// </summary>
        /// <param name="sortField">Поле для сортировки</param>
        /// <param name="ascending">Направление сортировки</param>
        void SortRabbits(int sortField, bool ascending);
    }

    /// <summary>
    /// Интерфейс для создания кроликов со случайными данными
    /// </summary>
    public interface IRabbitRandomCreatorMVP
    {
        /// <summary>
        /// Создает кролика со случайными данными и добавляет его в систему
        /// </summary>
        /// <returns>Сообщение о результате операции</returns>
        string AddRandomRabbit();
    }

    /// <summary>
    /// Интерфейс для отображения списка кроликов
    /// </summary>
    public interface IRabbitDisplayerMVP
    {
        /// <summary>
        /// Форматирует и возвращает строку со списком всех кроликов
        /// </summary>
        /// <returns>Отформатированная строка со списком кроликов</returns>
        string ShowAllRabbits();
    }

    /// <summary>
    /// Интерфейс для предоставления информации о породах кроликов
    /// </summary>
    public interface IRabbitBreedProviderMVP
    {
        /// <summary>
        /// Возвращает список доступных пород кроликов
        /// </summary>
        /// <returns>Массив строк с названиями пород</returns>
        string[] GetBreeds();
    }

    /// <summary>
    /// Интерфейс для предоставления объектов кроликов
    /// </summary>
    public interface IRabbitProviderMVP
    {
        /// <summary>
        /// Получает объект кролика по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор кролика</param>
        /// <returns>Объект Rabbit или null если не найден</returns>
        Rabbit GetRabbitById(int id);

        /// <summary>
        /// Получает список всех кроликов в системе
        /// </summary>
        /// <returns>Список объектов Rabbit</returns>
        List<Rabbit> GetAllRabbits();
    }
}