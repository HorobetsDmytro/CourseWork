using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media;
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using TheArtOfDevHtmlRenderer.Adapters;

namespace Project
{
    public partial class TicketControl : UserControl
    {
        private const int Radius = 20;
        private bool isExpanded = false;
        public int RouteId { get; set; }
        public int TicketPrice { get; set; }
        public string UserPhoneNumber { get; set; }
        public bool IsUserLoggedIn { get; set; }

        public TicketControl()
        {
            InitializeComponent();
            SetStyle(ControlStyles.ResizeRedraw, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, this.Width, this.Height);
            using (GraphicsPath path = GetRoundedRect(rect, Radius))
            {
                Region = new Region(path);
            }
        }

        private GraphicsPath GetRoundedRect(System.Drawing.Rectangle rect, int radius)
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

        // У класі TicketControl
        private string GetRouteInformation(int routeId)
        {
            string connectionString = "server=127.0.0.1;database=db;uid=root;pwd=Gd_135790;";
            string query = @"
        SELECT 
            MIN(CASE WHEN order_index = 1 THEN station_city END) AS StartStation,
            MAX(CASE WHEN order_index = (SELECT MAX(order_index) FROM intermediatestations WHERE routeId = @RouteId) THEN station_city END) AS EndStation
        FROM intermediatestations
        WHERE routeId = @RouteId;
    ";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@RouteId", routeId);
                connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string startStation = reader["StartStation"].ToString();
                        string endStation = reader["EndStation"].ToString();
                        return $"Маршрут {routeId}: {startStation} - {endStation}";
                    }
                }
            }
            return "Інформація про маршрут не знайдена.";
        }

        public void CreateIntermediateStationLabels(RouteInfo routeInfoForm)
        {
            string connectionString = "server=127.0.0.1;database=db;uid=root;pwd=Gd_135790;";
            string query = @"
        SELECT 
            i.order_index,
            i.departure_date,
            i.departure_time,
            i.arrival_date,
            i.arrival_time,
            c.station_city,
            c.station_name,
            c.station_address
        FROM intermediatestations i
        LEFT JOIN characteristicsofstations c ON i.station_name = c.station_name AND i.station_city = c.station_city
        WHERE i.routeId = @RouteId
        ORDER BY i.order_index;
    ";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@RouteId", RouteId);
                connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    int startY = routeInfoForm.lblRoute.Location.Y + routeInfoForm.lblRoute.Height + 50;
                    while (reader.Read())
                    {
                        int orderIndex = reader.GetInt32(0);
                        DateTime departureDate = reader.GetDateTime(1);
                        TimeSpan departureTime = reader.GetTimeSpan(2);
                        DateTime arrivalDate = reader.GetDateTime(3);
                        TimeSpan arrivalTime = reader.GetTimeSpan(4);
                        string stationCity = reader.GetString(5);
                        string stationName = reader.GetString(6);
                        string stationAddress = reader.GetString(7);

                        string labelText;
                        if (orderIndex == reader.FieldCount - 4) // Остання станція
                        {
                            labelText = $"{arrivalDate.ToString("yyyy-MM-dd")} {arrivalTime.ToString(@"hh\:mm")}, {stationCity}, {stationName}, {stationAddress}";
                        }
                        else
                        {
                            labelText = $"{departureDate.ToString("yyyy-MM-dd")} {departureTime.ToString(@"hh\:mm")}, {stationCity}, {stationName}, {stationAddress}";
                        }

                        Label label = new Label
                        {
                            Text = labelText,
                            Location = new Point(20, startY),
                            AutoSize = true,
                            Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)))
                        };
                        routeInfoForm.Controls.Add(label);
                        startY += label.Height + 10;
                    }
                }
            }
        }

        private void label11_Click(object sender, EventArgs e)
        {
            RouteInfo routeInfoForm = new RouteInfo();
            routeInfoForm.lblRoute.Text = GetRouteInformation(RouteId);
            routeInfoForm.Show();
            CreateIntermediateStationLabels(routeInfoForm);
        }

        private void hopeRoundButton1_Click(object sender, EventArgs e)
        {
            TicketControl selectedTicketControl = this;
            if (selectedTicketControl != null)
            {
                FormHome homeForm = (FormHome)((Form1)Application.OpenForms["Form1"]).MdiChildren[0];
                if (homeForm.IsUserLoggedIn)
                {
                    //MessageBox.Show($"Ви успішно увійшли в систему з номером телефону: {homeForm.enteredPhoneNumber}", "Успішний вхід", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FormNumberSeats formNumberSeats = new FormNumberSeats(this, homeForm.enteredPhoneNumber);
                    formNumberSeats.Show();
                }
                else
                {
                    MessageBox.Show("Ви не увійшли в систему. Будь ласка, увійдіть, щоб забронювати квитки.", "Помилка входу", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        public void hopeRoundButton2_Click(object sender, EventArgs e)
        {
            SaveTicketToPdf();
        }

        private void SaveTicketToPdf()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF Files|*.pdf";
            saveFileDialog.Title = "Save Ticket to PDF";
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                using (FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    Document document = new Document(PageSize.A4, 25f, 25f, 30f, 30f);
                    PdfWriter.GetInstance(document, fileStream);

                    document.Open();

                    // Set font that supports Cyrillic characters
                    string ttf = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIAL.TTF");
                    var baseFont = BaseFont.CreateFont(ttf, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                    var font = new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.NORMAL);
                    var boldFont = new iTextSharp.text.Font(baseFont, 18, iTextSharp.text.Font.BOLD);

                    string vehicleNumber = GetVehicleNumber(RouteId);

                    // Add ticket number at the top center
                    Paragraph ticketNumberParagraph = new Paragraph($"БІЛЕТ № {label20.Text}", boldFont);
                    ticketNumberParagraph.Alignment = Element.ALIGN_CENTER;
                    document.Add(ticketNumberParagraph);

                    // Load image and get instance
                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance("C:\\Users\\user\\source\\repos\\CourseWork\\bus.jpg");
                    // Resize image
                    image.ScaleToFit(140f, 120f);
                    // Set position to upper right corner, higher up
                    image.SetAbsolutePosition(document.PageSize.Width - image.ScaledWidth - 25f,
                                              document.PageSize.Height - image.ScaledHeight - 5f);
                    document.Add(image);

                    // Add code to create PDF file with ticket information
                    document.Add(new Paragraph($"Номер маршруту: {RouteId}", font));
                    document.Add(new Paragraph($"Пункт відправлення: {label6.Text}, {label8.Text} {label18.Text}", font));
                    document.Add(new Paragraph($"Пункт призначення: {label7.Text}, {label9.Text} {label19.Text}", font));
                    document.Add(new Paragraph($"Дата відправлення: {label3.Text} {label1.Text}", font));
                    document.Add(new Paragraph($"Дата прибуття: {label4.Text} {label2.Text}", font));
                    document.Add(new Paragraph($"Номер місця: {label15.Text}", font));
                    document.Add(new Paragraph($"Ціна: {label10.Text}", font));
                    document.Add(new Paragraph($"Пасажир: {label13.Text}", font));
                    document.Add(new Paragraph($"{label16.Text} {label17.Text}", font));
                    document.Add(new Paragraph($"Номер {label16.Text}у: {vehicleNumber}", font));

                    document.Close();
                }
            }
        }

        private string GetVehicleNumber(int routeId)
        {
            string vehicleNumber = "";
            string connectionString = "server=127.0.0.1;database=db;uid=root;pwd=Gd_135790;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
                    SELECT v.vehicle_number
                    FROM tickets t
                    JOIN routes r ON t.route_id = r.route_id
                    JOIN vehicles v ON r.route_id = v.route_id
                    WHERE t.route_id = @routeId
                    LIMIT 1;
                ";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@routeId", routeId);

                object result = command.ExecuteScalar();
                if (result != null)
                {
                    vehicleNumber = result.ToString();
                }
            }

            return vehicleNumber;
        }

    }
}