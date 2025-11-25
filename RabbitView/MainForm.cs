using System;
using System.Windows.Forms;
using Business_logic___rabbit;
using RabbitPresenter;
using RabbitShared;

namespace RabbitView
{
    public partial class MainForm : Form, IView
    {
        private Presenter _presenter;
        private bool _useEF;

        // IView events
        public event Action<int, string, int, int, string> AddRabbitRequested;
        public event Action<int> RemoveRabbitRequested;
        public event Action<int> ReadRabbitRequested;
        public event Action<int, string, int, int, string> UpdateRabbitRequested;
        public event Action ShowAverageAgeRequested;
        public event Action ShowAverageWeightRequested;
        public event Action AddRandomRabbitRequested;
        public event Action ShowAllRabbitsRequested;
        public event Action<int, bool> SortRabbitsRequested;

        public MainForm(bool useEntityFramework = true)
        {
            _useEF = useEntityFramework;
            InitializeComponent();
            InitializePresenter();
        }

        private void InitializePresenter()
        {
            var logic = LogicFactory.CreateLogic(_useEF);
            _presenter = new Presenter(this, logic);

            // Initialize breeds combo
            var breeds = _presenter.GetBreeds();
            comboBoxBreed.Items.Clear();
            comboBoxBreed.Items.AddRange(breeds);
            if (comboBoxBreed.Items.Count > 0)
                comboBoxBreed.SelectedIndex = 0;

            _presenter.Initialize();
        }

        // Event handlers
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtId.Text, out int id) &&
                int.TryParse(txtAge.Text, out int age) &&
                int.TryParse(txtWeight.Text, out int weight))
            {
                AddRabbitRequested?.Invoke(id, txtName.Text, age, weight, comboBoxBreed.SelectedItem?.ToString());
            }
            else
            {
                DisplayMessage("Проверьте правильность введенных данных");
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (dataGridViewRabbits.SelectedRows.Count > 0)
            {
                int id = (int)dataGridViewRabbits.SelectedRows[0].Cells[0].Value;
                RemoveRabbitRequested?.Invoke(id);
            }
            else
            {
                DisplayMessage("Выберите кролика для удаления");
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (dataGridViewRabbits.SelectedRows.Count > 0)
            {
                int id = (int)dataGridViewRabbits.SelectedRows[0].Cells[0].Value;
                ReadRabbitRequested?.Invoke(id);
            }
            else
            {
                DisplayMessage("Выберите кролика для просмотра");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtId.Text, out int id) &&
                int.TryParse(txtAge.Text, out int age) &&
                int.TryParse(txtWeight.Text, out int weight))
            {
                UpdateRabbitRequested?.Invoke(id, txtName.Text, age, weight, comboBoxBreed.SelectedItem?.ToString());
            }
            else
            {
                DisplayMessage("Проверьте правильность введенных данных");
            }
        }

        private void btnShowAll_Click(object sender, EventArgs e) => ShowAllRabbitsRequested?.Invoke();
        private void btnAddRandom_Click(object sender, EventArgs e) => AddRandomRabbitRequested?.Invoke();
        private void btnAvgAge_Click(object sender, EventArgs e) => ShowAverageAgeRequested?.Invoke();
        private void btnAvgWeight_Click(object sender, EventArgs e) => ShowAverageWeightRequested?.Invoke();

        // IView implementation
        public void DisplayMessage(string message)
        {
            MessageBox.Show(message, "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void DisplayAllRabbits(string rabbits)
        {
            dataGridViewRabbits.Rows.Clear();

            if (string.IsNullOrEmpty(rabbits) || rabbits.Contains("пуст"))
                return;

            string[] lines = rabbits.Split('\n');
            foreach (string line in lines)
            {
                if (line.Contains("ID:") && line.Contains("Имя:"))
                {
                    var parts = line.Split(new[] { " | " }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 5)
                    {
                        int id = int.Parse(parts[0].Replace("ID:", "").Trim());
                        string name = parts[1].Replace("Имя:", "").Trim();
                        string breed = parts[2].Replace("Порода:", "").Trim();
                        int age = int.Parse(parts[3].Replace("Возраст:", "").Trim());
                        int weight = int.Parse(parts[4].Replace("Вес:", "").Trim());

                        dataGridViewRabbits.Rows.Add(id, name, breed, age, weight);
                    }
                }
            }
        }

        public void DisplayRabbitDetails(string details)
        {
            MessageBox.Show(details, "Данные кролика", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void DisplayStatistics(string stats)
        {
            MessageBox.Show(stats, "Статистика", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public string[] GetBreeds()
        {
            return _presenter?.GetBreeds() ?? new string[0];
        }

        private void dataGridViewRabbits_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewRabbits.SelectedRows.Count > 0)
            {
                var row = dataGridViewRabbits.SelectedRows[0];
                txtId.Text = row.Cells[0].Value.ToString();
                txtName.Text = row.Cells[1].Value.ToString();
                txtAge.Text = row.Cells[3].Value.ToString();
                txtWeight.Text = row.Cells[4].Value.ToString();

                string breed = row.Cells[2].Value.ToString();
                if (comboBoxBreed.Items.Contains(breed))
                    comboBoxBreed.SelectedItem = breed;
            }
        }
    }
}