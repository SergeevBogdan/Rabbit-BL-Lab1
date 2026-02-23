using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitSharedMVP
{
    public interface IModel
    {
        string GetCurrentTechnology();
        string AddRabbit(int id, string name, int age, int weight, string breed);
        string RemoveRabbit(int id);
        string ReadRabbit(int id);
        void ChangeStatRabbit(int id, string name, int age, int weight, string breed);
        double GetAverageAge();
        double GetAverageWeight();
        void SortRabbits(int sortField, bool ascending);
        string AddRandomRabbit();
        string ShowAllRabbits();
        string[] GetBreeds();
        string AddRabbit(IDTO rabbitDto);
        string UpdateRabbit(IDTO rabbitDto);
        IDTO ReadRabbitDto(int id);
        List<IDTO> GetAllRabbits();

        event Action<string> DataChanged;
        event Action<List<IDTO>> RabbitsListChanged;
    }
}
