using System;
using System.Collections.Generic;
using RabbitModel;

namespace RabbitDAL
{
    /// <summary>
    /// Единый интерфейс репозитория для работы с кроликами
    /// Объединяет операции чтения и записи данных
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Добавляет нового кролика в хранилище данных
        /// </summary>
        /// <param name="rabbit">Объект кролика для добавления</param>
        void Add(Rabbit rabbit);

        /// <summary>
        /// Удаляет кролика из хранилища данных
        /// </summary>
        /// <param name="rabbit">Объект кролика для удаления</param>
        void Delete(Rabbit rabbit);

        /// <summary>
        /// Возвращает всех кроликов из хранилища данных
        /// </summary>
        /// <returns>Коллекция всех кроликов</returns>
        IEnumerable<Rabbit> ReadAll();

        /// <summary>
        /// Находит кролика по уникальному идентификатору
        /// </summary>
        /// <param name="id">Уникальный идентификатор кролика</param>
        /// <returns>Найденный кролик или null если не найден</returns>
        Rabbit ReadById(int id);

        /// <summary>
        /// Обновляет данные кролика в хранилище данных
        /// </summary>
        /// <param name="rabbit">Кролик с обновленными данными</param>
        void Update(Rabbit rabbit);
    }
} 