using System;

namespace RabbitShared
{
    public interface IView
    {
        event Action<int, string, int, int, string> AddRabbitRequested;
        event Action<int> RemoveRabbitRequested;
        event Action<int> ReadRabbitRequested;
        event Action<int, string, int, int, string> UpdateRabbitRequested;
        event Action ShowAverageAgeRequested;
        event Action ShowAverageWeightRequested;
        event Action AddRandomRabbitRequested;
        event Action ShowAllRabbitsRequested;
        event Action<int, bool> SortRabbitsRequested;

        void DisplayMessage(string message);
        void DisplayAllRabbits(string rabbits);
        void DisplayRabbitDetails(string details);
        void DisplayStatistics(string stats);
        string[] GetBreeds();
    }
}