﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project
{
    class Microbus : Vehicle
    {
        public bool HasAirConditioning {  get; set; }

        public Microbus() : base()
        {
            
        }
        
        public Microbus(bool hasAirConditioning, int routeId, string vehicleNumber, int numberOfSeats, string model) : base(routeId, vehicleNumber, numberOfSeats, model)
        {
            HasAirConditioning = hasAirConditioning;
            VehicleType = "Мікроавтобус";
        }

        public override void SaveToDatabase(MySqlConnection connection)
        {
            string queryCheck = "SELECT COUNT(*) FROM vehicles WHERE route_id = @routeId";
            using (MySqlCommand checkCommand = new MySqlCommand(queryCheck, connection))
            {
                checkCommand.Parameters.AddWithValue("@routeId", RouteId);

                try
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    int existingCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                    if (existingCount > 0)
                    {
                        MessageBox.Show("Для заданого маршрута вже існує транспортний засіб.", "Повідомлення", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        base.SaveToDatabase(connection);

                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }

                        string query = "UPDATE vehicles SET has_air_conditioning = @hasAirConditioning WHERE route_id = @routeId AND model = @model";
                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@hasAirConditioning", HasAirConditioning);
                            command.Parameters.AddWithValue("@routeId", RouteId);
                            command.Parameters.AddWithValue("@model", Model);

                            command.ExecuteNonQuery();
                        }
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
}
