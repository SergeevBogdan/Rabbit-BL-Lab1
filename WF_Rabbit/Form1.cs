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
using Business_logic_rabbit;

namespace WF_Rabbit
{
    public partial class Form1 : Form
    {
        Logic logic = new Logic();

        public Form1()
        {
            InitializeComponent();
            InitializeDataGridView();
            RefreshDataGridView();
            InitializeBreedComboBox();
            InitializeFilterComboBox();

            logic.SaveRabbitsToFile();
            logic.LoadRabbitsFromFile();
            RefreshDataGridView();
        }

        private void InitializeDataGridView()
        {
            
            dataGridViewRabbits.Columns.Clear();

            
            dataGridViewRabbits.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Id",
                HeaderText = "ID",
                SortMode = DataGridViewColumnSortMode.NotSortable 
            });

            dataGridViewRabbits.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Name",
                HeaderText = "Имя",
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            dataGridViewRabbits.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Age",
                HeaderText = "Возраст",
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            dataGridViewRabbits.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Weight",
                HeaderText = "Вес",
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            dataGridViewRabbits.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Breed",
                HeaderText = "Порода",
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            
            dataGridViewRabbits.Columns["Id"].Width = 50;
            dataGridViewRabbits.Columns["Name"].Width = 100;
            dataGridViewRabbits.Columns["Age"].Width = 70;
            dataGridViewRabbits.Columns["Weight"].Width = 70;
            dataGridViewRabbits.Columns["Breed"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

        
            dataGridViewRabbits.AllowUserToOrderColumns = false; 
            dataGridViewRabbits.EnableHeadersVisualStyles = false; 
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
            string allRabbits = logic.ShowAllRabbits();

            if (string.IsNullOrEmpty(allRabbits))
            {
                return; 
            }

            string[] rabbits = allRabbits.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string rabbit in rabbits)
            {
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

        private RabbitData ParseRabbitString(string rabbitString)
        {
            try
            {
                
                var parts = rabbitString.Split(' ');
                return new RabbitData
                {
                    Id = int.Parse(parts[2]),
                    Name = parts[4],
                    Weight = int.Parse(parts[6]),
                    Age = int.Parse(parts[8]),
                    Breed = parts.Length > 10 ? parts[10] : ""
                };
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
            logic.LoadRabbitsFromFile();
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        public class RabbitData
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
            public int Weight { get; set; }
            public string Breed { get; set; }
        }

    }
}
