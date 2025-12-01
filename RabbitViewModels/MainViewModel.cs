using RabbitSharedMVP;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RabbitViewModels
{
    // RabbitExtendedDTO остается как расширение в ViewModels
    public class RabbitExtendedDTO : RabbitDTO, INotifyPropertyChanged
    {
        private bool _isIdEditable;
        private DateTime _createdDate;
        private string _description;

        public bool IsIdEditable
        {
            get => _isIdEditable;
            set
            {
                if (_isIdEditable != value)
                {
                    _isIdEditable = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime CreatedDate
        {
            get => _createdDate;
            set
            {
                if (_createdDate != value)
                {
                    _createdDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public RabbitExtendedDTO()
        {
            IsIdEditable = true;
            CreatedDate = DateTime.Now;
        }

        // Конструктор для удобства
        public RabbitExtendedDTO(int id, string name, string breed, int age, int weight)
            : this()
        {
            Id = id;
            Name = name;
            Breed = breed;
            Age = age;
            Weight = weight;
        }

        // Метод для преобразования из базового RabbitDTO
        public static RabbitExtendedDTO FromRabbitDTO(RabbitDTO dto)
        {
            if (dto == null) return null;

            return new RabbitExtendedDTO
            {
                Id = dto.Id,
                Name = dto.Name,
                Breed = dto.Breed,
                Age = dto.Age,
                Weight = dto.Weight,
                IsIdEditable = false, // Существующие записи нельзя редактировать
                CreatedDate = DateTime.Now
            };
        }

        // Метод для преобразования в базовый RabbitDTO
        public RabbitDTO ToRabbitDTO()
        {
            return new RabbitDTO
            {
                Id = this.Id,
                Name = this.Name,
                Breed = this.Breed,
                Age = this.Age,
                Weight = this.Weight
            };
        }
    }















































    public class MainViewModel : BaseViewModel
    {
        private readonly IModel _model;
        private RabbitExtendedDTO _selectedRabbit;
        private RabbitExtendedDTO _newRabbit;
        private string _statusMessage;

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

        public MainViewModel(IModel model)
        {
            _model = model;
            InitializeData();
        }

        private void InitializeData()
        {
            Breeds = _model.GetBreeds();

            // Инициализация нового кролика
            NewRabbit = new RabbitExtendedDTO
            {
                Id = 1,
                Name = "Новый кролик",
                Age = 1,
                Weight = 1,
                Breed = Breeds?.FirstOrDefault() ?? "Беляк",
                IsIdEditable = true,
                CreatedDate = DateTime.Now
            };

            LoadRabbits();
        }

        public void LoadRabbits()
        {
            try
            {
                Rabbits.Clear();

                // Используем оригинальный метод GetAllRabbits()
                var allRabbits = _model.GetAllRabbits();

                // Преобразуем IDTO в RabbitExtendedDTO
                foreach (var rabbitDto in allRabbits)
                {
                    var rabbit = RabbitExtendedDTO.FromRabbitDTO((RabbitDTO)rabbitDto);
                    Rabbits.Add(rabbit);
                }

                StatusMessage = $"Загружено {Rabbits.Count} кроликов";

                // Обновляем следующий доступный ID
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
                // Проверяем валидность
                if (!ValidateRabbit(NewRabbit))
                    return;

                // Преобразуем в базовый DTO
                var rabbitDto = NewRabbit.ToRabbitDTO();

                // Используем оригинальный метод
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

                // Преобразуем в базовый DTO
                var rabbitDto = SelectedRabbit.ToRabbitDTO();

                // Используем оригинальный метод
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
                StatusMessage = $"Ошибка: {ex.Message}";
            }
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

            // Проверяем уникальность ID только для нового кролика
            if (rabbit == NewRabbit && Rabbits.Any(r => r.Id == rabbit.Id))
            {
                StatusMessage = $"Кролик с ID {rabbit.Id} уже существует";
                return false;
            }

            return true;
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

        private void ResetNewRabbitForm()
        {
            NewRabbit.Id = GetNextAvailableId();
            NewRabbit.Name = "Новый кролик";
            NewRabbit.Age = 1;
            NewRabbit.Weight = 1;
            NewRabbit.Breed = Breeds?.FirstOrDefault() ?? "Беляк";
        }
    }

}