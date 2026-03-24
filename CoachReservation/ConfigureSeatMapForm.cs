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
        private SeatMap currentSeatMap;
        private Dictionary<int, List<Button>> seatGrids; // floor -> buttons
        private Button selectedSeatButton;
        private Seat selectedSeat;

        public ConfigureSeatMapForm(SeatMapCatalog seatMapCatalog, VehicleCatalog vehicleCatalog)
        {
            this.seatMapCatalog = seatMapCatalog;
            this.vehicleCatalog = vehicleCatalog;
            seatGrids = new Dictionary<int, List<Button>>();
            InitializeComponent();
            LoadVehicles();
            SetupEventHandlers();
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

        private void SetupEventHandlers()
        {
            InitSeatGrid.Click += InitSeatGrid_Click;
            SaveSeatConfiguration.Click += SaveSeatConfiguration_Click;
            SaveSelection.Click += SaveSelection_Click;
        }

        private void InitSeatGrid_Click(object sender, EventArgs e)
        {
            if (cbVehicle.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn phương tiện");
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
                MessageBox.Show("Lỗi khởi tạo lưới ghế: " + ex.Message);
            }
        }

        private void DisplayExistingSeatMap(SeatMap seatMap)
        {
            // Clear existing tabs
            tabControlFloors.TabPages.Clear();
            seatGrids.Clear();

            // Create tabs for each floor
            List<Seat> allSeats = seatMapCatalog.GetSeatsBySeatMapId(seatMap.SeatMapId);

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
                MessageBox.Show("Vui lòng nhập số tầng, dãy và hàng > 0");
                return;
            }

            // Get vehicle
            Vehicle selectedVehicle = vehicleCatalog.GetVehicleById(vehicleId);
            if (selectedVehicle == null)
            {
                MessageBox.Show("Không tìm thấy phương tiện");
                return;
            }

            // Create SeatMap object
            currentSeatMap = new SeatMap
            {
                Vehicle = selectedVehicle,
                Floors = floors,
                GridColumns = columns,
                GridRows = rows
            };

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

                        Seat newSeat = new Seat
                        {
                            SeatMap = currentSeatMap,
                            Floor = floor,
                            RowIndex = row,
                            ColumnIndex = col,
                            SeatCode = "",
                            SeatType = ""
                        };

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
                seat = new Seat
                {
                    SeatCode = "",
                    SeatType = ""
                };
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

            button.Click += SeatButton_Click;
            return button;
        }

        private void SeatButton_Click(object sender, EventArgs e)
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

        private void SaveSelection_Click(object sender, EventArgs e)
        {
            if (selectedSeat == null)
            {
                MessageBox.Show("Vui lòng chọn một ghế");
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

                MessageBox.Show("Lưu ô thành công");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu ô: " + ex.Message);
            }
        }

        private void SaveSeatConfiguration_Click(object sender, EventArgs e)
        {
            if (currentSeatMap == null)
            {
                MessageBox.Show("Vui lòng khởi tạo lưới ghế trước");
                return;
            }

            try
            {
                // Save seat map to database
                int seatMapId = seatMapCatalog.SaveSeatMap(currentSeatMap);
                if (seatMapId <= 0)
                {
                    MessageBox.Show("Lỗi lưu sơ đồ ghế");
                    return;
                }
                currentSeatMap.SeatMapId = seatMapId;

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

                // Save all seats to database
                seatMapCatalog.SaveSeats(allSeats);

                MessageBox.Show("Lưu cấu hình ghế thành công");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu cấu hình: " + ex.Message);
            }
        }
    }
}
