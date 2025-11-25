using System;
using Business_logic___rabbit;
using RabbitShared;

namespace RabbitPresenter
{
    public class Presenter : IPresenter
    {
        private readonly IView _view;
        private readonly ILogic _logic;

        public Presenter(IView view, ILogic logic)
        {
            _view = view;
            _logic = logic;
            SubscribeToViewEvents();
        }

        public void Initialize()
        {
            // Initial setup
            RefreshRabbitsList();
        }

        public string[] GetBreeds()
        {
            return _logic.GetBreeds();
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

        private void OnAddRabbit(int id, string name, int age, int weight, string breed)
        {
            try
            {
                var result = _logic.AddRabbit(id, name, age, weight, breed);
                _view.DisplayMessage(result);
                RefreshRabbitsList();
            }
            catch (Exception ex)
            {
                _view.DisplayMessage($"Ошибка: {ex.Message}");
            }
        }

        private void OnRemoveRabbit(int id)
        {
            try
            {
                var result = _logic.RemoveRabbit(id);
                _view.DisplayMessage(result);
                RefreshRabbitsList();
            }
            catch (Exception ex)
            {
                _view.DisplayMessage($"Ошибка: {ex.Message}");
            }
        }

        private void OnReadRabbit(int id)
        {
            try
            {
                var result = _logic.ReadRabbit(id);
                _view.DisplayRabbitDetails(result);
            }
            catch (Exception ex)
            {
                _view.DisplayMessage($"Ошибка: {ex.Message}");
            }
        }

        private void OnUpdateRabbit(int id, string name, int age, int weight, string breed)
        {
            try
            {
                _logic.ChangeStatRabbit(id, name, age, weight, breed);
                _view.DisplayMessage("Данные кролика обновлены");
                RefreshRabbitsList();
            }
            catch (Exception ex)
            {
                _view.DisplayMessage($"Ошибка: {ex.Message}");
            }
        }

        private void OnShowAverageAge()
        {
            try
            {
                var average = _logic.GetAverageAge();
                _view.DisplayStatistics($"Средний возраст: {average:F2} лет");
            }
            catch (Exception ex)
            {
                _view.DisplayMessage($"Ошибка: {ex.Message}");
            }
        }

        private void OnShowAverageWeight()
        {
            try
            {
                var average = _logic.GetAverageWeight();
                _view.DisplayStatistics($"Средний вес: {average:F2} кг");
            }
            catch (Exception ex)
            {
                _view.DisplayMessage($"Ошибка: {ex.Message}");
            }
        }

        private void OnAddRandomRabbit()
        {
            try
            {
                var result = _logic.AddRandomRabbit();
                _view.DisplayMessage(result);
                RefreshRabbitsList();
            }
            catch (Exception ex)
            {
                _view.DisplayMessage($"Ошибка: {ex.Message}");
            }
        }

        private void OnShowAllRabbits()
        {
            RefreshRabbitsList();
        }

        private void OnSortRabbits(int field, bool ascending)
        {
            try
            {
                _logic.SortRabbits(field, ascending);
                RefreshRabbitsList();
                _view.DisplayMessage("Сортировка выполнена");
            }
            catch (Exception ex)
            {
                _view.DisplayMessage($"Ошибка: {ex.Message}");
            }
        }

        private void RefreshRabbitsList()
        {
            try
            {
                var rabbits = _logic.ShowAllRabbits();
                _view.DisplayAllRabbits(rabbits);
            }
            catch (Exception ex)
            {
                _view.DisplayMessage($"Ошибка при загрузке списка: {ex.Message}");
            }
        }
    }
}