using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitViewModels
{
    public interface IView
    {
        Type ViewType { get; }
        string Title { get; }

        event Action Requested;
        event Action Closed;

        void OnRequested();
        void OnClosed();
    }

    public interface IMainView : IView { }

    public interface IRabbitDetailsView : IView
    {
        int RabbitId { get; set; }
        string RabbitName { get; set; }
    }

    public interface IStatisticsView : IView
    {
        double AverageAge { get; set; }
        double AverageWeight { get; set; }
        int TotalRabbits { get; set; }
    }
}
