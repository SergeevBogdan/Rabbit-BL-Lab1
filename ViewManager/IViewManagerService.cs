using RabbitViewModels;
using System;

namespace ViewManager
{
    public interface IViewManagerService
    {
        void RegisterView(Type viewType, BaseViewModel viewModel);
        void ShowView(Type viewType);
        void CloseView(Type viewType);
        BaseViewModel GetViewModel(Type viewType);

        event Action<Type, BaseViewModel> ViewRequested;
        event Action<Type> ViewClosed;
    }
}