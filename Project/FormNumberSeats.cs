using MySql.Data.MySqlClient;
using ReaLTaiizor.Controls;
using System;
using System.Windows.Forms;

namespace Project
{
    public partial class FormNumberSeats : Form
    {
        private int routeId;
        private string connectionString = "server=127.0.0.1;database=db;uid=root;pwd=Gd_135790;";
        private TicketControl selectedTicketControl;
        private string userPhoneNumber;

        public FormNumberSeats(TicketControl ticketControl, string phoneNumber)
        {
            InitializeComponent();
            selectedTicketControl = ticketControl;
            routeId = selectedTicketControl.RouteId;
            userPhoneNumber = phoneNumber;
            ShowVehicleAndModel();
        }

        private void FormNumberSeats_Load(object sender, EventArgs e)
        {
            // Код у цьому місці не потрібен
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtNumberOfSeats.Text, out int numberOfSeats))
            {
                if (numberOfSeats > 0)
                {
                    numberOfSeats--;
                    txtNumberOfSeats.Text = numberOfSeats.ToString();
                }
                else
                {
                    MessageBox.Show("Кількість місць вже дорівнює нулю.", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Введіть коректне числове значення кількості місць.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtNumberOfSeats.Text, out int numberOfSeats))
            {
                numberOfSeats++;
                txtNumberOfSeats.Text = numberOfSeats.ToString();
            }
            else
            {
                MessageBox.Show("Введіть коректне числове значення кількості місць.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int numberOfSeats;
            if (int.TryParse(txtNumberOfSeats.Text, out numberOfSeats))
            {
                int maxSeats = int.Parse(label4.Text);

                if (numberOfSeats == 0)
                {
                    MessageBox.Show("Кількість місць не може бути нульовою.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (numberOfSeats > maxSeats)
                {
                    MessageBox.Show($"Кількість місць не може перевищувати максимальну кількість вільних місць ({maxSeats}).", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    FormPassengers formPassengers = new FormPassengers(numberOfSeats, selectedTicketControl, userPhoneNumber);
                    formPassengers.ShowDialog();
                    this.Close(); // Закриваємо поточну форму
                }
            }
            else
            {
                MessageBox.Show("Введіть коректну кількість місць.");
            }
        }

        private void ShowVehicleAndModel()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT\r\n    v.vehicle_type,\r\n    v.model,\r\n    v.number_of_seats - IFNULL((\r\n        SELECT COUNT(*)\r\n        FROM tickets t\r\n        WHERE t.route_id = v.route_id\r\n            AND ((t.start_city = @startCity AND t.end_city = @endCity)\r\n                OR (t.start_city = @startCity AND @endCity NOT IN (SELECT station_city FROM intermediatestations WHERE routeId = t.route_id AND order_index < (SELECT order_index FROM intermediatestations WHERE station_city = t.start_city AND routeId = t.route_id)))\r\n                OR (@startCity NOT IN (SELECT station_city FROM intermediatestations WHERE routeId = t.route_id AND order_index > (SELECT order_index FROM intermediatestations WHERE station_city = t.end_city AND routeId = t.route_id)) AND t.end_city = @endCity))\r\n            \r\n    ), 0) AS available_seats\r\nFROM vehicles v\r\nWHERE v.route_id = @routeId;\r\n";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@routeId", routeId);
                command.Parameters.AddWithValue("@startCity", selectedTicketControl.label6.Text);
                command.Parameters.AddWithValue("@endCity", selectedTicketControl.label7.Text);

                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    string vehicleType = reader.GetString(0);
                    string model = reader.GetString(1);
                    int availableSeats = reader.GetInt32(2);

                    label1.Text = vehicleType;
                    label2.Text = model;
                    label4.Text = availableSeats.ToString();
                }
                else
                {
                    MessageBox.Show("Не вдалося отримати дані про транспортний засіб.");
                }

                reader.Close();
            }
        }
    }
}