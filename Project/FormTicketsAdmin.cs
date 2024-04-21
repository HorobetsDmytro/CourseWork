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
                        //poisonComboBox2.Items.Add(cityName);
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

                    List<string> rowData = new List<string>
                    {
                        route.routeId.ToString(),
                        route.StartCity,
                        route.EndCity,
                        route.DepartureDate.ToShortDateString(),
                        route.DepartureTime.ToString("HH:mm"),
                        route.ArrivalDate.ToShortDateString(),
                        route.ArrivalTime.ToString("HH:mm"),
                        station,
                        stationAddress,
                        stationTime,
                        route.Price.ToString()
                    };
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

            string stationAddress = comboBox1.Text;

            Route newRoute = new Route(routeId, startCity, endCity, Convert.ToDateTime(departureDate), Convert.ToDateTime(arrivalDate), departureTime, arrivalTime, new List<(City, DateTime)>(), price);
            newRoute.IntermediateStations.Add((new City(station, stationAddress), stationTime));
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
                string station = comboBox2.Text;
                string stationAddress = comboBox1.Text;
                string stationTime = maskedTextBox3.Text;

                //dataGridView1.Columns.Add("Station", "Station");
                //dataGridView1.Columns.Add("StationAddress", "StationAddress");
                //dataGridView1.Columns.Add("StationTime", "StationTime");

                // Очищаємо вміст полів введення
                comboBox1.Text = "";
                comboBox2.Text = "";
                maskedTextBox3.Clear();

                AddNewRoute(station, stationAddress, stationTime);
            }
            else if (dialog == DialogResult.No)
            {
                AddNewRoute();
            }
        }

        private void AddNewRoute(string station, string stationAddress, string stationTime)
        {
            bool routeExists = false;
            foreach (var route in routes)
            {
                if (route.StartCity == txtStartCity.Text && route.EndCity == txtEndCity.Text &&
                    route.DepartureDate == Convert.ToDateTime(poisonDateTime1.Value.ToString("yyyy-MM-dd")) &&
                    route.ArrivalDate == Convert.ToDateTime(poisonDateTime2.Value.ToString("yyyy-MM-dd")) &&
                    route.DepartureTime == DateTime.ParseExact(maskedTextBox1.Text, "HH:mm", CultureInfo.InvariantCulture) &&
                    route.ArrivalTime == DateTime.ParseExact(maskedTextBox2.Text, "HH:mm", CultureInfo.InvariantCulture))
                {
                    route.IntermediateStations.Add((new City(station, stationAddress), DateTime.ParseExact(stationTime, "HH:mm", CultureInfo.InvariantCulture)));
                    routeExists = true;
                    break;
                }
            }

            if (!routeExists)
            {
                int routeId = int.Parse(txtRoudeId.Text);
                string startCity = txtStartCity.Text;
                string endCity = txtEndCity.Text;
                string departureDate = poisonDateTime1.Value.ToString("yyyy-MM-dd");
                string arrivalDate = poisonDateTime2.Value.ToString("yyyy-MM-dd");
                DateTime departureTime = DateTime.ParseExact(maskedTextBox1.Text, "HH:mm", CultureInfo.InvariantCulture);
                DateTime arrivalTime = DateTime.ParseExact(maskedTextBox2.Text, "HH:mm", CultureInfo.InvariantCulture);
                int price = int.Parse(textBox1.Text);

                Route newRoute = new Route(routeId, startCity, endCity, Convert.ToDateTime(departureDate), Convert.ToDateTime(arrivalDate), departureTime, arrivalTime, new List<(City, DateTime)>(), price);
                newRoute.IntermediateStations.Add((new City(station, stationAddress), DateTime.ParseExact(stationTime, "HH:mm", CultureInfo.InvariantCulture)));
                routes.Add(newRoute);
            }

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
    }
}