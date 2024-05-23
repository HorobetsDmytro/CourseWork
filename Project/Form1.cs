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
    public partial class Form1 : Form
    {
        FormHome formHome;
        FormTickets formTickets;
        FormTicketsAdmin formTicketsAdmin;
        FormCityAdmin formCityAdmin;
        FormNewRouteAdmin formNewRouteAdmin;
        private MySqlConnection connection;

        public Form1()
        {
            InitializeComponent();
            menuContainer.Visible = false;
            string connectionString = "server=127.0.0.1;database=db;uid=root;pwd=Gd_135790;";
            connection = new MySqlConnection(connectionString);
            mdiProp();
            if (formHome == null)
            {
                formHome = new FormHome();
                formHome.FormClosed += FormHome_FormClosed;
                formHome.MdiParent = this;
                formHome.Dock = DockStyle.Fill;
                formHome.Show();
            }
            else
            {
                formHome.Activate();
            }
        }

        private void mdiProp()
        {
            this.SetBevel(false);
            Controls.OfType<MdiClient>().FirstOrDefault().BackColor = Color.FromArgb(232, 234, 237);
        }

        bool menuExpand = false;

        private void menuTransition_Tick(object sender, EventArgs e)
        {
            if (menuExpand == false)
            {
                menuContainer.Height += 10;
                if (menuContainer.Height >= 262)
                {
                    menuTransition.Stop();
                    menuExpand = true;
                }
            }
            else
            {
                menuContainer.Height -= 10;
                if (menuContainer.Height <= 65)
                {
                    menuTransition.Stop();
                    menuExpand = false;
                }
            }
        }

        private void admin_Click(object sender, EventArgs e)
        {
            menuTransition.Start();
        }

        bool check = true;

        private void sidebarTransition_Tick(object sender, EventArgs e)
        {
            if (check)
            {
                sidebar.Width -= 10;
                if(sidebar.Width <= 65)
                {
                    check = false;
                    sidebarTransition.Stop();
                    pnHome.Width = sidebar.Width;
                    pnTickets.Width = sidebar.Width;
                    menuContainer.Width = sidebar.Width;
                }
            }
            else
            {
                sidebar.Width += 10;
                if(sidebar.Width >= 262)
                {
                    check = true;
                    sidebarTransition.Stop();
                    pnHome.Width = sidebar.Width;
                    pnTickets.Width = sidebar.Width;
                    menuContainer.Width = sidebar.Width;
                }
            }
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            sidebarTransition.Start();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(formHome == null)
            {
                formHome = new FormHome();
                formHome.FormClosed += FormHome_FormClosed;
                formHome.MdiParent = this;
                formHome.Dock = DockStyle.Fill;
                formHome.Show();
            }
            else
            {
                formHome.Activate();
            }
        }

        private void FormHome_FormClosed(object sender, FormClosedEventArgs e)
        {
            formHome = null;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (formTickets == null)
            {
                formTickets = new FormTickets();
                formTickets.FormClosed += FormTickets_FormClosed;
                formTickets.MdiParent = this;
                formTickets.Dock = DockStyle.Fill;
                formTickets.Show();
            }
            else
            {
                formTickets.Activate();
            }

            FormHome homeForm = (FormHome)Application.OpenForms["FormHome"];
            if (homeForm != null)
            {
                //MessageBox.Show($"Введений користувач:\nPhone number: {homeForm.enteredPhoneNumber}", "Введений користувач", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (homeForm.IsUserLoggedIn )
                {
                    formTickets.LoadBookedTickets(homeForm.enteredPhoneNumber);
                }
            }
            else
            {
                MessageBox.Show("Помилка!!!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormTickets_FormClosed(object sender, FormClosedEventArgs e)
        {
            formTickets = null;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (formTicketsAdmin == null)
            {
                formTicketsAdmin = new FormTicketsAdmin();
                formTicketsAdmin.FormClosed += FormTicketsAdmin_FormClosed;
                formTicketsAdmin.MdiParent = this;
                formTicketsAdmin.Dock = DockStyle.Fill;
                formTicketsAdmin.Show();
            }
            else
            {
                formTicketsAdmin.Activate();
            }
        }

        private void FormTicketsAdmin_FormClosed(object sender, FormClosedEventArgs e)
        {
            formTicketsAdmin = null;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (formCityAdmin == null)
            {
                formCityAdmin = new FormCityAdmin();
                formCityAdmin.FormClosed += FormCityAdmin_FormClosed;
                formCityAdmin.MdiParent = this;
                formCityAdmin.Dock = DockStyle.Fill;
                formCityAdmin.Show();
            }
            else
            {
                formCityAdmin.Activate();
            }
        }

        private void FormCityAdmin_FormClosed(object sender, FormClosedEventArgs e)
        {
            formCityAdmin = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (formNewRouteAdmin == null)
            {
                formNewRouteAdmin = new FormNewRouteAdmin();
                formNewRouteAdmin.FormClosed += FormNewRouteAdmin_FormClosed;
                formNewRouteAdmin.MdiParent = this;
                formNewRouteAdmin.Dock = DockStyle.Fill;
                formNewRouteAdmin.Show();
            }
            else
            {
                formNewRouteAdmin.Activate();
            }
        }

        private void FormNewRouteAdmin_FormClosed(object sender, FormClosedEventArgs e)
        {
            formNewRouteAdmin = null;
        }
    }
}
