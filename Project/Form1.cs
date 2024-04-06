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
        FormProfile formProfile;
        FormSettings formSettings;
        FormQuestions formQuestions;

        public Form1()
        {
            InitializeComponent();
            mdiProp();
        }

        private void mdiProp()
        {
            this.SetBevel(false);
            Controls.OfType<MdiClient>().FirstOrDefault().BackColor = Color.FromArgb(232, 234, 237);
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
                    pnProfile.Width = sidebar.Width;
                    pnSettings.Width = sidebar.Width;
                    pnQuestions.Width = sidebar.Width;
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
                    pnProfile.Width = sidebar.Width;
                    pnSettings.Width = sidebar.Width;
                    pnQuestions.Width = sidebar.Width;
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
        }

        private void FormTickets_FormClosed(object sender, FormClosedEventArgs e)
        {
            formTickets = null;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (formProfile == null)
            {
                formProfile = new FormProfile();
                formProfile.FormClosed += FormProfile_FormClosed;
                formProfile.MdiParent = this;
                formProfile.Dock = DockStyle.Fill;
                formProfile.Show();
            }
            else
            {
                formProfile.Activate();
            }
        }

        private void FormProfile_FormClosed(object sender, FormClosedEventArgs e)
        {
            formProfile = null;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (formSettings == null)
            {
                formSettings = new FormSettings();
                formSettings.FormClosed += FormSettings_FormClosed;
                formSettings.MdiParent = this;
                formSettings.Dock = DockStyle.Fill;
                formSettings.Show();
            }
            else
            {
                formSettings.Activate();
            }
        }

        private void FormSettings_FormClosed(object sender, FormClosedEventArgs e)
        {
            formSettings = null;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (formQuestions == null)
            {
                formQuestions = new FormQuestions();
                formQuestions.FormClosed += FormQuestions_FormClosed;
                formQuestions.MdiParent = this;
                formQuestions.Dock = DockStyle.Fill;
                formQuestions.Show();
            }
            else
            {
                formQuestions.Activate();
            }
        }

        private void FormQuestions_FormClosed(object sender, FormClosedEventArgs e)
        {
            formQuestions = null;
        }
    }
}
