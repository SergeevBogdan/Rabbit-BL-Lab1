namespace RabbitView
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dataGridViewRabbits;
        private System.Windows.Forms.Button btnShowAll;
        private System.Windows.Forms.Button btnAddRandom;
        private System.Windows.Forms.Button btnAvgAge;
        private System.Windows.Forms.Button btnAvgWeight;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtAge;
        private System.Windows.Forms.TextBox txtWeight;
        private System.Windows.Forms.ComboBox comboBoxBreed;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblAge;
        private System.Windows.Forms.Label lblWeight;
        private System.Windows.Forms.Label lblBreed;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dataGridViewRabbits = new System.Windows.Forms.DataGridView();
            this.btnShowAll = new System.Windows.Forms.Button();
            this.btnAddRandom = new System.Windows.Forms.Button();
            this.btnAvgAge = new System.Windows.Forms.Button();
            this.btnAvgWeight = new System.Windows.Forms.Button();
            this.txtId = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtAge = new System.Windows.Forms.TextBox();
            this.txtWeight = new System.Windows.Forms.TextBox();
            this.comboBoxBreed = new System.Windows.Forms.ComboBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.lblId = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblAge = new System.Windows.Forms.Label();
            this.lblWeight = new System.Windows.Forms.Label();
            this.lblBreed = new System.Windows.Forms.Label();

            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRabbits)).BeginInit();
            this.SuspendLayout();

            // dataGridViewRabbits
            this.dataGridViewRabbits.Location = new System.Drawing.Point(12, 150);
            this.dataGridViewRabbits.Size = new System.Drawing.Size(600, 300);
            this.dataGridViewRabbits.ReadOnly = true;
            this.dataGridViewRabbits.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            // btnShowAll
            this.btnShowAll.Location = new System.Drawing.Point(12, 12);
            this.btnShowAll.Size = new System.Drawing.Size(100, 30);
            this.btnShowAll.Text = "Показать всех";
            this.btnShowAll.Click += new System.EventHandler(this.btnShowAll_Click);

            // btnAddRandom
            this.btnAddRandom.Location = new System.Drawing.Point(118, 12);
            this.btnAddRandom.Size = new System.Drawing.Size(130, 30);
            this.btnAddRandom.Text = "Добавить случайного";
            this.btnAddRandom.Click += new System.EventHandler(this.btnAddRandom_Click);

            // btnAvgAge
            this.btnAvgAge.Location = new System.Drawing.Point(254, 12);
            this.btnAvgAge.Size = new System.Drawing.Size(100, 30);
            this.btnAvgAge.Text = "Средний возраст";
            this.btnAvgAge.Click += new System.EventHandler(this.btnAvgAge_Click);

            // btnAvgWeight
            this.btnAvgWeight.Location = new System.Drawing.Point(360, 12);
            this.btnAvgWeight.Size = new System.Drawing.Size(100, 30);
            this.btnAvgWeight.Text = "Средний вес";
            this.btnAvgWeight.Click += new System.EventHandler(this.btnAvgWeight_Click);

            // Input fields
            this.lblId.Location = new System.Drawing.Point(12, 50);
            this.lblId.Size = new System.Drawing.Size(80, 20);
            this.lblId.Text = "ID:";

            this.txtId.Location = new System.Drawing.Point(100, 50);
            this.txtId.Size = new System.Drawing.Size(80, 20);

            this.lblName.Location = new System.Drawing.Point(190, 50);
            this.lblName.Size = new System.Drawing.Size(80, 20);
            this.lblName.Text = "Имя:";

            this.txtName.Location = new System.Drawing.Point(270, 50);
            this.txtName.Size = new System.Drawing.Size(100, 20);

            this.lblAge.Location = new System.Drawing.Point(380, 50);
            this.lblAge.Size = new System.Drawing.Size(80, 20);
            this.lblAge.Text = "Возраст:";

            this.txtAge.Location = new System.Drawing.Point(460, 50);
            this.txtAge.Size = new System.Drawing.Size(80, 20);

            this.lblWeight.Location = new System.Drawing.Point(12, 80);
            this.lblWeight.Size = new System.Drawing.Size(80, 20);
            this.lblWeight.Text = "Вес:";

            this.txtWeight.Location = new System.Drawing.Point(100, 80);
            this.txtWeight.Size = new System.Drawing.Size(80, 20);

            this.lblBreed.Location = new System.Drawing.Point(190, 80);
            this.lblBreed.Size = new System.Drawing.Size(80, 20);
            this.lblBreed.Text = "Порода:";

            this.comboBoxBreed.Location = new System.Drawing.Point(270, 80);
            this.comboBoxBreed.Size = new System.Drawing.Size(120, 21);

            // Buttons
            this.btnAdd.Location = new System.Drawing.Point(400, 80);
            this.btnAdd.Size = new System.Drawing.Size(80, 30);
            this.btnAdd.Text = "Добавить";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);

            this.btnRemove.Location = new System.Drawing.Point(490, 80);
            this.btnRemove.Size = new System.Drawing.Size(80, 30);
            this.btnRemove.Text = "Удалить";
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);

            this.btnView.Location = new System.Drawing.Point(400, 115);
            this.btnView.Size = new System.Drawing.Size(80, 30);
            this.btnView.Text = "Просмотр";
            this.btnView.Click += new System.EventHandler(this.btnView_Click);

            this.btnUpdate.Location = new System.Drawing.Point(490, 115);
            this.btnUpdate.Size = new System.Drawing.Size(80, 30);
            this.btnUpdate.Text = "Обновить";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);

            // Form
            this.ClientSize = new System.Drawing.Size(624, 461);
            this.Controls.Add(this.dataGridViewRabbits);
            this.Controls.Add(this.btnShowAll);
            this.Controls.Add(this.btnAddRandom);
            this.Controls.Add(this.btnAvgAge);
            this.Controls.Add(this.btnAvgWeight);
            this.Controls.Add(this.txtId);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.txtAge);
            this.Controls.Add(this.txtWeight);
            this.Controls.Add(this.comboBoxBreed);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnView);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.lblId);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblAge);
            this.Controls.Add(this.lblWeight);
            this.Controls.Add(this.lblBreed);
            this.Text = "Rabbit MVP View";

            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRabbits)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}