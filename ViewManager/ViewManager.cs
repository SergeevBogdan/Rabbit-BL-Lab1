using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitViewModels;

namespace ViewManager
{
    public interface IViewManager
    {
        void RegisterView<TView, TViewModel>(TViewModel viewModel)
            where TView : IView
            where TViewModel : BaseViewModel;

        void ShowView<TView>() where TView : IView;
        void CloseView<TView>() where TView : IView;

        TViewModel GetViewModel<TView, TViewModel>()
            where TView : IView
            where TViewModel : BaseViewModel;

        event Action<Type, BaseViewModel> ViewRequested;
        event Action<Type> ViewClosed;
    }

    public class ViewManager : IViewManager
    {
        private readonly Dictionary<Type, BaseViewModel> _viewModels = new Dictionary<Type, BaseViewModel>();

        public event Action<Type, BaseViewModel> ViewRequested;
        public event Action<Type> ViewClosed;

        public void RegisterView<TView, TViewModel>(TViewModel viewModel)
            where TView : IView
            where TViewModel : BaseViewModel
        {
            var viewType = typeof(TView);
            _viewModels[viewType] = viewModel;
        }

        public void ShowView<TView>() where TView : IView
        {
            var viewType = typeof(TView);
            if (_viewModels.TryGetValue(viewType, out var viewModel))
            {
                ViewRequested?.Invoke(viewType, viewModel);
            }
            else
            {
                throw new KeyNotFoundException($"View типа {viewType.Name} не зарегистрирован");
            }
        }

        public void CloseView<TView>() where TView : IView
        {
            var viewType = typeof(TView);
            ViewClosed?.Invoke(viewType);
        }

        public TViewModel GetViewModel<TView, TViewModel>()
            where TView : IView
            where TViewModel : BaseViewModel
        {
            var viewType = typeof(TView);
            if (_viewModels.TryGetValue(viewType, out var viewModel) && viewModel is TViewModel typedViewModel)
            {
                return typedViewModel;
            }
            return null;
        }
    }
}
