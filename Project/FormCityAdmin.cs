using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Project
{
    public partial class FormCityAdmin : Form
    {
        private MySqlConnection connection;
        private string connectionString = "server=127.0.0.1;database=db;uid=root;pwd=Gd_135790;";

        public FormCityAdmin()
        {
            InitializeComponent();
            connection = new MySqlConnection(connectionString);
            LoadCitiesAutoComplete();
            
            //poisonDateTime2.Enabled = false;
            //maskedTextBox2.Enabled = false;
            maskedTextBox1.Validating += maskedTextBox_Validating;
            maskedTextBox2.Validating += maskedTextBox_Validating;
        }

        private void FormCityAdmin_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
            dataGridView1.AutoGenerateColumns = false;
        }

        private void txtRouteID_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtRouteID.Text))
            {
                int routeId = Convert.ToInt32(txtRouteID.Text);
                bool isFirstRecord = !CheckIfRouteIdExists(routeId, connection);
                ResetDateTimeFields(isFirstRecord);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (poisonDateTime1.Value < DateTime.Now.Date)
            {
                MessageBox.Show("Дата відправлення не може бути в минулому.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Перевірка, чи дата прибуття не є в минулому
            if (poisonDateTime2.Value.Date < DateTime.Now.Date)
            {
                MessageBox.Show("Дата прибуття не може бути в минулому.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Перевірка, чи дата прибуття пізніше дати відправлення
            if (poisonDateTime2.Value.Date < poisonDateTime1.Value.Date)
            {
                MessageBox.Show("Дата прибуття не може бути раніше дати відправлення.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            TimeSpan depTime;
            TimeSpan arrTime;
            if (poisonDateTime1.Value.Date == poisonDateTime2.Value.Date)
            {
                
                bool isDepartureTimeValid = TimeSpan.TryParseExact(maskedTextBox1.Text, "hh\\:mm", CultureInfo.InvariantCulture, out depTime);
                bool isArrivalTimeValid = TimeSpan.TryParseExact(maskedTextBox2.Text, "hh\\:mm", CultureInfo.InvariantCulture, out arrTime);

                if (isDepartureTimeValid && isArrivalTimeValid && arrTime <= depTime)
                {
                    MessageBox.Show("Час прибуття не може бути раніше або дорівнювати часу відправлення у той же день.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            int routeId = Convert.ToInt32(txtRouteID.Text);
            string stationCity = txtCity.Text;
            string stationName = txtStation.Text;
            string nextStationCity = txtNextCity.Text;
            string nextStationName = txtNextStation.Text;
            string price = txtPrice.Text;
            DateTime? departureDate = null;
            DateTime? arrivalDate = null;
            TimeSpan? departureTime = null;
            TimeSpan? arrivalTime = null;

            bool isFirstRecord = !CheckIfRouteIdExists(routeId, connection);

            if (isFirstRecord)
            {
                departureDate = poisonDateTime1.Value;
                if (!string.IsNullOrEmpty(maskedTextBox1.Text))
                {
                    departureTime = TimeSpan.ParseExact(maskedTextBox1.Text, "h\\:mm", null);
                }

                // Ask the user if they want to add another station
                DialogResult result = MessageBox.Show("Ви хочете додати ще одну станцію?", "Додавання станцій", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    // If only one station is entered, display an error message
                    MessageBox.Show("Щонайменше дві станції має бути на маршруті.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    poisonDateTime1.Enabled = false;
                    maskedTextBox1.Enabled = false;
                    poisonDateTime2.Enabled = true;
                    maskedTextBox2.Enabled = true;
                    // Save the first record with next_station value
                    IntermediateStation intermediateStation = new IntermediateStation(routeId, stationCity, stationName, nextStationCity, nextStationName, price, departureDate.Value, departureTime ?? TimeSpan.Zero, DateTime.MinValue, TimeSpan.Zero);
                    intermediateStation.SaveToDatabase(connection);

                    //string stationCity = txtCity.Text;
                    string stationAddress = txtAddress.Text;
                    Station station = new Station(stationCity, stationName, stationAddress);
                    station.SaveToDatabase(connection, stationName);

                    dataGridView1.Rows.Add(stationCity, stationName, stationAddress);

                    // Clear fields for the next station entry
                    ClearFieldsForNextStation();
                }
            }
            else
            {
                // For subsequent records, retrieve existing values from the database
                GetExistingDepartureDateAndTime(routeId, out departureDate, out departureTime);

                // Validate arrival date/time input
                if (!string.IsNullOrEmpty(maskedTextBox2.Text) && maskedTextBox2.Text.Length == 5)
                {
                    arrivalTime = TimeSpan.ParseExact(maskedTextBox2.Text, "h\\:mm", null);
                }
                else
                {
                    MessageBox.Show("Введіть час прибуття для наступної станції.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (poisonDateTime2.Value != DateTime.MinValue)
                {
                    arrivalDate = poisonDateTime2.Value;
                }
                else
                {
                    MessageBox.Show("Введіть дату прибуття для наступної станції.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validate next station name for subsequent records only when adding a new station
                if (nextStationName == string.Empty && stationName != string.Empty && txtNextStation.Focused)
                {
                    MessageBox.Show("Введіть назву наступної станції.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                TimeSpan departureTimeValue = departureTime ?? TimeSpan.Zero;
                IntermediateStation intermediateStation = new IntermediateStation(routeId, stationCity, stationName, nextStationCity, nextStationName, price, departureDate ?? DateTime.MinValue, departureTimeValue, arrivalDate.Value, arrivalTime.Value);

                intermediateStation.SaveToDatabase(connection);

                //string stationCity = txtCity.Text;
                string stationAddress = txtAddress.Text;
                Station station = new Station(stationCity, stationName, stationAddress);
                station.SaveToDatabase(connection, stationName);

                dataGridView1.Rows.Add(stationCity, stationName, stationAddress);

                // Clear fields for the next station entry
                ClearFieldsForNextStation();
            }
        }

        private void ResetDateTimeFields(bool isFirstRecord)
        {
            if (isFirstRecord)
            {
                poisonDateTime1.Enabled = true;
                maskedTextBox1.Enabled = true;
                poisonDateTime2.Enabled = true;
                maskedTextBox2.Enabled = true;
            }
            else
            {
                poisonDateTime1.Enabled = false;
                maskedTextBox1.Enabled = false;
                poisonDateTime2.Enabled = true;
                maskedTextBox2.Enabled = true;
            }
        }

        private void ClearFieldsForNextStation()
        {
            txtCity.Clear();
            txtStation.Clear();
            txtAddress.Clear();
            txtPrice.Clear();
            txtNextCity.Clear();
            txtNextStation.Clear();
            txtNextStationAddress.Clear();
            poisonDateTime2.Value = DateTime.Now;
            maskedTextBox2.Text = string.Empty;
        }

        private void GetExistingDepartureDateAndTime(int routeId, out DateTime? departureDate, out TimeSpan? departureTime)
        {
            departureDate = null;
            departureTime = null;
        }

        private bool CheckIfRouteIdExists(int routeId, MySqlConnection connection)
        {
            string query = "SELECT COUNT(*) FROM intermediatestations WHERE routeId = @routeId";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@routeId", routeId);

                try
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка: {ex.Message}");
                    return false;
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

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                string query = "SELECT * FROM characteristicsofstations";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                dataGridView1.Rows.Clear();

                while (reader.Read())
                {
                    string stationCity = reader.GetString("station_city");
                    string stationName = reader.GetString("station_name");
                    string stationAddress = reader.GetString("station_address");

                    dataGridView1.Rows.Add(stationCity, stationName, stationAddress);
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

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;

                string stationCity = dataGridView1.Rows[selectedRowIndex].Cells[0].Value.ToString();
                string stationName = dataGridView1.Rows[selectedRowIndex].Cells[1].Value.ToString();
                string stationAddress = dataGridView1.Rows[selectedRowIndex].Cells[2].Value.ToString();

                DeleteData(stationCity, stationName, stationAddress);

                dataGridView1.Rows.RemoveAt(selectedRowIndex);
            }
            else
            {
                MessageBox.Show("Виберіть рядок, який потрібно видалити.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteData(string stationCity, string stationName, string stationAddress)
        {
            try
            {
                connection.Open();
                string query = "DELETE FROM characteristicsofstations WHERE station_city = @stationCity AND station_name = @stationName AND station_address = @stationAddress";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@stationCity", stationCity);
                command.Parameters.AddWithValue("@stationName", stationName);
                command.Parameters.AddWithValue("@stationAddress", stationAddress);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка видалення даних з бази даних: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private void LoadCitiesAutoComplete()
        {
            AutoCompleteStringCollection citiesCollection = new AutoCompleteStringCollection();

            try
            {
                connection.Open();
                string query = "SELECT DISTINCT station_city FROM characteristicsofstations";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string city = reader.GetString("station_city");
                    citiesCollection.Add(city);
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

            txtCity.AutoCompleteCustomSource = citiesCollection;
            txtCity.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtCity.AutoCompleteSource = AutoCompleteSource.CustomSource;

            txtNextCity.AutoCompleteCustomSource = citiesCollection;
            txtNextCity.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtNextCity.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private void LoadStationsForCity(string selectedCity, TextBox textBox)
        {
            AutoCompleteStringCollection stationsCollection = new AutoCompleteStringCollection();

            try
            {
                connection.Open();
                string query = "SELECT DISTINCT station_name FROM characteristicsofstations WHERE station_city = @selectedCity";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@selectedCity", selectedCity);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string stationName = reader.GetString("station_name");
                    stationsCollection.Add(stationName);
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

            textBox.AutoCompleteCustomSource = stationsCollection;
            textBox.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;

            //txtNextStationAddress.AutoCompleteCustomSource = stationsCollection;
            //txtNextStationAddress.AutoCompleteMode = AutoCompleteMode.Suggest;
            //txtNextStationAddress.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private void txtCity_TextChanged(object sender, EventArgs e)
        {
            string selectedCity = txtCity.Text;
            LoadStationsForCity(selectedCity, txtStation);
        }

        private void txtNextCity_TextChanged(object sender, EventArgs e)
        {
            string selectedCity = txtNextCity.Text;
            LoadStationsForCity(selectedCity, txtNextStation);
        }

        private void GetStationAddress(string stationName, TextBox text)
        {
            try
            {
                connection.Open();
                string query = "SELECT station_address FROM characteristicsofstations WHERE station_name = @stationName";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@stationName", stationName);
                object result = command.ExecuteScalar();

                if (result != null)
                {
                    string stationAddress = result.ToString();
                    text.Text = stationAddress;
                }
                else
                {
                    text.Clear();
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

        private void txtStation_TextChanged(object sender, EventArgs e)
        {
            string selectedStationName = txtStation.Text;
            GetStationAddress(selectedStationName, txtAddress);
        }

        private void txtNextStation_TextChanged_1(object sender, EventArgs e)
        {
            string selectedStationName = txtNextStation.Text;
            GetStationAddress(selectedStationName, txtNextStationAddress);
        }
        
        private void txtRouteID_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Перевірка, чи введений символ є цифрою
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Якщо не цифра і не контрольний символ (наприклад, Backspace), блокуємо ввід
            }

            // Перевірка на максимальну довжину
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.Text.Length >= 5 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Якщо довжина тексту вже 5 символів і введений символ не є контрольним, блокуємо ввід
            }
        }

        private void txtCity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && e.KeyChar != '\'' && e.KeyChar != '-' && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Якщо символ не відповідає критеріям, блокуємо його ввід
            }
        }

        private void maskedTextBox_Validating(object sender, CancelEventArgs e)
        {
            MaskedTextBox maskedTextBox = sender as MaskedTextBox;
            if (maskedTextBox != null)
            {
                TimeSpan timeResult;
                bool isValidTime = TimeSpan.TryParseExact(maskedTextBox.Text, "hh\\:mm", CultureInfo.InvariantCulture, out timeResult);

                if (!isValidTime)
                {
                    MessageBox.Show("Введено некоректний час. Будь ласка, введіть час у форматі ГГ:ХХ.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true; // Залишаємо фокус на елементі для корекції
                }
            }
        }
    }
}
