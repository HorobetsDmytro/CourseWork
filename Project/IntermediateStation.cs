using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project
{
    class IntermediateStation
    {
        public int RouteId { get; set; }
        public string StationCity { get; set; }
        public string StationName { get; set; }
        public string NextStationCity { get; set; }
        public string NextStationName { get; set; }
        public string Price { get; set; }
        public DateTime DepartureDate { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public DateTime ArrivalDate { get; set; }
        public TimeSpan ArrivalTime { get; set; }

        public IntermediateStation(int routeId, string stationCity, string stationName, string nextStationCity, string nextStationName, string price, DateTime departureDate, TimeSpan departureTime, DateTime arrivalDate, TimeSpan arrivalTime)
        {
            RouteId = routeId;
            StationCity = stationCity;
            StationName = stationName;
            NextStationCity = nextStationCity;
            NextStationName = nextStationName;
            Price = price;
            DepartureDate = departureDate;
            DepartureTime = departureTime;
            ArrivalDate = arrivalDate;
            ArrivalTime = arrivalTime;
        }

        public void SaveToDatabase(MySqlConnection connection)
        {
            int nextOrderIndex = GetNextOrderIndex(connection, RouteId);
            string query = "INSERT INTO intermediatestations (routeId, station_city, station_name, next_city, next_station, order_index, price, departure_date, departure_time, arrival_date, arrival_time) VALUES (@routeId, @stationCity, @stationName, @nextStationCity,  @nextStationName, @orderIndex, @price, @departureDate, @departureTime, @arrivalDate, @arrivalTime)";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@routeId", RouteId);
                command.Parameters.AddWithValue("@stationCity", StationCity);
                command.Parameters.AddWithValue("@stationName", StationName);
                command.Parameters.AddWithValue("@nextStationCity", NextStationCity);
                command.Parameters.AddWithValue("@nextStationName", NextStationName);
                command.Parameters.AddWithValue("@orderIndex", nextOrderIndex);
                command.Parameters.AddWithValue("@price", Price);
                command.Parameters.AddWithValue("@departureDate", DepartureDate);
                command.Parameters.AddWithValue("@departureTime", DepartureTime);
                command.Parameters.AddWithValue("@arrivalDate", ArrivalDate);
                command.Parameters.AddWithValue("@arrivalTime", ArrivalTime);

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
                    connection.Close();
                }
            }
        }

        private int GetNextOrderIndex(MySqlConnection connection, int routeId)
        {
            string query = "SELECT IFNULL(MAX(order_index), 0) + 1 FROM intermediatestations WHERE routeId = @routeId";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@routeId", routeId);
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

    }
}
