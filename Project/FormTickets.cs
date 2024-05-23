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
    public partial class FormTickets : Form
    {
        private MySqlConnection connection;

        string connectionString = "server=127.0.0.1;database=db;uid=root;pwd=Gd_135790;";

        public FormTickets()
        {
            InitializeComponent();
            connection = new MySqlConnection(connectionString);
        }

        private void FormTickets_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
        }

        public void LoadBookedTickets(string phoneNumber)
        {
            FormHome homeForm = (FormHome)Application.OpenForms["FormHome"];

            if (homeForm != null && homeForm.IsUserLoggedIn)
            {
                int spacing = 20;
                int startX = 40;
                int startY = 30;
                bool hasTickets = false;

                foreach (Control control in this.Controls)
                {
                    if (control is TicketControl)
                    {
                        this.Controls.Remove(control);
                    }
                }

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"SELECT
                        t.ticket_id,
                        t.route_id,
                        t.start_city,
                        t.end_city,
                        t.departure_date,
                        t.departure_time,
                        t.arrival_date,
                        t.arrival_time,
                        t.seat_number,
                        t.passenger_name,
                        t.passenger_surname,
                        t.price,
                        start_station.station_name AS start_station,
                        end_station.station_name AS end_station,
                        v.vehicle_type,
                        v.model,
                        start_addr.station_address AS start_station_address,
                        end_addr.station_address AS end_station_address
                    FROM
                        tickets t
                    JOIN intermediatestations start_station ON t.start_city = start_station.station_city AND t.route_id = start_station.routeId
                    JOIN intermediatestations end_station ON t.end_city = end_station.station_city AND t.route_id = end_station.routeId
                    JOIN routes r ON t.route_id = r.route_id
                    JOIN vehicles v ON r.route_id = v.route_id
                    JOIN characteristicsofstations start_addr ON start_station.station_city = start_addr.station_city AND start_station.station_name = start_addr.station_name
                    JOIN characteristicsofstations end_addr ON end_station.station_city = end_addr.station_city AND end_station.station_name = end_addr.station_name
                    WHERE
                        t.phone_number = @phoneNumber
                    ";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                    MySqlDataReader reader = command.ExecuteReader();

                    int currentY = startY;

                    while (reader.Read())
                    {
                        hasTickets = true;

                        TicketControl ticketControl = new TicketControl();
                        ticketControl.hopeRoundButton1.Visible = false;
                        ticketControl.hopeRoundButton2.Visible = true;
                        ticketControl.RouteId = reader.GetInt32("route_id");
                        ticketControl.label1.Text = reader.GetTimeSpan("departure_time").ToString(@"hh\:mm");
                        ticketControl.label2.Text = reader.GetTimeSpan("arrival_time").ToString(@"hh\:mm");
                        ticketControl.label3.Text = reader.GetDateTime("departure_date").ToString("yyyy-MM-dd");
                        ticketControl.label4.Text = reader.GetDateTime("arrival_date").ToString("yyyy-MM-dd");
                        ticketControl.label6.Text = reader.GetString("start_city");
                        ticketControl.label7.Text = reader.GetString("end_city");
                        ticketControl.label8.Text = reader.GetString("start_station");
                        ticketControl.label9.Text = reader.GetString("end_station");
                        ticketControl.label10.Text = reader.GetInt32("price").ToString();
                        ticketControl.label13.Text = reader.GetString("passenger_name") + " " + reader.GetString("passenger_surname");
                        ticketControl.label15.Text = reader.GetInt32("seat_number").ToString();
                        ticketControl.label16.Text = reader.GetString("vehicle_type");
                        ticketControl.label17.Text = reader.GetString("model");
                        ticketControl.label18.Text = reader.GetString("start_station_address");
                        ticketControl.label19.Text = reader.GetString("end_station_address");
                        ticketControl.label20.Text = reader.GetInt32("ticket_id").ToString();

                        // Розрахунок різниці між датою/часом прибуття та відправлення
                        DateTime departureDateTime = reader.GetDateTime("departure_date") + reader.GetTimeSpan("departure_time");
                        DateTime arrivalDateTime = reader.GetDateTime("arrival_date") + reader.GetTimeSpan("arrival_time");
                        TimeSpan travelDuration = arrivalDateTime - departureDateTime;
                        int days = travelDuration.Days;
                        int hours = travelDuration.Hours % 24;
                        int minutes = travelDuration.Minutes;
                        ticketControl.label5.Text = $"{days * 24 + hours} год. {minutes} хв. в дорозі";

                        ticketControl.Location = new Point(startX, currentY);
                        this.Controls.Add(ticketControl);

                        currentY += ticketControl.Height + spacing;
                    }

                    reader.Close();
                }

                if (!hasTickets)
                {
                    MessageBox.Show($"Немає заброньованих квитків для такого користувача {phoneNumber}.", "Відсутні квитки", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Ви повинні увійти в систему, щоб переглянути свої квитки.", "Увійдіть в систему", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
