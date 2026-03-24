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
            // Create or update selected seat
            selectedSeat = new Seat
            {
                Floor = floor,
                RowIndex = row,
                ColumnIndex = column,
                SeatCode = button.Text
            };

            // Update UI
            txtSeatCode.Text = selectedSeat.SeatCode;
            
            // Find existing seat type
            var existingSeat = allSeats.FirstOrDefault(s => s.Floor == floor && s.RowIndex == row && s.ColumnIndex == column);
            if (existingSeat != null)
            {
                cbType.SelectedItem = existingSeat.SeatType;
            }
            else
            {
                cbType.SelectedIndex = 0;
            }
        }

        private void SaveSelection_Click(object sender, EventArgs e)
        {
            if (selectedSeat == null)
            {
                MessageBox.Show("Vui lòng chọn một ô trước", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            selectedSeat.SeatCode = txtSeatCode.Text;
            selectedSeat.SeatType = cbType.SelectedItem?.ToString() ?? "Ghế";

            if (cbVehicle.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn phương tiện", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Vehicle selectedVehicle = vehicles[cbVehicle.SelectedIndex];
            int floors = (int)nbFloor.Value;
            int columns = (int)nbColumn.Value;
            int rows = (int)nbRow.Value;

            // Create or get SeatMapId
            int seatMapId = seatMapCatalog.CreateOrGetSeatMapForVehicle(selectedVehicle.VehicleId, floors, columns, rows);

            if (seatMapId == -1)
            {
                MessageBox.Show("Lỗi khi tạo/lấy cấu hình sơ đồ ghế", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Save the seat to database
            bool success = seatMapCatalog.SaveSingleSeat(seatMapId, selectedSeat);

            if (success)
            {
                // Update or add to allSeats list
                var existingSeat = allSeats.FirstOrDefault(s => s.Floor == selectedSeat.Floor && s.RowIndex == selectedSeat.RowIndex && s.ColumnIndex == selectedSeat.ColumnIndex);
                if (existingSeat != null)
                {
                    existingSeat.SeatCode = selectedSeat.SeatCode;
                    existingSeat.SeatType = selectedSeat.SeatType;
                }
                else
                {
                    allSeats.Add(new Seat
                    {
                        Floor = selectedSeat.Floor,
                        RowIndex = selectedSeat.RowIndex,
                        ColumnIndex = selectedSeat.ColumnIndex,
                        SeatCode = selectedSeat.SeatCode,
                        SeatType = selectedSeat.SeatType
                    });
                }

                // Cập nhật hiển thị
                RefreshSeatGridDisplay();

                MessageBox.Show($"Đã lưu: {selectedSeat.SeatCode} - Loại: {selectedSeat.SeatType}", "Thông báo");
            }
            else
            {
                MessageBox.Show("Lỗi khi lưu ô ghế", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            MessageBox.Show("Cấu hình sơ đồ ghế đã được lưu thành công", "Thông báo");
        }
    }
}
