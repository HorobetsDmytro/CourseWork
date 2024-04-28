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

        public FormHome()
        {
            InitializeComponent();
            string connectionString = "server=127.0.0.1;database=db;uid=root;pwd=Gd_135790;";
            connection = new MySqlConnection(connectionString);
            this.Resize += new EventHandler(Form1_Resize); 
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
            txtUsername.Text = "";
            txtPassword.Text = "";
            panel2.Visible = true;
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
                    MessageBox.Show("You have successfully logged into your account", "Login Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    panel2.Visible = false;
                    if(username == "admin")
                    {
                        ((Form1)this.MdiParent).menuContainer.Visible = true;
                    }
                    else ((Form1)this.MdiParent).menuContainer.Visible = false;
                    button1.Text = "Вийти";
                }
                else
                {
                    MessageBox.Show("Invalid username or password, Please Try Again", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    username = "";
                    password = "";
                    txtUsername.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void checkBoxShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxShowPassword.Checked)
            {
                txtPassword.PasswordChar = '\0';
            }
            else
            {
                txtPassword.PasswordChar = '•';
            }
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
                MessageBox.Show("Username or Password fields are empty", "Registration failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        MessageBox.Show("This username already exists. Please choose a different username.", "Registration failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                        MessageBox.Show("Your Account has been successfully created", "Registration Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        panel3.Visible = false;
                        panel2.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Registration failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
            else
            {
                MessageBox.Show("Passwords does not match, Please Re-enter", "Registration failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            DateTime departureDate = poisonDateTime1.Value.Date; // Отримуємо дату відправлення

            // Отримуємо кількість маршрутів
            int numberOfRoutes = GetNumberOfRoutes(startCity, endCity, departureDate);

            // Видаляємо попередні TicketControl
            RemovePreviousTicketControls();

            // Створюємо і додаємо відповідну кількість нових TicketControl на форму
            AddTicketControls(numberOfRoutes);
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
                string query = "SELECT COUNT(*)\r\nFROM (\r\n  SELECT *, ROW_NUMBER() OVER (PARTITION BY route_id ORDER BY departure_date) AS rn\r\n  FROM routes\r\n  WHERE start_city = @startCity\r\n    AND end_city = @endCity\r\n    AND departure_date = @departureDate\r\n) t\r\nWHERE rn = 1;";
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
            int spacing = 20; // Відступ між кожним TicketControl
            int startY = 212; // Початкове положення y першого TicketControl

            try
            {
                connection.Open();
                string query = "SELECT *\r\nFROM (\r\n  SELECT *, ROW_NUMBER() OVER (PARTITION BY route_id ORDER BY departure_date) AS rn\r\n  FROM routes\r\n  WHERE start_city = @startCity\r\n    AND end_city = @endCity\r\n    AND departure_date = @departureDate\r\n) t\r\nWHERE rn = 1;";
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
                        ticketControl.Location = new Point(50, startY + i * (ticketControl.Height + spacing));

                        ticketControl.label1.Text = ((TimeSpan)reader["departure_time"]).ToString(@"hh\:mm");
                        ticketControl.label2.Text = ((TimeSpan)reader["arrival_time"]).ToString(@"hh\:mm");
                        ticketControl.label3.Text = ((DateTime)reader["departure_date"]).ToString("yyyy-MM-dd");
                        ticketControl.label4.Text = ((DateTime)reader["arrival_date"]).ToString("yyyy-MM-dd");
                        ticketControl.label6.Text = reader["start_city"].ToString();
                        ticketControl.label7.Text = reader["end_city"].ToString();
                        ticketControl.label10.Text = reader["price"].ToString();

                        // Розрахунок різниці між часом прибуття та відправлення
                        DateTime departureDateTime = (DateTime)reader["departure_date"] + (TimeSpan)reader["departure_time"];
                        DateTime arrivalDateTime = (DateTime)reader["arrival_date"] + (TimeSpan)reader["arrival_time"];
                        TimeSpan travelDuration = arrivalDateTime - departureDateTime;

                        // Відображення різниці у форматі "... год. ... хв. в дорозі"
                        int hours = travelDuration.Hours;
                        int minutes = travelDuration.Minutes;
                        ticketControl.label5.Text = $"{hours} год. {minutes} хв. в дорозі";

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


    }
}
