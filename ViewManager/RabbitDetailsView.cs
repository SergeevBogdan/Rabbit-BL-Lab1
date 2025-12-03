using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitViewModels;

namespace ViewManager
{
    public class RabbitDetailsView : IRabbitDetailsView
    {
        public Type ViewType => typeof(RabbitDetailsView);
        public string Title => "Детали кролика";

        public int RabbitId { get; set; }
        public string RabbitName { get; set; }

        public event Action Requested;
        public event Action Closed;

        public void OnRequested() => Requested?.Invoke();
        public void OnClosed() => Closed?.Invoke();
    }
}
