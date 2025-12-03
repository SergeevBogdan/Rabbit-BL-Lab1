using RabbitSharedMVP;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;


namespace RabbitViewModels
{


    public class MainViewModel : BaseViewModel
    {
        private readonly IModel _model;
        private RabbitExtendedDTO _selectedRabbit;
        private RabbitExtendedDTO _newRabbit;
        private string _statusMessage;
        private bool _isIdEditable = true;

        // Ссылки на View через интерфейсы
        private IMainView _mainView;
        private IRabbitDetailsView _detailsView;
        private IStatisticsView _statsView;

        public ObservableCollection<RabbitExtendedDTO> Rabbits { get; } = new ObservableCollection<RabbitExtendedDTO>();
        public string[] Breeds { get; private set; }

        public RabbitExtendedDTO SelectedRabbit
        {
            get => _selectedRabbit;
            set => SetProperty(ref _selectedRabbit, value);
        }

        public RabbitExtendedDTO NewRabbit
        {
            get => _newRabbit;
            set => SetProperty(ref _newRabbit, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public bool IsIdEditable
        {
            get => _isIdEditable;
            set
            {
                if (SetProperty(ref _isIdEditable, value))
                {
                    if (NewRabbit != null)
                        NewRabbit.IsIdEditable = value;
                }
            }
        }

        // Свойства для View через интерфейсы
        public IMainView MainView
        {
            get => _mainView;
            set
            {
                _mainView = value;
                if (_mainView != null)
                {
                    _mainView.Requested += OnMainViewRequested;
                    _mainView.Closed += OnMainViewClosed;
                }
            }
        }

        public IRabbitDetailsView DetailsView
        {
            get => _detailsView;
            set
            {
                _detailsView = value;
                if (_detailsView != null)
                {
                    _detailsView.Requested += OnDetailsViewRequested;
                    _detailsView.Closed += OnDetailsViewClosed;
                }
            }
        }

        public IStatisticsView StatsView
        {
            get => _statsView;
            set
            {
                _statsView = value;
                if (_statsView != null)
                {
                    _statsView.Requested += OnStatsViewRequested;
                    _statsView.Closed += OnStatsViewClosed;
                }
            }
        }

        public MainViewModel(IModel model)
        {
            _model = model;
            InitializeData();
        }

        // Метод для инициализации View (вызывается из ViewManager)
        public void InitializeViews(IMainView mainView, IRabbitDetailsView detailsView = null, IStatisticsView statsView = null)
        {
            MainView = mainView;
            DetailsView = detailsView;
            StatsView = statsView;
        }

        // Обработчики событий View
        private void OnMainViewRequested()
        {
            StatusMessage = "Главное окно открыто";
            LoadRabbits();
        }

        private void OnMainViewClosed()
        {
            StatusMessage = "Главное окно закрыто";
        }

        private void OnDetailsViewRequested()
        {
            if (SelectedRabbit != null && DetailsView != null)
            {
                DetailsView.RabbitId = SelectedRabbit.Id;
                DetailsView.RabbitName = SelectedRabbit.Name;
                StatusMessage = $"Детали кролика: {SelectedRabbit.Name}";
            }
        }

        private void OnDetailsViewClosed()
        {
            StatusMessage = "Окно деталей закрыто";
        }

        private void OnStatsViewRequested()
        {
            if (StatsView != null)
            {
                ShowStatistics();
                StatsView.AverageAge = _model.GetAverageAge();
                StatsView.AverageWeight = _model.GetAverageWeight();
                StatsView.TotalRabbits = Rabbits.Count;
                StatusMessage = "Открыта статистика";
            }
        }

        private void OnStatsViewClosed()
        {
            StatusMessage = "Окно статистики закрыто";
        }

        private void InitializeData()
        {
            Breeds = _model.GetBreeds();

            NewRabbit = new RabbitExtendedDTO
            {
                Id = 1,
                Name = "Новый кролик",
                Age = 1,
                Weight = 1,
                Breed = Breeds?.FirstOrDefault() ?? "Беляк",
                IsIdEditable = _isIdEditable,
                CreatedDate = DateTime.Now
            };
        }

        public void LoadRabbits()
        {
            try
            {
                Rabbits.Clear();
                var allRabbits = _model.GetAllRabbits();

                foreach (var rabbitDto in allRabbits)
                {
                    var rabbit = RabbitExtendedDTO.FromRabbitDTO((RabbitDTO)rabbitDto);
                    Rabbits.Add(rabbit);
                }

                StatusMessage = $"Загружено {Rabbits.Count} кроликов";
                NewRabbit.Id = GetNextAvailableId();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка загрузки: {ex.Message}";
            }
        }

        public void AddRabbit()
        {
            try
            {
                if (!ValidateRabbit(NewRabbit))
                    return;

                var rabbitDto = NewRabbit.ToRabbitDTO();
                var result = _model.AddRabbit(rabbitDto);
                StatusMessage = result;

                if (result.Contains("успешно") || result.Contains("добавлен"))
                {
                    LoadRabbits();
                    ResetNewRabbitForm();
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка: {ex.Message}";
            }
        }

        public void RemoveRabbit()
        {
            try
            {
                if (SelectedRabbit == null)
                {
                    StatusMessage = "Выберите кролика для удаления";
                    return;
                }

                var result = _model.RemoveRabbit(SelectedRabbit.Id);
                StatusMessage = result;

                if (result.Contains("удален") || result.Contains("успешно"))
                {
                    LoadRabbits();
                    SelectedRabbit = null;
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка: {ex.Message}";
            }
        }

        public void UpdateRabbit()
        {
            try
            {
                if (SelectedRabbit == null)
                {
                    StatusMessage = "Выберите кролика для обновления";
                    return;
                }

                if (!ValidateRabbit(SelectedRabbit))
                    return;

                var rabbitDto = SelectedRabbit.ToRabbitDTO();
                var result = _model.UpdateRabbit(rabbitDto);
                StatusMessage = result;

                if (result.Contains("обновлены") || result.Contains("успешно"))
                {
                    LoadRabbits();
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка: {ex.Message}";
            }
        }

        public void AddRandomRabbit()
        {
            try
            {
                var result = _model.AddRandomRabbit();
                StatusMessage = result;
                LoadRabbits();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка: {ex.Message}";
            }
        }

        public void ShowStatistics()
        {
            try
            {
                var avgAge = _model.GetAverageAge();
                var avgWeight = _model.GetAverageWeight();
                StatusMessage = $"Средний возраст: {avgAge:F2} лет, Средний вес: {avgWeight:F2} кг";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка при расчете статистики: {ex.Message}";
            }
        }

        private int GetNextAvailableId()
        {
            if (Rabbits.Count == 0) return 1;
            int maxId = Rabbits.Max(r => r.Id);

            for (int i = 1; i <= maxId + 1; i++)
            {
                if (!Rabbits.Any(r => r.Id == i))
                {
                    return i;
                }
            }

            return maxId + 1;
        }

        private void ResetNewRabbitForm()
        {
            NewRabbit.Id = GetNextAvailableId();
            NewRabbit.Name = "Новый кролик";
            NewRabbit.Age = 1;
            NewRabbit.Weight = 1;
            NewRabbit.Breed = Breeds?.FirstOrDefault() ?? "Беляк";
        }

        private bool ValidateRabbit(RabbitExtendedDTO rabbit)
        {
            if (rabbit.Id <= 0)
            {
                StatusMessage = "ID должен быть положительным числом";
                return false;
            }

            if (string.IsNullOrWhiteSpace(rabbit.Name))
            {
                StatusMessage = "Имя не может быть пустым";
                return false;
            }

            if (rabbit.Age <= 0 || rabbit.Weight <= 0)
            {
                StatusMessage = "Возраст и вес должны быть положительными числами";
                return false;
            }

            if (rabbit == NewRabbit && Rabbits.Any(r => r.Id == rabbit.Id))
            {
                StatusMessage = $"Кролик с ID {rabbit.Id} уже существует";
                return false;
            }

            return true;
        }

        // Методы для запроса View
        public void RequestDetailsView()
        {
            if (SelectedRabbit != null && DetailsView != null)
            {
                DetailsView.OnRequested();
            }
        }

        public void RequestStatsView()
        {
            if (StatsView != null)
            {
                StatsView.OnRequested();
            }
        }
    }

}