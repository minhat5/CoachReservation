namespace CoachReservation
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            Database database = new Database();
            RouteCatalog routeCatalog = new RouteCatalog(database);
            VehicleCatalog vehicleCatalog = new VehicleCatalog(database);
            TripCatalog tripCatalog = new TripCatalog(database);
            SeatMapCatalog seatMapCatalog = new SeatMapCatalog(database);
            TicketCatalog ticketCatalog = new TicketCatalog(database);
            SeatCatalog seatCatalog = new SeatCatalog(database);
            ApplicationConfiguration.Initialize();
            //Application.Run(new ConfigureSeatMapForm(seatMapCatalog, vehicleCatalog, seatCatalog));
            Application.Run(new SearchTripsForm(routeCatalog, vehicleCatalog, tripCatalog, seatMapCatalog, seatCatalog, ticketCatalog));
            //Application.Run(new CheckInForm(ticketCatalog));
        }
    }
}