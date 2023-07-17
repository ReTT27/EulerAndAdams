//####  Программа EulerAndAdams

//####  Курсовой проект
//####  по предмету МДК 03.01 Технология разработки программного
//####  обеспечения

//####  по теме "Разработка программы решения дифференциальных
//####  уравнений"
//####  Язык: C#
//####  Разработал: Шевяков И.М.

//####  Задание:
//####  Разработка программы решения дифференциальных уравнений:
//####  1) методом Эйлера;
//####  2) методом Адамса.

//####  Вызываемые подпрограммы:
//####  select - функция для выбора уравнения;
//####  Runge - функция вычисления методом Рунге-Кутта.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using TextBox = System.Windows.Forms.TextBox;

namespace EulerAndAdams
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private bool isMousePress = false;
        private Point _clickPoint;
        private Point _formStartPoint;

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            isMousePress = true;
            _clickPoint = Cursor.Position;
            _formStartPoint = Location;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMousePress)
            {
                var cursorOffsetPoint = new Point( //считаем смещение курсора от старта
                Cursor.Position.X - _clickPoint.X,
                Cursor.Position.Y - _clickPoint.Y);

                Location = new Point( //смещаем форму от начальной позиции в соответствии со смещением курсора
                _formStartPoint.X + cursorOffsetPoint.X,
                _formStartPoint.Y + cursorOffsetPoint.Y);
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            isMousePress = false;
            _clickPoint = Point.Empty;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            button1.BackColor = Color.FromArgb(121, 110, 168);
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackColor = Color.Transparent;
        }

//Функция для выбора уравнения
        private double select(double x, double y, double kx)
        {
            double s = 0;
            int index = comboBox2.SelectedIndex;

            if(tabControl1.SelectedIndex != 0)
                index = comboBox1.SelectedIndex;

            switch (index)
            {
                case 0:
                    s = y + (kx * x);
                    break;
                case 1:
                    s = Math.Cos(y + (kx * x));
                    break;
                case 2:
                    s = Math.Sin(y + (kx * x));
                    break;
                case 3:
                    s = Math.Cos(y) + (kx * x);
                    break;
                case 4:
                    s = y + Math.Cos((kx * x));
                    break;
                case 5:
                    s = Math.Sin(y) + (kx * x);
                    break;
                case 6:
                    s = y + Math.Sin((kx * x));
                    break;
                case 7:
                    s = Math.Sin(y) + Math.Sin((kx * x));
                    break;
                case 8:
                    s = Math.Cos(y) + Math.Cos((kx * x));
                    break;
                case 9:
                    s = Math.Cos(y) + Math.Sin((kx * x));
                    break;
                case 10:
                    s = Math.Sin(y) + Math.Cos((kx * x));
                    break;
            }

            return s;
        }

//####  Кнопка для решения уравнения методом Эйлера
//####  Используемы переменные:
//####  Xi - начальный интервал;
//####  Xkon - конечный инетрвал;
//####  n - кол-во шагов;
//####  h - шаг;
//####  Yi - начальный у;
//####  kx - коэффициент х;
//####  ky - коэффициент у;
//####  Y_ - решение функции;
        private void button2_Click(object sender, EventArgs e)
        {

            if(textBox1.Text == "" || textBox2.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox7.Text == "" || textBox8.Text == "" || textBox1.Text == "," || textBox2.Text == "," || textBox4.Text == "," || textBox5.Text == "," || textBox7.Text == "," || textBox8.Text == "," || textBox1.Text == "-" || textBox2.Text == "-" || textBox4.Text == "-" || textBox5.Text == "-" || textBox7.Text == "-" || textBox8.Text == "-")
            {
                MessageBox.Show("Заполните все поля корректно!");
                return;
            }

            dataGridView1.Rows.Clear();

            double h, Xi, Yi, Xkon, kx, ky, n, Yx, Y_;

            Xi = Convert.ToDouble(textBox4.Text);
            Xkon = Convert.ToDouble(textBox5.Text);
            n = Convert.ToDouble(textBox2.Text);
            Yi = Convert.ToDouble(textBox1.Text);
            kx = Convert.ToDouble(textBox7.Text);
            ky = Convert.ToDouble(textBox8.Text);

            if(Xkon<Xi || n<=0)
            {
                MessageBox.Show("Заполните все поля корректно!");
                return;
            }

            h = (Xkon - Xi) / n;
            dataGridView1.Rows.Add("Xi", "Yi");
            dataGridView1.Rows.Add(0, Yi);

            for (int i = 0; i <= n-1; i++)
            {
                Y_ = select((kx * Xi), (ky * Yi), 1);
                Yx = Y_ * h;
                Yi += Yx;
                Xi = Xi + h;
                dataGridView1.Rows.Add(Math.Round(Xi, 4), Math.Round(Yi, 4));
            }
        }

//Метод Рунге-Кутта
        public double Runge(double x, double y, double h, double kx)
        {
            double k1, k2, k3, k4, y_1;

            k1 = h * select(x, y, kx);
            k2 = h * select(x + (h / 2), y + (Math.Round(k1,4) / 2), kx);
            k3 = h * select(x + (h / 2), y + (Math.Round(k2,4) / 2), kx);
            k4 = h * select(x + h, y + Math.Round(k3,4), kx);
            y_1 = (Math.Round(k1,4) + 2 * Math.Round(k2,4) + 2 * Math.Round(k3,4) + Math.Round(k4,4)) / 6;
            return Math.Round(y_1,4);
        }

