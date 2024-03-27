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
        public Form1()
        {
            InitializeComponent();
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
    }
}
