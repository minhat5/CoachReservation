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
    public partial class CheckInForm : Form
    {
        private TicketCatalog ticketCatalog;
        private Ticket ticket;

        public CheckInForm()
        {
            InitializeComponent();
            ticketCatalog = new TicketCatalog();
        }

        private void FindTicket_Click(object sender, EventArgs e)
        {
            if (!ValidateFindTicket())
            {
                return;
            }
            DisplayInfo();
        }

        private void DisplayInfo()
        {
            txtName.Text = ticket.Passenger.FullName;
            txtPhoneNumber.Text = ticket.Passenger.PhoneNumber;
            txtDeparturePoint.Text = ticket.Trip.Route.DeparturePoint;
            txtDestination.Text = ticket.Trip.Route.Destination;
            txtDepartureTime.Text = ticket.Trip.DepartureTime.ToString(@"hh\:mm") + " ngày " + ticket.Trip.DepartureDate.ToString("dd/MM/yyyy");
            txtLicensePlate.Text = ticket.Trip.Vehicle.LicensePlate;
            txtTotal.Text = ticket.TotalAmount.ToString("C");
            txtStatus.Text = ticket.TicketStatus;
            List<string> seats = ticketCatalog.GetSeatsByTicket(ticket.TicketId);
            txtSeat.Text = string.Join(", ", seats);
        }

        private bool ValidateFindTicket()
        {
            if (String.IsNullOrEmpty(txtFind.Text.Trim()))
            {
                DisplayError("Vui lòng nhập mã vé!");
                return false;
            }

            if (!int.TryParse(txtFind.Text.Trim(), out int id))
            {
                DisplayError("Vui lòng nhập mã vé là số hợp lệ!");
                return false;
            }
            ticket = ticketCatalog.FindTicket(id);
            if (ticket == null)
            {
                DisplayError("Không tìm thấy thông tin vé. Vui lòng kiểm tra lại.");
                return false;
            }
            if (ticket.TicketStatus == "Đã lên xe")
            {
                DisplayError("Lỗi: Vé này đã được Check-in trước đó.");
                return false;
            }
            if (ticket.Trip.DepartureDate.Date < DateTime.Now.Date || (ticket.Trip.DepartureDate.Date == DateTime.Now.Date && ticket.Trip.DepartureTime < DateTime.Now.TimeOfDay))
            {
                DisplayError("Vé đã quá ngày/giờ khởi hành.");
                return false;
            }
            return true;
        }

        private void DisplayError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private bool ValidateCheckIn()
        {
            if (ticket == null)
            {
                DisplayError("Vui lòng tìm kiếm vé trước khi Check-in!");
                return false;
            }
            return true;
        }

        private void ClearInfo()
        {
            txtName.Text = "";
            txtPhoneNumber.Text = "";
            txtDeparturePoint.Text = "";
            txtDestination.Text = "";
            txtDepartureTime.Text = "";
            txtLicensePlate.Text = "";
            txtTotal.Text = "";
            txtStatus.Text = "";
            txtSeat.Text = "";
        }
        private void CheckIn_Click(object sender, EventArgs e)
        {
            if (!ValidateCheckIn())
            {
                return;
            }
            ticket.TicketStatus = "Đã lên xe";
            try
            {
                ticketCatalog.UpdateTicketStatus(ticket.TicketId, ticket.TicketStatus);
                MessageBox.Show("Check-in thành công!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearInfo();
            }
            catch (Exception ex)
            {
                DisplayError("Lỗi khi cập nhật trạng thái vé: " + ex.Message);
            }
        }
    }
}
