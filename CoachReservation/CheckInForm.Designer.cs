namespace CoachReservation
{
    partial class CheckInForm
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
            btnFind = new Button();
            txtFind = new TextBox();
            label1 = new Label();
            groupBox2 = new GroupBox();
            btnCheckIn = new Button();
            txtStatus = new TextBox();
            label10 = new Label();
            txtTotal = new TextBox();
            label9 = new Label();
            txtDepartureTime = new TextBox();
            label8 = new Label();
            txtSeat = new TextBox();
            label7 = new Label();
            txtLicensePlate = new TextBox();
            label6 = new Label();
            txtDestination = new TextBox();
            label5 = new Label();
            txtDeparturePoint = new TextBox();
            label4 = new Label();
            txtPhoneNumber = new TextBox();
            label3 = new Label();
            txtName = new TextBox();
            label2 = new Label();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnFind);
            groupBox1.Controls.Add(txtFind);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(546, 71);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Tìm kiếm vé";
            // 
            // btnFind
            // 
            btnFind.Location = new Point(451, 29);
            btnFind.Name = "btnFind";
            btnFind.Size = new Size(89, 23);
            btnFind.TabIndex = 2;
            btnFind.Text = "Tìm vé";
            btnFind.UseVisualStyleBackColor = true;
            btnFind.Click += FindTicket;
            // 
            // txtFind
            // 
            txtFind.Location = new Point(87, 30);
            txtFind.Name = "txtFind";
            txtFind.Size = new Size(358, 23);
            txtFind.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 33);
            label1.Name = "label1";
            label1.Size = new Size(42, 15);
            label1.TabIndex = 0;
            label1.Text = "Mã vé:";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(btnCheckIn);
            groupBox2.Controls.Add(txtStatus);
            groupBox2.Controls.Add(label10);
            groupBox2.Controls.Add(txtTotal);
            groupBox2.Controls.Add(label9);
            groupBox2.Controls.Add(txtDepartureTime);
            groupBox2.Controls.Add(label8);
            groupBox2.Controls.Add(txtSeat);
            groupBox2.Controls.Add(label7);
            groupBox2.Controls.Add(txtLicensePlate);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(txtDestination);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(txtDeparturePoint);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(txtPhoneNumber);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(txtName);
            groupBox2.Controls.Add(label2);
            groupBox2.Location = new Point(12, 99);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(546, 485);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Thông tin chi tiết vé";
            groupBox2.Visible = false;
            // 
            // btnCheckIn
            // 
            btnCheckIn.BackColor = Color.Green;
            btnCheckIn.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCheckIn.ForeColor = SystemColors.ButtonFace;
            btnCheckIn.Location = new Point(13, 420);
            btnCheckIn.Name = "btnCheckIn";
            btnCheckIn.Size = new Size(503, 59);
            btnCheckIn.TabIndex = 18;
            btnCheckIn.Text = "XÁC NHẬN LÊN XE";
            btnCheckIn.UseVisualStyleBackColor = false;
            btnCheckIn.Click += CheckIn;
            // 
            // txtStatus
            // 
            txtStatus.Location = new Point(338, 364);
            txtStatus.Name = "txtStatus";
            txtStatus.ReadOnly = true;
            txtStatus.Size = new Size(178, 23);
            txtStatus.TabIndex = 17;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(272, 369);
            label10.Name = "label10";
            label10.Size = new Size(60, 15);
            label10.TabIndex = 16;
            label10.Text = "Trạng thái";
            // 
            // txtTotal
            // 
            txtTotal.Location = new Point(103, 361);
            txtTotal.Name = "txtTotal";
            txtTotal.ReadOnly = true;
            txtTotal.Size = new Size(158, 23);
            txtTotal.TabIndex = 15;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(12, 364);
            label9.Name = "label9";
            label9.Size = new Size(58, 15);
            label9.TabIndex = 14;
            label9.Text = "Tổng tiền";
            // 
            // txtDepartureTime
            // 
            txtDepartureTime.Location = new Point(103, 316);
            txtDepartureTime.Name = "txtDepartureTime";
            txtDepartureTime.ReadOnly = true;
            txtDepartureTime.Size = new Size(413, 23);
            txtDepartureTime.TabIndex = 13;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(12, 319);
            label8.Name = "label8";
            label8.Size = new Size(68, 15);
            label8.TabIndex = 12;
            label8.Text = "Ngày giờ đi";
            // 
            // txtSeat
            // 
            txtSeat.Location = new Point(338, 265);
            txtSeat.Name = "txtSeat";
            txtSeat.ReadOnly = true;
            txtSeat.Size = new Size(178, 23);
            txtSeat.TabIndex = 11;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(272, 268);
            label7.Name = "label7";
            label7.Size = new Size(28, 15);
            label7.TabIndex = 10;
            label7.Text = "Ghế";
            // 
            // txtLicensePlate
            // 
            txtLicensePlate.Location = new Point(103, 265);
            txtLicensePlate.Name = "txtLicensePlate";
            txtLicensePlate.ReadOnly = true;
            txtLicensePlate.Size = new Size(158, 23);
            txtLicensePlate.TabIndex = 9;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(12, 268);
            label6.Name = "label6";
            label6.Size = new Size(45, 15);
            label6.TabIndex = 8;
            label6.Text = "Biển số";
            // 
            // txtDestination
            // 
            txtDestination.Location = new Point(103, 210);
            txtDestination.Name = "txtDestination";
            txtDestination.ReadOnly = true;
            txtDestination.Size = new Size(413, 23);
            txtDestination.TabIndex = 7;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 213);
            label5.Name = "label5";
            label5.Size = new Size(58, 15);
            label5.TabIndex = 6;
            label5.Text = "Điểm đến";
            // 
            // txtDeparturePoint
            // 
            txtDeparturePoint.Location = new Point(103, 153);
            txtDeparturePoint.Name = "txtDeparturePoint";
            txtDeparturePoint.ReadOnly = true;
            txtDeparturePoint.Size = new Size(413, 23);
            txtDeparturePoint.TabIndex = 5;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 156);
            label4.Name = "label4";
            label4.Size = new Size(48, 15);
            label4.TabIndex = 4;
            label4.Text = "Điểm đi";
            // 
            // txtPhoneNumber
            // 
            txtPhoneNumber.Location = new Point(103, 97);
            txtPhoneNumber.Name = "txtPhoneNumber";
            txtPhoneNumber.ReadOnly = true;
            txtPhoneNumber.Size = new Size(413, 23);
            txtPhoneNumber.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 100);
            label3.Name = "label3";
            label3.Size = new Size(76, 15);
            label3.TabIndex = 2;
            label3.Text = "Số điện thoại";
            // 
            // txtName
            // 
            txtName.Location = new Point(103, 44);
            txtName.Name = "txtName";
            txtName.ReadOnly = true;
            txtName.Size = new Size(413, 23);
            txtName.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 47);
            label2.Name = "label2";
            label2.Size = new Size(60, 15);
            label2.TabIndex = 0;
            label2.Text = "Họ và Tên";
            // 
            // CheckInForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(570, 596);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "CheckInForm";
            Text = "Hệ thống Quản lý vé xe - Check-in vé";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private Button btnFind;
        private TextBox txtFind;
        private Label label1;
        private GroupBox groupBox2;
        private TextBox txtPhoneNumber;
        private Label label3;
        private TextBox txtName;
        private Label label2;
        private Label label4;
        private TextBox txtDestination;
        private Label label5;
        private TextBox txtDeparturePoint;
        private TextBox txtDepartureTime;
        private Label label8;
        private TextBox txtSeat;
        private Label label7;
        private TextBox txtLicensePlate;
        private Label label6;
        private Button btnCheckIn;
        private TextBox txtStatus;
        private Label label10;
        private TextBox txtTotal;
        private Label label9;
    }
}