using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Project
{
    public partial class TicketControl : UserControl
    {
        private int radius = 20; // Радіус заокруглення кутів

        public TicketControl()
        {
            InitializeComponent();
            SetStyle(ControlStyles.ResizeRedraw, true); // Перемальовувати контрол при зміні розміру
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            using (GraphicsPath path = GetRoundedRect(rect, radius))
            {
                Region = new Region(path);
            }
        }

        private GraphicsPath GetRoundedRect(Rectangle rect, int radius)
        {
            int diameter = radius * 2;
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(rect.Left, rect.Top, diameter, diameter, 180, 90);
            path.AddLine(rect.Left + radius, rect.Top, rect.Right - radius, rect.Top);
            path.AddArc(rect.Right - diameter, rect.Top, diameter, diameter, 270, 90);
            path.AddLine(rect.Right, rect.Top + radius, rect.Right, rect.Bottom - radius);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddLine(rect.Right - radius, rect.Bottom, rect.Left + radius, rect.Bottom);
            path.AddArc(rect.Left, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }

        bool ticketControlExpand = false;

        private void ticketControlTransition_Tick(object sender, EventArgs e)
        {
            if (ticketControlExpand == false)
            {
                this.Height += 10;
                if (this.Height >= 363)
                {
                    ticketControlTransition.Stop();
                    ticketControlExpand = true;
                }
            }
            else
            {
                this.Height -= 10;
                if (this.Height <= 169)
                {
                    ticketControlTransition.Stop();
                    ticketControlExpand = false;
                }
            }
        }

        private void label11_Click(object sender, EventArgs e)
        {
            ticketControlTransition.Start();
        }

    }
}
