using RabbitSharedMVP;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace RabbitViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IModel _model;
        private RabbitDTO _selectedRabbit;
        private string _statusMessage;

        public ObservableCollection<RabbitDTO> Rabbits { get; } = new ObservableCollection<RabbitDTO>();
        public string[] Breeds { get; private set; }

        public RabbitDTO SelectedRabbit
        {
            get => _selectedRabbit;
            set => SetProperty(ref _selectedRabbit, value);
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
            LoadRabbits();
        }

        public void LoadRabbits()
        {
            Rabbits.Clear();
            var allRabbits = _model.GetAllRabbits();
            foreach (var rabbit in allRabbits)
            {
                Rabbits.Add((RabbitDTO)rabbit);
            }
            StatusMessage = $"Загружено {Rabbits.Count} кроликов";
        }

        public void AddRabbit()
        {
            var newRabbit = new RabbitDTO
            {
                Id = Rabbits.Count > 0 ? Rabbits.Max(r => r.Id) + 1 : 1,
                Name = "Новый кролик",
                Age = 1,
                Weight = 1,
                Breed = Breeds.FirstOrDefault()
            };

            var result = _model.AddRabbit(newRabbit);
            StatusMessage = result;
            if (result.Contains("успешно"))
            {
                LoadRabbits();
            }
        }

        public void RemoveRabbit()
        {
            if (SelectedRabbit != null)
            {
                var result = _model.RemoveRabbit(SelectedRabbit.Id);
                StatusMessage = result;
                if (result.Contains("удален"))
                {
                    LoadRabbits();
                    SelectedRabbit = null;
                }
            }
        }

        public void UpdateRabbit()
        {
            if (SelectedRabbit != null)
            {
                var result = _model.UpdateRabbit(SelectedRabbit);
                StatusMessage = result;
                if (result.Contains("обновлены"))
                {
                    LoadRabbits();
                }
            }
        }

        public void AddRandomRabbit()
        {
            var result = _model.AddRandomRabbit();
            StatusMessage = result;
            LoadRabbits();
        }

        public void ShowStatistics()
        {
            var avgAge = _model.GetAverageAge();
            var avgWeight = _model.GetAverageWeight();
            StatusMessage = $"Средний возраст: {avgAge:F2} лет, Средний вес: {avgWeight:F2} кг";
        }

        public bool CanModifyRabbit() => SelectedRabbit != null;
    }
}
