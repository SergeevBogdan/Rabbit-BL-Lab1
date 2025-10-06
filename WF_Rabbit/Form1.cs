using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Business_logic___rabbit;


namespace WF_Rabbit
{
    public partial class Form1 : Form
    {
        private Logic logic;
        private bool useEntityFramework = true;
        private ToolStripMenuItem technologyStatusItem;

        public Form1()
        {
            InitializeComponent();

            // Сначала инициализируем логику
            logic = new Logic(useEntityFramework);

            // Потом создаем интерфейс
            InitializeTechnologySelection();
            InitializeDataGridView();
            InitializeBreedComboBox();
            InitializeFilterComboBox();
            RefreshDataGridView();

            // Показываем текущую технологию при запуске
            UpdateTechnologyDisplay();
        }

        private void InitializeTechnologySelection()
        {
            // Создаем меню для выбора технологии
            var menuStrip = new MenuStrip();

            var technologyMenu = new ToolStripMenuItem("Технология данных");
            var efItem = new ToolStripMenuItem("Entity Framework", null, (s, e) => SwitchTechnology(true));
            var dapperItem = new ToolStripMenuItem("Dapper", null, (s, e) => SwitchTechnology(false));

            // Создаем пустой элемент, заполним после инициализации logic
            technologyStatusItem = new ToolStripMenuItem("Текущая: загрузка...");

            var menuItems = new ToolStripItem[]
            {
                efItem,
                dapperItem,
                new ToolStripSeparator(),
                technologyStatusItem
            };

            technologyMenu.DropDownItems.AddRange(menuItems);

            menuStrip.Items.Add(technologyMenu);
            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);
        }

        private void UpdateTechnologyDisplay()
        {
            if (technologyStatusItem != null)
            {
                technologyStatusItem.Text = $"Текущая: {logic.GetCurrentTechnology()}";
            }
            this.Text = $"Кролики - {logic.GetCurrentTechnology()}";
        }

        private void SwitchTechnology(bool useEF)
        {
            try
            {
                useEntityFramework = useEF;

                // Пересоздаем объект Logic с новой технологией
                logic = new Logic(useEntityFramework);
                RefreshDataGridView();

                // Обновляем отображение
                UpdateTechnologyDisplay();

                MessageBox.Show($"Переключено на: {logic.GetCurrentTechnology()}", "Технология данных");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при переключении технологии: {ex.Message}", "Ошибка");
            }
        }

