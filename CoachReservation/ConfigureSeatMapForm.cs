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
        private SeatMapCatalog seatMapCatalog;
        private VehicleCatalog vehicleCatalog;
        private SeatCatalog seatCatalog;
        private SeatMap currentSeatMap;
        private Dictionary<int, List<Button>> seatGrids;
        private Button selectedSeatButton;
        private Seat selectedSeat;

        public ConfigureSeatMapForm(SeatMapCatalog seatMapCatalog, VehicleCatalog vehicleCatalog, SeatCatalog seatCatalog)
        {
            this.seatMapCatalog = seatMapCatalog;
            this.vehicleCatalog = vehicleCatalog;
            this.seatCatalog = seatCatalog;
            seatGrids = new Dictionary<int, List<Button>>();
            InitializeComponent();
            LoadVehicles();
        }

        private void LoadVehicles()
        {
            try
            {
                List<Vehicle> vehicles = vehicleCatalog.GetAllVehicles();
                cbVehicle.DataSource = vehicles;
                cbVehicle.DisplayMember = "LicensePlate";
                cbVehicle.ValueMember = "VehicleId";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách phương tiện: " + ex.Message);
            }
        }

        private void DisplayError(string message)
        {
            MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void DisplayInfo(string message)
        {
            MessageBox.Show(message, "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void InitSeatGrid(object sender, EventArgs e)
        {
            if (cbVehicle.SelectedValue == null)
            {
                DisplayError("Vui lòng chọn phương tiện");
                return;
            }

            try
            {
                int vehicleId = (int)cbVehicle.SelectedValue;
                currentSeatMap = seatMapCatalog.GetSeatMapByVehicleId(vehicleId);

                if (currentSeatMap != null)
                {
                    // Load existing seat map
                    DisplayExistingSeatMap(currentSeatMap);
                }
                else
                {
                    // Create new seat map
                    CreateNewSeatMap(vehicleId);
                }
            }
            catch (Exception ex)
            {
                DisplayError("Lỗi khởi tạo lưới ghế: " + ex.Message);
            }
        }

        private void DisplayExistingSeatMap(SeatMap seatMap)
        {
            // Clear existing tabs
            tabControlFloors.TabPages.Clear();
            seatGrids.Clear();

            // Create tabs for each floor
            List<Seat> allSeats = seatCatalog.GetSeatsBySeatMapId(seatMap.SeatMapId);

            for (int floor = 1; floor <= seatMap.Floors; floor++)
            {
                TabPage tabPage = new TabPage($"Tầng {floor}");
                TableLayoutPanel tableLayout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    RowCount = seatMap.GridRows,
                    ColumnCount = seatMap.GridColumns,
                    AutoSize = true
                };

                List<Button> floorSeats = new List<Button>();

                for (int row = 0; row < seatMap.GridRows; row++)
                {
                    tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / seatMap.GridRows));

                    for (int col = 0; col < seatMap.GridColumns; col++)
                    {
                        if (row == 0)
                        {
                            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / seatMap.GridColumns));
                        }

                        Seat seat = allSeats.FirstOrDefault(s => 
                            s.Floor == floor && s.RowIndex == row && s.ColumnIndex == col);

                        if (seat == null)
                        {
                            seat = new Seat(seatMap, "", floor, row, col, "");
                        }

                        Button seatButton = CreateSeatButton(seat);
                        tableLayout.Controls.Add(seatButton, col, row);
                        floorSeats.Add(seatButton);
                    }
                }

                tabPage.Controls.Add(tableLayout);
                tabControlFloors.TabPages.Add(tabPage);
                seatGrids[floor] = floorSeats;
            }
        }

        private void CreateNewSeatMap(int vehicleId)
        {
            int floors = (int)nbFloor.Value;
            int columns = (int)nbColumn.Value;
            int rows = (int)nbRow.Value;

            if (floors <= 0 || columns <= 0 || rows <= 0)
            {
                DisplayError("Vui lòng nhập số tầng, dãy và hàng > 0");
                return;
            }

            // Get vehicle
            Vehicle selectedVehicle = vehicleCatalog.GetVehicleById(vehicleId);
            if (selectedVehicle == null)
            {
                DisplayError("Không tìm thấy phương tiện");
                return;
            }

            // Create SeatMap object
            currentSeatMap = new SeatMap(selectedVehicle, floors, columns, rows);

            // Clear existing tabs
            tabControlFloors.TabPages.Clear();
            seatGrids.Clear();

            // Create tabs for each floor
            for (int floor = 1; floor <= floors; floor++)
            {
                TabPage tabPage = new TabPage($"Tầng {floor}");
                TableLayoutPanel tableLayout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    RowCount = rows,
                    ColumnCount = columns,
                    AutoSize = true
                };

                List<Button> floorSeats = new List<Button>();

                for (int row = 0; row < rows; row++)
                {
                    tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / rows));

                    for (int col = 0; col < columns; col++)
                    {
                        if (row == 0)
                        {
                            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / columns));
                        }

                        Seat newSeat = new Seat(currentSeatMap, "", floor, row, col, "");

                        Button seatButton = CreateSeatButton(newSeat);
                        tableLayout.Controls.Add(seatButton, col, row);
                        floorSeats.Add(seatButton);
                    }
                }

                tabPage.Controls.Add(tableLayout);
                tabControlFloors.TabPages.Add(tabPage);
                seatGrids[floor] = floorSeats;
            }
        }

        private Button CreateSeatButton(Seat seat)
        {
            // Create a default seat if null
            if (seat == null)
            {
                seat = new Seat("", "");
            }

            Button button = new Button
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(2),
                Font = new Font("Arial", 8),
                Tag = seat
            };

            // Display seat code and set button appearance based on seat type
            button.Text = !string.IsNullOrEmpty(seat.SeatCode) ? seat.SeatCode : "";
            
            if (!string.IsNullOrEmpty(seat.SeatType))
            {
                button.BackColor = seat.SeatType == "Lối đi" ? Color.LightGray : Color.LightGreen;
            }
            else
            {
                button.BackColor = Color.White;
            }

            button.Click += SeatButton;
            return button;
        }

        private void SeatButton(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button?.Tag is Seat seat)
            {
                selectedSeatButton = button;
                selectedSeat = seat;

                // Update properties panel
                cbType.SelectedItem = seat.SeatType;
                txtSeatCode.Text = seat.SeatCode;
            }
        }

        private void SaveSelection(object sender, EventArgs e)
        {
            if (selectedSeat == null)
            {
                DisplayError("Vui lòng chọn một ghế");
                return;
            }

            try
            {
                selectedSeat.SeatType = cbType.SelectedItem?.ToString() ?? "";
                selectedSeat.SeatCode = txtSeatCode.Text;

                // Update button appearance and text
                selectedSeatButton.Text = !string.IsNullOrEmpty(selectedSeat.SeatCode) ? selectedSeat.SeatCode : "";
                
                if (!string.IsNullOrEmpty(selectedSeat.SeatType))
                {
                    selectedSeatButton.BackColor = selectedSeat.SeatType == "Lối đi" ? Color.LightGray : Color.LightGreen;
                }
                else
                {
                    selectedSeatButton.BackColor = Color.White;
                }

                DisplayInfo("Lưu ô thành công");
            }
            catch (Exception ex)
            {
                DisplayError("Lỗi lưu ô: " + ex.Message);
            }
        }

        private void SaveSeatConfiguration(object sender, EventArgs e)
        {
            if (currentSeatMap == null)
            {
                DisplayError("Vui lòng khởi tạo lưới ghế trước");
                return;
            }

            try
            {
                // Collect all seats from the grid
                List<Seat> allSeats = new List<Seat>();
                foreach (var kvp in seatGrids)
                {
                    foreach (Button button in kvp.Value)
                    {
                        if (button.Tag is Seat seat)
                        {
                            seat.SeatMap = currentSeatMap;
                            allSeats.Add(seat);
                        }
                    }
                }

                // Validate seat codes before saving
                var duplicateSeatCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                var seenSeatCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                var emptySeats = new List<string>();

                foreach (var seat in allSeats)
                {
                    bool isWalkway = string.Equals(seat.SeatType, "Lối đi", StringComparison.OrdinalIgnoreCase);
                    string normalizedCode = (seat.SeatCode ?? string.Empty).Trim();

                    if (isWalkway)
                    {
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(normalizedCode))
                    {
                        emptySeats.Add($"Tầng {seat.Floor} - Hàng {seat.RowIndex + 1} - Cột {seat.ColumnIndex + 1}");
                        continue;
                    }

                    if (!seenSeatCodes.Add(normalizedCode))
                    {
                        duplicateSeatCodes.Add(normalizedCode);
                    }

                    seat.SeatCode = normalizedCode;
                }

                if (emptySeats.Count > 0)
                {
                    DisplayError("Có ghế chưa nhập mã:\n" + string.Join("\n", emptySeats));
                    return;
                }

                if (duplicateSeatCodes.Count > 0)
                {
                    DisplayError("Mã ghế bị trùng: " + string.Join(", ", duplicateSeatCodes));
                    return;
                }

                // Save seat map to database
                int seatMapId = seatMapCatalog.SaveSeatMap(currentSeatMap);
                if (seatMapId <= 0)
                {
                    DisplayError("Lỗi lưu sơ đồ ghế");
                    return;
                }
                currentSeatMap.SeatMapId = seatMapId;

                // Save all seats to database
                seatCatalog.SaveSeats(allSeats);
                DisplayInfo("Lưu cấu hình ghế thành công");
            }
            catch (Exception ex)
            {
                DisplayError("Lỗi lưu cấu hình: " + ex.Message);
            }
        }
    }
}
