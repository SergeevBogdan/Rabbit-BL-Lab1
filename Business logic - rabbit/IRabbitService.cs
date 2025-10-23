using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
namespace Business_logic___rabbit
{ 
    public interface IRabbitAdder
    {
        string AddRabbit(int id, string name, int age, int weight, string breed);
    }

    public interface IRabbitRemover
    {
        string RemoveRabbit(int id);
    }

    public interface IRabbitReader
    {
        string ReadRabbit(int id);
    }

    public interface IRabbitUpdater
    {
        void ChangeStatRabbit(int id, string name, int age, int weight, string breed);
    }

    public interface IRabbitAgeCalculator
    {
        double GetAverageAge();
    }

    public interface IRabbitWeightCalculator
    {
        double GetAverageWeight();
    }

    public interface IRabbitSorter
    {
        void SortRabbits(int sortField, bool ascending);
    }

    public interface IRabbitRandomCreator
    {
        string AddRandomRabbit();
    }

    public interface IRabbitDisplayer
    {
        string ShowAllRabbits();
    }

    public interface IRabbitBreedProvider
    {
        string[] GetBreeds();
    }
}

