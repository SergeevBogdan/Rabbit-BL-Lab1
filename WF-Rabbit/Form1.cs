using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WF_Rabbit
{
    public partial class Form1 : Form
    {
        public Form1()
        {

            InitializeComponent();
            this.Load += Form1_Load;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // Инициализация столбцов
            dataGridView1.Columns.Add("name", "Название");
            dataGridView1.Columns.Add("price", "Цена");
            dataGridView1.Columns.Add("count", "Количество");

            // Добавление строк
            dataGridView1.Rows.Add("Товар 1", 1000, 5);
            dataGridView1.Rows.Add("Товар 2", 2000, 3);

        }


        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "БЛА_БЛА_БЛА_БЛЯ_БЛА";
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
