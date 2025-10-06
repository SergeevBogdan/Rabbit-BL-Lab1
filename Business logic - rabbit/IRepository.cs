using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_logic___rabbit
{
    public interface IRepository<T> where T : IDomainObject
    {
        // <summary>
        /// Добавляет новый доменный объект в хранилище данных
        /// </summary>
        /// <param name="entity">Доменный объект для добавления</param>
        /// <exception cref="System.Exception">Может выбрасывать исключения при ошибках доступа к данным</exception>
        void Add(T entity);

        /// <summary>
        /// Удаляет доменный объект из хранилища данных
        /// </summary>
        /// <param name="entity">Доменный объект для удаления</param>
        /// <exception cref="System.Exception">Может выбрасывать исключения при ошибках доступа к данным</exception>
        void Delete(T entity);

        /// <summary>
        /// Возвращает все доменные объекты из хранилища данных
        /// </summary>
        /// <returns>Коллекция доменных объектов</returns>
        /// <exception cref="System.Exception">Может выбрасывать исключения при ошибках доступа к данным</exception>
        IEnumerable<T> ReadAll();

        /// <summary>
        /// Находит доменный объект по уникальному идентификатору
        /// </summary>
        /// <param name="id">Уникальный идентификатор доменного объекта</param>
        /// <returns>Найденный доменный объект или null если объект не найден</returns>
        /// <exception cref="System.Exception">Может выбрасывать исключения при ошибках доступа к данным</exception>
        T ReadById(int id);

        /// <summary>
        /// Обновляет данные доменного объекта в хранилище данных
        /// </summary>
        /// <param name="entity">Доменный объект с обновленными данными</param>
        /// <exception cref="System.Exception">Может выбрасывать исключения при ошибках доступа к данным</exception>
        void Update(T entity);
    }
}