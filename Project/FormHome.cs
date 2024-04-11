using ReaLTaiizor.Forms;
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
    public partial class FormHome : Form
    {
        LoginForm loginForm;
        public FormHome()
        {
            InitializeComponent();
            this.Resize += new EventHandler(Form1_Resize); 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        }

        private void FormHome_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (loginForm == null)
            {
                loginForm = new LoginForm();
                loginForm.FormClosed += LoginForm_FormClosed;
                //loginForm.Owner = this;
                loginForm.Dock = DockStyle.Fill;
                loginForm.Show();
            }
            else
            {
                loginForm.Activate();
            }
        }

        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            loginForm = null;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            button1.Location = new Point(this.ClientSize.Width - button1.Size.Width, 0); // Перемістіть кнопку у верхній правий кут
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("fdhdfhgdfhg");
        }
    }
}
