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
        LoginForm loginForm;

        public FormHome()
        {
            InitializeComponent();
            this.Resize += new EventHandler(Form1_Resize); 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            panel1.Visible = false;
        }

        private void FormHome_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel1.Location = new Point(419, 115);
            panel1.Size = new Size(346, 800);
            loginForm = new LoginForm();
            loginForm.TopLevel = false; 
            panel1.Controls.Add(loginForm);
            loginForm.Show(); 
            loginForm.BringToFront(); 
            loginForm.Dock = DockStyle.Fill; 
            button1.Text = "Вийти";
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            button1.Location = new Point(this.ClientSize.Width - button1.Size.Width, 0); 
        }

    }
}
