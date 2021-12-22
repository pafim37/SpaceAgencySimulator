using Sas.SolarSystem;

namespace Sas.GraphicRepresentation
{
    public partial class Form1 : Form
    {
        private SolarSystem.SolarSystem solarSystem;

        Pen pen = new Pen(Color.Blue);
        Graphics graphics;
        public Form1()
        {
            InitializeComponent();
            solarSystem = new SolarSystem.SolarSystem();
            solarSystem.Init();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            graphics = canvas.CreateGraphics();
            draw_point();
            draw_line();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            canvas.Refresh();
        }

        private void draw_line()
        {
            Point p1 = new Point(100, 100);
            Point p2 = new Point(200, 200);
            graphics.DrawLine(pen, p1, p2);
        }

        private void draw_point()
        {
            IList<SolarSystem.Models.Body>? bodies = solarSystem.GetBodies();
            List<Point> points = new List<Point>();
            foreach (var body in bodies)
            {
                int x = (int)(body.AbsolutePosition.X / Int64.Parse(ScaleValue.Text));
                int y = (int)(body.AbsolutePosition.Y / Int64.Parse(ScaleValue.Text));
                points.Add(new Point(x, y));
            }
            foreach (var point in points)
            {
                Rectangle rect = new Rectangle(point.X, point.Y, 10, 10);
                graphics.DrawEllipse(pen, rect);

            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            graphics.Clear(Color.White);
            // canvas.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            long curr = Int64.Parse(ScaleValue.Text);
            curr = 10 * curr;
            ScaleValue.Text = curr.ToString();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            long curr = long.Parse(ScaleValue.Text);
            curr = curr / 10;
            ScaleValue.Text = curr.ToString();
        }
    }
}