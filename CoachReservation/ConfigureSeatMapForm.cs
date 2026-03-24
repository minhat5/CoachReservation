using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CoachReservation
{
    public partial class ConfigureSeatMapForm : Form
    {
        private Dictionary<int, Button[,]> floorGrids = new();
        private Seat selectedSeat;
        private SeatMapCatalog seatMapCatalog;
        private VehicleCatalog vehicleCatalog;
        private List<Seat> allSeats = new();
        private SeatMap currentSeatMap;
        private List<Vehicle> vehicles = new();

        public ConfigureSeatMapForm(SeatMapCatalog seatMapCatalog, VehicleCatalog vehicleCatalog)
        {
            this.seatMapCatalog = seatMapCatalog;
            this.vehicleCatalog = vehicleCatalog;
            InitializeComponent();
            InitializeComboBoxes();
            LoadVehicles();
        }

        private void InitializeComboBoxes()
        {
            cbType.Items.Add("Ghế");
            cbType.Items.Add("Lối đi");
            cbType.SelectedIndex = 0;
        }

        private void LoadVehicles()
        {
            try
            {
                vehicles = vehicleCatalog.GetAllVehicles();

                cbVehicle.Items.Clear();
                foreach (Vehicle vehicle in vehicles)
                {
                    cbVehicle.Items.Add($"{vehicle.LicensePlate} - {vehicle.VehicleType}");
                }

                cbVehicle.DisplayMember = "Text";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách phương tiện: " + ex.Message);
            }
        }

        private void CbVehicle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbVehicle.SelectedIndex == -1)
                return;

            Vehicle selectedVehicle = vehicles[cbVehicle.SelectedIndex];

            // Try to load existing SeatMap
            currentSeatMap = seatMapCatalog.GetSeatMapByVehicleId(selectedVehicle.VehicleId);

            if (currentSeatMap != null)
            {
                nbFloor.Value = currentSeatMap.Floors;
                nbColumn.Value = currentSeatMap.GridColumns;
                nbRow.Value = currentSeatMap.GridRows;
            }
            else
            {
                // Reset to default values
                nbFloor.Value = 1;
                nbColumn.Value = 10;
                nbRow.Value = 10;
            }
        }

        private void InitSeatGrid_Click(object sender, EventArgs e)
        {
            if (cbVehicle.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn phương tiện", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Vehicle selectedVehicle = vehicles[cbVehicle.SelectedIndex];

            // Check if SeatMap exists for this vehicle
            currentSeatMap = seatMapCatalog.GetSeatMapByVehicleId(selectedVehicle.VehicleId);

            if (currentSeatMap != null)
            {
                // Load existing configuration
                nbFloor.Value = currentSeatMap.Floors;
                nbColumn.Value = currentSeatMap.GridColumns;
                nbRow.Value = currentSeatMap.GridRows;

                // Load seats from database
                allSeats = seatMapCatalog.GetSeatsBySeatMapId(currentSeatMap.SeatMapId);
                DisplaySeatGrid(currentSeatMap.Floors, (int)nbColumn.Value, (int)nbRow.Value);
            }
            else
            {
                // Create new configuration
                int floors = (int)nbFloor.Value;
                int columns = (int)nbColumn.Value;
                int rows = (int)nbRow.Value;

                if (floors <= 0 || columns <= 0 || rows <= 0)
                {
                    MessageBox.Show("Số tầng, dãy, hàng phải lớn hơn 0", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                allSeats = new List<Seat>();
                DisplaySeatGrid(floors, columns, rows);
            }
        }

        private void DisplaySeatGrid(int floors, int columns, int rows)
        {
            tabControlFloors.TabPages.Clear();
            floorGrids.Clear();

            // Tạo TabPages cho mỗi tầng
            for (int floor = 1; floor <= floors; floor++)
            {
                TabPage tabPage = new TabPage($"Tầng {floor}");
                tabControlFloors.TabPages.Add(tabPage);

                Panel panel = new Panel();
                panel.Dock = DockStyle.Fill;
                panel.AutoScroll = true;
                panel.BackColor = SystemColors.Control;
                tabPage.Controls.Add(panel);

                Button[,] gridButtons = new Button[rows, columns];
                int buttonSize = 40;
                int padding = 5;

                // Tạo grid button cho tầng này
                for (int row = 0; row < rows; row++)
                {
                    for (int col = 0; col < columns; col++)
                    {
                        Button btn = new Button();
                        btn.Width = buttonSize;
                        btn.Height = buttonSize;
                        btn.Left = col * (buttonSize + padding) + 10;
                        btn.Top = row * (buttonSize + padding) + 10;
                        btn.Text = $"{row + 1}{(char)('A' + col)}";
                        btn.BackColor = Color.White;
                        btn.ForeColor = Color.Black;

                        // Check if this seat exists in database
                        var existingSeat = allSeats.FirstOrDefault(s => s.Floor == floor && s.RowIndex == row && s.ColumnIndex == col);
                        if (existingSeat != null)
                        {
                            if (existingSeat.SeatType == "Lối đi")
                            {
                                btn.BackColor = Color.Gray;
                            }
                            else
                            {
                                btn.BackColor = Color.LightBlue;
                            }
                        }

                        btn.Click += (s, args) => SeatButton_Click(btn, floor, row, col);
                        panel.Controls.Add(btn);
                        gridButtons[row, col] = btn;
                    }
                }

                floorGrids[floor] = gridButtons;
            }
        }

        private void SeatButton_Click(Button button, int floor, int row, int column)
        {
            // Kiểm tra ghế đã tồn tại trong database chưa
            var existingSeat = allSeats.FirstOrDefault(s => s.Floor == floor && s.RowIndex == row && s.ColumnIndex == column);
            
            if (existingSeat != null)
            {
                // Copy toàn bộ từ existing seat
                selectedSeat = new Seat
                {
                    SeatId = existingSeat.SeatId,
                    SeatCode = existingSeat.SeatCode,
                    Floor = existingSeat.Floor,
                    RowIndex = existingSeat.RowIndex,
                    ColumnIndex = existingSeat.ColumnIndex,
                    SeatType = existingSeat.SeatType
                };
            }
            else
            {
                // Tạo mới nếu chưa tồn tại
                selectedSeat = new Seat
                {
                    Floor = floor,
                    RowIndex = row,
                    ColumnIndex = column,
                    SeatCode = button.Text,
                    SeatType = "Ghế"  // Default
                };
            }

            // Update UI
            txtSeatCode.Text = selectedSeat.SeatCode;
            cbType.SelectedItem = selectedSeat.SeatType;
        }

        private void SaveSelection_Click(object sender, EventArgs e)
        {
            try
            {
                // 0️⃣ Kiểm tra đã chọn ghế hay chưa
                if (selectedSeat == null)
                {
                    MessageBox.Show("Vui lòng chọn một ghế trước", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 1️⃣ Lấy thông tin ô ghế đã chọn
                selectedSeat.SeatCode = txtSeatCode.Text;
                selectedSeat.SeatType = cbType.SelectedItem?.ToString() ?? "Ghế";

                // 2️⃣ Lấy xe đã chọn
                if (cbVehicle.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng chọn phương tiện", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Vehicle selectedVehicle = vehicles[cbVehicle.SelectedIndex];
                int floors = (int)nbFloor.Value;
                int columns = (int)nbColumn.Value;
                int rows = (int)nbRow.Value;

                // 3️⃣ Tạo hoặc lấy SeatMap từ DB (nếu chưa có)
                int seatMapId = seatMapCatalog.CreateOrGetSeatMapForVehicle(
                    selectedVehicle.VehicleId, floors, columns, rows
                );

                if (seatMapId == -1)
                {
                    MessageBox.Show("Lỗi khi tạo sơ đồ ghế", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 4️⃣ Lưu 1 ô ghế vào DB
                bool success = seatMapCatalog.SaveSingleSeat(seatMapId, selectedSeat);

                // 5️⃣ Cập nhật danh sách local + hiển thị
                if (success)
                {
                    // Kiểm tra ghế đã tồn tại trong danh sách chưa
                    var existingSeat = allSeats.FirstOrDefault(s => 
                        s.Floor == selectedSeat.Floor && 
                        s.RowIndex == selectedSeat.RowIndex && 
                        s.ColumnIndex == selectedSeat.ColumnIndex);

                    if (existingSeat != null)
                    {
                        // Cập nhật ghế cũ
                        existingSeat.SeatCode = selectedSeat.SeatCode;
                        existingSeat.SeatType = selectedSeat.SeatType;
                    }
                    else
                    {
                        // Thêm ghế mới
                        allSeats.Add(selectedSeat);
                    }

                    RefreshSeatGridDisplay();
                    MessageBox.Show($"Đã lưu: {selectedSeat.SeatCode}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Lỗi khi lưu ghế. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshSeatGridDisplay()
        {
            foreach (var floor in floorGrids.Keys)
            {
                var gridButtons = floorGrids[floor];
                for (int row = 0; row < gridButtons.GetLength(0); row++)
                {
                    for (int col = 0; col < gridButtons.GetLength(1); col++)
                    {
                        var btn = gridButtons[row, col];
                        var seat = allSeats.FirstOrDefault(s => s.Floor == floor && s.RowIndex == row && s.ColumnIndex == col);
                        
                        if (seat != null)
                        {
                            btn.BackColor = seat.SeatType == "Lối đi" ? Color.Gray : Color.LightBlue;
                        }
                        else
                        {
                            btn.BackColor = Color.White;
                        }
                        btn.ForeColor = Color.Black;
                    }
                }
            }
        }

        private void SaveSeatConfiguration_Click(object sender, EventArgs e)
        {
            // 1️⃣ Kiểm tra xe được chọn
            if (cbVehicle.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn phương tiện", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2️⃣ Kiểm tra có ghế được cấu hình
            if (allSeats.Count == 0)
            {
                MessageBox.Show("Vui lòng cấu hình ít nhất một ghế", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 3️⃣ Lấy thông tin xe và cấu hình
            Vehicle selectedVehicle = vehicles[cbVehicle.SelectedIndex];
            int floors = (int)nbFloor.Value;
            int columns = (int)nbColumn.Value;
            int rows = (int)nbRow.Value;

            // 4️⃣ Hỏi xác nhận lưu
            DialogResult result = MessageBox.Show(
                $"Lưu cấu hình xe {selectedVehicle.LicensePlate}?\n" +
                $"Tổng ghế: {allSeats.Count}\n" +
                $"Cấu hình: {floors} tầng, {columns} dãy, {rows} hàng",
                "Xác nhận", MessageBoxButtons.OKCancel
            );

            if (result != DialogResult.OK) return;

            // 5️⃣ LƯU VÀO DATABASE
            try
            {
                seatMapCatalog.SaveSeatMapConfiguration(
                    selectedVehicle.VehicleId,
                    floors, columns, rows,
                    allSeats
                );

                MessageBox.Show("Lưu cấu hình ghế thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Reset form
                cbVehicle.SelectedIndex = -1;
                allSeats.Clear();
                tabControlFloors.TabPages.Clear();
                floorGrids.Clear();
                txtSeatCode.Clear();
                cbType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu cấu hình: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
