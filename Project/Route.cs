using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Project
{
    class Route
    {
        public int RouteId { get; set; }
        public string StartCity { get; set; }
        public string EndCity { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int Price { get; set; }
        public Vehicle Vehicle { get; set; }

        public Route()
        {
            
        }

        public Route(int routeId, string startCity, string endCity, DateTime departureDate, DateTime arrivalDate, DateTime departureTime, DateTime arrivalTime, List<(City, DateTime, double)> intermediateStations, int price, Vehicle vehicle)
        {
            RouteId = routeId;
            StartCity = startCity;
            EndCity = endCity;
            DepartureDate = departureDate;
            ArrivalDate = arrivalDate;
            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;
            Price = price;
            Vehicle = vehicle;
        }

        public Route(int routeId, string startCity, string endCity, int price)
        {
            RouteId = routeId;
            StartCity = startCity;
            EndCity = endCity;
            Price = price;
        }


        public void SaveToDatabase(MySqlConnection connection)
        {
            string query = "INSERT INTO routes (route_id, start_city, end_city, price) VALUES (@routeId, @startCity, @endCity, @price)";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@routeId", RouteId);
                command.Parameters.AddWithValue("@startCity", StartCity);
                command.Parameters.AddWithValue("@endCity", EndCity);
                command.Parameters.AddWithValue("@price", Price);

                try
                {
                    if (connection.State != ConnectionState.Open)
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
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }
    }
}
