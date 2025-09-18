namespace WF_Rabbit
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.listBoxRabbits = new System.Windows.Forms.ListBox();
            this.txtId = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtAge = new System.Windows.Forms.TextBox();
            this.txtWeight = new System.Windows.Forms.TextBox();
            this.txtBreed = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnAverageAge = new System.Windows.Forms.Button();
            this.btnAverageWeight = new System.Windows.Forms.Button();
            this.btnAddRandom = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnClearFields = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxFilterField = new System.Windows.Forms.ComboBox();
            this.radioAscending = new System.Windows.Forms.RadioButton();
            this.radioDescending = new System.Windows.Forms.RadioButton();
            this.btnApplyFilter = new System.Windows.Forms.Button();
            this.btnClearFilter = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBoxRabbits
            // 
            this.listBoxRabbits.FormattingEnabled = true;
            this.listBoxRabbits.Location = new System.Drawing.Point(12, 12);
            this.listBoxRabbits.Name = "listBoxRabbits";
            this.listBoxRabbits.Size = new System.Drawing.Size(400, 394);
            this.listBoxRabbits.TabIndex = 0;
            // 
            // txtId
            // 
            this.txtId.Location = new System.Drawing.Point(420, 40);
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(150, 20);
            this.txtId.TabIndex = 1;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(420, 80);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(150, 20);
            this.txtName.TabIndex = 2;
            // 
            // txtAge
            // 
            this.txtAge.Location = new System.Drawing.Point(420, 120);
            this.txtAge.Name = "txtAge";
            this.txtAge.Size = new System.Drawing.Size(150, 20);
            this.txtAge.TabIndex = 3;
            // 
            // txtWeight
            // 
            this.txtWeight.Location = new System.Drawing.Point(420, 160);
            this.txtWeight.Name = "txtWeight";
            this.txtWeight.Size = new System.Drawing.Size(150, 20);
            this.txtWeight.TabIndex = 4;
            // 
            // txtBreed
            // 
            this.txtBreed.Location = new System.Drawing.Point(420, 200);
            this.txtBreed.Name = "txtBreed";
            this.txtBreed.Size = new System.Drawing.Size(150, 20);
            this.txtBreed.TabIndex = 5;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(420, 230);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 23);
            this.btnAdd.TabIndex = 11;
            this.btnAdd.Text = "Добавить";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(420, 260);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(100, 23);
            this.btnRemove.TabIndex = 12;
            this.btnRemove.Text = "Удалить";
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(420, 290);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(100, 23);
            this.btnView.TabIndex = 13;
            this.btnView.Text = "Просмотреть";
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(420, 320);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(100, 23);
            this.btnEdit.TabIndex = 14;
            this.btnEdit.Text = "Редактировать";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(420, 350);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(100, 23);
            this.btnUpdate.TabIndex = 15;
            this.btnUpdate.Text = "Обновить";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnAverageAge
            // 
            this.btnAverageAge.Location = new System.Drawing.Point(6, 19);
            this.btnAverageAge.Name = "btnAverageAge";
            this.btnAverageAge.Size = new System.Drawing.Size(104, 23);
            this.btnAverageAge.TabIndex = 0;
            this.btnAverageAge.Text = "Средний возраст";
            this.btnAverageAge.Click += new System.EventHandler(this.btnAverageAge_Click);
            // 
            // btnAverageWeight
            // 
            this.btnAverageWeight.Location = new System.Drawing.Point(6, 48);
            this.btnAverageWeight.Name = "btnAverageWeight";
            this.btnAverageWeight.Size = new System.Drawing.Size(104, 23);
            this.btnAverageWeight.TabIndex = 1;
            this.btnAverageWeight.Text = "Средний вес";
            this.btnAverageWeight.Click += new System.EventHandler(this.btnAverageWeight_Click);
            // 
            // btnAddRandom
            // 
            this.btnAddRandom.Location = new System.Drawing.Point(116, 19);
            this.btnAddRandom.Name = "btnAddRandom";
            this.btnAddRandom.Size = new System.Drawing.Size(75, 52);
            this.btnAddRandom.TabIndex = 2;
            this.btnAddRandom.Text = "Случайный кролик";
            this.btnAddRandom.Click += new System.EventHandler(this.btnAddRandom_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(420, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "ID:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(420, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 7;
            this.label2.Text = "Имя:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(420, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 23);
            this.label3.TabIndex = 8;
            this.label3.Text = "Возраст:";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(420, 140);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 23);
            this.label4.TabIndex = 9;
            this.label4.Text = "Вес:";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(420, 180);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 23);
            this.label5.TabIndex = 10;
            this.label5.Text = "Порода:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnAverageAge);
            this.groupBox1.Controls.Add(this.btnAverageWeight);
            this.groupBox1.Controls.Add(this.btnAddRandom);
            this.groupBox1.Location = new System.Drawing.Point(580, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 100);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Статистика";
            // 
            // btnClearFields
            // 
            this.btnClearFields.Location = new System.Drawing.Point(420, 380);
            this.btnClearFields.Name = "btnClearFields";
            this.btnClearFields.Size = new System.Drawing.Size(100, 23);
            this.btnClearFields.TabIndex = 16;
            this.btnClearFields.Text = "Очистить поля";
            this.btnClearFields.Click += new System.EventHandler(this.btnClearFields_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.comboBoxFilterField);
            this.groupBox2.Controls.Add(this.radioAscending);
            this.groupBox2.Controls.Add(this.radioDescending);
            this.groupBox2.Controls.Add(this.btnApplyFilter);
            this.groupBox2.Controls.Add(this.btnClearFilter);
            this.groupBox2.Location = new System.Drawing.Point(580, 120);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 150);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Фильтрация";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(10, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 23);
            this.label6.TabIndex = 0;
            this.label6.Text = "Поле:";
            // 
            // comboBoxFilterField
            // 
            this.comboBoxFilterField.Location = new System.Drawing.Point(10, 40);
            this.comboBoxFilterField.Name = "comboBoxFilterField";
            this.comboBoxFilterField.Size = new System.Drawing.Size(180, 21);
            this.comboBoxFilterField.TabIndex = 1;
            // 
            // radioAscending
            // 
            this.radioAscending.Location = new System.Drawing.Point(10, 70);
            this.radioAscending.Name = "radioAscending";
            this.radioAscending.Size = new System.Drawing.Size(114, 24);
            this.radioAscending.TabIndex = 2;
            this.radioAscending.Text = "По возрастанию";
            // 
            // radioDescending
            // 
            this.radioDescending.Location = new System.Drawing.Point(10, 90);
            this.radioDescending.Name = "radioDescending";
            this.radioDescending.Size = new System.Drawing.Size(100, 24);
            this.radioDescending.TabIndex = 3;
            this.radioDescending.Text = "По убыванию";
            // 
            // btnApplyFilter
            // 
            this.btnApplyFilter.Location = new System.Drawing.Point(10, 120);
            this.btnApplyFilter.Name = "btnApplyFilter";
            this.btnApplyFilter.Size = new System.Drawing.Size(75, 23);
            this.btnApplyFilter.TabIndex = 4;
            this.btnApplyFilter.Text = "Применить фильтр";
            this.btnApplyFilter.Click += new System.EventHandler(this.btnApplyFilter_Click);
            // 
            // btnClearFilter
            // 
            this.btnClearFilter.Location = new System.Drawing.Point(120, 120);
            this.btnClearFilter.Name = "btnClearFilter";
            this.btnClearFilter.Size = new System.Drawing.Size(75, 23);
            this.btnClearFilter.TabIndex = 5;
            this.btnClearFilter.Text = "Сбросить";
            this.btnClearFilter.Click += new System.EventHandler(this.btnClearFilter_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.listBoxRabbits);
            this.Controls.Add(this.txtId);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.txtAge);
            this.Controls.Add(this.txtWeight);
            this.Controls.Add(this.txtBreed);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnView);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnClearFields);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "Form1";
            this.Text = "Кролики";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.ListBox listBoxRabbits;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtAge;
        private System.Windows.Forms.TextBox txtWeight;
        private System.Windows.Forms.TextBox txtBreed;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnAverageAge;
        private System.Windows.Forms.Button btnAverageWeight;
        private System.Windows.Forms.Button btnAddRandom;
        private System.Windows.Forms.Button btnClearFields;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboBoxFilterField;
        private System.Windows.Forms.RadioButton radioAscending;
        private System.Windows.Forms.RadioButton radioDescending;
        private System.Windows.Forms.Button btnApplyFilter;
        private System.Windows.Forms.Button btnClearFilter;
        private System.Windows.Forms.Label label6;
    }//не работает 
}


#endregion