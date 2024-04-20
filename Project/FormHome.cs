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
    }
}
