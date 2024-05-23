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
    public partial class FormPassengers : Form
    {
        private int numberOfPassengers;
        private TicketControl selectedTicketControl;
        public decimal TotalPrice { get; set; }
        private MySqlConnection connection;
        string connectionString = "server=127.0.0.1;database=db;uid=root;pwd=Gd_135790;";
        public string UserPhoneNumber { get; set; } // Додано нову властивість

        public FormPassengers(int numPassengers, TicketControl selectedTicketControl, string userPhoneNumber)
        {
            InitializeComponent();

            connection = new MySqlConnection(connectionString);
            numberOfPassengers = numPassengers;
            UserPhoneNumber = userPhoneNumber;
            button1 = this.Controls.OfType<Button>().FirstOrDefault(b => b.Name == "button1");

            this.selectedTicketControl = selectedTicketControl;
            CreatePassengersInfo();
        }

        

        private void CreatePassengersInfo()
        {
            int startY = 20;
            int maxY = startY;
            this.Controls.Clear();
            TotalPrice = 0; // Обнулити загальну ціну

            for (int i = 0; i < numberOfPassengers; i++)
            {
                PassengersInfo passengerInfo = new PassengersInfo(selectedTicketControl);
                passengerInfo.Location = new Point(20, startY);
                this.Controls.Add(passengerInfo);
                startY += passengerInfo.Height + 20;
                maxY = startY;
                passengerInfo.lblNumberOfPassengers.Text = Convert.ToString(i + 1);

                // Встановити ціну квитка та додати її до TotalPrice
                int ticketPrice = selectedTicketControl.TicketPrice;
                passengerInfo.SetPrice(ticketPrice);
                TotalPrice += ticketPrice;
            }

            // Додати CustomerInfo після останнього PassengersInfo
            CustomerInfo customerInfo = new CustomerInfo();
            customerInfo.Location = new Point(20, startY);
            this.Controls.Add(customerInfo);
            startY += customerInfo.Height + 20;
            maxY = startY;

            Button buttonToRemove = this.Controls.OfType<Button>().FirstOrDefault(b => b.Name == "button1");
            if (buttonToRemove != null)
            {
                this.Controls.Remove(buttonToRemove);
            }

            int buttonY = maxY + 20;
            button1.Location = new Point((this.ClientSize.Width - button1.Width) / 2, buttonY);
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.Controls.Add(button1);

            int labelY = buttonY - 30; // Зміна відступу для label1, label2, label3
            int labelCenterX = this.ClientSize.Width / 2; // Центральна точка по горизонталі

            label1.Location = new Point(labelCenterX - label1.Width / 2 - 50, labelY);
            this.Controls.Add(label1);

            label2.Location = new Point(labelCenterX - label2.Width / 2 + 50, labelY);
            this.Controls.Add(label2);

            label3.Location = new Point(labelCenterX + label2.Width / 2 + 50, labelY); // Зміщення label3 праворуч від label2
            label2.Text = TotalPrice.ToString(); // Форматувати TotalPrice як валюту
            this.Controls.Add(label3);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void FormPassengers_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Отримуємо дані з вибраного TicketControl
            int routeId = selectedTicketControl.RouteId;
            string startCity = selectedTicketControl.label6.Text;
            string endCity = selectedTicketControl.label7.Text;
            DateTime departureDate = DateTime.Parse(selectedTicketControl.label3.Text);
            TimeSpan departureTime = TimeSpan.Parse(selectedTicketControl.label1.Text);
            DateTime arrivalDate = DateTime.Parse(selectedTicketControl.label4.Text);
            TimeSpan arrivalTime = TimeSpan.Parse(selectedTicketControl.label2.Text);

            // Отримуємо кількість вільних місць для вибраного маршруту
            int availableSeats = GetAvailableSeats(routeId);

            // Створюємо квитки для кожного пасажира
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                int seatNumber = 1;
                List<int> bookedSeats = GetBookedSeats(routeId, departureDate, connection);

                foreach (PassengersInfo passengerInfo in this.Controls.OfType<PassengersInfo>())
                {
                    // Знаходимо наступне вільне місце
                    while (bookedSeats.Contains(seatNumber))
                    {
                        seatNumber++;
                    }

                    if (seatNumber > availableSeats)
                    {
                        MessageBox.Show("Немає достатньої кількості вільних місць для бронювання.");
                        return;
                    }

                    Ticket ticket = new Ticket
                    {
                        RouteId = routeId,
                        StartCity = startCity,
                        EndCity = endCity,
                        DepartureDate = departureDate,
                        DepartureTime = departureTime,
                        ArrivalDate = arrivalDate,
                        ArrivalTime = arrivalTime,
                        SeatNumber = seatNumber,
                        PhoneNumber = UserPhoneNumber, // Використовуємо властивість UserPhoneNumber
                        PassengerName = passengerInfo.Name,
                        PassengerSurname = passengerInfo.Surname,
                        Price = int.Parse(passengerInfo.Price)
                    };

                    ticket.SaveToDatabase(connection);
                    bookedSeats.Add(seatNumber);
                    seatNumber++;
                }
            }

            MessageBox.Show("Квитки успішно забронювано. Приємної поїздки!");
            this.Close();
        }

        private int GetAvailableSeats(int routeId)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT number_of_seats FROM vehicles WHERE route_id = @routeId";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@routeId", routeId);

                object result = command.ExecuteScalar();
                if (result != null)
                {
                    return (int)result;
                }
            }

            return 0;
        }

        private List<int> GetBookedSeats(int routeId, DateTime departureDate, MySqlConnection connection)
        {
            List<int> bookedSeats = new List<int>();

            string query = @"
            SELECT seat_number FROM tickets WHERE route_id = @routeId AND departure_date = @departureDate";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@routeId", routeId);
            command.Parameters.AddWithValue("@departureDate", departureDate);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    bookedSeats.Add(reader.GetInt32("seat_number"));
                }
            }

            return bookedSeats;
        }
    }
}