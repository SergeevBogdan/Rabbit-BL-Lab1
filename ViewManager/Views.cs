using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitViewModels;


namespace ViewManager
{
    public abstract class BaseView : IView
    {
        public abstract Type ViewType { get; }
        public abstract string Title { get; }

        public event Action Requested;
        public event Action Closed;

        public virtual void OnRequested() => Requested?.Invoke();
        public virtual void OnClosed() => Closed?.Invoke();
    }

    public class MainView : BaseView, IMainView
    {
        // Убираем статическое свойство Type, используем typeof()
        public override Type ViewType => typeof(MainView);
        public override string Title => "Главное окно управления кроликами";
    }

    public class RabbitDetailsView : BaseView, IRabbitDetailsView
    {
        public override Type ViewType => typeof(RabbitDetailsView);
        public override string Title => "Детали кролика";

        public int RabbitId { get; set; }
        public string RabbitName { get; set; }
    }

    public class StatisticsView : BaseView, IStatisticsView
    {
        public override Type ViewType => typeof(StatisticsView);
        public override string Title => "Статистика кроликов";

        public double AverageAge { get; set; }
        public double AverageWeight { get; set; }
        public int TotalRabbits { get; set; }
    }
}
