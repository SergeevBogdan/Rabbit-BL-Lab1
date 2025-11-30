using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitViewModels
{
    public class ViewModelManager
    {
        private readonly Dictionary<Type, BaseViewModel> _viewModels = new Dictionary<Type, BaseViewModel>();
        private readonly Dictionary<BaseViewModel, Type> _viewModelTypes = new Dictionary<BaseViewModel, Type>();

        public T GetViewModel<T>() where T : BaseViewModel, new()
        {
            var type = typeof(T);
            if (!_viewModels.ContainsKey(type))
            {
                var viewModel = new T();
                _viewModels[type] = viewModel;
                _viewModelTypes[viewModel] = type;
            }
            return (T)_viewModels[type];
        }

        public void CloseViewModel(BaseViewModel viewModel)
        {
            if (_viewModelTypes.ContainsKey(viewModel))
            {
                var type = _viewModelTypes[viewModel];
                _viewModels.Remove(type);
                _viewModelTypes.Remove(viewModel);
            }
        }
    }
}
