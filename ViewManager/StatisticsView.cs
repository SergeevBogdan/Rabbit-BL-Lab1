using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitViewModels;

namespace ViewManager
{
    public class StatisticsView : IStatisticsView
    {
        public Type ViewType => typeof(StatisticsView);
        public string Title => "Статистика кроликов";

        public double AverageAge { get; set; }
        public double AverageWeight { get; set; }
        public int TotalRabbits { get; set; }

        public event Action Requested;
        public event Action Closed;

        public void OnRequested() => Requested?.Invoke();
        public void OnClosed() => Closed?.Invoke();
    }
}
