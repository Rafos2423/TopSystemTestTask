using System.Drawing;
using System.Linq.Expressions;
using System.Reflection;

namespace WinFormsApp1
{
    enum FigureColor
    {
        Red, Green, Blue
    }

    enum FigureType
    {
        Circle, Square, Triangle
    }

    public partial class Form1 : Form
    {
        const int FigureCountWidth = 4;
        const int FigureCountHeight = 3;

        public Form1()
        {
            InitializeComponent();
            comboBox1.Text = "Круг";
            comboBox2.Text = "Красный";
            textBox1.Text = "6";
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var typeIndex = comboBox1.Items.IndexOf(comboBox1.Text);
            if (typeIndex < 0)
            {
                MessageBox.Show("Не возможно отобразить фигуру");
                comboBox1.Text = "";
                return;
            }

            if (string.IsNullOrEmpty(textBox1.Text) || !int.TryParse(textBox1.Text, out var count) || count > 12)
            {
                MessageBox.Show("Не возможно отобразить данное количество фигур, максимум 12");
                textBox1.Text = "";
                return;
            }

            var colorIndex = comboBox2.Items.IndexOf(comboBox2.Text);
            if (colorIndex < 0)
            {
                MessageBox.Show("Не возможно отобразить фигуру данного цвета");
                comboBox2.Text = "";
                return;
            }


            var type = Enum.GetValues<FigureType>()[typeIndex];
            var color = Enum.GetValues<FigureColor>()[colorIndex];

            var g = CreateGraphics();

            var brush = color switch
            {
                FigureColor.Red => Brushes.Red,
                FigureColor.Green => Brushes.Green,
                FigureColor.Blue => Brushes.Blue,
            };

            Action<int, int, Graphics, Brush> draw = type switch
            {
                FigureType.Square => DrawRectangle,
                FigureType.Triangle => DrawPolygon,
                FigureType.Circle => DrawCircle,
            };

            DrawAllFigures(g, draw, brush, count);

            button2.Enabled = true;
            button1.Enabled = false;

            g.Dispose();
        }

        public static void DrawAllFigures(Graphics g, Action<int, int, Graphics, Brush> draw, Brush brush, int count)
        {
            for (int i = 0; i < FigureCountHeight; i++)
            {
                for (int j = 0; j < FigureCountWidth; j++)
                {
                    if (i * FigureCountWidth + j == count) return;
                    draw(i, j, g, brush);
                }
            }

        }
        public static void DrawPolygon(int i, int j, Graphics g, Brush brush) =>
            g.FillPolygon(brush, new Point[]
            {
                new Point(250 + j * 150, 20 + i * 150),
                new Point(200 + j * 150, 120 + i * 150),
                new Point(300 + j * 150, 120 + i * 150)
            });

        public static void DrawRectangle(int i, int j, Graphics g, Brush brush) =>
            g.FillRectangle(brush, new Rectangle
            (
                200 + j * 150,
                20 + i * 150,
                100,
                100
            ));

        public static void DrawCircle(int i, int j, Graphics g, Brush brush) =>
            g.FillEllipse(brush, new Rectangle
            (
                200 + j * 150,
                20 + i * 150,
                100,
                100
            ));

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Invalidate();
            button1.Enabled = true;
            button2.Enabled = false;
        }
    }
}