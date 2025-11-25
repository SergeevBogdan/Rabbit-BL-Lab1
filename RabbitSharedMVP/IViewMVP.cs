using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitSharedMVP
{
    public interface IView
    {
        event Action<RabbitDTO> AddRabbitRequested;
        event Action<int> RemoveRabbitRequested;
        event Action<int> ReadRabbitRequested;
        event Action<RabbitDTO> UpdateRabbitRequested;
        event Action ShowAverageAgeRequested;
        event Action ShowAverageWeightRequested;
        event Action AddRandomRabbitRequested;
        event Action ShowAllRabbitsRequested;
        event Action<SortOperationDTO> SortRabbitsRequested;
        void DisplayMessage(string message);
        void DisplayAllRabbits(string rabbits);
        void DisplayRabbitDetails(string details);
        void DisplayStatistics(string stats);
        string[] GetBreeds();
    }
    
}