        private void InitializeDataGridView()
        {
            dataGridViewRabbits.Columns.Clear();

            dataGridViewRabbits.Columns.Add("Id", "ID");
            dataGridViewRabbits.Columns.Add("Name", "Имя");
            dataGridViewRabbits.Columns.Add("Age", "Возраст");
            dataGridViewRabbits.Columns.Add("Weight", "Вес");
            dataGridViewRabbits.Columns.Add("Breed", "Порода");

            dataGridViewRabbits.Columns["Id"].Width = 50;
            dataGridViewRabbits.Columns["Name"].Width = 100;
            dataGridViewRabbits.Columns["Age"].Width = 70;
            dataGridViewRabbits.Columns["Weight"].Width = 70;
            dataGridViewRabbits.Columns["Breed"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridViewRabbits.AllowUserToAddRows = false;
            dataGridViewRabbits.AllowUserToDeleteRows = false;
            dataGridViewRabbits.ReadOnly = true;
            dataGridViewRabbits.RowHeadersVisible = false;
            dataGridViewRabbits.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void InitializeBreedComboBox()
        {
            comboBoxBreed.Items.Clear();
            comboBoxBreed.Items.AddRange(new string[]
            {
                "Беляк", "Русак", "Толай", "Маньжурский", "Оранжевый"
            });
            comboBoxBreed.SelectedIndex = 0;
        }

        private void InitializeFilterComboBox()
        {
            comboBoxFilterField.Items.AddRange(new string[] {
                "ID", "Имя", "Порода", "Возраст", "Вес"
            });
            comboBoxFilterField.SelectedIndex = 0;
            radioAscending.Checked = true;
        }

        private void RefreshDataGridView()
        {
            dataGridViewRabbits.Rows.Clear();

            try
            {
                string allRabbits = logic.ShowAllRabbits();
                Console.WriteLine($"ShowAllRabbits returned: {allRabbits}"); // для отладки

                if (string.IsNullOrEmpty(allRabbits) || allRabbits == "Список кроликов пуст")
                {
                    Console.WriteLine("No rabbits to display"); // для отладки
                    return;
                }

                string[] rabbits = allRabbits.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                Console.WriteLine($"Found {rabbits.Length} lines"); // для отладки

                foreach (string rabbit in rabbits)
                {
                    Console.WriteLine($"Processing: {rabbit}"); // для отладки

                    // Пропускаем заголовок
                    if (rabbit.Contains("СПИСОК") || rabbit.Contains("---"))
                        continue;

                    var rabbitData = ParseRabbitString(rabbit);
                    if (rabbitData != null)
                    {
                        dataGridViewRabbits.Rows.Add(
                            rabbitData.Id,
                            rabbitData.Name,
                            rabbitData.Age,
                            rabbitData.Weight,
                            rabbitData.Breed
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка");
                Console.WriteLine($"Refresh error: {ex}"); // для отладки
            }
        }

        private Rabbit ParseRabbitString(string rabbitString)
        {
            try
            {
                // Улучшенный парсинг для формата из ShowAllRabbits
                if (rabbitString.Contains("ID:"))
                {
                    var parts = rabbitString.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 5)
                    {
                        return new Rabbit
                        {
                            Id = int.Parse(parts[0].Replace("ID:", "").Trim()),
                            Name = parts[1].Replace("Имя:", "").Trim(),
                            Breed = parts[2].Replace("Порода:", "").Trim(),
                            Age = int.Parse(parts[3].Replace("Возраст:", "").Trim()),
                            Weight = int.Parse(parts[4].Replace("Вес:", "").Trim())
                        };
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        private void ClearInputFields()
        {
            txtId.Text = "";
            txtName.Text = "";
            txtAge.Text = "";
            txtWeight.Text = "";
            comboBoxBreed.SelectedIndex = 0;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtId.Text) || string.IsNullOrEmpty(txtName.Text) ||
                    string.IsNullOrEmpty(txtAge.Text) || string.IsNullOrEmpty(txtWeight.Text) ||
                    comboBoxBreed.SelectedItem == null)
                {
                    MessageBox.Show("Заполните все поля", "Ошибка");
                    return;
                }

                int id = int.Parse(txtId.Text);
                string name = txtName.Text;
                int age = int.Parse(txtAge.Text);
                int weight = int.Parse(txtWeight.Text);
                string breed = comboBoxBreed.SelectedItem.ToString();

                string result = logic.AddRabbit(id, name, age, weight, breed);

                if (result == "Кролик успешно добавлен")
                {
                    MessageBox.Show(result, "Успех");
                    ClearInputFields();
                    RefreshDataGridView();
                }
                else
                {
                    MessageBox.Show(result, "Ошибка");
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Пожалуйста, введите корректные числовые значения для ID, возраста и веса", "Ошибка");
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (dataGridViewRabbits.SelectedRows.Count > 0)
            {
                int rabbitId = (int)dataGridViewRabbits.SelectedRows[0].Cells["Id"].Value;

                DialogResult result = MessageBox.Show($"Удалить кролика с ID {rabbitId}?",
                    "Подтверждение удаления", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    string message = logic.RemoveRabbit(rabbitId);
                    MessageBox.Show(message);
                    RefreshDataGridView();
                }
            }
            else
            {
                MessageBox.Show("Выберите кролика для удаления");
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (dataGridViewRabbits.SelectedRows.Count > 0)
            {
                int rabbitId = (int)dataGridViewRabbits.SelectedRows[0].Cells["Id"].Value;
                string rabbitInfo = logic.ReadRabbit(rabbitId);
                MessageBox.Show(rabbitInfo, "Информация о кролике");
            }
            else
            {
                MessageBox.Show("Выберите кролика для просмотра");
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridViewRabbits.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewRabbits.SelectedRows[0];
                int rabbitId = (int)selectedRow.Cells["Id"].Value;

                txtId.Text = rabbitId.ToString();
                txtName.Text = selectedRow.Cells["Name"].Value.ToString();
                txtAge.Text = selectedRow.Cells["Age"].Value.ToString();
                txtWeight.Text = selectedRow.Cells["Weight"].Value.ToString();

                string breedValue = selectedRow.Cells["Breed"].Value.ToString();
                int index = comboBoxBreed.Items.IndexOf(breedValue);
                if (index != -1)
                    comboBoxBreed.SelectedIndex = index;
                else
                {
                    comboBoxBreed.Items.Add(breedValue);
                    comboBoxBreed.SelectedIndex = comboBoxBreed.Items.Count - 1;
                }

                MessageBox.Show("Данные кролика загружены для редактирования. Измените нужные поля и нажмите 'Обновить'",
                    "Редактирование");
            }
            else
            {
                MessageBox.Show("Выберите кролика для редактирования");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtId.Text))
                {
                    MessageBox.Show("Нет данных для обновления. Сначала выберите кролика для редактирования", "Ошибка");
                    return;
                }

                int id = int.Parse(txtId.Text);
                string name = txtName.Text;
                int age = int.Parse(txtAge.Text);
                int weight = int.Parse(txtWeight.Text);
                string breed = comboBoxBreed.SelectedItem?.ToString() ?? "";

                logic.ChangeStatRabbit(id, name, age, weight, breed);
                MessageBox.Show("Данные кролика обновлены", "Успех");
                ClearInputFields();
                RefreshDataGridView();
            }
            catch (FormatException)
            {
                MessageBox.Show("Пожалуйста, введите корректные числовые значения для возраста и веса", "Ошибка");
            }
        }

        private void btnAverageAge_Click(object sender, EventArgs e)
        {
            double averageAge = logic.GetAverageAge();
            MessageBox.Show($"Средний возраст кроликов: {averageAge:F2}", "Статистика");
        }

        private void btnAverageWeight_Click(object sender, EventArgs e)
        {
            double averageWeight = logic.GetAverageWeight();
            MessageBox.Show($"Средний вес кроликов: {averageWeight:F2}", "Статистика");
        }

        private void btnAddRandom_Click(object sender, EventArgs e)
        {
            string message = logic.AddRandomRabbit();
            MessageBox.Show(message);
            RefreshDataGridView();
        }

        private void btnApplyFilter_Click(object sender, EventArgs e)
        {
            int field = comboBoxFilterField.SelectedIndex + 1;
            bool direction = radioAscending.Checked;

            logic.SortRabbits(field, direction);
            MessageBox.Show("Фильтр применен", "Успех");
            RefreshDataGridView();
        }

        private void btnClearFields_Click(object sender, EventArgs e)
        {
            ClearInputFields();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        private void dataGridViewRabbits_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewRabbits.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewRabbits.SelectedRows[0];
                txtId.Text = selectedRow.Cells["Id"].Value.ToString();
                txtName.Text = selectedRow.Cells["Name"].Value.ToString();
                txtAge.Text = selectedRow.Cells["Age"].Value.ToString();
                txtWeight.Text = selectedRow.Cells["Weight"].Value.ToString();

                string breedValue = selectedRow.Cells["Breed"].Value.ToString();
                int index = comboBoxBreed.Items.IndexOf(breedValue);
                if (index != -1)
                    comboBoxBreed.SelectedIndex = index;
            }
        }

        // Убрал обработчик comboBox1_SelectedIndexChanged так как он не нужен
    }
}
