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
    public partial class SearchTripsForm : Form
    {
        private RouteCatalog routeCatalog;
        private VehicleCatalog vehicleCatalog;
        private TripCatalog tripCatalog;
        private SeatMapCatalog seatMapCatalog;
        private SeatCatalog seatCatalog;
        private TicketCatalog ticketCatalog;
        private List<Trip> searchResults;

        public SearchTripsForm(RouteCatalog routeCatalog, VehicleCatalog vehicleCatalog, TripCatalog tripCatalog, SeatMapCatalog seatMapCatalog, SeatCatalog seatCatalog, TicketCatalog ticketCatalog)
        {
            this.routeCatalog = routeCatalog;
            this.vehicleCatalog = vehicleCatalog;
            this.tripCatalog = tripCatalog;
            this.seatMapCatalog = seatMapCatalog;
            this.seatCatalog = seatCatalog;
            this.ticketCatalog = ticketCatalog;
            this.searchResults = new List<Trip>();
            InitializeComponent();

        }

        private void SearchTrip(object sender, EventArgs e)
        {
            if (dtpDeparture.Value.Date < DateTime.Now.Date)
            {
                DisplayError("Vui lòng chọn ngày trong tương lai!");
                return;
            }

            string departurePoint = cbDeparture.SelectedItem?.ToString();
            string destination = cbDestination.SelectedItem?.ToString();
            DateTime departureDate = dtpDeparture.Value.Date;
            int requiredSeats = int.Parse(comboBox1.SelectedItem?.ToString() ?? "1");

            searchResults = tripCatalog.SearchTrips(departurePoint, destination, departureDate);

            if (searchResults.Count == 0)
            {
                DisplayError("Không tìm thấy chuyến đi phù hợp.");
                dataGridView1.Rows.Clear();
                return;
            }

            // Filter results based on available seats
            var filteredResults = new List<Trip>();
            foreach (Trip trip in searchResults)
            {
                int emptySeats = tripCatalog.GetEmptySeatsCount(trip.TripId);
                if (emptySeats >= requiredSeats)
                {
                    filteredResults.Add(trip);
                }
            }

            if (filteredResults.Count == 0)
            {
                DisplayError($"Không tìm thấy chuyến đi có đủ {requiredSeats} ghế.");
                dataGridView1.Rows.Clear();
                return;
            }

            searchResults = filteredResults;
            DisplaySearchResults();
        }

        private void DisplayError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void DisplaySearchResults()
        {
            dataGridView1.Rows.Clear();

            foreach (Trip trip in searchResults)
            {
                int emptySeats = tripCatalog.GetEmptySeatsCount(trip.TripId);
                
                int rowIndex = dataGridView1.Rows.Add(
                    trip.DepartureTime.ToString(@"hh\:mm"),
                    trip.Route.DeparturePoint,
                    trip.Route.Destination,
                    trip.Vehicle.VehicleType,
                    trip.BasePrice.ToString("C"),
                    emptySeats,
                    "Đặt vé"
                );

                dataGridView1.Rows[rowIndex].Tag = trip.TripId;
            }

            dataGridView1.CellClick += BookTicket;
        }

        private void BookTicket(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 6 && e.RowIndex >= 0)
            {
                int tripId = (int)dataGridView1.Rows[e.RowIndex].Tag;
                Trip selectedTrip = searchResults.FirstOrDefault(t => t.TripId == tripId);

                if (selectedTrip != null)
                {
                    BookTicketForm bookTicketForm = new BookTicketForm(selectedTrip, tripCatalog, seatMapCatalog, seatCatalog, ticketCatalog);
                    bookTicketForm.ShowDialog();
                }
            }
        }

        private void SearchTripsForm_Load(object sender, EventArgs e)
        {
            LoadRoutes();
        }

        private void LoadRoutes()
        {
            try
            {
                List<string> departurePoints = routeCatalog.GetUniqueDeparturePoints();

                if (departurePoints.Count > 0)
                {
                    cbDeparture.DataSource = departurePoints;
                    cbDeparture.SelectedIndexChanged += CbDeparture_SelectedIndexChanged;

                    CbDeparture_SelectedIndexChanged(null, null);

                    comboBox1.Items.Clear();
                    for (int i = 1; i <= vehicleCatalog.GetMaxSeats(); i++)
                    {
                        comboBox1.Items.Add(i);
                    }
                    if (comboBox1.Items.Count > 0)
                    {
                        comboBox1.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayError("Error loading routes: " + ex.Message);
            }
        }

        private void CbDeparture_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbDeparture.SelectedItem == null)
                return;

            string selectedDeparture = cbDeparture.SelectedItem.ToString();
            List<string> destinations = routeCatalog.GetDestinationsByDeparture(selectedDeparture);

            cbDestination.DataSource = destinations;
        }
    }
}

