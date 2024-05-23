using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project
{
    public partial class FormNewRouteAdmin : Form
    {
        private MySqlConnection connection;
        private string connectionString = "server=127.0.0.1;database=db;uid=root;pwd=Gd_135790;";
        public FormNewRouteAdmin()
        {
            InitializeComponent();
            connection = new MySqlConnection(connectionString);
        }

        private void FormNewRouteAdmin_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
            LoadRouteIDToComboBox();
        }

        private void LoadRouteIDToComboBox()
        {
            try
            {
                connection.Open();
                string query = "SELECT DISTINCT routeId FROM intermediatestations";
                MySqlCommand command = new MySqlCommand(query, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int routeId = reader.GetInt32(0);
                        cmbRouteID.Items.Add(routeId);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка під час завантаження міст: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void cmbRouteID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRouteID.SelectedItem != null)
            {
                int selectedRouteId = Convert.ToInt32(cmbRouteID.SelectedItem);
                LoadStartAndEndCity(selectedRouteId);
            }
        }

        private void LoadStartAndEndCity(int routeId)
        {
            try
            {
                connection.Open();
                string query = @"
            SELECT cs.station_city
            FROM characteristicsOfStations cs
            JOIN intermediateStations ist ON cs.station_name = ist.station_name
            WHERE ist.routeId = @routeId AND ist.order_index = 1;";
                string queryEnd = @"
            SELECT cs.station_city
            FROM characteristicsOfStations cs
            JOIN intermediateStations ist ON cs.station_name = ist.station_name
            WHERE ist.routeId = @routeId AND ist.next_station = '';";
                string queryTotalPrice = @"
            select sum(price) FROM intermediatestations where routeId = @routeId";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@routeId", routeId);
                string startCity = command.ExecuteScalar()?.ToString() ?? "";

                MySqlCommand commandEnd = new MySqlCommand(queryEnd, connection);
                commandEnd.Parameters.AddWithValue("@routeId", routeId);
                string endCity = commandEnd.ExecuteScalar()?.ToString() ?? "";

                MySqlCommand commandTotalPrice = new MySqlCommand(queryTotalPrice, connection);
                commandTotalPrice.Parameters.AddWithValue("@routeId", routeId);
                string totalPrice = commandTotalPrice.ExecuteScalar()?.ToString() ?? "";

                txtStartCity.Text = startCity;
                txtEndCity.Text = endCity;
                txtPrice.Text = totalPrice;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int routeId = Convert.ToInt32(cmbRouteID.SelectedItem);
            string startCity = txtStartCity.Text;
            string endCity = txtEndCity.Text;
            int price = Convert.ToInt32(txtPrice.Text);

            string model = txtModel.Text;
            string vehicleNumber = txtVehicleNumber.Text;
            int numberOfSeats = Convert.ToInt32(txtNumberOfSeats.Text);
            bool hasToilet = checkBox1.Checked;
            bool hasWifi = checkBox2.Checked;
            bool hasAirConditioning = checkBox3.Checked;

            Vehicle vehicle;
            if (cmbVehicleType.SelectedItem?.ToString() == "Автобус")
            {
                vehicle = new Bus(hasToilet, hasWifi, routeId, vehicleNumber, numberOfSeats, model);
            }
            else
            {
                vehicle = new Microbus(hasAirConditioning, routeId, vehicleNumber, numberOfSeats, model);
            }

            bool canAddRoute = CanAddRoute(connection, routeId);

            if (canAddRoute)
            {
                Route newRoute = new Route(routeId, startCity, endCity, price);
                newRoute.SaveToDatabase(connection);

                DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                row.Cells[0].Value = newRoute.RouteId;
                row.Cells[1].Value = newRoute.StartCity;
                row.Cells[2].Value = newRoute.EndCity;
                row.Cells[3].Value = newRoute.Price;
                dataGridView1.Rows.Add(row);

                vehicle.SaveToDatabase(connection);
            }

            cmbRouteID.SelectedIndex = -1;
            txtStartCity.Clear();
            txtEndCity.Clear();
            txtPrice.Clear();
            txtModel.Clear();
            txtVehicleNumber.Clear();
            txtNumberOfSeats.Clear();
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            cmbVehicleType.SelectedIndex = -1;
        }

        private bool CanAddRoute(MySqlConnection connection, int routeId)
        {
            string queryCheck = "SELECT COUNT(*) FROM vehicles WHERE route_id = @routeId";
            using (MySqlCommand checkCommand = new MySqlCommand(queryCheck, connection))
            {
                checkCommand.Parameters.AddWithValue("@routeId", routeId);
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
                        return false;
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

            return true;
        }

        private void cmbVehicleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbVehicleType.SelectedItem != null)
            {
                string selectedItem = cmbVehicleType.SelectedItem.ToString();
                if (selectedItem == "Автобус")
                {
                    checkBox1.Visible = true;
                    checkBox2.Visible = true;
                    checkBox3.Visible = false;
                }
                else if (selectedItem == "Мікроавтобус")
                {
                    checkBox1.Visible = false;
                    checkBox2.Visible = false;
                    checkBox3.Visible = true;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                string query = "SELECT * FROM routes";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                dataGridView1.Rows.Clear();

                while (reader.Read())
                {
                    int routeId = reader.GetInt32("route_id");
                    string stationName = reader.GetString("start_city");
                    string stationAddress = reader.GetString("end_city");
                    int price = reader.GetInt32("price");

                    dataGridView1.Rows.Add(routeId, stationName, stationAddress, price);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                string query = "SELECT * FROM vehicles";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                dataGridView1.Rows.Clear();

                while (reader.Read())
                {
                    int routeId = reader.GetInt32("route_id");
                    string vehicleType = reader.GetString("vehicle_type");
                    string vehicleNumber = reader.GetString("vehicle_number");
                    string model = reader.GetString("model");
                    int numberOfSeats = reader.GetInt32("number_of_seats");

                    // Перевірка на NULL перед перетворенням значень
                    bool? toilet = null;
                    if (!reader.IsDBNull(reader.GetOrdinal("has_toilet")))
                    {
                        toilet = reader.GetInt32("has_toilet") == 1;
                    }

                    bool? wifi = null;
                    if (!reader.IsDBNull(reader.GetOrdinal("has_wifi")))
                    {
                        wifi = reader.GetInt32("has_wifi") == 1;
                    }

                    bool? cond = null;
                    if (!reader.IsDBNull(reader.GetOrdinal("has_air_conditioning")))
                    {
                        cond = reader.GetInt32("has_air_conditioning") == 1;
                    }

                    dataGridView2.Rows.Add(routeId, vehicleType, vehicleNumber, model, numberOfSeats, toilet, wifi, cond);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
