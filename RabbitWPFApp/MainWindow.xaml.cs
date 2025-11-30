using RabbitViewModels;
using System.Windows;
using System.Windows.Controls;

namespace RabbitWPFApp
{
    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel;

        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        private void LoadRabbits_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.LoadRabbits();
        }

        private void AddRabbit_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.AddRabbit();
        }

        private void RemoveRabbit_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.RemoveRabbit();
        }

        private void UpdateRabbit_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.UpdateRabbit();
        }

        private void AddRandomRabbit_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.AddRandomRabbit();
        }

        private void ShowStatistics_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ShowStatistics();
        }
    }
}