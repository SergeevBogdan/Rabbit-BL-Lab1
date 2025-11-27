using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Business_logic___rabbit;
using RabbitSharedMVP;

namespace RabbitViewModels
{
    public class RabbitViewModel : BaseViewModel
    {
        private readonly ILogic _logic;
        private RabbitDTO _selectedRabbit;
        private string _message;

        public ObservableCollection<RabbitDTO> Rabbits { get; } = new ObservableCollection<RabbitDTO>();

        public RabbitDTO SelectedRabbit
        {
            get => _selectedRabbit;
            set
            {
                if (SetField(ref _selectedRabbit, value))
                {
                    // Обновляем состояние команд при изменении выбранного кролика
                    (RemoveCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    (UpdateCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public string Message
        {
            get => _message;
            set => SetField(ref _message, value);
        }

        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand ShowAllCommand { get; }
        public ICommand AddRandomCommand { get; }

        public RabbitViewModel(ILogic logic)
        {
            _logic = logic;

            AddCommand = new RelayCommand(AddRabbit);
            RemoveCommand = new RelayCommand(RemoveRabbit, () => SelectedRabbit != null);
            UpdateCommand = new RelayCommand(UpdateRabbit, () => SelectedRabbit != null);
            ShowAllCommand = new RelayCommand(ShowAllRabbits);
            AddRandomCommand = new RelayCommand(AddRandomRabbit);

            LoadRabbits();
        }

        private void LoadRabbits()
        {
            Rabbits.Clear();
            var allRabbits = _logic.ShowAllRabbits();

            if (string.IsNullOrEmpty(allRabbits) || allRabbits.Contains("пуст"))
                return;

            var rabbitLines = allRabbits.Split('\n')
                .Where(line => line.Contains("ID:") && line.Contains("Имя:"))
                .ToList();

            foreach (var line in rabbitLines)
            {
                var rabbit = ParseRabbitLine(line);
                if (rabbit != null)
                    Rabbits.Add(rabbit);
            }

            // Обновляем состояние команд после загрузки данных
            (RemoveCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (UpdateCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        private RabbitDTO ParseRabbitLine(string line)
        {
            try
            {
                var parts = line.Split('|');
                if (parts.Length >= 5)
                {
                    return new RabbitDTO
                    {
                        Id = int.Parse(parts[0].Replace("ID:", "").Trim()),
                        Name = parts[1].Replace("Имя:", "").Trim(),
                        Breed = parts[2].Replace("Порода:", "").Trim(),
                        Age = int.Parse(parts[3].Replace("Возраст:", "").Trim()),
                        Weight = int.Parse(parts[4].Replace("Вес:", "").Trim())
                    };
                }
            }
            catch { }
            return null;
        }

        private void AddRabbit()
        {
            var newRabbit = new RabbitDTO { Id = GetNextId(), Name = "Новый кролик", Age = 1, Weight = 1, Breed = "Беляк" };
            var result = _logic.AddRabbit(newRabbit.Id, newRabbit.Name, newRabbit.Age, newRabbit.Weight, newRabbit.Breed);
            Message = result;
            LoadRabbits();
        }

        private void RemoveRabbit()
        {
            if (SelectedRabbit != null)
            {
                var result = _logic.RemoveRabbit(SelectedRabbit.Id);
                Message = result;
                LoadRabbits();
            }
        }

        private void UpdateRabbit()
        {
            if (SelectedRabbit != null)
            {
                _logic.ChangeStatRabbit(SelectedRabbit.Id, SelectedRabbit.Name, SelectedRabbit.Age, SelectedRabbit.Weight, SelectedRabbit.Breed);
                Message = "Кролик обновлен";
                LoadRabbits();
            }
        }

        private void ShowAllRabbits()
        {
            LoadRabbits();
            Message = $"Загружено кроликов: {Rabbits.Count}";
        }

        private void AddRandomRabbit()
        {
            var result = _logic.AddRandomRabbit();
            Message = result;
            LoadRabbits();
        }

        private int GetNextId()
        {
            return Rabbits.Count > 0 ? Rabbits.Max(r => r.Id) + 1 : 1;
        }
    }
}