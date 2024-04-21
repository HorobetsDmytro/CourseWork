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

            // Перевіряємо, чи не пусті дані
            if (!string.IsNullOrEmpty(countryName) && !string.IsNullOrEmpty(cityName) && !string.IsNullOrEmpty(parkingAddress))
            {
                // Створюємо об'єкт Country
                Country country = new Country();
                country.NameOfCountry = countryName;

                // Ініціалізуємо список міст для кожного об'єкта Country
                country.CityList = new List<City>();

                // Створюємо об'єкт City
                City city = new City();
                city.NameOfCity = cityName;
                city.ParkingAddress = parkingAddress;

                // Додаємо місто до країни
                country.CityList.Add(city);

                // Додаємо країну до списку країн
                countries.Add(country);

                txtCity.Text = "";
                txtCountry.Text = "";
                txtParking.Text = "";

                // Вставляємо дані до таблиці в базі даних
                InsertData(countryName, cityName, parkingAddress);

                // Оновлюємо DataGridView
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
            // Очищаємо дані у DataGridView
            dataGridView1.Rows.Clear();

            // Додаємо всі міста з усіх країн у DataGridView
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

                // Очищаємо дані у DataGridView перед додаванням нових записів
                dataGridView1.Rows.Clear();

                while (reader.Read())
                {
                    // Отримуємо значення з кожного стовпця в поточному рядку
                    string country = reader.GetString("country");
                    string city = reader.GetString("city");
                    string parking = reader.GetString("parking_address");

                    // Додаємо запис до DataGridView
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

                // Отримуємо значення з першого стовпця, яке відповідає ідентифікатору запису в базі даних
                string countryName = dataGridView1.Rows[selectedRowIndex].Cells[0].Value.ToString();
                string cityName = dataGridView1.Rows[selectedRowIndex].Cells[1].Value.ToString();

                // Видаляємо запис з бази даних
                DeleteData(countryName, cityName);

                // Видаляємо рядок з DataGridView
                dataGridView1.Rows.RemoveAt(selectedRowIndex);
            }
            else
            {
                MessageBox.Show("Виберіть рядок, який потрібно видалити.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteData(string country, string city)
        {
            try
            {
                connection.Open();
                string query = "DELETE FROM countries WHERE country = @country AND city = @city";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@country", country);
                command.Parameters.AddWithValue("@city", city);
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
