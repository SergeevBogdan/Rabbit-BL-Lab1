using RabbitViewModels;
using System;
using System.Windows;

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

            // Подписываемся на события ViewModel
            if (_viewModel.MainView != null)
            {
                _viewModel.MainView.Closed += OnMainViewClosed;
            }
        }

        private void OnMainViewClosed()
        {
            // Закрываем окно при закрытии View
            Dispatcher.Invoke(() => Close());
        }

        // Обработчики кнопок
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
            // Запрашиваем View статистики через ViewModel
            _viewModel.RequestStatsView();
        }

        private void ShowDetails_Click(object sender, RoutedEventArgs e)
        {
            // Запрашиваем View деталей через ViewModel
            _viewModel.RequestDetailsView();

            // Показываем детали в MessageBox
            if (_viewModel.SelectedRabbit != null)
            {
                MessageBox.Show(
                    $"Детали кролика:\n" +
                    $"ID: {_viewModel.SelectedRabbit.Id}\n" +
                    $"Имя: {_viewModel.SelectedRabbit.Name}\n" +
                    $"Порода: {_viewModel.SelectedRabbit.Breed}\n" +
                    $"Возраст: {_viewModel.SelectedRabbit.Age}\n" +
                    $"Вес: {_viewModel.SelectedRabbit.Weight}",
                    "Детали кролика",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Уведомляем View о закрытии
            _viewModel.MainView?.OnClosed();
        }
    }
}