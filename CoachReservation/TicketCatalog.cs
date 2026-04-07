using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace CoachReservation
{
    public class TicketCatalog
    {
        public TicketCatalog()
        {
        }

        public void UpdateTicketStatus(int ticketId, string newStatus)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection("server=localhost;user=root;password=123456;database=coachreservationdb;"))
                {
                    string query = "UPDATE Ticket SET TicketStatus = @newStatus WHERE TicketId = @ticketId";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@newStatus", newStatus);
                    cmd.Parameters.AddWithValue("@ticketId", ticketId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating ticket status: " + ex.Message);
            }
        }
        public List<string> GetSeatsByTicket(int ticketId)
        {
            List<string> seats = new List<string>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection("server=localhost;user=root;password=123456;database=coachreservationdb;"))
                {
                    connection.Open();

                    string query = @"
                    SELECT s.SeatCode
                    FROM TicketDetail td
                    LEFT JOIN TripSeat ts ON td.TripSeatId = ts.TripSeatId
                    LEFT JOIN Seat s ON ts.SeatId = s.SeatId
                    WHERE td.TicketId = @ticketId
                    ORDER BY s.SeatCode";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@ticketId", ticketId);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            seats.Add(reader.GetString("SeatCode"));
                        }
                    }

                    reader.Close();
                    return seats;
                }
            }
            catch (Exception ex)
            {
                return new List<string>();
            }
        }

        public Ticket FindTicket(int ticketId)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection("server=localhost;user=root;password=123456;database=coachreservationdb;"))
                {
                    connection.Open();

                    string query = @"
                    SELECT t.TicketId, t.TripId, t.PassengerId, t.BookingDate, t.TotalAmount, t.TicketStatus,
                           tr.TripId AS TripId_Trip, tr.RouteId, tr.VehicleId, tr.DepartureDate, tr.DepartureTime, tr.BasePrice, tr.Status,
                           r.RouteId AS RouteId_Route, r.DeparturePoint, r.Destination,
                           v.VehicleId AS VehicleId_Vehicle, v.LicensePlate, v.VehicleType, v.TotalSeats,
                           p.PassengerId AS PassengerId_Passenger, p.FullName, p.PhoneNumber
                    FROM Ticket t
                    LEFT JOIN Trip tr ON t.TripId = tr.TripId
                    LEFT JOIN Route r ON tr.RouteId = r.RouteId
                    LEFT JOIN Vehicle v ON tr.VehicleId = v.VehicleId
                    LEFT JOIN Passenger p ON t.PassengerId = p.PassengerId
                    WHERE t.TicketId = @ticketId";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@ticketId", ticketId);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        int readTicketId = reader.GetInt32("TicketId");
                        DateTime bookingDate = reader.GetDateTime("BookingDate");
                        decimal totalAmount = reader.GetDecimal("TotalAmount");
                        string ticketStatus = reader.GetString("TicketStatus");

                        Route route = null;
                        int routeIdOrd = reader.GetOrdinal("RouteId_Route");
                        if (!reader.IsDBNull(routeIdOrd))
                        {
                            int routeId = reader.GetInt32(routeIdOrd);
                            string departurePoint = reader.GetString("DeparturePoint");
                            string destination = reader.GetString("Destination");

                            route = new Route(routeId, departurePoint, destination);
                        }

                        Vehicle vehicle = null;
                        int vehicleIdOrd = reader.GetOrdinal("VehicleId_Vehicle");
                        if (!reader.IsDBNull(vehicleIdOrd))
                        {
                            int vehicleId = reader.GetInt32(vehicleIdOrd);
                            string licensePlate = reader.GetString("LicensePlate");
                            string vehicleType = reader.GetString("VehicleType");
                            int totalSeats = reader.GetInt32("TotalSeats");

                            vehicle = new Vehicle(vehicleId, licensePlate, vehicleType, totalSeats);
                        }

                        Trip trip = null;
                        int tripIdOrd = reader.GetOrdinal("TripId_Trip");
                        if (!reader.IsDBNull(tripIdOrd))
                        {
                            int tripId = reader.GetInt32(tripIdOrd);
                            DateTime departureDate = reader.GetDateTime("DepartureDate");
                            TimeSpan departureTime = reader.GetTimeSpan("DepartureTime");
                            decimal basePrice = reader.GetDecimal("BasePrice");
                            string tripStatus = reader.GetString("Status");

                            trip = new Trip(tripId, route, vehicle, departureDate, departureTime, basePrice, tripStatus);
                        }

                        Passenger passenger = null;
                        int passengerIdOrd = reader.GetOrdinal("PassengerId_Passenger");
                        if (!reader.IsDBNull(passengerIdOrd))
                        {
                            int psgId = reader.GetInt32(passengerIdOrd);
                            string fullName = reader.GetString("FullName");
                            string phoneNumber = reader.GetString("PhoneNumber");

                            passenger = new Passenger(psgId, fullName, phoneNumber);
                        }

                        reader.Close();

                        return new Ticket(readTicketId, trip, passenger, bookingDate, totalAmount, ticketStatus);
                    }

                    reader.Close();
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Passenger GetPassengerById(int passengerId)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection("server=localhost;user=root;password=123456;database=coachreservationdb;"))
                {
                    connection.Open();
                    string query = "SELECT PassengerId, FullName, PhoneNumber FROM Passenger WHERE PassengerId = @passengerId";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@passengerId", passengerId);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        Passenger passenger = new Passenger
                        {
                            PassengerId = reader.GetInt32("PassengerId"),
                            FullName = reader.GetString("FullName"),
                            PhoneNumber = reader.GetString("PhoneNumber")
                        };
                        reader.Close();
                        return passenger;
                    }

                    reader.Close();
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting passenger: " + ex.Message);
                return null;
            }
        }

        public bool AreSeatsStillAvailable(int tripId, List<int> seatIds)
        {
            if (seatIds == null || seatIds.Count == 0)
            {
                return false;
            }

            try
            {
                using (MySqlConnection connection = new MySqlConnection("server=localhost;user=root;password=123456;database=coachreservationdb;"))
                {
                    connection.Open();
                    string seatIdList = string.Join(",", seatIds);
                    string query = $@"SELECT COUNT(*)
                                 FROM TripSeat
                                 WHERE TripId = @tripId
                                 AND Status = 'Đã đặt'
                                 AND SeatId IN ({seatIdList})";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@tripId", tripId);
                    object result = cmd.ExecuteScalar();
                    int bookedCount = result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;

                    return bookedCount == 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error checking seat availability: " + ex.Message);
                return false;
            }
        }

        public int CreateTicketWithSeats(int tripId, int passengerId, List<int> seatIds, decimal basePrice)
        {
            if (seatIds == null || seatIds.Count == 0)
            {
                return -1;
            }

            MySqlTransaction transaction = null;
            try
            {
                using (MySqlConnection connection = new MySqlConnection("server=localhost;user=root;password=123456;database=coachreservationdb;"))
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();

                    string seatIdList = string.Join(",", seatIds);
                    string checkQuery = $@"SELECT COUNT(*)
                                      FROM TripSeat
                                      WHERE TripId = @tripId
                                      AND Status = 'Đã đặt'
                                      AND SeatId IN ({seatIdList})";

                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, connection, transaction);
                    checkCmd.Parameters.AddWithValue("@tripId", tripId);
                    int bookedCount = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (bookedCount > 0)
                    {
                        transaction.Rollback();
                        return -1;
                    }

                    decimal totalAmount = basePrice * seatIds.Count;
                    string insertTicketQuery = @"INSERT INTO Ticket (TripId, PassengerId, BookingDate, TotalAmount, TicketStatus)
                                           VALUES (@tripId, @passengerId, @bookingDate, @totalAmount, @ticketStatus)";
                    MySqlCommand insertTicketCmd = new MySqlCommand(insertTicketQuery, connection, transaction);
                    insertTicketCmd.Parameters.AddWithValue("@tripId", tripId);
                    insertTicketCmd.Parameters.AddWithValue("@passengerId", passengerId);
                    insertTicketCmd.Parameters.AddWithValue("@bookingDate", DateTime.Now);
                    insertTicketCmd.Parameters.AddWithValue("@totalAmount", totalAmount);
                    insertTicketCmd.Parameters.AddWithValue("@ticketStatus", "Đã thanh toán");
                    insertTicketCmd.ExecuteNonQuery();

                    int ticketId = (int)insertTicketCmd.LastInsertedId;

                    foreach (int seatId in seatIds)
                    {
                        int tripSeatId;

                        string findTripSeatQuery = @"SELECT TripSeatId FROM TripSeat WHERE TripId = @tripId AND SeatId = @seatId LIMIT 1";
                        MySqlCommand findTripSeatCmd = new MySqlCommand(findTripSeatQuery, connection, transaction);
                        findTripSeatCmd.Parameters.AddWithValue("@tripId", tripId);
                        findTripSeatCmd.Parameters.AddWithValue("@seatId", seatId);
                        object existingTripSeatId = findTripSeatCmd.ExecuteScalar();

                        if (existingTripSeatId != null)
                        {
                            tripSeatId = Convert.ToInt32(existingTripSeatId);
                            string updateTripSeatQuery = @"UPDATE TripSeat SET Status = 'Đã đặt' WHERE TripSeatId = @tripSeatId";
                            MySqlCommand updateTripSeatCmd = new MySqlCommand(updateTripSeatQuery, connection, transaction);
                            updateTripSeatCmd.Parameters.AddWithValue("@tripSeatId", tripSeatId);
                            updateTripSeatCmd.ExecuteNonQuery();
                        }
                        else
                        {
                            string insertTripSeatQuery = @"INSERT INTO TripSeat (TripId, SeatId, Status) VALUES (@tripId, @seatId, 'Đã đặt')";
                            MySqlCommand insertTripSeatCmd = new MySqlCommand(insertTripSeatQuery, connection, transaction);
                            insertTripSeatCmd.Parameters.AddWithValue("@tripId", tripId);
                            insertTripSeatCmd.Parameters.AddWithValue("@seatId", seatId);
                            insertTripSeatCmd.ExecuteNonQuery();
                            tripSeatId = (int)insertTripSeatCmd.LastInsertedId;
                        }

                        string insertDetailQuery = @"INSERT INTO TicketDetail (TicketId, TripSeatId, Price)
                                               VALUES (@ticketId, @tripSeatId, @price)";
                        MySqlCommand insertDetailCmd = new MySqlCommand(insertDetailQuery, connection, transaction);
                        insertDetailCmd.Parameters.AddWithValue("@ticketId", ticketId);
                        insertDetailCmd.Parameters.AddWithValue("@tripSeatId", tripSeatId);
                        insertDetailCmd.Parameters.AddWithValue("@price", basePrice);
                        insertDetailCmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    return ticketId;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating ticket with seats: " + ex.Message);
                try
                {
                    transaction?.Rollback();
                }
                catch
                {
                }

                return -1;
            }
        }
    }
}
