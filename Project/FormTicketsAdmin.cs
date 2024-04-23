using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Project
{
    public partial class FormTicketsAdmin : Form
    {
        private List<Route> routes = new List<Route>();
        private MySqlConnection connection;
        private string connectionString = "server=127.0.0.1;database=db;uid=root;pwd=Gd_135790;";

        public FormTicketsAdmin()
        {
            InitializeComponent();
            checkBox1.Visible = false;
            checkBox2.Visible = false;
            checkBox3.Visible = false;
            connection = new MySqlConnection(connectionString);
        }

        private void FormTicketsAdmin_Load(object sender, EventArgs e)
        {
            LoadCitiesToComboBox();
        }

        private void LoadCitiesToComboBox()
        {
            try
            {
                connection.Open();
                string query = "SELECT DISTINCT city FROM countries";
                MySqlCommand command = new MySqlCommand(query, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string cityName = reader.GetString(0);
                        comboBox2.Items.Add(cityName);
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

        private void DisplayRoutesInDataGridView()
        {
            dataGridView1.Rows.Clear();

            foreach (var route in routes)
            {
                foreach (var intermediateStation in route.IntermediateStations)
                {
                    string station = intermediateStation.Item1.NameOfCity;
                    string stationAddress = intermediateStation.Item1.ParkingAddress;
                    string stationTime = intermediateStation.Item2.ToString("HH:mm");

                    List<object> rowData = new List<object>
            {
                route.RouteId,
                route.StartCity,
                route.EndCity,
                route.DepartureDate.ToShortDateString(),
                route.DepartureTime.ToString("HH:mm"),
                route.ArrivalDate.ToShortDateString(),
                route.ArrivalTime.ToString("HH:mm"),
                station,
                stationAddress,
                stationTime,
                route.Price
            };

                    // Додавання додаткових даних про транспортний засіб
                    if (route.Vehicle is Bus)
                    {
                        rowData.Add("Bus");
                        rowData.Add((route.Vehicle as Bus).Model);
                        rowData.Add((route.Vehicle as Bus).NumberOfSeats);
                        rowData.Add((route.Vehicle as Bus).HasToilet);
                        rowData.Add((route.Vehicle as Bus).HasWifi);
                        rowData.Add(null); // Для поля "isConditioning" не потрібно значення
                    }
                    else if (route.Vehicle is Microbus)
                    {
                        rowData.Add("Microbus");
                        rowData.Add((route.Vehicle as Microbus).Model);
                        rowData.Add((route.Vehicle as Microbus).NumberOfSeats);
                        rowData.Add(null); // Для поля "isToilet" не потрібно значення
                        rowData.Add(null); // Для поля "isWifi" не потрібно значення
                        rowData.Add((route.Vehicle as Microbus).HasAirConditioning);
                    }

                    dataGridView1.Rows.Add(rowData.ToArray());
                }
            }
        }

        private void AddNewRoute()
        {
            int routeId = int.Parse(txtRoudeId.Text);
            string startCity = txtStartCity.Text;
            string endCity = txtEndCity.Text;
            string departureDate = poisonDateTime1.Value.ToString("yyyy-MM-dd");
            string arrivalDate = poisonDateTime2.Value.ToString("yyyy-MM-dd");
            DateTime departureTime = DateTime.ParseExact(maskedTextBox1.Text, "HH:mm", CultureInfo.InvariantCulture);
            DateTime arrivalTime = DateTime.ParseExact(maskedTextBox2.Text, "HH:mm", CultureInfo.InvariantCulture);
            string station = comboBox2.Text;
            DateTime stationTime = DateTime.ParseExact(maskedTextBox3.Text, "HH:mm", CultureInfo.InvariantCulture);
            int price = int.Parse(textBox1.Text);

            Vehicle vehicle;
            if (comboBox3.SelectedItem.ToString() == "Bus")
            {
                vehicle = new Bus
                {
                    Model = txtModel.Text,
                    NumberOfSeats = int.Parse(txtNumberOfSeats.Text),
                    HasToilet = checkBox1.Checked,
                    HasWifi = checkBox2.Checked
                };
            }
            else if (comboBox3.SelectedItem.ToString() == "Microbus")
            {
                vehicle = new Microbus
                {
                    Model = txtModel.Text,
                    NumberOfSeats = int.Parse(txtNumberOfSeats.Text),
                    HasAirConditioning = checkBox3.Checked
                };
            }
            else
            {
                return;
            }

            string stationAddress = comboBox1.Text;

            Route newRoute = new Route
            {
                RouteId = routeId,
                StartCity = startCity,
                EndCity = endCity,
                DepartureDate = Convert.ToDateTime(departureDate),
                ArrivalDate = Convert.ToDateTime(arrivalDate),
                DepartureTime = departureTime,
                ArrivalTime = arrivalTime,
                IntermediateStations = new List<(City, DateTime)> { (new City(station, stationAddress), stationTime) },
                Price = price,
                Vehicle = vehicle
            };

            routes.Add(newRoute);

            DisplayRoutesInDataGridView();
            ClearInputFields();
        }

        private void ClearInputFields()
        {
            txtRoudeId.Clear();
            txtStartCity.Clear();
            txtEndCity.Clear();
            maskedTextBox1.Clear();
            maskedTextBox2.Clear();
            maskedTextBox3.Clear();
            textBox1.Clear();
            comboBox1.Text = "";
            comboBox2.Text = "";
            comboBox3.Text = "";
            txtModel.Text = "";
            txtNumberOfSeats.Text = "";
        }

        private void ClearStationFields()
        {
            maskedTextBox3.Clear();
            comboBox1.Text = "";
            comboBox2.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Бажаєте додати ще проміжну станцію?", "Додати станцію", MessageBoxButtons.YesNo);

            if (dialog == DialogResult.Yes)
            {
                txtRoudeId.Enabled = false;
                txtStartCity.Enabled = false;
                txtEndCity.Enabled = false;
                maskedTextBox1.Enabled = false;
                maskedTextBox2.Enabled = false;
                //maskedTextBox3.Clear();
                textBox1.Enabled = false;
                //comboBox1.Text = "";
                //comboBox2.Text = "";
                comboBox3.Enabled = false;
                txtModel.Enabled = false;
                txtNumberOfSeats.Enabled = false;
                checkBox1.Enabled = false;
                checkBox2.Enabled = false;
                checkBox3.Enabled = false;
                poisonDateTime1.Enabled = false;
                poisonDateTime2.Enabled = false;

                string station = comboBox2.Text;
                string stationAddress = comboBox1.Text;
                string stationTime = maskedTextBox3.Text;

                comboBox1.Text = "";
                comboBox2.Text = "";
                maskedTextBox3.Clear();

                AddNewRoute(station, stationAddress, stationTime);
            }
            else if (dialog == DialogResult.No)
            {
                txtRoudeId.Enabled = true;
                txtStartCity.Enabled = true;
                txtEndCity.Enabled = true;
                maskedTextBox1.Enabled = true;
                maskedTextBox2.Enabled = true;
                //maskedTextBox3.Clear();
                textBox1.Enabled = true;
                //comboBox1.Text = "";
                //comboBox2.Text = "";
                comboBox3.Enabled = true;
                txtModel.Enabled = true;
                txtNumberOfSeats.Enabled = true;
                checkBox1.Enabled = true;
                checkBox2.Enabled = true;
                checkBox3.Enabled = true;
                poisonDateTime1.Enabled = true;
                poisonDateTime2.Enabled = true;
                AddNewRoute();
            }
        }

        private void AddNewRoute(string station = null, string stationAddress = null, string stationTime = null)
        {
            int routeId = int.Parse(txtRoudeId.Text);
            string startCity = txtStartCity.Text;
            string endCity = txtEndCity.Text;
            string departureDate = poisonDateTime1.Value.ToString("yyyy-MM-dd");
            string arrivalDate = poisonDateTime2.Value.ToString("yyyy-MM-dd");
            DateTime departureTime = DateTime.ParseExact(maskedTextBox1.Text, "HH:mm", CultureInfo.InvariantCulture);
            DateTime arrivalTime = DateTime.ParseExact(maskedTextBox2.Text, "HH:mm", CultureInfo.InvariantCulture);
            int price = int.Parse(textBox1.Text);

            Vehicle vehicle;
            if (comboBox3.SelectedItem.ToString() == "Bus")
            {
                vehicle = new Bus
                {
                    Model = txtModel.Text,
                    NumberOfSeats = int.Parse(txtNumberOfSeats.Text),
                    HasToilet = checkBox1.Checked,
                    HasWifi = checkBox2.Checked
                };
            }
            else if (comboBox3.SelectedItem.ToString() == "Microbus")
            {
                vehicle = new Microbus
                {
                    Model = txtModel.Text,
                    NumberOfSeats = int.Parse(txtNumberOfSeats.Text),
                    HasAirConditioning = checkBox3.Checked
                };
            }
            else
            {
                return;
            }

            string stationAddressToAdd = stationAddress ?? comboBox1.Text;
            string stationToAdd = station ?? comboBox2.Text;
            string stationTimeToAdd = stationTime ?? maskedTextBox3.Text;

            Route newRoute = new Route
            {
                RouteId = routeId,
                StartCity = startCity,
                EndCity = endCity,
                DepartureDate = Convert.ToDateTime(departureDate),
                ArrivalDate = Convert.ToDateTime(arrivalDate),
                DepartureTime = departureTime,
                ArrivalTime = arrivalTime,
                Price = price,
                Vehicle = vehicle
            };

            if (station != null && stationAddress != null && stationTime != null)
            {
                newRoute.IntermediateStations = new List<(City, DateTime)> { (new City(stationToAdd, stationAddressToAdd), DateTime.ParseExact(stationTimeToAdd, "HH:mm", CultureInfo.InvariantCulture)) };
            }

            routes.Add(newRoute);

            DisplayRoutesInDataGridView();
            ClearStationFields();
        }

        private void LoadParkingAddresses(string selectedCity)
        {
            comboBox1.Items.Clear();
            try
            {
                connection.Open();
                string query = "SELECT parking_address FROM countries WHERE city = @city";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@city", selectedCity);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string parkingAddress = reader.GetString(0);
                        comboBox1.Items.Add(parkingAddress);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка під час завантаження адрес стоянок: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCity = comboBox2.SelectedItem.ToString();
            LoadParkingAddresses(selectedCity);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedItem.ToString() == "Bus")
            {
                checkBox1.Visible = true;
                checkBox2.Visible = true;
                checkBox3.Visible = false;
            }
            if (comboBox3.SelectedItem.ToString() == "Microbus")
            {
                checkBox1.Visible = false;
                checkBox2.Visible = false;
                checkBox3.Visible = true;
            }
        }
    }
}
