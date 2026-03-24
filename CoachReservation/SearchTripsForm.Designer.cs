namespace CoachReservation
{
    partial class SearchTripsForm
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
            SearchTrip = new Button();
            comboBox1 = new ComboBox();
            label4 = new Label();
            dtpDeparture = new DateTimePicker();
            label3 = new Label();
            cbDestination = new ComboBox();
            label2 = new Label();
            cbDeparture = new ComboBox();
            label1 = new Label();
            groupBox2 = new GroupBox();
            dataGridView1 = new DataGridView();
            departureTime = new DataGridViewTextBoxColumn();
            departure = new DataGridViewTextBoxColumn();
            destination = new DataGridViewTextBoxColumn();
            coachType = new DataGridViewTextBoxColumn();
            price = new DataGridViewTextBoxColumn();
            empty = new DataGridViewTextBoxColumn();
            choose = new DataGridViewTextBoxColumn();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(SearchTrip);
            groupBox1.Controls.Add(comboBox1);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(dtpDeparture);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(cbDestination);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(cbDeparture);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(546, 283);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Tìm kiếm chuyến xe";
            // 
            // SearchTrip
            // 
            SearchTrip.BackColor = Color.Green;
            SearchTrip.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            SearchTrip.ForeColor = Color.White;
            SearchTrip.Location = new Point(21, 225);
            SearchTrip.Name = "SearchTrip";
            SearchTrip.Size = new Size(507, 38);
            SearchTrip.TabIndex = 6;
            SearchTrip.Text = "TÌM CHUYẾN XE";
            SearchTrip.UseVisualStyleBackColor = false;
            SearchTrip.Click += SearchTrip_Click;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(118, 180);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(410, 23);
            comboBox1.TabIndex = 3;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(21, 183);
            label4.Name = "label4";
            label4.Size = new Size(35, 15);
            label4.TabIndex = 2;
            label4.Text = "Số vé";
            // 
            // dtpDeparture
            // 
            dtpDeparture.Location = new Point(118, 133);
            dtpDeparture.MinDate = new DateTime(2026, 3, 24, 0, 0, 0, 0);
            dtpDeparture.Name = "dtpDeparture";
            dtpDeparture.Size = new Size(410, 23);
            dtpDeparture.TabIndex = 5;
            dtpDeparture.Value = new DateTime(2026, 3, 24, 8, 11, 4, 0);
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(21, 139);
            label3.Name = "label3";
            label3.Size = new Size(91, 15);
            label3.TabIndex = 4;
            label3.Text = "Ngày khởi hành";
            // 
            // cbDestination
            // 
            cbDestination.FormattingEnabled = true;
            cbDestination.Location = new Point(118, 86);
            cbDestination.Name = "cbDestination";
            cbDestination.Size = new Size(410, 23);
            cbDestination.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(21, 89);
            label2.Name = "label2";
            label2.Size = new Size(58, 15);
            label2.TabIndex = 2;
            label2.Text = "Điểm đến";
            // 
            // cbDeparture
            // 
            cbDeparture.FormattingEnabled = true;
            cbDeparture.Location = new Point(118, 39);
            cbDeparture.Name = "cbDeparture";
            cbDeparture.Size = new Size(410, 23);
            cbDeparture.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(21, 42);
            label1.Name = "label1";
            label1.Size = new Size(48, 15);
            label1.TabIndex = 0;
            label1.Text = "Điểm đi";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(dataGridView1);
            groupBox2.Location = new Point(12, 301);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(546, 283);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Kết quả tìm kiếm";
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { departureTime, departure, destination, coachType, price, empty, choose });
            dataGridView1.Location = new Point(6, 22);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(534, 255);
            dataGridView1.TabIndex = 0;
            // 
            // departureTime
            // 
            departureTime.HeaderText = "Giờ chạy";
            departureTime.Name = "departureTime";
            departureTime.ReadOnly = true;
            departureTime.Width = 50;
            // 
            // departure
            // 
            departure.HeaderText = "Điểm đi";
            departure.Name = "departure";
            departure.ReadOnly = true;
            // 
            // destination
            // 
            destination.HeaderText = "Điểm đến";
            destination.Name = "destination";
            destination.ReadOnly = true;
            // 
            // coachType
            // 
            coachType.HeaderText = "Loại xe";
            coachType.Name = "coachType";
            coachType.ReadOnly = true;
            // 
            // price
            // 
            price.HeaderText = "Giá vé";
            price.Name = "price";
            price.ReadOnly = true;
            price.Width = 80;
            // 
            // empty
            // 
            empty.HeaderText = "Ghế trống";
            empty.Name = "empty";
            empty.ReadOnly = true;
            empty.Width = 50;
            // 
            // choose
            // 
            choose.HeaderText = "Chọn";
            choose.Name = "choose";
            choose.ReadOnly = true;
            choose.Width = 70;
            // 
            // SearchTripsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(570, 596);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "SearchTripsForm";
            Text = "Hệ thống Quản lý vé xe  - Tìm kiếm chuyến đi";
            Load += SearchTripsForm_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private Label label1;
        private Button SearchTrip;
        private ComboBox comboBox1;
        private Label label4;
        private DateTimePicker dtpDeparture;
        private Label label3;
        private ComboBox cbDestination;
        private Label label2;
        private ComboBox cbDeparture;
        private GroupBox groupBox2;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn departureTime;
        private DataGridViewTextBoxColumn departure;
        private DataGridViewTextBoxColumn destination;
        private DataGridViewTextBoxColumn coachType;
        private DataGridViewTextBoxColumn price;
        private DataGridViewTextBoxColumn empty;
        private DataGridViewTextBoxColumn choose;
    }
}