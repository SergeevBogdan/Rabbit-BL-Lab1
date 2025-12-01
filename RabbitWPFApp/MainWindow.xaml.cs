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

            LoadRabbits_Click(null, null);
        }

        private void LoadRabbits_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewModel.LoadRabbits();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddRabbit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(txtNewId.Text, out int id) || id <= 0)
                {
                    MessageBox.Show("Введите корректный положительный ID", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtNewId.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtNewName.Text))
                {
                    MessageBox.Show("Введите имя кролика", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtNewName.Focus();
                    return;
                }

                if (!int.TryParse(txtNewAge.Text, out int age) || age <= 0)
                {
                    MessageBox.Show("Введите корректный возраст", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtNewAge.Focus();
                    return;
                }

                if (!int.TryParse(txtNewWeight.Text, out int weight) || weight <= 0)
                {
                    MessageBox.Show("Введите корректный вес", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtNewWeight.Focus();
                    return;
                }

                _viewModel.AddRabbit();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RemoveRabbit_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedRabbit == null)
            {
                MessageBox.Show("Выберите кролика для удаления", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show(
                $"Удалить кролика '{_viewModel.SelectedRabbit.Name}' (ID: {_viewModel.SelectedRabbit.Id})?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _viewModel.RemoveRabbit();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void UpdateRabbit_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedRabbit == null)
            {
                MessageBox.Show("Выберите кролика для редактирования", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                if (string.IsNullOrWhiteSpace(txtEditName.Text))
                {
                    MessageBox.Show("Введите имя кролика", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtEditName.Focus();
                    return;
                }

                if (!int.TryParse(txtEditAge.Text, out int age) || age <= 0)
                {
                    MessageBox.Show("Введите корректный возраст", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtEditAge.Focus();
                    return;
                }

                if (!int.TryParse(txtEditWeight.Text, out int weight) || weight <= 0)
                {
                    MessageBox.Show("Введите корректный вес", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtEditWeight.Focus();
                    return;
                }

                _viewModel.UpdateRabbit();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddRandomRabbit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewModel.AddRandomRabbit();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowStatistics_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewModel.ShowStatistics();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBox.Show("Спасибо за использование Rabbit Management!", "Выход",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}