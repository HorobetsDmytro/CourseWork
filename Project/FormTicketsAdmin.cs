using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            //dataGridView1.Rows.Clear();

            //foreach (var route in routes)
            //{
            //    foreach (var intermediateStation in route.IntermediateStations)
            //    {
            //        string station = intermediateStation.Item1.NameOfCity;
            //        string stationAddress = intermediateStation.Item1.ParkingAddress;
            //        string stationTime = intermediateStation.Item2.ToString("HH:mm");
            //        string stationPrice = intermediateStation.Item3.ToString();

            //        string departureDate = route.DepartureDate.ToString("yyyy-MM-dd"); // Перетворення дати в рядок з вказаним форматом
            //        string arrivalDate = route.ArrivalDate.ToString("yyyy-MM-dd"); // Перетворення дати в рядок з вказаним форматом

            //        List<object> rowData = new List<object>
            //        {
            //            route.RouteId,
            //            route.StartCity,
            //            route.EndCity,
            //            route.ParkingAddressStartCity,
            //            route.ParkingAddressEndCity,
            //            departureDate, // Використання рядка з датою у відповідному форматі
            //            route.DepartureTime.ToString("HH:mm"),
            //            arrivalDate, // Використання рядка з датою у відповідному форматі
            //            route.ArrivalTime.ToString("HH:mm"),
            //            route.Price,
            //            station,
            //            stationAddress,
            //            stationTime,
            //            stationPrice,
            //        };

            //        // Додавання додаткових даних про транспортний засіб
            //        if (route.Vehicle is Bus)
            //        {
            //            rowData.Add("Bus");
            //            rowData.Add((route.Vehicle as Bus).Model);
            //            rowData.Add((route.Vehicle as Bus).NumberOfSeats);
            //            rowData.Add((route.Vehicle as Bus).HasToilet);
            //            rowData.Add((route.Vehicle as Bus).HasWifi);
            //            rowData.Add(null); // Для поля "isConditioning" не потрібно значення
            //        }
            //        else if (route.Vehicle is Microbus)
            //        {
            //            rowData.Add("Microbus");
            //            rowData.Add((route.Vehicle as Microbus).Model);
            //            rowData.Add((route.Vehicle as Microbus).NumberOfSeats);
            //            rowData.Add(null); // Для поля "isToilet" не потрібно значення
            //            rowData.Add(null); // Для поля "isWifi" не потрібно значення
            //            rowData.Add((route.Vehicle as Microbus).HasAirConditioning);
            //        }

            //        dataGridView1.Rows.Add(rowData.ToArray());
            //    }
            //}
        }


        private void AddNewRoute()
        {
            //int routeId = int.Parse(txtRoudeId.Text);
            //string startCity = txtStartCity.Text;
            //string endCity = txtEndCity.Text;
            //string addressStartCity = textBox2.Text;
            //string addressEndCity = textBox3.Text;
            //string departureDate = poisonDateTime1.Value.ToString("yyyy-MM-dd");
            //string arrivalDate = poisonDateTime2.Value.ToString("yyyy-MM-dd");
            //DateTime departureTime = DateTime.ParseExact(maskedTextBox1.Text, "HH:mm", CultureInfo.InvariantCulture);
            //DateTime arrivalTime = DateTime.ParseExact(maskedTextBox2.Text, "HH:mm", CultureInfo.InvariantCulture);
            //string station = comboBox2.Text;
            //DateTime stationTime = DateTime.ParseExact(maskedTextBox3.Text, "HH:mm", CultureInfo.InvariantCulture);
            //double stationPrice = double.Parse(textBox4.Text);
            //int price = int.Parse(textBox1.Text);

            //Vehicle vehicle;
            //if (comboBox3.SelectedItem.ToString() == "Bus")
            //{
            //    vehicle = new Bus
            //    {
            //        Model = txtModel.Text,
            //        NumberOfSeats = int.Parse(txtNumberOfSeats.Text),
            //        HasToilet = checkBox1.Checked,
            //        HasWifi = checkBox2.Checked
            //    };
            //}
            //else if (comboBox3.SelectedItem.ToString() == "Microbus")
            //{
            //    vehicle = new Microbus
            //    {
            //        Model = txtModel.Text,
            //        NumberOfSeats = int.Parse(txtNumberOfSeats.Text),
            //        HasAirConditioning = checkBox3.Checked
            //    };
            //}
            //else
            //{
            //    return;
            //}

            //string stationAddress = comboBox1.Text;

            //Route newRoute = new Route
            //{
            //    RouteId = routeId,
            //    StartCity = startCity,
            //    EndCity = endCity,
            //    ParkingAddressStartCity = addressStartCity,
            //    ParkingAddressEndCity = addressEndCity,
            //    DepartureDate = Convert.ToDateTime(departureDate),
            //    ArrivalDate = Convert.ToDateTime(arrivalDate),
            //    DepartureTime = departureTime,
            //    ArrivalTime = arrivalTime,
            //    Price = price,
            //    IntermediateStations = new List<(City, DateTime, double)> { (new City(station, stationAddress), stationTime, stationPrice) },
            //    Vehicle = vehicle
            //};

            //routes.Add(newRoute);

            //DisplayRoutesInDataGridView();
            //AddRouteToDatabase(newRoute);
            //ClearInputFields();
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
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
        }

        private void ClearStationFields()
        {
            maskedTextBox3.Clear();
            comboBox1.Text = "";
            comboBox2.Text = "";
            textBox4.Text = "";
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
                textBox1.Enabled = false;
                comboBox3.Enabled = false;
                txtModel.Enabled = false;
                txtNumberOfSeats.Enabled = false;
                checkBox1.Enabled = false;
                checkBox2.Enabled = false;
                checkBox3.Enabled = false;
                poisonDateTime1.Enabled = false;
                poisonDateTime2.Enabled = false;
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                string station = comboBox2.Text;
                string stationAddress = comboBox1.Text;
                string stationTime = maskedTextBox3.Text;
                string stationPrice = textBox4.Text;

                comboBox1.Text = "";
                comboBox2.Text = "";
                maskedTextBox3.Clear();
                textBox4.Clear();

                AddNewRoute(station, stationAddress, stationTime, stationPrice);
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
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                AddNewRoute();
            }
        }

        private void AddNewRoute(string station = null, string stationAddress = null, string stationTime = null, string stationPrice = null)
        {
            //int routeId = int.Parse(txtRoudeId.Text);
            //string startCity = txtStartCity.Text;
            //string endCity = txtEndCity.Text;
            //string addressStartCity = textBox2.Text;
            //string addressEndCity = textBox3.Text;
            //string departureDate = poisonDateTime1.Value.ToString("yyyy-MM-dd");
            //string arrivalDate = poisonDateTime2.Value.ToString("yyyy-MM-dd");
            //DateTime departureTime = DateTime.ParseExact(maskedTextBox1.Text, "HH:mm", CultureInfo.InvariantCulture);
            //DateTime arrivalTime = DateTime.ParseExact(maskedTextBox2.Text, "HH:mm", CultureInfo.InvariantCulture);
            //int price = int.Parse(textBox1.Text);

            //Vehicle vehicle;
            //if (comboBox3.SelectedItem.ToString() == "Bus")
            //{
            //    vehicle = new Bus
            //    {
            //        Model = txtModel.Text,
            //        NumberOfSeats = int.Parse(txtNumberOfSeats.Text),
            //        HasToilet = checkBox1.Checked,
            //        HasWifi = checkBox2.Checked
            //    };
            //}
            //else if (comboBox3.SelectedItem.ToString() == "Microbus")
            //{
            //    vehicle = new Microbus
            //    {
            //        Model = txtModel.Text,
            //        NumberOfSeats = int.Parse(txtNumberOfSeats.Text),
            //        HasAirConditioning = checkBox3.Checked
            //    };
            //}
            //else
            //{
            //    return;
            //}

            //string stationAddressToAdd = stationAddress ?? comboBox1.Text;
            //string stationToAdd = station ?? comboBox2.Text;
            //string stationTimeToAdd = stationTime ?? maskedTextBox3.Text;
            //string stationPriceToAdd = stationPrice ?? textBox4.Text;

            //Route newRoute = new Route
            //{
            //    RouteId = routeId,
            //    StartCity = startCity,
            //    EndCity = endCity,
            //    ParkingAddressStartCity = addressStartCity,
            //    ParkingAddressEndCity = addressEndCity,
            //    DepartureDate = Convert.ToDateTime(departureDate),
            //    ArrivalDate = Convert.ToDateTime(arrivalDate),
            //    DepartureTime = departureTime,
            //    ArrivalTime = arrivalTime,
            //    Price = price,
            //    Vehicle = vehicle
            //};

            //if (station != null && stationAddress != null && stationTime != null && stationPrice != null)
            //{
            //    newRoute.IntermediateStations = new List<(City, DateTime, double)> { (new City(stationToAdd, stationAddressToAdd), DateTime.ParseExact(stationTimeToAdd, "HH:mm", CultureInfo.InvariantCulture), double.Parse(stationPriceToAdd)) };
            //}

            //routes.Add(newRoute);

            //DisplayRoutesInDataGridView();
            //AddRouteToDatabase(newRoute);
            //ClearStationFields();
        }

        private void AddRouteToDatabase(Route route)
        {
            //using (MySqlConnection connection = new MySqlConnection(connectionString))
            //{
            //    string query = @"INSERT INTO routes (route_id, start_city, end_city, address_startCity, address_endCity, departure_date, departure_time, arrival_date, arrival_time, price, station_name, station_address, station_time, station_price, vehicle_type, model, number_of_seats, has_toilet, has_wifi, has_air_conditioning) 
            //            VALUES (@routeId, @startCity, @endCity, @addressStartCity, @addressEndCity, @departureDate, @departureTime, @arrivalDate, @arrivalTime, @price, @station, @stationAddress, @stationTime, @stationPrice, @vehicleType, @model, @numberOfSeats, @hasToilet, @hasWifi, @hasAirConditioning)";
            //    MySqlCommand command = new MySqlCommand(query, connection);

            //    // Параметризація SQL-запиту
            //    command.Parameters.AddWithValue("@routeId", route.RouteId);
            //    command.Parameters.AddWithValue("@startCity", route.StartCity);
            //    command.Parameters.AddWithValue("@endCity", route.EndCity);
            //    command.Parameters.AddWithValue("@addressStartCity", route.ParkingAddressStartCity);
            //    command.Parameters.AddWithValue("@addressEndCity", route.ParkingAddressEndCity);
            //    command.Parameters.AddWithValue("@departureDate", route.DepartureDate);
            //    command.Parameters.AddWithValue("@departureTime", route.DepartureTime);
            //    command.Parameters.AddWithValue("@arrivalDate", route.ArrivalDate);
            //    command.Parameters.AddWithValue("@arrivalTime", route.ArrivalTime);
            //    command.Parameters.AddWithValue("@price", route.Price);
            //    command.Parameters.AddWithValue("@station", route.IntermediateStations[0].Item1.NameOfCity);
            //    command.Parameters.AddWithValue("@stationAddress", route.IntermediateStations[0].Item1.ParkingAddress);
            //    command.Parameters.AddWithValue("@stationTime", route.IntermediateStations[0].Item2);
            //    command.Parameters.AddWithValue("@stationPrice", route.IntermediateStations[0].Item3);
            //    command.Parameters.AddWithValue("@vehicleType", route.Vehicle.GetType().Name);
            //    command.Parameters.AddWithValue("@model", (route.Vehicle as Bus)?.Model ?? (route.Vehicle as Microbus)?.Model);
            //    command.Parameters.AddWithValue("@numberOfSeats", (route.Vehicle as Bus)?.NumberOfSeats ?? (route.Vehicle as Microbus)?.NumberOfSeats);
            //    command.Parameters.AddWithValue("@hasToilet", (route.Vehicle as Bus)?.HasToilet ?? false);
            //    command.Parameters.AddWithValue("@hasWifi", (route.Vehicle as Bus)?.HasWifi ?? false);
            //    command.Parameters.AddWithValue("@hasAirConditioning", (route.Vehicle as Microbus)?.HasAirConditioning ?? false);

            //    try
            //    {
            //        connection.Open();
            //        int rowsAffected = command.ExecuteNonQuery();
            //        if (rowsAffected < 0)
            //        {
            //            MessageBox.Show("Не вдалося додати маршрут до бази даних.");
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("Помилка під час додавання маршруту до бази даних: " + ex.Message);
            //    }
            //    finally
            //    {
            //        connection.Close();
            //    }
            //}
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM routes";

                MySqlCommand command = new MySqlCommand(query, connection);

                try
                {
                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string routeId = reader["route_id"].ToString();
                            string startCity = reader["start_city"].ToString();
                            string endCity = reader["end_city"].ToString();
                            string addressStartCity = reader["address_startCity"].ToString();
                            string addressEndCity = reader["address_endCity"].ToString();
                            string departureDate = DateTime.Parse(reader["departure_date"].ToString()).ToString("yyyy-MM-dd");
                            string departureTime = DateTime.Parse(reader["departure_time"].ToString()).ToString("HH:mm");
                            string arrivalDate = DateTime.Parse(reader["arrival_date"].ToString()).ToString("yyyy-MM-dd");
                            string arrivalTime = DateTime.Parse(reader["arrival_time"].ToString()).ToString("HH:mm");
                            string price = reader["price"].ToString();
                            string stationName = reader["station_name"].ToString();
                            string stationAddress = reader["station_address"].ToString();
                            string stationTime = DateTime.Parse(reader["station_time"].ToString()).ToString("HH:mm");
                            string stationPrice = reader["station_price"].ToString();
                            string vehicleType = reader["vehicle_type"].ToString();
                            string vehicleModel = reader["model"].ToString();
                            string numberOfSeats = reader["number_of_seats"].ToString();
                            string hasToilet = reader["has_toilet"].ToString();
                            string hasWifi = reader["has_wifi"].ToString();
                            string hasAirConditioning = reader["has_air_conditioning"].ToString();

                            dataGridView1.Rows.Add(routeId, startCity, endCity, addressStartCity, addressEndCity, departureDate, departureTime, arrivalDate, arrivalTime, price, stationName, stationAddress, stationTime, stationPrice, vehicleType, vehicleModel, numberOfSeats, hasToilet, hasWifi, hasAirConditioning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка під час відображення маршрутів: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
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

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
                
                int routeId = int.Parse(dataGridView1.Rows[selectedRowIndex].Cells[0].Value.ToString());
                string startCity = dataGridView1.Rows[selectedRowIndex].Cells[1].Value.ToString();
                string endCity = dataGridView1.Rows[selectedRowIndex].Cells[2].Value.ToString();
                string addressStartCity = dataGridView1.Rows[selectedRowIndex].Cells[3].Value.ToString();
                string addressEndCity = dataGridView1.Rows[selectedRowIndex].Cells[4].Value.ToString();
                string departureDate = dataGridView1.Rows[selectedRowIndex].Cells[5].Value.ToString();
                string departureTime = dataGridView1.Rows[selectedRowIndex].Cells[6].Value.ToString();
                string arrivalDate = dataGridView1.Rows[selectedRowIndex].Cells[7].Value.ToString();
                string arrivalTime = dataGridView1.Rows[selectedRowIndex].Cells[8].Value.ToString();
                string price = dataGridView1.Rows[selectedRowIndex].Cells[9].Value.ToString();
                string stationName = dataGridView1.Rows[selectedRowIndex].Cells[10].Value.ToString();
                string stationAddress = dataGridView1.Rows[selectedRowIndex].Cells[11].Value.ToString();
                string stationTime = dataGridView1.Rows[selectedRowIndex].Cells[12].Value.ToString();
                string stationPrice = dataGridView1.Rows[selectedRowIndex].Cells[13].Value.ToString();
                string vehicleType = dataGridView1.Rows[selectedRowIndex].Cells[14].Value.ToString();
                string model = dataGridView1.Rows[selectedRowIndex].Cells[15].Value.ToString();
                string numberOfSeats = dataGridView1.Rows[selectedRowIndex].Cells[16].Value.ToString();

                DeleteRoute(routeId, startCity, endCity, addressStartCity, addressEndCity, departureDate, departureTime, arrivalDate, arrivalTime, price, stationName, stationAddress, stationTime, stationPrice, vehicleType, model, numberOfSeats);

                dataGridView1.Rows.RemoveAt(selectedRowIndex);
            }
            else
            {
                MessageBox.Show("Виберіть рядок, який потрібно видалити.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteRoute(int routeId, string startCity, string endCity, string addressStartCity, string addressEndCity, string departureDate, string departureTime, string arrivalDate, string arrivalTime, string price, string stationName, string stationAddress, string stationTime, string stationPrice, string vehicleType, string model, string numberOfSeats)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"DELETE FROM routes 
                             WHERE route_id = @routeId 
                             AND start_city = @startCity 
                             AND end_city = @endCity 
                             AND address_startCity = @addressStartCity 
                             AND address_endCity = @addressEndCity 
                             AND departure_date = @departureDate 
                             AND departure_time = @departureTime 
                             AND arrival_date = @arrivalDate 
                             AND arrival_time = @arrivalTime 
                             AND price = @price 
                             AND station_name = @stationName 
                             AND station_address = @stationAddress 
                             AND station_time = @stationTime 
                             AND station_price = @stationPrice 
                             AND vehicle_type = @vehicleType 
                             AND model = @model 
                             AND number_of_seats = @numberOfSeats";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@routeId", routeId);
                    command.Parameters.AddWithValue("@startCity", startCity);
                    command.Parameters.AddWithValue("@endCity", endCity);
                    command.Parameters.AddWithValue("@addressStartCity", addressStartCity);
                    command.Parameters.AddWithValue("@addressEndCity", addressEndCity);
                    command.Parameters.AddWithValue("@departureDate", departureDate);
                    command.Parameters.AddWithValue("@departureTime", departureTime);
                    command.Parameters.AddWithValue("@arrivalDate", arrivalDate);
                    command.Parameters.AddWithValue("@arrivalTime", arrivalTime);
                    command.Parameters.AddWithValue("@price", price);
                    command.Parameters.AddWithValue("@stationName", stationName);
                    command.Parameters.AddWithValue("@stationAddress", stationAddress);
                    command.Parameters.AddWithValue("@stationTime", stationTime);
                    command.Parameters.AddWithValue("@StationPrice", stationPrice);
                    command.Parameters.AddWithValue("@vehicleType", vehicleType);
                    command.Parameters.AddWithValue("@model", model);
                    command.Parameters.AddWithValue("@numberOfSeats", numberOfSeats);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка видалення маршруту з бази даних: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
