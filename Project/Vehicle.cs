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
    class Vehicle
    {
        public int RouteId { get; set; }
        public int NumberOfSeats { get; set; }
        public string Model { get; set; }
        public string VehicleType { get; set; }
        public string VehicleNumber { get; set; }
        
        public Vehicle() { }

        public Vehicle(int routeId, string vehicleNumber, int numberOfSeats, string model)
        {
            RouteId = routeId;
            NumberOfSeats = numberOfSeats;
            Model = model;
            VehicleNumber = vehicleNumber;
        }

        public virtual void SaveToDatabase(MySqlConnection connection)
        {
            string query = "INSERT INTO vehicles (route_id, vehicle_type, vehicle_number, model, number_of_seats) VALUES (@routeId, @vehicleType, @vehicleNumber, @model, @numberOfSeats)";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@routeId", RouteId);
                command.Parameters.AddWithValue("@vehicleType", VehicleType);
                command.Parameters.AddWithValue("@vehicleNumber", VehicleNumber);
                command.Parameters.AddWithValue("@model", Model);
                command.Parameters.AddWithValue("@numberOfSeats", NumberOfSeats);
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
