using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoachReservation
{
    public partial class BookTicketForm : Form
    {
        private const int DemoPassengerId = 1;
        private Trip selectedTrip;
        private SeatMapCatalog seatMapCatalog;
        private SeatCatalog seatCatalog;
        private TicketCatalog ticketCatalog;
        private bool isSeatSelectionConfirmed;
        private List<int> selectedSeats = new List<int>();
        private Dictionary<int, List<Button>> seatGrids;
        private SeatMap currentSeatMap;

        public BookTicketForm(Trip trip, SeatMapCatalog seatMapCatalog, SeatCatalog seatCatalog, TicketCatalog ticketCatalog)
        {
            this.selectedTrip = trip;
            this.seatMapCatalog = seatMapCatalog;
            this.seatCatalog = seatCatalog;
            this.ticketCatalog = ticketCatalog;
            this.seatGrids = new Dictionary<int, List<Button>>();
            InitializeComponent();
            LoadTripInfo();
            LoadDemoPassenger();
            LoadSeatMap();
        }

        private void LoadDemoPassenger()
        {
            Passenger passenger = ticketCatalog.GetPassengerById(DemoPassengerId);
            if (passenger == null)
            {
                txtName.Text = "Demo Passenger";
                txtPhone.Text = "0000000000";
                return;
            }

            txtName.Text = passenger.FullName;
            txtPhone.Text = passenger.PhoneNumber;
        }

        private void LoadTripInfo()
        {
            try
            {
                txtDeparture.Text = selectedTrip.Route.DeparturePoint;
                txtDestination.Text = selectedTrip.Route.Destination;
                txtVehicle.Text = selectedTrip.Vehicle.VehicleType;
                txtDepartureTime.Text = selectedTrip.DepartureTime.ToString(@"hh\:mm");
                txtLicensePlate.Text = selectedTrip.Vehicle.LicensePlate;
                txtDepartureDate.Text = selectedTrip.DepartureDate.ToString("dd/MM/yyyy");
                txtPrice.Text = selectedTrip.BasePrice.ToString("C");
            }
            catch (Exception ex)
            {
                DisplayError("Lỗi khi tải thông tin chuyến đi: " + ex.Message);
            }
        }

        private void LoadSeatMap()
        {
            try
            {
                currentSeatMap = seatMapCatalog.GetSeatMapByVehicleId(selectedTrip.Vehicle.VehicleId);
                if (currentSeatMap == null)
                {
                    DisplayError("Không tìm thấy sơ đồ ghế cho phương tiện này");
                    return;
                }

                DisplaySeatMap(currentSeatMap);
            }
            catch (Exception ex)
            {
                DisplayError("Lỗi khi tải sơ đồ ghế: " + ex.Message);
            }
        }

        private void DisplaySeatMap(SeatMap seatMap)
        {
            tabControlFloors.TabPages.Clear();
            seatGrids.Clear();

            try
            {
                // Get all seats for this seat map
                List<Seat> allSeats = seatCatalog.GetSeatsBySeatMapId(seatMap.SeatMapId);

                // Get booked seats for this trip
                List<int> bookedSeatIds = seatCatalog.GetBookedSeatsForTrip(selectedTrip.TripId);

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

                            Button seatButton = CreateSeatButton(seat, bookedSeatIds);
                            tableLayout.Controls.Add(seatButton, col, row);
                            floorSeats.Add(seatButton);
                        }
                    }

                    tabPage.Controls.Add(tableLayout);
                    tabControlFloors.TabPages.Add(tabPage);
                    seatGrids[floor] = floorSeats;
                }
            }
            catch (Exception ex)
            {
                DisplayError("Lỗi khi tải sơ đồ ghế: " + ex.Message);
            }
        }

        private Button CreateSeatButton(Seat seat, List<int> bookedSeatIds)
        {
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

            button.Text = !string.IsNullOrEmpty(seat.SeatCode) ? seat.SeatCode : "";

            // Determine button color based on seat status
            if (seat.SeatType == "Lối đi")
            {
                button.BackColor = Color.LightGray;
                button.Enabled = false;
            }
            else if (seat.SeatId > 0 && bookedSeatIds.Contains(seat.SeatId))
            {
                // Booked seat - red color
                button.BackColor = Color.Red;
                button.ForeColor = Color.White;
                button.Enabled = false;
            }
            else if (!string.IsNullOrEmpty(seat.SeatCode))
            {
                // Empty seat - green color
                button.BackColor = Color.LightGreen;
                button.Enabled = true;
            }
            else
            {
                button.BackColor = Color.White;
                button.Enabled = false;
            }

            button.Click += PickSeat;
            return button;
        }

        private void PickSeat(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button?.Tag is Seat seat && seat.SeatId > 0 && !string.IsNullOrEmpty(seat.SeatCode))
            {
                if (selectedSeats.Contains(seat.SeatId))
                {
                    // Deselect seat
                    selectedSeats.Remove(seat.SeatId);
                    button.BackColor = Color.LightGreen;
                }
                else
                {
                    // Select seat
                    selectedSeats.Add(seat.SeatId);
                    button.BackColor = Color.Yellow;
                }

                isSeatSelectionConfirmed = false;
                UpdateSeatDisplay();
            }
        }

        private void UpdateSeatDisplay()
        {
            try
            {
                string seatCodes = "";
                decimal totalAmount = 0;

                if (selectedSeats.Count > 0)
                {
                    seatCodes = seatCatalog.GetSeatCodesBySeatIds(selectedSeats);
                    totalAmount = selectedTrip.BasePrice * selectedSeats.Count;
                }

                txtSeats.Text = seatCodes;
                txtAmount.Text = selectedSeats.Count.ToString();
                txtTotal.Text = totalAmount.ToString("C");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi tải sơ đồ ghế: " + ex.Message);
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

        private void PayTicket(object sender, EventArgs e)
        {
            if (selectedSeats.Count == 0)
            {
                DisplayError("Vui lòng chọn ghế.");
                return;
            }

            if (!ticketCatalog.AreSeatsStillAvailable(selectedTrip.TripId, selectedSeats))
            {
                DisplayError("Ghế đã không còn trống. Vui lòng chọn lại ghế khác.");
                selectedSeats.Clear();
                isSeatSelectionConfirmed = false;
                UpdateSeatDisplay();
                LoadSeatMap();
                return;
            }

            DialogResult paymentResult = MessageBox.Show(
                "Mô phỏng thanh toán:\n- Chọn Yes: Thanh toán thành công\n- Chọn No: Thanh toán thất bại",
                "Thanh toán",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (paymentResult == DialogResult.No)
            {
                DisplayError("Thanh toán lỗi. Vui lòng kiểm tra lại số dư hoặc thử phương thức khác!");
                return;
            }

            int ticketId = ticketCatalog.CreateTicketWithSeats(
                selectedTrip.TripId,
                DemoPassengerId,
                selectedSeats,
                selectedTrip.BasePrice);

            if (ticketId <= 0)
            {
                DisplayError("Không thể tạo vé. Vui lòng thử lại.");
                return;
            }

            DisplayInfo($"Đặt vé thành công!\nMã vé: {ticketId}\nĐã gửi vé điện tử (SMS) cho người dùng (demo).");

            selectedSeats.Clear();
            isSeatSelectionConfirmed = false;
            UpdateSeatDisplay();
            LoadSeatMap();
        }
    }
}


