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
            RouteCatalog routeCatalog = new RouteCatalog();
            VehicleCatalog vehicleCatalog = new VehicleCatalog();
            TripCatalog tripCatalog = new TripCatalog();
            SeatMapCatalog seatMapCatalog = new SeatMapCatalog();
            TicketCatalog ticketCatalog = new TicketCatalog();
            SeatCatalog seatCatalog = new SeatCatalog();
            ApplicationConfiguration.Initialize();
            Application.Run(new ConfigureSeatMapForm(seatMapCatalog, vehicleCatalog, seatCatalog));
            //Application.Run(new SearchTripsForm(routeCatalog, vehicleCatalog, tripCatalog, seatMapCatalog, seatCatalog, ticketCatalog));
            //Application.Run(new CheckInForm(ticketCatalog));
        }
    }
}