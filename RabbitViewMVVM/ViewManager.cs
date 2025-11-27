using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RabbitViewMVVM
{
    public static class ViewManager
    {
        public static void ShowMainWindow()
        {
            var logic = Business_logic___rabbit.LogicFactory.CreateLogic();
            var viewModel = new RabbitViewModels.RabbitViewModel(logic);
            var view = new MainWindow { DataContext = viewModel };
            view.Show();
        }

        public static void CloseWindow(Window window)
        {
            window?.Close();
        }
    }
}
