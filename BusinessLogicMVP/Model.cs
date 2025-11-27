using System;
using System.Collections.Generic;
using System.Linq;
using RabbitSharedMVP;

namespace BusinessLogicMVP
{
    public class Model : IModel
    {
        private readonly ILogicMVP _logic;

        public event Action<string> DataChanged;
        public event Action<List<IDTO>> RabbitsListChanged;

        public Model(ILogicMVP logic)
        {
            _logic = logic;
        }
        public string GetCurrentTechnology() => _logic.GetCurrentTechnology();

        public string AddRabbit(int id, string name, int age, int weight, string breed)
        {
            var result = _logic.AddRabbit(id, name, age, weight, breed);
            NotifyDataChanged();
            return result;
        }

        public string RemoveRabbit(int id)
        {
            var result = _logic.RemoveRabbit(id);
            NotifyDataChanged();
            return result;
        }

        public string ReadRabbit(int id)
        {
            return _logic.ReadRabbit(id);
        }

        public void ChangeStatRabbit(int id, string name, int age, int weight, string breed)
        {
            _logic.ChangeStatRabbit(id, name, age, weight, breed);
            NotifyDataChanged();
        }

        public double GetAverageAge() => _logic.GetAverageAge();
        public double GetAverageWeight() => _logic.GetAverageWeight();

        public void SortRabbits(int sortField, bool ascending)
        {
            _logic.SortRabbits(sortField, ascending);
            NotifyDataChanged();
        }

        public string AddRandomRabbit()
        {
            var result = _logic.AddRandomRabbit();
            NotifyDataChanged();
            return result;
        }

        public string ShowAllRabbits()
        {
            return _logic.ShowAllRabbits();
        }

        public string[] GetBreeds() => _logic.GetBreeds();

        public string AddRabbit(IDTO rabbitDto)
        {
            var result = _logic.AddRabbit(rabbitDto.Id, rabbitDto.Name, rabbitDto.Age, rabbitDto.Weight, rabbitDto.Breed);
            NotifyDataChanged();
            return result;
        }

        public string UpdateRabbit(IDTO rabbitDto)
        {
            _logic.ChangeStatRabbit(rabbitDto.Id, rabbitDto.Name, rabbitDto.Age, rabbitDto.Weight, rabbitDto.Breed);
            NotifyDataChanged();
            return "Данные кролика обновлены";
        }

        public IDTO ReadRabbitDto(int id)
        {
            var rabbit = _logic.GetRabbitById(id);
            if (rabbit == null) return null;

            return new RabbitDTO
            {
                Id = rabbit.Id,
                Name = rabbit.Name,
                Age = rabbit.Age,
                Weight = rabbit.Weight,
                Breed = rabbit.Breed
            };
        }

        public List<IDTO> GetAllRabbits()
        {
            var rabbits = _logic.GetAllRabbits();
            return rabbits.Select(r => new RabbitDTO
            {
                Id = r.Id,
                Name = r.Name,
                Breed = r.Breed,
                Age = r.Age,
                Weight = r.Weight
            }).ToList<IDTO>(); // Убрали Cast, используем ToList<IDTO>
        }

        private void NotifyDataChanged()
        {
            DataChanged?.Invoke("Данные обновлены");
            var rabbits = GetAllRabbits();
            RabbitsListChanged?.Invoke(rabbits);
        }
    }
}