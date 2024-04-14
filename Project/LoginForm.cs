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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Project
{
    public partial class LoginForm : Form
    {
        private MySqlConnection connection;
        public LoginForm()
        {
            InitializeComponent();
            string connectionString = "server=127.0.0.1;database=db;uid=root;pwd=Gd_135790;";
            connection = new MySqlConnection(connectionString);
        }

        private void button1_Click(object sender, EventArgs e)
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
                    FormHome formHome = new FormHome();
                    MessageBox.Show("You have successfully logged into your account", "Login Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    panel1.Visible = false;
                    this.Hide();
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
            LoginForm loginForm = new LoginForm();
            RegisterForm registerForm = new RegisterForm();
            registerForm.TopLevel = false; 
            panel1.Controls.Add(registerForm);
            registerForm.BringToFront();
            registerForm.Dock = DockStyle.Fill;
            loginForm.Hide();
            registerForm.Show();
        }
    }
}
