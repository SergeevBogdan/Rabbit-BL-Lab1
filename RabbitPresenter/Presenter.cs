using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogicMVP;
using RabbitSharedMVP;

namespace RabbitPresenter
{
    /// <summary>
    /// Представляет презентер в архитектуре MVP, координирующий взаимодействие между View и Model.
    /// Реализует паттерн Наблюдатель для обработки событий от View и уведомлений от Model.
    /// </summary>
    public interface IPresenter
    {
        /// <summary>
        /// Инициализирует презентер и загружает начальные данные в View
        /// </summary>
        void Initialize();

        /// <summary>
        /// Возвращает список доступных пород кроликов
        /// </summary>
        /// <returns>Массив строк с названиями пород</returns>
        string[] GetBreeds();

        /// <summary>
        /// Обновляет список кроликов в View
        /// </summary>
        void RefreshRabbitsList();
    }

    /// <summary>
    /// Реализация презентера для системы управления кроликами.
    /// Координирует поток данных между View (пользовательский интерфейс) и Model (бизнес-логика).
    /// Подписывается на события View и Model, преобразует DTO между слоями.
    /// </summary>
    /// <remarks>
    /// Responsibilities:
    /// - Обработка пользовательских действий из View
    /// - Вызов соответствующих методов Model
    /// - Преобразование данных между DTO и Domain Model
    /// - Обновление View в ответ на изменения в Model
    /// - Управление жизненным циклом данных
    /// </remarks>
    public class Presenter : IPresenter
    {
        private readonly IView _view;
        private readonly IModel _model;

        /// <summary>
        /// Инициализирует новый экземпляр презентера с указанными View и Model
        /// </summary>
        /// <param name="view">Реализация интерфейса View для отображения данных</param>
        /// <param name="model">Реализация интерфейса Model для доступа к бизнес-логике</param>
        /// <exception cref="ArgumentNullException">Выбрасывается если view или model равны null</exception>
        public Presenter(IView view, IModel model)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _model = model ?? throw new ArgumentNullException(nameof(model));
            SubscribeToViewEvents();
            SubscribeToModelEvents();
        }

        /// <summary>
        /// Подписывается на все события View для обработки пользовательских действий
        /// </summary>
        private void SubscribeToViewEvents()
        {
            _view.AddRabbitRequested += OnAddRabbit;
            _view.RemoveRabbitRequested += OnRemoveRabbit;
            _view.ReadRabbitRequested += OnReadRabbit;
            _view.UpdateRabbitRequested += OnUpdateRabbit;
            _view.ShowAverageAgeRequested += OnShowAverageAge;
            _view.ShowAverageWeightRequested += OnShowAverageWeight;
            _view.AddRandomRabbitRequested += OnAddRandomRabbit;
            _view.ShowAllRabbitsRequested += OnShowAllRabbits;
            _view.SortRabbitsRequested += OnSortRabbits;
        }

        /// <summary>
        /// Подписывается на события Model для получения уведомлений об изменениях данных
        /// </summary>
        private void SubscribeToModelEvents()
        {
            _model.DataChanged += OnDataChanged;
            _model.RabbitsListChanged += OnRabbitsListChanged;
        }

        /// <summary>
        /// Обрабатывает запрос на добавление нового кролика
        /// </summary>
        /// <param name="rabbitDto">DTO с данными нового кролика</param>
        private void OnAddRabbit(RabbitDTO rabbitDto)
        {
            var result = _model.AddRabbit(rabbitDto);
            _view.DisplayMessage(result);
        }

        /// <summary>
        /// Обрабатывает запрос на удаление кролика по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор кролика для удаления</param>
        private void OnRemoveRabbit(int id)
        {
            var result = _model.RemoveRabbit(id);
            _view.DisplayMessage(result);
        }

