using RabbitViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ViewManager
{
    public class ViewManagerService : IViewManagerService
    {
        private readonly Dictionary<Type, BaseViewModel> _viewModels = new Dictionary<Type, BaseViewModel>();
        private readonly Dictionary<BaseViewModel, Type> _viewModelToView = new Dictionary<BaseViewModel, Type>();

        public event Action<Type, BaseViewModel> ViewRequested;
        public event Action<Type> ViewClosed;

        public void RegisterView(Type viewType, BaseViewModel viewModel)
        {
            if (viewType == null) throw new ArgumentNullException(nameof(viewType));
            if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));

            // Проверяем, что тип реализует IView
            if (!typeof(IView).IsAssignableFrom(viewType))
            {
                throw new ArgumentException($"Тип {viewType.Name} должен реализовывать интерфейс IView");
            }

            if (!_viewModels.ContainsKey(viewType))
            {
                _viewModels[viewType] = viewModel;
                _viewModelToView[viewModel] = viewType;
            }
        }

        public void ShowView(Type viewType)
        {
            if (_viewModels.TryGetValue(viewType, out var viewModel))
            {
                ViewRequested?.Invoke(viewType, viewModel);
            }
            else
            {
                throw new KeyNotFoundException($"View типа {viewType.Name} не зарегистрирован");
            }
        }

        public void CloseView(Type viewType)
        {
            if (_viewModels.ContainsKey(viewType))
            {
                ViewClosed?.Invoke(viewType);
            }
        }

        public BaseViewModel GetViewModel(Type viewType)
        {
            return _viewModels.TryGetValue(viewType, out var viewModel) ? viewModel : null;
        }

        public TViewModel GetViewModel<TViewModel>() where TViewModel : BaseViewModel
        {
            return _viewModels.Values.OfType<TViewModel>().FirstOrDefault();
        }

        public Type GetViewType(BaseViewModel viewModel)
        {
            return _viewModelToView.TryGetValue(viewModel, out var viewType) ? viewType : null;
        }

        public void UnregisterView(Type viewType)
        {
            if (_viewModels.TryGetValue(viewType, out var viewModel))
            {
                _viewModels.Remove(viewType);
                _viewModelToView.Remove(viewModel);
            }
        }

        public IEnumerable<Type> GetAllViewTypes()
        {
            return _viewModels.Keys.ToList();
        }

        public IEnumerable<BaseViewModel> GetAllViewModels()
        {
            return _viewModels.Values.ToList();
        }
    }
}