//Кнопка для решения методом Адамса
//####  Используемы переменные:
//####  Xi - начальный интервал;
//####  Xkon - конечный инетрвал;
//####  n - кол-во шагов;
//####  h - шаг;
//####  Yi - начальный у;
//####  kx - коэффициент х;
//####  ky - коэффициент у;
//####  Ya1 - решение функции;
//####  Oky - ответ;
//####  listA - лист для записи разностей;
//####  listA1 -  лист для записи разностей;
        private void button3_Click(object sender, EventArgs e)
        {

            if (textBox3.Text == "" || textBox9.Text == "" || textBox11.Text == "" || textBox6.Text == "" || textBox10.Text == "" || textBox3.Text == "," || textBox9.Text == "," || textBox6.Text == "," || textBox11.Text == "," || textBox10.Text == "," || textBox3.Text == "-" || textBox9.Text == "-" || textBox6.Text == "-" || textBox10.Text == "-" || textBox11.Text == "-")
            {
                MessageBox.Show("Заполните все поля корректно!");
                return;
            }

            double h, Xi, Xii, Yi, Yii, Xkon, n, Ya1, q, Xk, Oky;
            List<double> listA = new List<double>();
            List<double> listA1 = new List<double>();
            Yi = Convert.ToDouble(textBox3.Text);
            Xi = Convert.ToDouble(textBox9.Text);
            Xkon = Convert.ToDouble(textBox6.Text);
            n = Convert.ToDouble(textBox10.Text);
            Xk = Convert.ToDouble(textBox11.Text);

            if (Xkon < Xi || n <= 0)
            {
                MessageBox.Show("Заполните все поля корректно!");
                return;
            }

            h = Math.Pow(n, 0.25);
            Ya1 = select(Math.Round(Xi, 4), Math.Round(Yi, 4), Xk);
            q = Math.Round(h,2) * Math.Round(Ya1,4);
            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add("Xi","Yi","Y'","q");
            dataGridView1.Rows.Add(Math.Round(Xi, 4), Math.Round(Yi, 4), Math.Round(Ya1, 4), Math.Round(q, 4));
            listA.Add(q);

            for (int i=1; i<4; i++)
            {
                Xii = Math.Round(Xi,4) + Math.Round(h,2);
                Yii = Runge(Math.Round(Xi,4), Math.Round(Yi,4), Math.Round(h,2), Xk) + Math.Round(Yi,4);
                Ya1 = select(Math.Round(Xii, 4), Math.Round(Yii, 4), Xk);
                q = Math.Round(h,2) * Math.Round(Ya1,4);
                Xi = Math.Round(Xii,4);
                Yi = Math.Round(Yii,4);
                listA.Add(Math.Round(q, 4));
                dataGridView1.Rows.Add(Math.Round(Xi, 4), Math.Round(Yi, 4), Math.Round(Ya1, 4), Math.Round(q, 4));
            }

            int j = 3;

            for (int i=0; i<3; i++)
            {
                listA1.Add(Math.Round(listA[j],4) - Math.Round(listA[j-1],4));
                j -= 1;
            }

            for (int i = 0; i < 2; i++)
            {
                listA1.Add(Math.Round(listA1[i],4) - Math.Round(listA1[i+1],4));
            }

            listA1.Add(Math.Round(listA1[3],4) - Math.Round(listA1[4],4));

            dataGridView1.Rows.Add("");
            dataGridView1.Rows.Add("'q1", "'q2", "'q3");
            dataGridView1.Rows.Add(Math.Round(listA1[2], 4), Math.Round(listA1[4], 4), Math.Round(listA1[5], 4));
            dataGridView1.Rows.Add(Math.Round(listA1[1], 4), Math.Round(listA1[3], 4));
            dataGridView1.Rows.Add(Math.Round(listA1[0],4));
            dataGridView1.Rows.Add("");

            Oky = Math.Round(q,4) + (Math.Round(listA1[0],4) / 2) + (5 / 12 * Math.Round(listA1[3],4)) + (3 / 8 * Math.Round(listA1[5], 4));

            dataGridView1.Rows.Add("▲Yi");
            dataGridView1.Rows.Add(Math.Round(Oky, 4));

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox a = (TextBox)sender;

            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != 44 && (e.KeyChar != '-' || (a.SelectionStart != 0)))
            {
                e.Handled = true;
            }

            if (e.KeyChar == ',')
            {
                if (a.Text.Split(',').Length > 1)
                {
                    e.Handled = true;
                }
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(tabControl1.SelectedIndex)
            {
                case 0:
                    textBox1.Clear(); 
                    textBox2.Clear(); 
                    textBox4.Clear(); 
                    textBox5.Clear();
                    textBox7.Clear(); 
                    textBox8.Clear();
                    dataGridView1.Columns.Remove(dataGridView1.Columns[3]);
                    dataGridView1.Columns.Remove(dataGridView1.Columns[2]);
                    dataGridView1.Rows.Clear();
                    comboBox2.SelectedIndex = 0;
                    break;

                case 1:
                    textBox3.Clear();
                    textBox9.Clear();
                    textBox6.Clear(); 
                    textBox10.Clear();
                    textBox11.Clear();
                    dataGridView1.Columns.Add("Column3","");
                    dataGridView1.Columns.Add("Column4", "");
                    dataGridView1.Rows.Clear();
                    comboBox1.SelectedIndex = 0;
                    break;

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.Columns.Remove(dataGridView1.Columns[3]);
            dataGridView1.Columns.Remove(dataGridView1.Columns[2]);
            comboBox2.SelectedIndex = 0;
        }
    }
}
