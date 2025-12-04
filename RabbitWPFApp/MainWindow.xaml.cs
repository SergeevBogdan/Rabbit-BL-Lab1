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

            // Подписываемся на события View
            if (_viewModel.MainView != null)
            {
                _viewModel.MainView.Closed += OnMainViewClosed;
            }

            if (_viewModel.DetailsView != null)
            {
                _viewModel.DetailsView.Requested += OnDetailsViewRequested;
                _viewModel.DetailsView.Closed += OnDetailsViewClosed;
            }

            if (_viewModel.StatsView != null)
            {
                _viewModel.StatsView.Requested += OnStatsViewRequested;
                _viewModel.StatsView.Closed += OnStatsViewClosed;
            }
        }

        // Обработчики событий View
        private void OnMainViewClosed()
        {
            Dispatcher.Invoke(() => Close());
        }

        private void OnDetailsViewRequested()
        {
            // Детали запрошены - можно обновить UI
            if (_viewModel.SelectedRabbit != null)
            {
                // Например, выделить выбранного кролика или показать панель деталей
                txtStatus.Text = $"Просмотр деталей: {_viewModel.SelectedRabbit.Name}";
            }
        }

        private void OnDetailsViewClosed()
        {
            txtStatus.Text = "Детали закрыты";
        }

        private void OnStatsViewRequested()
        {
            // Статистика запрошена
            txtStatus.Text = "Статистика открыта";
        }

        private void OnStatsViewClosed()
        {
            txtStatus.Text = "Статистика закрыта";
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
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Уведомляем View о закрытии
            _viewModel.MainView?.OnClosed();
        }
    }
}