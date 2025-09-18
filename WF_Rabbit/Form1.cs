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


        public Form1()
        {
            InitializeComponent();
            RefreshRabbitList();
            InitializeBreedComboBox();
            InitializeFilterComboBox();
            Logic.SaveRabbitsToFile();
            Logic.LoadRabbitsFromFile();
            RefreshRabbitList();
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

        private void RefreshRabbitList()
        {
            listBoxRabbits.Items.Clear();
            string allRabbits = Logic.ShowAllRabbits();
            
            if (string.IsNullOrEmpty(allRabbits))
            {
                listBoxRabbits.Items.Add("Список кроликов пуст");
                return;
            }

            string[] rabbits = allRabbits.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            
            foreach (string rabbit in rabbits)
            {
                listBoxRabbits.Items.Add(rabbit);
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

                string result = Logic.AddRabbit(id, name, age, weight, breed);
                
                if (result == "Кролик успешно добавлен")
                {
                    MessageBox.Show(result, "Успех");
                    ClearInputFields();
                    RefreshRabbitList();
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
            if (listBoxRabbits.SelectedIndex != -1 && listBoxRabbits.SelectedItem.ToString() != "Список кроликов пуст")
            {
                // Получаем ID из текста выбранного элемента
                string selectedItem = listBoxRabbits.SelectedItem.ToString();
                int startIndex = selectedItem.IndexOf("ID ") + 3;
                int endIndex = selectedItem.IndexOf(" ", startIndex);
                
                if (endIndex == -1) endIndex = selectedItem.Length;
                
                if (int.TryParse(selectedItem.Substring(startIndex, endIndex - startIndex), out int rabbitId))
                {
                    DialogResult result = MessageBox.Show($"Удалить кролика с ID {rabbitId}?", 
                        "Подтверждение удаления", MessageBoxButtons.YesNo);
                    
                    if (result == DialogResult.Yes)
                    {
                        string message = Logic.RemoveRabbit(rabbitId);
                        MessageBox.Show(message);
                        RefreshRabbitList();
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите кролика для удаления");
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (listBoxRabbits.SelectedIndex != -1 && listBoxRabbits.SelectedItem.ToString() != "Список кроликов пуст")
            {
                // Получаем ID из текста выбранного элемента
                string selectedItem = listBoxRabbits.SelectedItem.ToString();
                int startIndex = selectedItem.IndexOf("ID ") + 3;
                int endIndex = selectedItem.IndexOf(" ", startIndex);
                
                if (endIndex == -1) endIndex = selectedItem.Length;
                
                if (int.TryParse(selectedItem.Substring(startIndex, endIndex - startIndex), out int rabbitId))
                {
                    string rabbitInfo = Logic.ReadRabbit(rabbitId);
                    MessageBox.Show(rabbitInfo, "Информация о кролике");
                }
            }
            else
            {
                MessageBox.Show("Выберите кролика для просмотра");
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (listBoxRabbits.SelectedIndex != -1 && listBoxRabbits.SelectedItem.ToString() != "Список кроликов пуст")
            {
                // Получаем ID из текста выбранного элемента
                string selectedItem = listBoxRabbits.SelectedItem.ToString();
                int startIndex = selectedItem.IndexOf("ID ") + 3;
                int endIndex = selectedItem.IndexOf(" ", startIndex);
                
                if (endIndex == -1) endIndex = selectedItem.Length;
                
                if (int.TryParse(selectedItem.Substring(startIndex, endIndex - startIndex), out int rabbitId))
                {
                    // Загружаем данные выбранного кролика в поля для редактирования
                    string rabbitInfo = Logic.ReadRabbit(rabbitId);
                    string[] lines = rabbitInfo.Split('\n');
                    
                    foreach (string line in lines)
                    {
                        if (line.Contains("Имя:")) txtName.Text = line.Replace("Имя:", "").Trim();
                        if (line.Contains("Возраст:")) txtAge.Text = line.Replace("Возраст:", "").Trim();
                        if (line.Contains("Вес:")) txtWeight.Text = line.Replace("Вес:", "").Trim();
                        if (line.Contains("Порода:"))
                        {
                            string breedValue = line.Replace("Порода:", "").Trim();
                            int index = comboBoxBreed.Items.IndexOf(breedValue);
                            if (index != -1)
                                comboBoxBreed.SelectedIndex = index;
                            else
                            {
                                comboBoxBreed.Items.Add(breedValue);
                                comboBoxBreed.SelectedIndex = comboBoxBreed.Items.Count - 1;
                            }
                        }
                    }
                    txtId.Text = rabbitId.ToString();
                    
                    MessageBox.Show("Данные кролика загружены для редактирования. Измените нужные поля и нажмите 'Обновить'", 
                        "Редактирование");
                }
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

                Logic.ChangeStatRabbit(id, name, age, weight, breed);
                MessageBox.Show("Данные кролика обновлены", "Успех");
                ClearInputFields();
                RefreshRabbitList();
            }
            catch (FormatException)
            {
                MessageBox.Show("Пожалуйста, введите корректные числовые значения для возраста и веса", "Ошибка");
            }
        }

        private void btnAverageAge_Click(object sender, EventArgs e)
        {
            double averageAge = Logic.GetAverageAge();
            MessageBox.Show($"Средний возраст кроликов: {averageAge:F2}", "Статистика");
        }

        private void btnAverageWeight_Click(object sender, EventArgs e)
        {
            double averageWeight = Logic.GetAverageWeight();
            MessageBox.Show($"Средний вес кроликов: {averageWeight:F2}", "Статистика");
        }

        private void btnAddRandom_Click(object sender, EventArgs e)
        {
            string message = Logic.AddRandomRabbit();
            MessageBox.Show(message);
            RefreshRabbitList();
        }

        private void btnApplyFilter_Click(object sender, EventArgs e)
        {
            int field = comboBoxFilterField.SelectedIndex + 1;
            bool direction = radioAscending.Checked;
            
            Logic.SortRabbits(field, direction);
            MessageBox.Show("Фильтр применен", "Успех");
            RefreshRabbitList();
        }

        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            // Просто обновляем список чтобы показать исходный порядок
            RefreshRabbitList();
            MessageBox.Show("Фильтр сброшен", "Успех");
        }

        private void btnClearFields_Click(object sender, EventArgs e)
        {
            ClearInputFields();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Logic.LoadRabbitsFromFile();
        }

        private void txtBreed_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
