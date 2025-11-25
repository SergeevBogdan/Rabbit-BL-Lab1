using System;
using System.Windows.Forms;
using RabbitPresenter;
using RabbitSharedMVP;

namespace RabbitView
{
    public partial class MainForm : Form, IView
    {
        public event Action<RabbitDTO> AddRabbitRequested;
        public event Action<int> RemoveRabbitRequested;
        public event Action<int> ReadRabbitRequested;
        public event Action<RabbitDTO> UpdateRabbitRequested;
        public event Action ShowAverageAgeRequested;
        public event Action ShowAverageWeightRequested;
        public event Action AddRandomRabbitRequested;
        public event Action ShowAllRabbitsRequested;
        public event Action<SortOperationDTO> SortRabbitsRequested;

        private Presenter _presenter;
        

        public MainForm(IModel model)
        {
            
            InitializeComponent();
            InitializeDataGridViewColumns();
            InitializePresenter(model);
        }

        private void InitializeDataGridViewColumns()
        {
            dataGridViewRabbits.Columns.Clear();
            dataGridViewRabbits.Columns.Add("Id", "ID");
            dataGridViewRabbits.Columns.Add("Name", "Имя");
            dataGridViewRabbits.Columns.Add("Breed", "Порода");
            dataGridViewRabbits.Columns.Add("Age", "Возраст");
            dataGridViewRabbits.Columns.Add("Weight", "Вес");
            dataGridViewRabbits.Columns["Id"].Width = 50;
            dataGridViewRabbits.Columns["Name"].Width = 100;
            dataGridViewRabbits.Columns["Breed"].Width = 100;
            dataGridViewRabbits.Columns["Age"].Width = 70;
            dataGridViewRabbits.Columns["Weight"].Width = 70;
            dataGridViewRabbits.AllowUserToAddRows = false;
            dataGridViewRabbits.AllowUserToDeleteRows = false;
            dataGridViewRabbits.ReadOnly = true;
            dataGridViewRabbits.RowHeadersVisible = false;
            dataGridViewRabbits.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void InitializePresenter(IModel model)
        {
            _presenter = new Presenter(this, model);
            var breeds = _presenter.GetBreeds();
            comboBoxBreed.Items.Clear();
            comboBoxBreed.Items.AddRange(breeds);
            if (comboBoxBreed.Items.Count > 0)
                comboBoxBreed.SelectedIndex = 0;

            _presenter.Initialize();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtId.Text, out int id) &&
                int.TryParse(txtAge.Text, out int age) &&
                int.TryParse(txtWeight.Text, out int weight))
            {
                var rabbitDto = new RabbitDTO
                {
                    Id = id,
                    Name = txtName.Text,
                    Age = age,
                    Weight = weight,
                    Breed = comboBoxBreed.SelectedItem?.ToString()
                };
                AddRabbitRequested?.Invoke(rabbitDto); 
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
                var rabbitDto = new RabbitDTO
                {
                    Id = id,
                    Name = txtName.Text,
                    Age = age,
                    Weight = weight,
                    Breed = comboBoxBreed.SelectedItem?.ToString()
                };
                UpdateRabbitRequested?.Invoke(rabbitDto); 
            }
            else
            {
                DisplayMessage("Проверьте правильность введенных данных");
            }
        }

        private void btnShowAll_Click(object sender, EventArgs e)
            => ShowAllRabbitsRequested?.Invoke();

        private void btnAddRandom_Click(object sender, EventArgs e)
            => AddRandomRabbitRequested?.Invoke();

        private void btnAvgAge_Click(object sender, EventArgs e)
            => ShowAverageAgeRequested?.Invoke();

        private void btnAvgWeight_Click(object sender, EventArgs e)
            => ShowAverageWeightRequested?.Invoke();

        private void btnSort_Click(object sender, EventArgs e)
        {
            var sortDialog = new Form()
            {
                Text = "Сортировка кроликов",
                Size = new System.Drawing.Size(300, 200),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent
            };

            var comboField = new ComboBox() { Location = new System.Drawing.Point(20, 20), Width = 200 };
            comboField.Items.AddRange(new string[] { "ID", "Имя", "Порода", "Возраст", "Вес" });
            comboField.SelectedIndex = 0;

            var radioAsc = new RadioButton() { Text = "По возрастанию", Location = new System.Drawing.Point(20, 60), Checked = true };
            var radioDesc = new RadioButton() { Text = "По убыванию", Location = new System.Drawing.Point(20, 85) };

            var btnOk = new Button() { Text = "OK", Location = new System.Drawing.Point(20, 120), DialogResult = DialogResult.OK };
            var btnCancel = new Button() { Text = "Отмена", Location = new System.Drawing.Point(120, 120), DialogResult = DialogResult.Cancel };

            sortDialog.Controls.AddRange(new Control[] { comboField, radioAsc, radioDesc, btnOk, btnCancel });

            if (sortDialog.ShowDialog() == DialogResult.OK)
            {
                int field = comboField.SelectedIndex + 1;
                bool ascending = radioAsc.Checked;
                var sortDto = new SortOperationDTO(field, ascending); 
                SortRabbitsRequested?.Invoke(sortDto); 
            }
        }

        public void DisplayMessage(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(DisplayMessage), message);
                return;
            }

            MessageBox.Show(message, "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void DisplayAllRabbits(string rabbits)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(DisplayAllRabbits), rabbits);
                return;
            }

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
            if (InvokeRequired)
            {
                Invoke(new Action<string>(DisplayRabbitDetails), details);
                return;
            }

            MessageBox.Show(details, "Данные кролика", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void DisplayStatistics(string stats)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(DisplayStatistics), stats);
                return;
            }

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