using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogicMVP;
using RabbitSharedMVP;

namespace RabbitPresenter
{
    public class Presenter : IPresenter
    {
        private readonly IView _view;
        private readonly IModel _model;

        public Presenter(IView view, IModel model)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _model = model ?? throw new ArgumentNullException(nameof(model));
            SubscribeToViewEvents();
            SubscribeToModelEvents();
        }

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

        private void SubscribeToModelEvents()
        {
            _model.DataChanged += OnDataChanged;
            _model.RabbitsListChanged += OnRabbitsListChanged;
        }

        private void OnAddRabbit(RabbitDTO rabbitDto)
        {
            // Явное приведение типа к IDTO
            var result = _model.AddRabbit((IDTO)rabbitDto);
            _view.DisplayMessage(result);
        }

        private void OnRemoveRabbit(int id)
        {
            var result = _model.RemoveRabbit(id);
            _view.DisplayMessage(result);
        }

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

        private void OnUpdateRabbit(RabbitDTO rabbitDto)
        {
            // Явное приведение типа к IDTO
            var result = _model.UpdateRabbit((IDTO)rabbitDto);
            _view.DisplayMessage(result);
        }

        private void OnShowAverageAge()
        {
            var average = _model.GetAverageAge();
            _view.DisplayStatistics($"Средний возраст: {average:F2} лет");
        }

        private void OnShowAverageWeight()
        {
            var average = _model.GetAverageWeight();
            _view.DisplayStatistics($"Средний вес: {average:F2} кг");
        }

        private void OnAddRandomRabbit()
        {
            var result = _model.AddRandomRabbit();
            _view.DisplayMessage(result);
        }

        private void OnShowAllRabbits()
        {
            RefreshRabbitsList();
        }

        private void OnSortRabbits(SortOperationDTO sortDto)
        {
            _model.SortRabbits(sortDto.SortField, sortDto.Ascending);
            _view.DisplayMessage("Сортировка выполнена");
        }

        private void OnDataChanged(string message)
        {
            _view.DisplayMessage(message);
        }

        private void OnRabbitsListChanged(List<IDTO> rabbits)
        {
            DisplayRabbitsInView(rabbits);
        }

        public void RefreshRabbitsList()
        {
            var rabbits = _model.GetAllRabbits();
            DisplayRabbitsInView(rabbits);
        }

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

        public void Initialize()
        {
            RefreshRabbitsList();
        }

        public string[] GetBreeds()
        {
            return _model.GetBreeds();
        }
    }
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
}