        /// <summary>
        /// Обрабатывает запрос на просмотр детальной информации о кролике
        /// </summary>
        /// <param name="id">Идентификатор кролика для просмотра</param>
        private void OnReadRabbit(int id)
        {
            var rabbitDto = _model.ReadRabbitDto(id);
            if (rabbitDto != null)
            {
                var details = $"Имя: {rabbitDto.Name}\nВозраст: {rabbitDto.Age}\nВес: {rabbitDto.Weight}\nПорода: {rabbitDto.Breed}";
                _view.DisplayRabbitDetails(details);
            }
            else
            {
                _view.DisplayRabbitDetails("Кролик с заданным Id не найден");
            }
        }

        /// <summary>
        /// Обрабатывает запрос на обновление данных кролика
        /// </summary>
        /// <param name="rabbitDto">DTO с обновленными данными кролика</param>
        private void OnUpdateRabbit(RabbitDTO rabbitDto)
        {
            var result = _model.UpdateRabbit(rabbitDto);
            _view.DisplayMessage(result);
        }

        /// <summary>
        /// Обрабатывает запрос на отображение среднего возраста кроликов
        /// </summary>
        private void OnShowAverageAge()
        {
            var average = _model.GetAverageAge();
            _view.DisplayStatistics($"Средний возраст: {average:F2} лет");
        }

        /// <summary>
        /// Обрабатывает запрос на отображение среднего веса кроликов
        /// </summary>
        private void OnShowAverageWeight()
        {
            var average = _model.GetAverageWeight();
            _view.DisplayStatistics($"Средний вес: {average:F2} кг");
        }

        /// <summary>
        /// Обрабатывает запрос на добавление случайного кролика
        /// </summary>
        private void OnAddRandomRabbit()
        {
            var result = _model.AddRandomRabbit();
            _view.DisplayMessage(result);
        }

        /// <summary>
        /// Обрабатывает запрос на отображение всех кроликов
        /// </summary>
        private void OnShowAllRabbits()
        {
            RefreshRabbitsList();
        }

        /// <summary>
        /// Обрабатывает запрос на сортировку списка кроликов
        /// </summary>
        /// <param name="sortDto">DTO с параметрами сортировки</param>
        private void OnSortRabbits(SortOperationDTO sortDto)
        {
            _model.SortRabbits(sortDto.SortField, sortDto.Ascending);
            _view.DisplayMessage("Сортировка выполнена");
        }

        /// <summary>
        /// Обрабатывает уведомление от Model об изменении данных
        /// </summary>
        /// <param name="message">Сообщение об изменении</param>
        private void OnDataChanged(string message)
        {
            _view.DisplayMessage(message);
        }

        /// <summary>
        /// Обрабатывает уведомление от Model об изменении списка кроликов
        /// </summary>
        /// <param name="rabbits">Обновленный список кроликов в формате DTO</param>
        private void OnRabbitsListChanged(List<IDTO> rabbits)
        {
            DisplayRabbitsInView(rabbits);
        }

        /// <summary>
        /// Обновляет список кроликов в View
        /// </summary>
        public void RefreshRabbitsList()
        {
            var rabbits = _model.GetAllRabbits();
            DisplayRabbitsInView(rabbits);
        }

        /// <summary>
        /// Форматирует и отображает список кроликов в View
        /// </summary>
        /// <param name="rabbits">Список кроликов для отображения</param>
        private void DisplayRabbitsInView(List<IDTO> rabbits)
        {
            if (rabbits == null || rabbits.Count == 0)
            {
                _view.DisplayAllRabbits("Список кроликов пуст");
                return;
            }

            string result = "=== СПИСОК ВСЕХ КРОЛИКОВ ===\n";
            foreach (var rabbit in rabbits)
            {
                result += $"ID: {rabbit.Id} | Имя: {rabbit.Name} | Порода: {rabbit.Breed} | Возраст: {rabbit.Age} | Вес: {rabbit.Weight}\n";
            }

            _view.DisplayAllRabbits(result);
        }

        /// <summary>
        /// Инициализирует презентер и загружает начальные данные
        /// </summary>
        public void Initialize()
        {
            RefreshRabbitsList();
        }

        /// <summary>
        /// Возвращает список доступных пород кроликов
        /// </summary>
        /// <returns>Массив строк с названиями пород</returns>
        public string[] GetBreeds()
        {
            return _model.GetBreeds();
        }
    }
}