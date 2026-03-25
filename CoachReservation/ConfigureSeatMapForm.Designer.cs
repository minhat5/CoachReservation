namespace CoachReservation
{
    partial class ConfigureSeatMapForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            groupBox1 = new GroupBox();
            btnInit = new Button();
            nbRow = new NumericUpDown();
            label4 = new Label();
            nbColumn = new NumericUpDown();
            label3 = new Label();
            nbFloor = new NumericUpDown();
            label2 = new Label();
            cbVehicle = new ComboBox();
            label1 = new Label();
            groupBox2 = new GroupBox();
            btnSave = new Button();
            txtSeatCode = new TextBox();
            label6 = new Label();
            cbType = new ComboBox();
            label5 = new Label();
            btnSaveConfiguration = new Button();
            groupBox3 = new GroupBox();
            tabControlFloors = new TabControl();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nbRow).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nbColumn).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nbFloor).BeginInit();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnInit);
            groupBox1.Controls.Add(nbRow);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(nbColumn);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(nbFloor);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(cbVehicle);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(375, 262);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "THÔNG TIN KHỞI TẠO";
            // 
            // btnInit
            // 
            btnInit.Location = new Point(6, 218);
            btnInit.Name = "btnInit";
            btnInit.Size = new Size(363, 32);
            btnInit.TabIndex = 8;
            btnInit.Text = "Khởi tạo lưới ghế";
            btnInit.UseVisualStyleBackColor = true;
            btnInit.Click += InitSeatGrid;
            // 
            // nbRow
            // 
            nbRow.Location = new Point(95, 179);
            nbRow.Name = "nbRow";
            nbRow.Size = new Size(274, 23);
            nbRow.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 181);
            label4.Name = "label4";
            label4.Size = new Size(50, 15);
            label4.TabIndex = 6;
            label4.Text = "Số hàng";
            // 
            // nbColumn
            // 
            nbColumn.Location = new Point(95, 133);
            nbColumn.Name = "nbColumn";
            nbColumn.Size = new Size(274, 23);
            nbColumn.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 135);
            label3.Name = "label3";
            label3.Size = new Size(42, 15);
            label3.TabIndex = 4;
            label3.Text = "Số dãy";
            // 
            // nbFloor
            // 
            nbFloor.Location = new Point(95, 83);
            nbFloor.Name = "nbFloor";
            nbFloor.Size = new Size(274, 23);
            nbFloor.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 85);
            label2.Name = "label2";
            label2.Size = new Size(47, 15);
            label2.TabIndex = 2;
            label2.Text = "Số tầng";
            // 
            // cbVehicle
            // 
            cbVehicle.FormattingEnabled = true;
            cbVehicle.Location = new Point(95, 34);
            cbVehicle.Name = "cbVehicle";
            cbVehicle.Size = new Size(274, 23);
            cbVehicle.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 37);
            label1.Name = "label1";
            label1.Size = new Size(72, 15);
            label1.TabIndex = 0;
            label1.Text = "Phương tiện";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(btnSave);
            groupBox2.Controls.Add(txtSeatCode);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(cbType);
            groupBox2.Controls.Add(label5);
            groupBox2.Location = new Point(12, 280);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(375, 161);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "THUỘC TÍNH Ô ĐANG CHỌN";
            // 
            // btnSave
            // 
            btnSave.Location = new Point(6, 121);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(363, 34);
            btnSave.TabIndex = 4;
            btnSave.Text = "Lưu ô";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += SaveSelection;
            // 
            // txtSeatCode
            // 
            txtSeatCode.Location = new Point(95, 74);
            txtSeatCode.Name = "txtSeatCode";
            txtSeatCode.Size = new Size(274, 23);
            txtSeatCode.TabIndex = 3;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(6, 77);
            label6.Name = "label6";
            label6.Size = new Size(47, 15);
            label6.TabIndex = 2;
            label6.Text = "Mã ghế";
            // 
            // cbType
            // 
            cbType.FormattingEnabled = true;
            cbType.Items.AddRange(new object[] { "Ghế", "Lối đi" });
            cbType.Location = new Point(95, 31);
            cbType.Name = "cbType";
            cbType.Size = new Size(274, 23);
            cbType.TabIndex = 1;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(6, 34);
            label5.Name = "label5";
            label5.Size = new Size(29, 15);
            label5.TabIndex = 0;
            label5.Text = "Loại";
            // 
            // btnSaveConfiguration
            // 
            btnSaveConfiguration.Location = new Point(6, 385);
            btnSaveConfiguration.Name = "btnSaveConfiguration";
            btnSaveConfiguration.Size = new Size(383, 34);
            btnSaveConfiguration.TabIndex = 4;
            btnSaveConfiguration.Text = "Lưu cấu hình";
            btnSaveConfiguration.UseVisualStyleBackColor = true;
            btnSaveConfiguration.Click += SaveSeatConfiguration;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(tabControlFloors);
            groupBox3.Controls.Add(btnSaveConfiguration);
            groupBox3.Location = new Point(393, 18);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(395, 423);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "SƠ ĐỒ LƯỚI GHẾ";
            // 
            // tabControlFloors
            // 
            tabControlFloors.Location = new Point(6, 22);
            tabControlFloors.Name = "tabControlFloors";
            tabControlFloors.SelectedIndex = 0;
            tabControlFloors.Size = new Size(383, 355);
            tabControlFloors.TabIndex = 5;
            // 
            // ConfigureSeatMapForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 448);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "ConfigureSeatMapForm";
            Text = "Quản lý xe - Cấu hình Sơ đồ ghế";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nbRow).EndInit();
            ((System.ComponentModel.ISupportInitialize)nbColumn).EndInit();
            ((System.ComponentModel.ISupportInitialize)nbFloor).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private ComboBox cbVehicle;
        private Label label1;
        private Button btnInit;
        private NumericUpDown nbRow;
        private Label label4;
        private NumericUpDown nbColumn;
        private Label label3;
        private NumericUpDown nbFloor;
        private Label label2;
        private GroupBox groupBox2;
        private Label label5;
        private TextBox txtSeatCode;
        private Label label6;
        private ComboBox cbType;
        private Button btnSave;
        private Button btnSaveConfiguration;
        private GroupBox groupBox3;
        private TabControl tabControlFloors;
    }
}