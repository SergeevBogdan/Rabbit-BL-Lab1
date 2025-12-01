using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using RabbitSharedMVP;

namespace RabbitViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IModel _model;
        private RabbitExtendedDTO _selectedRabbit;
        private RabbitExtendedDTO _newRabbit;
        private string _statusMessage;
        private bool _isIdEditable;

        public ObservableCollection<RabbitExtendedDTO> Rabbits { get; } = new ObservableCollection<RabbitExtendedDTO>();
        public string[] Breeds { get; private set; }

        public RabbitExtendedDTO SelectedRabbit
        {
            get => _selectedRabbit;
            set
            {
                if (SetProperty(ref _selectedRabbit, value))
                {
                    OnPropertyChanged(nameof(CanModifyRabbit));
                    OnPropertyChanged(nameof(CanEditSelected));
                }
            }
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
                    NewRabbit.IsIdEditable = value;
                    OnPropertyChanged(nameof(NewRabbit));
                }
            }
        }

        public bool CanModifyRabbit => SelectedRabbit != null;
        public bool CanEditSelected => SelectedRabbit != null && SelectedRabbit.IsIdEditable;

        public MainViewModel(IModel model)
        {
            _model = model;
            InitializeData();
        }

        private void InitializeData()
        {
            Breeds = _model.GetBreeds();

            // Инициализация нового кролика
            _newRabbit = new RabbitExtendedDTO
            {
                Id = GetNextAvailableId(),
                Name = "Новый кролик",
                Age = 1,
                Weight = 1,
                Breed = Breeds?.FirstOrDefault() ?? "Беляк",
                IsIdEditable = true,
                CreatedDate = DateTime.Now
            };

            IsIdEditable = true;
            LoadRabbits();
        }

        public void LoadRabbits()
        {
            try
            {
                Rabbits.Clear();

                // Проверяем, есть ли расширенный метод
                var allRabbits = _model.GetAllRabbits();

                // Преобразуем базовые DTO в расширенные
                foreach (IDTO rabbit in allRabbits)
                {
                    var extendedRabbit = new RabbitExtendedDTO
                    {
                        Id = rabbit.Id,
                        Name = rabbit.Name,
                        Breed = rabbit.Breed,
                        Age = rabbit.Age,
                        Weight = rabbit.Weight,
                        IsIdEditable = true, // По умолчанию можно редактировать
                        CreatedDate = DateTime.Now
                    };
                    Rabbits.Add(extendedRabbit);
                }

                StatusMessage = $"Загружено {Rabbits.Count} кроликов";

                // Обновляем следующий доступный ID для нового кролика
                NewRabbit.Id = GetNextAvailableId();
                OnPropertyChanged(nameof(NewRabbit));
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
                // Проверяем валидность ID
                if (NewRabbit.Id <= 0)
                {
                    StatusMessage = "ID должен быть положительным числом";
                    return;
                }

                // Проверяем уникальность ID
                if (Rabbits.Any(r => r.Id == NewRabbit.Id))
                {
                    StatusMessage = "Кролик с таким ID уже существует";
                    return;
                }

                // Проверяем имя
                if (string.IsNullOrWhiteSpace(NewRabbit.Name))
                {
                    StatusMessage = "Имя не может быть пустым";
                    return;
                }

                // Проверяем возраст и вес
                if (NewRabbit.Age <= 0 || NewRabbit.Weight <= 0)
                {
                    StatusMessage = "Возраст и вес должны быть положительными числами";
                    return;
                }

                // Используем старый метод для обратной совместимости
                var result = _model.AddRabbit(NewRabbit.Id, NewRabbit.Name,
                                             NewRabbit.Age, NewRabbit.Weight, NewRabbit.Breed);

                StatusMessage = result;

                if (result.Contains("успешно") || result.Contains("добавлен"))
                {
                    // Перезагружаем список
                    LoadRabbits();

                    // Сбрасываем форму только если ID не редактируется пользователем
                    if (!IsIdEditable)
                    {
                        NewRabbit.Id = GetNextAvailableId();
                    }
                    NewRabbit.Name = "Новый кролик";
                    NewRabbit.Age = 1;
                    NewRabbit.Weight = 1;
                    NewRabbit.Breed = Breeds?.FirstOrDefault() ?? "Беляк";

                    OnPropertyChanged(nameof(NewRabbit));
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка при добавлении: {ex.Message}";
            }
        }

        public void RemoveRabbit()
        {
            try
            {
                if (SelectedRabbit != null)
                {
                    var result = _model.RemoveRabbit(SelectedRabbit.Id);
                    StatusMessage = result;

                    if (result.Contains("удален") || result.Contains("успешно"))
                    {
                        LoadRabbits();
                        SelectedRabbit = null;
                    }
                }
                else
                {
                    StatusMessage = "Выберите кролика для удаления";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка при удалении: {ex.Message}";
            }
        }

        public void UpdateRabbit()
        {
            try
            {
                if (SelectedRabbit != null)
                {
                    // Проверяем данные перед обновлением
                    if (string.IsNullOrWhiteSpace(SelectedRabbit.Name))
                    {
                        StatusMessage = "Имя не может быть пустым";
                        return;
                    }

                    if (SelectedRabbit.Age <= 0 || SelectedRabbit.Weight <= 0)
                    {
                        StatusMessage = "Возраст и вес должны быть положительными числами";
                        return;
                    }

                    // Создаем базовый DTO для обратной совместимости
                    var rabbitDto = new RabbitDTO
                    {
                        Id = SelectedRabbit.Id,
                        Name = SelectedRabbit.Name,
                        Breed = SelectedRabbit.Breed,
                        Age = SelectedRabbit.Age,
                        Weight = SelectedRabbit.Weight
                    };

                    var result = _model.UpdateRabbit(rabbitDto);
                    StatusMessage = result;

                    if (result.Contains("обновлены") || result.Contains("успешно"))
                    {
                        LoadRabbits();
                    }
                }
                else
                {
                    StatusMessage = "Выберите кролика для обновления";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка при обновлении: {ex.Message}";
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
                StatusMessage = $"Ошибка при создании случайного кролика: {ex.Message}";
            }
        }

        public void ShowStatistics()
        {
            try
            {
                var avgAge = _model.GetAverageAge();
                var avgWeight = _model.GetAverageWeight();
                StatusMessage = $"Статистика: Средний возраст: {avgAge:F2} лет, Средний вес: {avgWeight:F2} кг";
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

            // Ищем первое свободное ID
            for (int i = 1; i <= maxId + 1; i++)
            {
                if (!Rabbits.Any(r => r.Id == i))
                {
                    return i;
                }
            }

            return maxId + 1;
        }

        public void ToggleIdEditMode()
        {
            IsIdEditable = !IsIdEditable;
            StatusMessage = IsIdEditable ? "Режим: Ручной ввод ID" : "Режим: Автоматический ID";
        }

        public void SelectRabbitForEdit(int id)
        {
            SelectedRabbit = Rabbits.FirstOrDefault(r => r.Id == id);
            if (SelectedRabbit != null)
            {
                StatusMessage = $"Выбран кролик: {SelectedRabbit.Name} (ID: {SelectedRabbit.Id})";
            }
        }
    }

    // Расширенный DTO класс
    public class RabbitExtendedDTO : RabbitDTO
    {
        private bool _isIdEditable;
        private DateTime _createdDate;
        private string _description;

        public bool IsIdEditable
        {
            get => _isIdEditable;
            set
            {
                _isIdEditable = value;
                OnPropertyChanged();
            }
        }

        public DateTime CreatedDate
        {
            get => _createdDate;
            set
            {
                _createdDate = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Конструкторы для удобства
        public RabbitExtendedDTO() : base()
        {
            IsIdEditable = true;
            CreatedDate = DateTime.Now;
        }

        public RabbitExtendedDTO(int id, string name, string breed, int age, int weight)
            : this()
        {
            Id = id;
            Name = name;
            Breed = breed;
            Age = age;
            Weight = weight;
        }
    }
}
