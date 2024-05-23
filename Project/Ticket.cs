using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Project
{
    class Ticket
    {
        public int TicketId { get; set; }
        public int RouteId { get; set; }
        public string StartCity { get; set; }
        public string EndCity { get; set; }
        public DateTime DepartureDate { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public DateTime ArrivalDate { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public int SeatNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string PassengerName { get; set; }
        public string PassengerSurname { get; set; }
        public int Price { get; set; }

        public void SaveToDatabase(MySqlConnection connection)
        {
            string query = "INSERT INTO tickets (route_id, start_city, end_city, departure_date, departure_time, arrival_date, arrival_time, seat_number, phone_number, passenger_name, passenger_surname, price) " +
                           "VALUES (@routeId, @startCity, @endCity, @departureDate, @departureTime, @arrivalDate, @arrivalTime, @seatNumber, @phoneNumber, @passengerName, @passengerSurname, @price)";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@routeId", RouteId);
                command.Parameters.AddWithValue("@startCity", StartCity);
                command.Parameters.AddWithValue("@endCity", EndCity);
                command.Parameters.AddWithValue("@departureDate", DepartureDate);
                command.Parameters.AddWithValue("@departureTime", DepartureTime);
                command.Parameters.AddWithValue("@arrivalDate", ArrivalDate);
                command.Parameters.AddWithValue("@arrivalTime", ArrivalTime);
                command.Parameters.AddWithValue("@seatNumber", SeatNumber);
                command.Parameters.AddWithValue("@phoneNumber", PhoneNumber);
                command.Parameters.AddWithValue("@passengerName", PassengerName);
                command.Parameters.AddWithValue("@passengerSurname", PassengerSurname);
                command.Parameters.AddWithValue("@price", Price);

                try
                {
                    if (connection.State != System.Data.ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка: {ex.Message}");
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }
    }
}