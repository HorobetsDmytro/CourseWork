using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Project
{
    public partial class FormCityAdmin : Form
    {
        private List<Country> countries = new List<Country>();
        private MySqlConnection connection;
        private string connectionString = "server=127.0.0.1;database=db;uid=root;pwd=Gd_135790;";

        public FormCityAdmin()
        {
            InitializeComponent();
            connection = new MySqlConnection(connectionString);
        }

        private void FormCityAdmin_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
            dataGridView1.AutoGenerateColumns = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string countryName = txtCountry.Text;
            string cityName = txtCity.Text;
            string parkingAddress = txtParking.Text;

            if (!string.IsNullOrEmpty(countryName) && !string.IsNullOrEmpty(cityName) && !string.IsNullOrEmpty(parkingAddress))
            {
                Country country = new Country();
                country.NameOfCountry = countryName;

                country.CityList = new List<City>();

                City city = new City();
                city.NameOfCity = cityName;
                city.ParkingAddress = parkingAddress;

                country.CityList.Add(city);

                countries.Add(country);

                txtCity.Text = "";
                txtCountry.Text = "";
                txtParking.Text = "";

                InsertData(countryName, cityName, parkingAddress);

                DisplayAllCitiesInDataGridView();
            }
            else
            {
                MessageBox.Show("Будь ласка, заповніть всі поля.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InsertData(string country, string city, string parking)
        {
            try
            {
                connection.Open();
                string query = "INSERT INTO countries (country, city, parking_address) VALUES (@country, @city, @parking)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@country", country);
                command.Parameters.AddWithValue("@city", city);
                command.Parameters.AddWithValue("@parking", parking);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка додавання даних до бази даних: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private void DisplayAllCitiesInDataGridView()
        {
            dataGridView1.Rows.Clear();

            foreach (var country in countries)
            {
                foreach (var city in country.CityList)
                {
                    dataGridView1.Rows.Add(country.NameOfCountry, city.NameOfCity, city.ParkingAddress);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                string query = "SELECT * FROM countries";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                dataGridView1.Rows.Clear();

                while (reader.Read())
                {
                    string country = reader.GetString("country");
                    string city = reader.GetString("city");
                    string parking = reader.GetString("parking_address");

                    dataGridView1.Rows.Add(country, city, parking);
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

                string countryName = dataGridView1.Rows[selectedRowIndex].Cells[0].Value.ToString();
                string cityName = dataGridView1.Rows[selectedRowIndex].Cells[1].Value.ToString();
                string address = dataGridView1.Rows[selectedRowIndex].Cells[2].Value.ToString();

                DeleteData(countryName, cityName, address);

                dataGridView1.Rows.RemoveAt(selectedRowIndex);
            }
            else
            {
                MessageBox.Show("Виберіть рядок, який потрібно видалити.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteData(string country, string city, string address)
        {
            try
            {
                connection.Open();
                string query = "DELETE FROM countries WHERE country = @country AND city = @city AND parking_address = @parkingAddress";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@country", country);
                command.Parameters.AddWithValue("@city", city);
                command.Parameters.AddWithValue("@parkingAddress", address);
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
    }
}
