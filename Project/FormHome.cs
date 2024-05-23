using MySql.Data.MySqlClient;
using ReaLTaiizor.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Project
{
    public partial class FormHome : Form
    {
        private MySqlConnection connection;
        public string enteredPhoneNumber;
        public bool IsUserLoggedIn { get; private set; }

        public void SetUserLoggedInStatus(bool isLoggedIn)
        {
            IsUserLoggedIn = isLoggedIn;
        }

        public FormHome()
        {
            InitializeComponent();
            string connectionString = "server=127.0.0.1;database=db;uid=root;pwd=Gd_135790;";
            connection = new MySqlConnection(connectionString);
            this.Resize += new EventHandler(Form1_Resize);
            LoadCitiesAutoComplete();
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            panel2.Visible = false;
            panel3.Visible = false;

            panel2.Location = new Point(455, 154);
            panel2.Size = new Size(262, 383);

            panel3.Location = new Point(455, 154);
            panel3.Size = new Size(262, 439);
        }


        private void FormHome_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Вийти")
            {
                LogoutUser();
                if(txtUsername.Text == "admin")
                {
                    Form1 form1 = (Form1)Application.OpenForms["Form1"];
                    form1.menuContainer.Visible = false;
                }
            }
            else
            {
                txtUsername.Text = "";
                txtRegPassword.Text = "";
                txtPassword.Text = "";
                panel2.Visible = true;
                txtStartCity.Visible = false;
                txtEndCity.Visible = false;
                label14.Visible = false;
                label13.Visible = false;
                label12.Visible = false;
                pictureBox1.Visible = false;
                poisonDateTime1.Visible = false;
                button6.Visible = false;
                RemoveTicketControl();
            }
        }

        private void LogoutUser()
        {
            DialogResult result = MessageBox.Show("Ви впевнені, що хочете вийти?", "Вихід", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                button1.Text = "Увійти";
                RemoveTicketControl();
                SetUserLoggedInStatus(false);

                FormTickets ticketsForm = (FormTickets)Application.OpenForms["FormTickets"];
                // Видалення існуючих контролів TicketControl на формі FormTickets
                if (ticketsForm != null)
                {
                    foreach (Control control in ticketsForm.Controls)
                    {
                        if (control is TicketControl)
                        {
                            //ticketsForm.Controls.Remove(control);
                            control.Visible = false;
                        }
                    }
                }

                MessageBox.Show("Ви успішно вийшли з аккаунту", "Вихід", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void RemoveTicketControl()
        {
            foreach (Control control in Controls)
            {
                if (control is TicketControl)
                {
                    control.Visible = false;
                }
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            button1.Location = new Point(this.ClientSize.Width - button1.Size.Width, 0); 
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            User user = new User(username, password);

            try
            {
                connection.Open();
                string query = "SELECT * FROM users WHERE username = @username AND password = @password";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@username", user.Username);
                command.Parameters.AddWithValue("@password", user.Password);
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    if (username == "admin@gmail.com")
                    {
                        MessageBox.Show("Ви ввійшли як адміністратор", "Успішний вхід адміністратора", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ((Form1)this.MdiParent).menuContainer.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show("Ви успішно ввійшли у свій аккаунт", "Успішний вхід", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ((Form1)this.MdiParent).menuContainer.Visible = false;
                    }

                    panel2.Visible = false;
                    button1.Text = "Вийти";
                    enteredPhoneNumber = username;

                    txtStartCity.Visible = true;
                    txtEndCity.Visible = true;
                    label14.Visible = true;
                    label13.Visible = true;
                    label12.Visible = true;
                    pictureBox1.Visible = true;
                    poisonDateTime1.Visible = true;
                    button6.Visible = true;
                    SetUserLoggedInStatus(true);
                }
                else
                {
                    MessageBox.Show("Неправильний пароль або пошта, Спробуйте ще раз", "Помилка входу", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    username = "";
                    password = "";
                    txtUsername.Focus();
                    SetUserLoggedInStatus(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message, "Помилка входу", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtUsername.Focus();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel3.Visible = true;
            txtRegUsername.Text = "";
            txtRegPassword.Text = "";
            txtComPassword.Text = "";
        }

        private void label2_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
            panel2.Visible = true;
            txtUsername.Text = "";
            txtPassword.Text = "";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string username = txtRegUsername.Text;
            string password = txtRegPassword.Text;
            string confirmPassword = txtComPassword.Text;

            if (username == "" || password == "" || confirmPassword == "")
            {
                MessageBox.Show("Введіть пароль або пошту, поля не можуть бути пусті", "Помилка реєстрації", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (password == confirmPassword)
            {
                User user = new User(username, password);
                try
                {
                    connection.Open();
                    string queryCheck = "SELECT COUNT(*) FROM users WHERE username = @username";
                    MySqlCommand commandCheck = new MySqlCommand(queryCheck, connection);
                    commandCheck.Parameters.AddWithValue("@username", user.Username);
                    int userCount = Convert.ToInt32(commandCheck.ExecuteScalar());

                    if (userCount > 0)
                    {
                        MessageBox.Show("Такий користувач вже існує. Введіть іншу пошту.", "Помилка реєстрації", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        string query = "INSERT INTO users (username, password) VALUES (@username, @password)";
                        MySqlCommand command = new MySqlCommand(query, connection);
                        command.Parameters.AddWithValue("@username", user.Username);
                        command.Parameters.AddWithValue("@password", user.Password);
                        command.ExecuteNonQuery();

                        username = "";
                        password = "";
                        confirmPassword = "";

                        MessageBox.Show("Ваш аккаунт успішно створено", "Реєстрація успішна", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        panel3.Visible = false;
                        panel2.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка: " + ex.Message, "Помилка реєстрації", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
            else
            {
                MessageBox.Show("Паролі не співпадають, Повторіть спробу", "Помилка реєстрації", MessageBoxButtons.OK, MessageBoxIcon.Error);
                password = "";
                confirmPassword = "";
                txtComPassword.Focus();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            txtRegUsername.Text = "";
            txtRegPassword.Text = "";
            txtComPassword.Text = "";
            txtRegUsername.Focus();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                txtRegPassword.PasswordChar = '\0';
                txtComPassword.PasswordChar = '\0';
            }
            else
            {
                txtRegPassword.PasswordChar = '•';
                txtComPassword.PasswordChar = '•';
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                txtPassword.PasswordChar = '\0';
            }
            else
            {
                txtPassword.PasswordChar = '•';
            }
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            string temp = txtStartCity.Text;
            txtStartCity.Text = txtEndCity.Text;
            txtEndCity.Text = temp;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string startCity = txtStartCity.Text;
            string endCity = txtEndCity.Text;
            DateTime departureDate = poisonDateTime1.Value.Date;

            int numberOfRoutes = GetNumberOfRoutes(startCity, endCity, departureDate);

            if (numberOfRoutes == 0)
            {
                MessageBox.Show("Квитків за вказаними параметрами не знайдено.", "Пошук квитків", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                RemovePreviousTicketControls();

                AddTicketControls(numberOfRoutes);
            }
        }

        private void RemovePreviousTicketControls()
        {
            // Проходимося по всіх контролах у формі
            for (int i = Controls.Count - 1; i >= 0; i--)
            {
                Control control = Controls[i];
                // Якщо контрол - TicketControl, видаляємо його
                if (control is TicketControl)
                {
                    Controls.Remove(control);
                    control.Dispose(); // Важливо видалити контрол з пам'яті
                }
            }
        }

        private int GetNumberOfRoutes(string startCity, string endCity, DateTime departureDate)
        {
            int numberOfRoutes = 0;
            try
            {
                connection.Open();
                string query = @"
                SELECT COUNT(DISTINCT r.route_id)
                FROM routes r
                JOIN intermediatestations i1 ON r.route_id = i1.routeId
                                            AND (i1.station_city = @startCity OR i1.station_name = @startCity)
                JOIN intermediatestations i2 ON r.route_id = i2.routeId
                                            AND (i2.station_city = @endCity OR i2.station_name = @endCity)
                JOIN (SELECT routeId, MIN(order_index) AS min_order_index FROM intermediatestations GROUP BY routeId) i1_min
                    ON i1.routeId = i1_min.routeId
                WHERE ((i1.order_index = i1_min.min_order_index AND i1.departure_date = @departureDate)
                   OR (i1.order_index != i1_min.min_order_index AND i1.arrival_date = @departureDate))
                AND i1.order_index < i2.order_index;
                ";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@startCity", startCity);
                command.Parameters.AddWithValue("@endCity", endCity);
                command.Parameters.AddWithValue("@departureDate", departureDate);

                numberOfRoutes = Convert.ToInt32(command.ExecuteScalar());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }

            return numberOfRoutes;
        }

        private void AddTicketControls(int numberOfControls)
        {
            int spacing = 20;
            int startY = 212;

            try
            {
                connection.Open();
                string query = @"
        SELECT
            r.route_id,
            i1.station_city AS start_city,
            i1.station_name AS start_station,
            i2.station_city AS end_city,
            i2.station_name AS end_station,
            (
                SELECT
                    CASE
                        WHEN i2.order_index = (SELECT MAX(order_index) FROM intermediatestations WHERE routeId = r.route_id) THEN
                            (
                                SELECT SUM(i3.price)
                                FROM intermediatestations i3
                                WHERE i3.routeId = r.route_id
                                    AND i3.order_index >= i1.order_index
                                    AND i3.order_index <= i2.order_index
                            )
                        ELSE
                            (
                                SELECT SUM(i3.price)
                                FROM intermediatestations i3
                                WHERE i3.routeId = r.route_id
                                    AND i3.order_index >= i1.order_index
                                    AND i3.order_index < i2.order_index
                            )
                    END
            ) AS total_price,
            CASE
                WHEN i1.order_index = i1_min.min_order_index THEN i1.departure_date
                ELSE i1.arrival_date
            END AS effective_departure_date,
            CASE
                WHEN i1.order_index = i1_min.min_order_index THEN i1.departure_time
                ELSE i1.arrival_time
            END AS effective_departure_time,
            i2.arrival_date,
            i2.arrival_time,
            v.vehicle_type,
            v.model,
            cs1.station_address AS start_station_address,
            cs2.station_address AS end_station_address
        FROM routes r
        JOIN intermediatestations i1 ON r.route_id = i1.routeId
                                    AND (i1.station_city = @startCity OR i1.station_name = @startCity)
        JOIN intermediatestations i2 ON r.route_id = i2.routeId
                                    AND (i2.station_city = @endCity OR i2.station_name = @endCity)
        JOIN (SELECT routeId, MIN(order_index) AS min_order_index FROM intermediatestations GROUP BY routeId) i1_min
            ON i1.routeId = i1_min.routeId
        JOIN vehicles v ON r.route_id = v.route_id
        JOIN characteristicsofstations cs1 ON i1.station_city = cs1.station_city AND i1.station_name = cs1.station_name
        JOIN characteristicsofstations cs2 ON i2.station_city = cs2.station_city AND i2.station_name = cs2.station_name
        WHERE ((i1.order_index = i1_min.min_order_index AND i1.departure_date = @departureDate)
            OR (i1.order_index != i1_min.min_order_index AND i1.arrival_date = @departureDate))
        AND i1.order_index < i2.order_index
        ORDER BY effective_departure_date, effective_departure_time, i2.arrival_date, i2.arrival_time;";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@startCity", txtStartCity.Text);
                command.Parameters.AddWithValue("@endCity", txtEndCity.Text);
                command.Parameters.AddWithValue("@departureDate", poisonDateTime1.Value.Date);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    int i = 0;
                    while (reader.Read() && i < numberOfControls)
                    {
                        TicketControl ticketControl = new TicketControl();
                        ticketControl.hopeRoundButton2.Visible = false;
                        ticketControl.label13.Visible = false;
                        ticketControl.label14.Visible = false;
                        ticketControl.label15.Visible = false;
                        ticketControl.RouteId = reader.GetInt32("route_id");
                        ticketControl.TicketPrice = reader.GetInt32("total_price");
                        ticketControl.Location = new Point(50, startY + i * (ticketControl.Height + spacing));

                        ticketControl.label1.Text = ((TimeSpan)reader["effective_departure_time"]).ToString(@"hh\:mm");
                        ticketControl.label2.Text = ((TimeSpan)reader["arrival_time"]).ToString(@"hh\:mm");
                        ticketControl.label3.Text = ((DateTime)reader["effective_departure_date"]).ToString("yyyy-MM-dd");
                        ticketControl.label4.Text = ((DateTime)reader["arrival_date"]).ToString("yyyy-MM-dd");
                        ticketControl.label6.Text = reader["start_city"].ToString();
                        ticketControl.label7.Text = reader["end_city"].ToString();
                        ticketControl.label8.Text = reader["start_station"].ToString();
                        ticketControl.label9.Text = reader["end_station"].ToString();
                        ticketControl.label10.Text = reader["total_price"].ToString();
                        ticketControl.label16.Text = reader["vehicle_type"].ToString();
                        ticketControl.label17.Text = reader["model"].ToString();
                        ticketControl.label18.Text = reader["start_station_address"].ToString();
                        ticketControl.label19.Text = reader["end_station_address"].ToString();

                        // Розрахунок різниці між датою/часом прибуття та відправлення
                        DateTime departureDateTime = (DateTime)reader["effective_departure_date"] + (TimeSpan)reader["effective_departure_time"];
                        DateTime arrivalDateTime = (DateTime)reader["arrival_date"] + (TimeSpan)reader["arrival_time"];
                        TimeSpan travelDuration = arrivalDateTime - departureDateTime;

                        int days = travelDuration.Days;
                        int hours = travelDuration.Hours % 24;
                        int minutes = travelDuration.Minutes;
                        ticketControl.label5.Text = $"{days * 24 + hours} год. {minutes} хв. в дорозі";

                        this.Controls.Add(ticketControl);
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            txtStartCity.AutoCompleteCustomSource = citiesCollection;
            txtStartCity.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtStartCity.AutoCompleteSource = AutoCompleteSource.CustomSource;

            txtEndCity.AutoCompleteCustomSource = citiesCollection;
            txtEndCity.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtEndCity.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private void txtStartCity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && e.KeyChar != '\'' && e.KeyChar != '-' && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Якщо символ не відповідає критеріям, блокуємо його ввід
            }
        }
    }
}
