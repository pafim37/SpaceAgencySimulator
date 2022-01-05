namespace Sas.GraphicRepresentation
{
    public partial class RootWindow : Form
    {
        private Pen pen = new Pen(Color.Blue);
        private Graphics graphics;
        private GraphicController _controller;
        
        public RootWindow()
        {
            InitializeComponent();
            _controller = new GraphicController();
        }

        private void RootWindow_Load(object sender, EventArgs e)
        {
        }

        private void board_Paint(object sender, PaintEventArgs e)
        {
            graphics = board.CreateGraphics();
            draw_points();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            board.Refresh();
        }

        private void draw_points()
        {
            _controller.SetPointsInCenter();
            foreach (var point in _controller.GetPoints())
            {
                Rectangle rect = new Rectangle(point.X - point.R / 2, point.Y - point.R / 2, point.R, point.R);
                graphics.DrawEllipse(pen, rect);
            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            graphics.Clear(Color.White);
            // board.Refresh();
        }

        private void scalePlus_Click(object sender, EventArgs e)
        {
            int curr = Int32.Parse(scaleValue.Text);
            curr += 10;
            scaleValue.Text = curr.ToString();
            _controller.Scale = Int32.Parse(scaleValue.Text);
        }

        private void scaleMinus_Click(object sender, EventArgs e)
        {
            int curr = Int32.Parse(scaleValue.Text);
            curr -= 10;
            if(curr < 0) curr = 0;
            scaleValue.Text = curr.ToString();
            _controller.Scale = Int32.Parse(scaleValue.Text);
        }
    }
}