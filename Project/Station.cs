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
    class Station
    {
        public string StationCity { get; set; }
        public string StationName { get; set; }
        public string StationAddress { get; set; }

        public Station(string stationCity, string stationName, string stationAddress)
        {
            StationCity = stationCity;
            StationName = stationName;
            StationAddress = stationAddress;
        }

        public void SaveToDatabase(MySqlConnection connection, string stationNameFromIntermediateStations)
        {
            // Перевірка, чи дані вже існують у базі даних
            string checkQuery = "SELECT COUNT(1) FROM characteristicsofstations WHERE station_city = @stationCity AND station_name = @stationName AND station_address = @stationAddress";
            MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection);
            checkCommand.Parameters.AddWithValue("@stationCity", StationCity);
            checkCommand.Parameters.AddWithValue("@stationName", stationNameFromIntermediateStations);
            checkCommand.Parameters.AddWithValue("@stationAddress", StationAddress);

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                int exists = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (exists == 0)
                {
                    // Якщо дані не існують, вставка нового запису
                    string query = "INSERT INTO characteristicsofstations (station_city, station_name, station_address) VALUES (@stationCity, @stationName, @stationAddress)";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@stationCity", StationCity);
                    command.Parameters.AddWithValue("@stationName", stationNameFromIntermediateStations);
                    command.Parameters.AddWithValue("@stationAddress", StationAddress);
                    command.ExecuteNonQuery();
                }
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
